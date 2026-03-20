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
                // 1. BLOCK PAST DATES in the browser picker
                txtDate.Attributes["min"] = DateTime.Now.ToString("yyyy-MM-dd");

                string roomFromUrl = Request.QueryString["room"];
                if (!string.IsNullOrEmpty(roomFromUrl))
                {
                    if (ddlStudio.Items.FindByValue(roomFromUrl) != null)
                    {
                        ddlStudio.SelectedValue = roomFromUrl;
                    }
                }

                UpdateRunningTotal();
            }
        }

        protected void CalculateTotal(object sender, EventArgs e)
        {
            UpdateRunningTotal();
        }

        private decimal UpdateRunningTotal()
        {
            decimal totalPrice = 0;

            // 1. Studio Price Calculation
            switch (ddlStudio.SelectedValue)
            {
                case "studio-a": totalPrice += 300; break;
                case "studio-b": totalPrice += 600; break;
                case "studio-c": totalPrice += 900; break;
            }

            // 2. Instrument Prices Calculation
            foreach (ListItem item in cblInstruments.Items)
            {
                if (item.Selected)
                {
                    totalPrice += Convert.ToDecimal(item.Value);
                }
            }

            // 3. Update the Label UI
            lblTotalPriceDisplay.Text = "P" + totalPrice.ToString("N2");

            return totalPrice;
        }

        protected void btnRequestBooking_Click(object sender, EventArgs e)
        {
            string email = Session["UserEmail"]?.ToString();
            string room = ddlStudio.SelectedValue;
            string dateInput = txtDate.Text;
            string time = ddlTime.SelectedValue;

            if (string.IsNullOrEmpty(email)) { Response.Redirect("Login.aspx"); return; }

            // --- DATE VALIDATION SECTION ---
            DateTime selectedDate;
            if (DateTime.TryParse(dateInput, out selectedDate))
            {
                if (selectedDate < DateTime.Today)
                {
                    lblStatus.Text = "Error: You cannot book a date in the past.";
                    lblStatus.ForeColor = System.Drawing.Color.Red;
                    return;
                }
            }

            // --- MATH SECTION ---
            decimal totalPrice = UpdateRunningTotal(); // Use the helper method to get the latest price

            List<string> selectedNames = new List<string>();
            foreach (ListItem item in cblInstruments.Items)
            {
                if (item.Selected)
                {
                    selectedNames.Add(item.Text);
                }
            }
            string instrumentsString = string.Join(", ", selectedNames);

            // --- DATABASE SECTION ---
            string connString = ConfigurationManager.ConnectionStrings["MyStudioConnString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connString))
            {
                con.Open();

                string checkQuery = "SELECT COUNT(*) FROM Bookings WHERE StudioRoom = @Room AND BookingDate = @Date AND BookingTime = @Time";
                SqlCommand checkCmd = new SqlCommand(checkQuery, con);
                checkCmd.Parameters.AddWithValue("@Room", room);
                checkCmd.Parameters.AddWithValue("@Date", dateInput);
                checkCmd.Parameters.AddWithValue("@Time", time);

                if ((int)checkCmd.ExecuteScalar() > 0)
                {
                    lblStatus.Text = "This slot is already taken!";
                    lblStatus.ForeColor = System.Drawing.Color.Red;
                    return;
                }

                string insertQuery = @"INSERT INTO Bookings (UserEmail, StudioRoom, BookingDate, BookingTime, SelectedInstruments, TotalPrice) 
                               VALUES (@Email, @Room, @Date, @Time, @Instruments, @Price)";

                SqlCommand cmd = new SqlCommand(insertQuery, con);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Room", room);
                cmd.Parameters.AddWithValue("@Date", dateInput);
                cmd.Parameters.AddWithValue("@Time", time);
                cmd.Parameters.AddWithValue("@Instruments", instrumentsString);
                cmd.Parameters.AddWithValue("@Price", totalPrice);

                cmd.ExecuteNonQuery();

                lblStatus.Text = $"Booking Successful! Total Price: P{totalPrice}";
                lblStatus.ForeColor = System.Drawing.Color.Green;
            }
        }
    }
}