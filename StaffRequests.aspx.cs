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
            var isAdmin = Session["IsAdmin"] != null && Session["IsAdmin"].ToString() == "True";
            if (!isAdmin)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                LoadRequests();
            }
        }

        protected void rptRequests_ItemCommand(object source, System.Web.UI.WebControls.RepeaterCommandEventArgs e)
        {
            if (!int.TryParse(e.CommandArgument.ToString(), out int bookingId))
                return;

            string newStatus = e.CommandName == "Accept" ? "Accepted"
                             : e.CommandName == "Decline" ? "Declined"
                             : null;

            if (newStatus == null) return;

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
            var data = new List<BookingViewModel>();
            string connString = ConfigurationManager.ConnectionStrings["MyStudioConnString"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connString))
            {
                con.Open();
                string sql = @"SELECT BookingID, UserEmail, StudioRoom, BookingDate, BookingTime,
                                      CreatedAt, SelectedInstruments, TotalPrice, Status
                               FROM Bookings
                               ORDER BY CreatedAt DESC";

                SqlCommand cmd = new SqlCommand(sql, con);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var vm = new BookingViewModel
                    {
                        Id                  = reader.GetInt32(reader.GetOrdinal("BookingID")),
                        CustomerEmail       = reader["UserEmail"].ToString(),
                        CustomerName        = reader["UserEmail"].ToString(),   // no separate name column; use email
                        StudioRoom          = reader["StudioRoom"].ToString(),
                        BookingDate         = Convert.ToDateTime(reader["BookingDate"]),
                        BookingTime         = reader["BookingTime"].ToString(),
                        CreatedAt           = Convert.ToDateTime(reader["CreatedAt"]),
                        SelectedInstruments = reader["SelectedInstruments"].ToString(),
                        TotalPrice          = Convert.ToDecimal(reader["TotalPrice"]),
                        Status              = reader["Status"].ToString()
                    };
                    data.Add(vm);
                }
            }

            if (data.Count == 0)
            {
                PanelEmpty.Visible = true;
                rptRequests.Visible = false;
                return;
            }

            PanelEmpty.Visible = false;
            rptRequests.Visible = true;

            foreach (var item in data)
                item.StatusCss = GetStatusCss(item.Status);

            rptRequests.DataSource = data;
            rptRequests.DataBind();
        }

        private static string GetStatusCss(string status)
        {
            if (string.Equals(status, "Accepted", StringComparison.OrdinalIgnoreCase)) return "request-accepted";
            if (string.Equals(status, "Declined", StringComparison.OrdinalIgnoreCase)) return "request-declined";
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

            public string DateDisplay        => BookingDate.ToString("MMM dd, yyyy");
            public string CreatedAtDisplay   => CreatedAt.ToString("MMM dd, yyyy • hh:mm tt");
            public string StudioName         => FriendlyStudio(StudioRoom);
            public string TotalPriceDisplay  => "P" + TotalPrice.ToString("N2");

            private static string FriendlyStudio(string room)
            {
                switch (room)
                {
                    case "studio-a": return "Studio A – Modern Recording Room";
                    case "studio-b": return "Studio B – Vocal Booth & Mixing Gear";
                    case "studio-c": return "Studio C – Large Band Room";
                    default: return room;
                }
            }
        }
    }
}
