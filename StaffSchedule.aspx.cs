using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

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
                    ScheduleBookingViewModel b = new ScheduleBookingViewModel();
                    b.BookingDate         = Convert.ToDateTime(reader["BookingDate"]);
                    b.CustomerName        = reader["UserEmail"].ToString();
                    b.StudioName          = FriendlyStudio(reader["StudioRoom"].ToString());
                    b.BookingTime         = reader["BookingTime"].ToString();
                    b.SelectedInstruments = reader["SelectedInstruments"].ToString();
                    b.TotalPrice          = Convert.ToDecimal(reader["TotalPrice"]);
                    accepted.Add(b);
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

            //==========GROUP ACCEPTED BOOKINGS BY DATE USING SIMPLE FOREACH
            var grouped = new List<ScheduleDayViewModel>();
            foreach (var b in accepted)
            {
                ScheduleDayViewModel existing = null;
                foreach (var day in grouped)
                {
                    if (day.Date == b.BookingDate.Date)
                    {
                        existing = day;
                        break;
                    }
                }

                if (existing == null)
                {
                    existing = new ScheduleDayViewModel();
                    existing.Date = b.BookingDate.Date;
                    existing.Bookings = new List<ScheduleBookingViewModel>();
                    grouped.Add(existing);
                }

                existing.Bookings.Add(b);
            }

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

        //==========CONVERT STUDIO KEY TO DISPLAY NAME
        private static string FriendlyStudio(string room)
        {
            if (room == "studio-a") return "Studio A – Modern Recording Room";
            if (room == "studio-b") return "Studio B – Vocal Booth & Mixing Gear";
            if (room == "studio-c") return "Studio C – Large Band Room";
            return room;
        }

        private class ScheduleDayViewModel
        {
            public DateTime Date { get; set; }
            public List<ScheduleBookingViewModel> Bookings { get; set; }

            public string DateDisplay
            {
                get { return Date.ToString("MMM dd, yyyy (dddd)"); }
            }
            public string CountLabel
            {
                get
                {
                    int count = 0;
                    if (Bookings != null)
                        count = Bookings.Count;

                    if (count == 1)
                        return "1 booking";
                    return count + " bookings";
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

            public string TimeRange
            {
                get { return BookingTime; }
            }
            public string TotalPriceDisplay
            {
                get { return "P" + TotalPrice.ToString("N2"); }
            }
        }
    }
}


