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

            // 1. Determine Hourly Rate
            switch (ddlStudio.SelectedValue)
            {
                case "studio-a": hourlyRate = 300; break;
                case "studio-b": hourlyRate = 600; break;
                case "studio-c": hourlyRate = 900; break;
            }

            // 2. Count selected time slots
            foreach (ListItem timeItem in cblTime.Items)
            {
                if (timeItem.Selected) selectedHours++;
            }

            // 3. Instrument Prices Calculation
            foreach (ListItem item in cblInstruments.Items)
            {
                if (item.Selected)
                {
                    instrumentTotal += Convert.ToDecimal(item.Value);
                }
            }

            // 4. Calculate Final Total (Rate * Hours + Instruments)
            decimal total = (hourlyRate * selectedHours) + instrumentTotal;

            lblTotalPriceDisplay.Text = "P" + total.ToString("N2");

            return total;
        }

        protected void btnRequestBooking_Click(object sender, EventArgs e)
        {
            string email = Session["UserEmail"]?.ToString();
            string room = ddlStudio.SelectedValue;
            string dateInput = txtDate.Text;

            if (string.IsNullOrEmpty(email)) { Response.Redirect("Login.aspx"); return; }

            // --- DATE VALIDATION ---
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

            // --- COLLECT MULTIPLE TIMES ---
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

            string timesFormatted = string.Join(", ", selectedTimes);

            // --- MATH SECTION ---
            decimal totalPrice = UpdateRunningTotal();

            List<string> selectedInstruments = new List<string>();
            foreach (ListItem item in cblInstruments.Items)
            {
                if (item.Selected) selectedInstruments.Add(item.Text);
            }
            string instrumentsString = string.Join(", ", selectedInstruments);

            // --- DATABASE SECTION ---
            string connString = ConfigurationManager.ConnectionStrings["MyStudioConnString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connString))
            {
                con.Open();

                // Check for overlapping bookings
                // Note: This checks if ANY of the selected hours exist in a booking for that room/date
                foreach (string timeSlot in selectedTimes)
                {
                    string checkQuery = "SELECT COUNT(*) FROM Bookings WHERE StudioRoom = @Room AND BookingDate = @Date AND BookingTime LIKE '%' + @Slot + '%'";
                    SqlCommand checkCmd = new SqlCommand(checkQuery, con);
                    checkCmd.Parameters.AddWithValue("@Room", room);
                    checkCmd.Parameters.AddWithValue("@Date", dateInput);
                    checkCmd.Parameters.AddWithValue("@Slot", timeSlot);

                    if ((int)checkCmd.ExecuteScalar() > 0)
                    {
                        lblStatus.Text = $"The slot {timeSlot} is already taken!";
                        lblStatus.ForeColor = System.Drawing.Color.Red;
                        return;
                    }
                }

                string insertQuery = @"INSERT INTO Bookings (UserEmail, StudioRoom, BookingDate, BookingTime, SelectedInstruments, TotalPrice) 
                                       VALUES (@Email, @Room, @Date, @Time, @Instruments, @Price)";

                SqlCommand cmd = new SqlCommand(insertQuery, con);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Room", room);
                cmd.Parameters.AddWithValue("@Date", dateInput);
                cmd.Parameters.AddWithValue("@Time", timesFormatted);
                cmd.Parameters.AddWithValue("@Instruments", instrumentsString);
                cmd.Parameters.AddWithValue("@Price", totalPrice);

                cmd.ExecuteNonQuery();

                lblStatus.Text = $"Booking Successful for {selectedTimes.Count} hours! Total: P{totalPrice}";
                lblStatus.ForeColor = System.Drawing.Color.Green;
            }
        }
    }
}