using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace Music_Studio_Booking
{
    public partial class Profile : System.Web.UI.Page
    {
        string connString = ConfigurationManager.ConnectionStrings["MyStudioConnString"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            // Existing Redirect Logic
            if (Session["UserID"] == null) { Response.Redirect("Login.aspx"); return; }

            if (!IsPostBack)
            {
                // Fill labels
                lblName.Text = Session["UserName"]?.ToString() ?? "—";
                lblEmail.Text = Session["UserEmail"]?.ToString() ?? "—";
                lblUserId.Text = Session["UserID"]?.ToString() ?? "—";

                BindBookings();
            }
        }

        private void BindBookings()
        {
            string email = Session["UserEmail"]?.ToString();
            using (SqlConnection con = new SqlConnection(connString))
            {
                string query = "SELECT BookingID, StudioRoom, BookingDate, BookingTime, TotalPrice FROM Bookings WHERE UserEmail = @Email ORDER BY BookingDate DESC";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Email", email);

                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);

                gvUserBookings.DataSource = dt;
                gvUserBookings.DataBind();
            }
        }

        protected void gvUserBookings_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
        {
            int bookingId = Convert.ToInt32(gvUserBookings.DataKeys[e.RowIndex].Value);

            using (SqlConnection con = new SqlConnection(connString))
            {
                string deleteQuery = "DELETE FROM Bookings WHERE BookingID = @ID";
                SqlCommand cmd = new SqlCommand(deleteQuery, con);
                cmd.Parameters.AddWithValue("@ID", bookingId);

                con.Open();
                cmd.ExecuteNonQuery();
            }

            lblMessage.Text = "Booking cancelled successfully!";
            lblMessage.ForeColor = System.Drawing.Color.Green;
            BindBookings(); // Refresh the list
        }
    }
}