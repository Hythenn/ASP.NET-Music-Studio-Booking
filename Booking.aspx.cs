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
            //if no one is logged in, redirecting agad sa login page
            if (Session["UserID"] == null)
            {
                Response.Redirect("Login.aspx");
            }

            //IsPostBack is true if the user clicks a button — so first load lang tayo papasok dito
            if (!IsPostBack)
            {
                txtDate.Attributes["min"] = DateTime.Now.ToString("yyyy-MM-dd");

                UpdateRunningTotal();
            }
        }

        protected void CalculateTotal(object sender, EventArgs e)
        {
            UpdateRunningTotal();
        }

        private decimal UpdateRunningTotal()
        {
            decimal hourlyRate = 0;
            int selectedHours = 0;
            decimal instrumentTotal = 0;

            //==========DETERMINE HOURLY RATE BY STUDIO SELECTION
            switch (ddlStudio.SelectedValue)
            {
                case "studio-a": hourlyRate = 300; break;
                case "studio-b": hourlyRate = 600; break;
                case "studio-c": hourlyRate = 900; break;
            }

            //==========COUNT SELECTED TIME SLOTS
            foreach (ListItem timeItem in cblTime.Items)
            {
                if (timeItem.Selected) selectedHours++;
            }

            //==========SUM INSTRUMENT ADD-ON PRICES
            foreach (ListItem item in cblInstruments.Items)
            {
                if (item.Selected)
                {
                    instrumentTotal += Convert.ToDecimal(item.Value);
                }
            }

            //==========CALCULATE FINAL TOTAL: (RATE * HOURS) + INSTRUMENTS
            decimal total = (hourlyRate * selectedHours) + instrumentTotal;

            lblTotalPriceDisplay.Text = "P" + total.ToString("N2");

            return total;
        }

        protected void btnRequestBooking_Click(object sender, EventArgs e)
        {
            string connString = ConfigurationManager.ConnectionStrings["MyStudioConnString"].ConnectionString;
            // 1. Basic Session and Input Retrieval
            string email = Session["UserEmail"]?.ToString();
            string room = ddlStudio.SelectedValue;
            string dateInput = txtDate.Text;

            if (string.IsNullOrEmpty(email)) { Response.Redirect("Login.aspx"); return; }

            // 2. Date Validation
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

            // 3. Collect Selected Time Slots
            List<string> selectedTimes = new List<string>();
            foreach (ListItem item in cblTime.Items)
            {
                if (item.Selected) selectedTimes.Add(item.Text);
            }

            if (selectedTimes.Count == 0)
            {
                lblStatus.Text = "Please select at least one time slot.";
                lblStatus.ForeColor = System.Drawing.Color.Red;
                return;
            }

            // 4. Calculate Final Data for Insertion
            string timesFormatted = string.Join(", ", selectedTimes);
            decimal totalPrice = UpdateRunningTotal();

            List<string> selectedInstruments = new List<string>();
            foreach (ListItem item in cblInstruments.Items)
            {
                if (item.Selected) selectedInstruments.Add(item.Text);
            }
            string instrumentsString = string.Join(", ", selectedInstruments);

            // 5. Database Logic: Capacity Check & Insertion
            using (SqlConnection con = new SqlConnection(connString))
            {
                con.Open();

                // A. GET MAX CAPACITY for the chosen studio room
                int maxRooms = 1; // Default fallback if studio is not found
                string capQuery = "SELECT TotalRooms FROM Studios WHERE StudioID = @Room";
                SqlCommand capCmd = new SqlCommand(capQuery, con);
                capCmd.Parameters.AddWithValue("@Room", room);

                object capResult = capCmd.ExecuteScalar();
                if (capResult != null) maxRooms = Convert.ToInt32(capResult);

                // B. CHECK AVAILABILITY for each selected slot
                foreach (string timeSlot in selectedTimes)
                {
                    // Count how many confirmed/active bookings exist for this specific slot
                    string checkQuery = @"SELECT COUNT(*) FROM Bookings 
                                  WHERE StudioRoom = @Room 
                                  AND BookingDate = @Date 
                                  AND BookingTime LIKE '%' + @Slot + '%'
                                  AND Status != 'Cancelled'";

                    SqlCommand checkCmd = new SqlCommand(checkQuery, con);
                    checkCmd.Parameters.AddWithValue("@Room", room);
                    checkCmd.Parameters.AddWithValue("@Date", dateInput);
                    checkCmd.Parameters.AddWithValue("@Slot", timeSlot);

                    int alreadyBooked = (int)checkCmd.ExecuteScalar();

                    // C. Capacity Enforcement
                    if (alreadyBooked >= maxRooms)
                    {
                        lblStatus.Text = $"Slot {timeSlot} is fully booked. (Max {maxRooms} rooms allowed).";
                        lblStatus.ForeColor = System.Drawing.Color.Red;
                        return; // Stop the execution here
                    }
                }

                // D. PROCEED TO INSERT if all slots passed the check
                string insertQuery = @"INSERT INTO Bookings (UserEmail, StudioRoom, BookingDate, BookingTime, SelectedInstruments, TotalPrice, Status, CreatedAt) 
                               VALUES (@Email, @Room, @Date, @Time, @Instruments, @Price, 'Confirmed', GETDATE())";

                SqlCommand cmd = new SqlCommand(insertQuery, con);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Room", room);
                cmd.Parameters.AddWithValue("@Date", dateInput);
                cmd.Parameters.AddWithValue("@Time", timesFormatted);
                cmd.Parameters.AddWithValue("@Instruments", instrumentsString);
                cmd.Parameters.AddWithValue("@Price", totalPrice);

                cmd.ExecuteNonQuery();

                lblStatus.Text = $"Booking Successful! Total: P{totalPrice:N2}";
                lblStatus.ForeColor = System.Drawing.Color.Green;
            }
        }

    }

}
