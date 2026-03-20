using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;

namespace Music_Studio_Booking
{
    public partial class StaffSchedule : System.Web.UI.Page
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
                LoadSchedule();
            }
        }

        private void LoadSchedule()
        {
            var accepted = new List<ScheduleBookingViewModel>();
            string connString = ConfigurationManager.ConnectionStrings["MyStudioConnString"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connString))
            {
                con.Open();
                string sql = @"SELECT BookingID, UserEmail, StudioRoom, BookingDate, BookingTime,
                                      SelectedInstruments, TotalPrice
                               FROM Bookings
                               WHERE Status = 'Accepted'
                               ORDER BY BookingDate ASC, BookingTime ASC";

                SqlCommand cmd = new SqlCommand(sql, con);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    accepted.Add(new ScheduleBookingViewModel
                    {
                        BookingDate         = Convert.ToDateTime(reader["BookingDate"]),
                        CustomerName        = reader["UserEmail"].ToString(),
                        StudioName          = FriendlyStudio(reader["StudioRoom"].ToString()),
                        BookingTime         = reader["BookingTime"].ToString(),
                        SelectedInstruments = reader["SelectedInstruments"].ToString(),
                        TotalPrice          = Convert.ToDecimal(reader["TotalPrice"])
                    });
                }
            }

            if (accepted.Count == 0)
            {
                PanelEmptySchedule.Visible = true;
                rptSchedule.Visible = false;
                return;
            }

            PanelEmptySchedule.Visible = false;
            rptSchedule.Visible = true;

            var grouped = accepted
                .GroupBy(b => b.BookingDate.Date)
                .OrderBy(g => g.Key)
                .Select(g => new ScheduleDayViewModel
                {
                    Date     = g.Key,
                    Bookings = g.ToList()
                })
                .ToList();

            rptSchedule.DataSource = grouped;
            rptSchedule.DataBind();
        }

        protected void rptSchedule_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != System.Web.UI.WebControls.ListItemType.Item &&
                e.Item.ItemType != System.Web.UI.WebControls.ListItemType.AlternatingItem)
                return;

            var day = e.Item.DataItem as ScheduleDayViewModel;
            if (day == null || day.Bookings == null) return;

            var inner = (System.Web.UI.WebControls.Repeater)e.Item.FindControl("rptBookings");
            if (inner == null) return;

            inner.DataSource = day.Bookings;
            inner.DataBind();
        }

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

        private class ScheduleDayViewModel
        {
            public DateTime Date { get; set; }
            public List<ScheduleBookingViewModel> Bookings { get; set; }

            public string DateDisplay => Date.ToString("MMM dd, yyyy (dddd)");
            public string CountLabel
            {
                get
                {
                    int count = Bookings != null ? Bookings.Count : 0;
                    return count == 1 ? "1 booking" : count + " bookings";
                }
            }
        }

        private class ScheduleBookingViewModel
        {
            public DateTime BookingDate         { get; set; }
            public string   CustomerName        { get; set; }
            public string   StudioName          { get; set; }
            public string   BookingTime         { get; set; }
            public string   SelectedInstruments { get; set; }
            public decimal  TotalPrice          { get; set; }

            public string TimeRange          => BookingTime;
            public string TotalPriceDisplay  => "P" + TotalPrice.ToString("N2");
        }
    }
}
