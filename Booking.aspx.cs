using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.UI.WebControls;

namespace Music_Studio_Booking
{
    public partial class WebForm7 : System.Web.UI.Page
    {
        // Centralized connection string
        string connString = ConfigurationManager.ConnectionStrings["MyStudioConnString"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserID"] == null) Response.Redirect("Login.aspx");

            if (!IsPostBack)
            {
                txtDate.Attributes["min"] = DateTime.Now.ToString("yyyy-MM-dd");
                LoadInstruments();
                UpdateRunningTotal();
            }
        }

        private void LoadInstruments()
        {
            if (cblInstruments.Items.Count > 0) return;

            using (SqlConnection con = new SqlConnection(connString))
            {
                // SELECT
                string query = "SELECT InstrumentID, InstrumentName, RentalPrice FROM Instruments";
                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    string name = reader["InstrumentName"].ToString();
                    decimal price = Convert.ToDecimal(reader["RentalPrice"]);
                    string id = reader["InstrumentID"].ToString();

                    string priceText = price > 0 ? $"(P{price:N0})" : "(Free)";
                    cblInstruments.Items.Add(new ListItem($"{name} {priceText}", id));
                }
            }
        }

        protected void CalculateTotal(object sender, EventArgs e) => UpdateRunningTotal();

        private decimal UpdateRunningTotal()
        {
            decimal hourlyRate = 0;
            // LINQ: Count how many time checkboxes are checked
            int selectedHours = cblTime.Items.Cast<ListItem>().Count(i => i.Selected);
            decimal instrumentTotal = 0;

            // Switch for Studio Rates
            switch (ddlStudio.SelectedValue)
            {
                case "studio-a": hourlyRate = 300; break;
                case "studio-b": hourlyRate = 600; break;
                case "studio-c": hourlyRate = 900; break;
            }

            // Instrument Calculation
            var selectedIds = cblInstruments.Items.Cast<ListItem>()
                                .Where(i => i.Selected)
                                .Select(i => i.Value).ToList();

            if (selectedIds.Any())
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    // Create parameters (@p0, @p1...) for each ID to prevent SQL injection
                    var pars = selectedIds.Select((id, i) => "@p" + i).ToList();
                    string query = $"SELECT SUM(RentalPrice) FROM Instruments WHERE InstrumentID IN ({string.Join(",", pars)})";

                    SqlCommand cmd = new SqlCommand(query, con);
                    for (int i = 0; i < selectedIds.Count; i++)
                        cmd.Parameters.AddWithValue("@p" + i, selectedIds[i]);

                    con.Open();
                    object result = cmd.ExecuteScalar();
                    instrumentTotal = result != DBNull.Value ? Convert.ToDecimal(result) : 0;
                }
            }

            decimal total = (hourlyRate * selectedHours) + instrumentTotal;
            lblTotalPriceDisplay.Text = "P" + total.ToString("N2");
            return total;
        }

        protected void btnRequestBooking_Click(object sender, EventArgs e)
        {
            string room = ddlStudio.SelectedValue;
            string date = txtDate.Text;

            // 1. Get list of selected times
            var selectedTimes = cblTime.Items.Cast<ListItem>()
                                .Where(i => i.Selected)
                                .Select(i => i.Text).ToList();

            if (selectedTimes.Count == 0)
            {
                lblStatus.Text = "Please select at least one time slot.";
                return;
            }

            using (SqlConnection con = new SqlConnection(connString))
            {
                con.Open();

                // --- PHASE 1: AVAILABILITY CHECK ---
                foreach (string timeSlot in selectedTimes)
                {
                    // Count how many people booked this specific room + date + timeslot
                    string checkSql = @"SELECT COUNT(*) FROM Bookings 
                                WHERE StudioRoom = @Room 
                                AND BookingDate = @Date 
                                AND BookingTime LIKE @Slot
                                AND Status != 'Cancelled'";

                    SqlCommand checkCmd = new SqlCommand(checkSql, con);
                    checkCmd.Parameters.AddWithValue("@Room", room);
                    checkCmd.Parameters.AddWithValue("@Date", date);
                    checkCmd.Parameters.AddWithValue("@Slot", "%" + timeSlot + "%");

                    int existingBookings = (int)checkCmd.ExecuteScalar();

                    if (existingBookings >= 1)
                    {
                        lblStatus.Text = $"Sorry! {room} is already booked for {timeSlot}.";
                        lblStatus.ForeColor = System.Drawing.Color.Red;
                        return; 
                    }
                }

                // --- PHASE 2: INSERTION (Only runs if Phase 1 passed) ---
                decimal finalPrice = UpdateRunningTotal();
                string instList = string.Join(", ", cblInstruments.Items.Cast<ListItem>().Where(i => i.Selected).Select(i => i.Text));
                string timesString = string.Join(", ", selectedTimes);

                string insertQuery = @"INSERT INTO Bookings (UserEmail, StudioRoom, BookingDate, BookingTime, SelectedInstruments, TotalPrice, Status, CreatedAt) 
                               VALUES (@Email, @Room, @Date, @Time, @Instruments, @Price, 'Confirmed', GETDATE())";

                SqlCommand cmd = new SqlCommand(insertQuery, con);
                cmd.Parameters.AddWithValue("@Email", Session["UserEmail"]?.ToString() ?? "Unknown");
                cmd.Parameters.AddWithValue("@Room", room);
                cmd.Parameters.AddWithValue("@Date", date);
                cmd.Parameters.AddWithValue("@Time", timesString);
                cmd.Parameters.AddWithValue("@Instruments", instList);
                cmd.Parameters.AddWithValue("@Price", finalPrice);

                cmd.ExecuteNonQuery();
                Response.Redirect("Profile.aspx");
            }
        }
    }
}