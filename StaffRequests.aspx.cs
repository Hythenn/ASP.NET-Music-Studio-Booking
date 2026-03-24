using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;

namespace Music_Studio_Booking
{
    public partial class StaffRequests : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //checking kung admin ba ang naka-login — based sa Session
            var isAdmin = Session["IsAdmin"] != null && Session["IsAdmin"].ToString() == "True";
            if (!isAdmin)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            //if it's the first page load, i-load na the data
            if (!IsPostBack)
            {
                LoadRequests();
            }
        }

        protected void rptRequests_ItemCommand(object source, System.Web.UI.WebControls.RepeaterCommandEventArgs e)
        {
            int bookingId;
            if (!int.TryParse(e.CommandArgument.ToString(), out bookingId))
                return;

            string newStatus = "";
            if (e.CommandName == "Accept")
            {
                newStatus = "Accepted";
            }
            else if (e.CommandName == "Decline")
            {
                newStatus = "Declined";
            }
            else
            {
                return;
            }

            string connString = ConfigurationManager.ConnectionStrings["MyStudioConnString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connString))
            {
                con.Open();
                string sql = "UPDATE Bookings SET Status = @Status WHERE BookingID = @Id";
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@Status", newStatus);
                cmd.Parameters.AddWithValue("@Id", bookingId);
                cmd.ExecuteNonQuery();
            }

            LoadRequests();
        }

        private void LoadRequests()
        {
            var allBookings = new List<BookingViewModel>();
            string connString = ConfigurationManager.ConnectionStrings["MyStudioConnString"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connString))
            {
                con.Open();
                string sql = @"SELECT BookingID, UserEmail, StudioRoom, BookingDate, BookingTime,
                              CreatedAt, SelectedInstruments, TotalPrice, Status
                       FROM Bookings
                       ORDER BY BookingDate DESC"; // Order by the actual event date

                SqlCommand cmd = new SqlCommand(sql, con);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    allBookings.Add(new BookingViewModel
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("BookingID")),
                        CustomerEmail = reader["UserEmail"].ToString(),
                        StudioRoom = reader["StudioRoom"].ToString(),
                        BookingDate = Convert.ToDateTime(reader["BookingDate"]),
                        BookingTime = reader["BookingTime"].ToString(),
                        CreatedAt = Convert.ToDateTime(reader["CreatedAt"]),
                        SelectedInstruments = reader["SelectedInstruments"].ToString(),
                        TotalPrice = Convert.ToDecimal(reader["TotalPrice"]),
                        Status = reader["Status"].ToString(),
                        StatusCss = GetStatusCss(reader["Status"].ToString())
                    });
                }
            }

            if (allBookings.Count == 0)
            {
                PanelEmpty.Visible = true;
                rptGroups.Visible = false;
                return;
            }


            var groupedData = allBookings
                .GroupBy(b => b.BookingDate.ToString("MMMM yyyy")) // Group by "Month Year"
                .Select(g => new {
                    GroupName = g.Key,
                    Bookings = g.ToList()
                })
                .ToList();

            PanelEmpty.Visible = false;
            rptGroups.Visible = true;

            rptGroups.DataSource = groupedData;
            rptGroups.DataBind();
        }

        //==========RETURN CSS CLASS NAME BASED ON BOOKING STATUS
        private static string GetStatusCss(string status)
        {
            if (status == "Accepted") return "request-accepted";
            if (status == "Declined") return "request-declined";
            return "request-pending";
        }

        [Serializable]
        public class BookingViewModel
        {
            public int      Id                  { get; set; }
            public string   CustomerEmail       { get; set; }
            public string   CustomerName        { get; set; }
            public string   StudioRoom          { get; set; }
            public DateTime BookingDate         { get; set; }
            public string   BookingTime         { get; set; }
            public DateTime CreatedAt           { get; set; }
            public string   SelectedInstruments { get; set; }
            public decimal  TotalPrice          { get; set; }
            public string   Status              { get; set; }
            public string   StatusCss           { get; set; }

            public string DateDisplay
            {
                get { return BookingDate.ToString("MMM dd, yyyy"); }
            }
            public string CreatedAtDisplay
            {
                get { return CreatedAt.ToString("MMM dd, yyyy • hh:mm tt"); }
            }
            public string StudioName
            {
                get { return FriendlyStudio(StudioRoom); }
            }
            public string TotalPriceDisplay
            {
                get { return "P" + TotalPrice.ToString("N2"); }
            }

            //==========CONVERT STUDIO KEY TO DISPLAY NAME
            private static string FriendlyStudio(string room)
            {
                if (room == "studio-a") return "Studio A – Modern Recording Room";
                if (room == "studio-b") return "Studio B – Vocal Booth & Mixing Gear";
                if (room == "studio-c") return "Studio C – Large Band Room";
                return room;
            }
        }
    }
}


