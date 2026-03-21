using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace Music_Studio_Booking
{
    public partial class StaffRequests : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //tinitingnan kung ang naka-login ba ay admin — kinukuha sa Session
            var isAdmin = Session["IsAdmin"] != null && Session["IsAdmin"].ToString() == "True";
            if (!isAdmin)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            //kung first load ng page (hindi galing sa button click), i-load ang data
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
                //binabasa ang bawat row ng resulta ng query gamit ang reader
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    BookingViewModel vm = new BookingViewModel();
                    vm.Id                  = reader.GetInt32(reader.GetOrdinal("BookingID"));
                    vm.CustomerEmail       = reader["UserEmail"].ToString();
                    vm.CustomerName        = reader["UserEmail"].ToString(); //==========NO SEPARATE NAME COLUMN, USE EMAIL
                    vm.StudioRoom          = reader["StudioRoom"].ToString();
                    vm.BookingDate         = Convert.ToDateTime(reader["BookingDate"]);
                    vm.BookingTime         = reader["BookingTime"].ToString();
                    vm.CreatedAt           = Convert.ToDateTime(reader["CreatedAt"]);
                    vm.SelectedInstruments = reader["SelectedInstruments"].ToString();
                    vm.TotalPrice          = Convert.ToDecimal(reader["TotalPrice"]);
                    vm.Status              = reader["Status"].ToString();
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

            //ikinukuha sa repeater yung list ng bookings para ma-display sa page
            rptRequests.DataSource = data;
            rptRequests.DataBind();
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


