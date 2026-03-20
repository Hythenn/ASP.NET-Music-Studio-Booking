using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Music_Studio_Booking
{
    public partial class WebForm7 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserID"] == null)
            {
                Response.Redirect("Login.aspx");
            }

            if (!IsPostBack)
            {
                string roomFromUrl = Request.QueryString["room"];

                if (!string.IsNullOrEmpty(roomFromUrl))
                {
                    ddlStudio.SelectedValue = roomFromUrl;
                }
            }
        }

        protected void btnRequestBooking_Click(object sender, EventArgs e)
        {
            string email = Session["UserEmail"]?.ToString();
            string room = ddlStudio.SelectedValue;
            string date = txtDate.Text;
            string time = ddlTime.SelectedValue;

            if (string.IsNullOrEmpty(email)) { Response.Redirect("Login.aspx"); return; }

            // --- MATH SECTION ---
            decimal totalPrice = 0;

            // 1. Calculate Studio Price (assuming 1 hour per slot)
            switch (room)
            {
                case "studio-a": totalPrice += 300; break;
                case "studio-b": totalPrice += 600; break;
                case "studio-c": totalPrice += 900; break;
            }

            // 2. Calculate Instruments & Build the List
            List<string> selectedNames = new List<string>();
            foreach (ListItem item in cblInstruments.Items)
            {
                if (item.Selected)
                {
                    // Add the name (text) to our list
                    selectedNames.Add(item.Text);
                    // Add the value (price) to our total
                    totalPrice += Convert.ToDecimal(item.Value);
                }
            }
            string instrumentsString = string.Join(", ", selectedNames);

            // --- DATABASE SECTION ---
            string connString = ConfigurationManager.ConnectionStrings["MyStudioConnString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connString))
            {
                con.Open();

                // Double-check if busy
                string checkQuery = "SELECT COUNT(*) FROM Bookings WHERE StudioRoom = @Room AND BookingDate = @Date AND BookingTime = @Time";
                SqlCommand checkCmd = new SqlCommand(checkQuery, con);
                checkCmd.Parameters.AddWithValue("@Room", room);
                checkCmd.Parameters.AddWithValue("@Date", date);
                checkCmd.Parameters.AddWithValue("@Time", time);

                if ((int)checkCmd.ExecuteScalar() > 0)
                {
                    lblStatus.Text = "This slot is already taken!";
                    lblStatus.ForeColor = System.Drawing.Color.Red;
                    return;
                }

                // SAVE EVERYTHING
                string insertQuery = @"INSERT INTO Bookings (UserEmail, StudioRoom, BookingDate, BookingTime, SelectedInstruments, TotalPrice) 
                               VALUES (@Email, @Room, @Date, @Time, @Instruments, @Price)";

                SqlCommand cmd = new SqlCommand(insertQuery, con);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Room", room);
                cmd.Parameters.AddWithValue("@Date", date);
                cmd.Parameters.AddWithValue("@Time", time);
                cmd.Parameters.AddWithValue("@Instruments", instrumentsString);
                cmd.Parameters.AddWithValue("@Price", totalPrice);

                cmd.ExecuteNonQuery();

                // Show success with the total price
                lblStatus.Text = $"Booking Successful! Total Price: P{totalPrice}";
                lblStatus.ForeColor = System.Drawing.Color.Green;
            }
        }
    }
}