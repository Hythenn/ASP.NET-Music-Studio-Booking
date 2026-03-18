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
            // 1. Get data
            string email = Session["UserEmail"]?.ToString();
            string room = ddlStudio.SelectedValue;
            string date = txtDate.Text;
            string time = ddlTime.SelectedValue;

            if (string.IsNullOrEmpty(email)) { Response.Redirect("Login.aspx"); return; }

            string connString = ConfigurationManager.ConnectionStrings["MyStudioConnString"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connString))
            {
                con.Open();

                // 2. CHECK IF EXISTS
                string checkQuery = "SELECT COUNT(*) FROM Bookings WHERE StudioRoom = @Room AND BookingDate = @Date AND BookingTime = @Time";
                SqlCommand checkCmd = new SqlCommand(checkQuery, con);
                checkCmd.Parameters.AddWithValue("@Room", room);
                checkCmd.Parameters.AddWithValue("@Date", date);
                checkCmd.Parameters.AddWithValue("@Time", time);

                int count = (int)checkCmd.ExecuteScalar();

                if (count > 0)
                {
                    // Already booked!
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('This slot is already taken!');", true);
                }
                else
                {
                    // 3. SAVE TO DATABASE
                    string insertQuery = "INSERT INTO Bookings (UserEmail, StudioRoom, BookingDate, BookingTime) VALUES (@Email, @Room, @Date, @Time)";
                    SqlCommand cmd = new SqlCommand(insertQuery, con);
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@Room", room);
                    cmd.Parameters.AddWithValue("@Date", date);
                    cmd.Parameters.AddWithValue("@Time", time);

                    cmd.ExecuteNonQuery();

                    Response.Redirect("Account.aspx"); 
                }
            }
        }
    }
}