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
            // Security Check
            if (Session["UserID"] == null) Response.Redirect("Login.aspx");

            if (!IsPostBack)
            {
                // Set the minimum date to today
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
                string query = "SELECT InstrumentID, InstrumentName, RentalPrice FROM Instruments WHERE IsActive = 1";
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
            int selectedHours = cblTime.Items.Cast<ListItem>().Count(i => i.Selected);
            decimal instrumentTotal = 0;

            // Fetch dynamic rates from the DB or use your existing switch
            switch (ddlStudio.SelectedValue)
            {
                case "studio-a": hourlyRate = 300; break;
                case "studio-b": hourlyRate = 600; break;
                case "studio-c": hourlyRate = 900; break;
            }

            var selectedIds = cblInstruments.Items.Cast<ListItem>()
                                .Where(i => i.Selected)
                                .Select(i => i.Value).ToList();

            if (selectedIds.Any())
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
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
            lblStatus.Text = ""; // Clear status
            string room = ddlStudio.SelectedValue;
            string date = txtDate.Text;

            // 1. Prepare selected times (Standardized format)
            var selectedTimes = cblTime.Items.Cast<ListItem>()
                                .Where(i => i.Selected)
                                .Select(i => i.Text.Replace(" ", "").Trim())
                                .ToList();

            var selectedInstruments = cblInstruments.Items.Cast<ListItem>()
                                        .Where(i => i.Selected)
                                        .ToList();

            if (string.IsNullOrEmpty(date) || selectedTimes.Count == 0)
            {
                lblStatus.Text = "Please select a date and at least one time slot.";
                lblStatus.ForeColor = System.Drawing.Color.Red;
                return;
            }

            using (SqlConnection con = new SqlConnection(connString))
            {
                con.Open();

                // --- PHASE 1: DYNAMIC CAPACITY CHECK ---
                // Query your Studios table for the TotalRooms allowed for the selected room
                SqlCommand capacityCmd = new SqlCommand("SELECT TotalRooms FROM Studios WHERE StudioID = @RoomID", con);
                capacityCmd.Parameters.AddWithValue("@RoomID", room);
                object capacityResult = capacityCmd.ExecuteScalar();
                int maxCapacity = (capacityResult != null) ? Convert.ToInt32(capacityResult) : 1;

                foreach (string timeSlot in selectedTimes)
                {
                    string checkSql = @"SELECT COUNT(*) FROM Bookings 
                                        WHERE StudioRoom = @Room 
                                        AND BookingDate = @Date 
                                        AND (',' + REPLACE(BookingTime, ' ', '') + ',') LIKE @Slot 
                                        AND Status NOT IN ('Declined', 'Cancelled')";

                    SqlCommand checkCmd = new SqlCommand(checkSql, con);
                    checkCmd.Parameters.AddWithValue("@Room", room);
                    checkCmd.Parameters.AddWithValue("@Date", date);
                    checkCmd.Parameters.AddWithValue("@Slot", "%," + timeSlot + ",%");

                    int currentBookings = (int)checkCmd.ExecuteScalar();

                    if (currentBookings >= maxCapacity)
                    {
                        lblStatus.Text = $"Sorry! {room} has reached its limit of {maxCapacity} rooms for {timeSlot}.";
                        lblStatus.ForeColor = System.Drawing.Color.Red;
                        return;
                    }
                }

                // --- PHASE 2: INSTRUMENT INVENTORY CHECK ---
                foreach (ListItem item in selectedInstruments)
                {
                    int instID = int.Parse(item.Value);
                    string instName = item.Text.Split('(')[0].Trim();

                    SqlCommand stockCmd = new SqlCommand("SELECT TotalQuantity FROM Instruments WHERE InstrumentID = @ID", con);
                    stockCmd.Parameters.AddWithValue("@ID", instID);
                    int totalStock = Convert.ToInt32(stockCmd.ExecuteScalar());

                    foreach (string timeSlot in selectedTimes)
                    {
                        string usageSql = @"SELECT COUNT(*) FROM BookingItems bi
                                            JOIN Bookings b ON bi.BookingID = b.BookingID
                                            WHERE bi.InstrumentID = @InstID 
                                            AND b.BookingDate = @Date 
                                            AND (',' + REPLACE(b.BookingTime, ' ', '') + ',') LIKE @Slot 
                                            AND b.Status NOT IN ('Declined', 'Cancelled')";

                        SqlCommand usageCmd = new SqlCommand(usageSql, con);
                        usageCmd.Parameters.AddWithValue("@InstID", instID);
                        usageCmd.Parameters.AddWithValue("@Date", date);
                        usageCmd.Parameters.AddWithValue("@Slot", "%," + timeSlot + ",%");

                        if ((int)usageCmd.ExecuteScalar() >= totalStock)
                        {
                            lblStatus.Text = $"All {instName}s are booked for {timeSlot}.";
                            lblStatus.ForeColor = System.Drawing.Color.Red;
                            return;
                        }
                    }
                }

                // --- PHASE 3: TRANSACTIONAL INSERT ---
                SqlTransaction transaction = con.BeginTransaction();
                try
                {
                    decimal finalPrice = UpdateRunningTotal();
                    string timesForDb = "," + string.Join(",", selectedTimes) + ",";

                    string insertBookingSql = @"INSERT INTO Bookings (UserEmail, StudioRoom, BookingDate, BookingTime, TotalPrice, Status, CreatedAt) 
                                               OUTPUT INSERTED.BookingID
                                               VALUES (@Email, @Room, @Date, @Time, @Price, 'Pending', GETDATE())";

                    SqlCommand cmd = new SqlCommand(insertBookingSql, con, transaction);
                    cmd.Parameters.AddWithValue("@Email", Session["UserEmail"].ToString());
                    cmd.Parameters.AddWithValue("@Room", room);
                    cmd.Parameters.AddWithValue("@Date", date);
                    cmd.Parameters.AddWithValue("@Time", timesForDb);
                    cmd.Parameters.AddWithValue("@Price", finalPrice);

                    int newBookingID = (int)cmd.ExecuteScalar();

                    foreach (ListItem item in selectedInstruments)
                    {
                        int instrumentID = int.Parse(item.Value);

                        SqlCommand priceCmd = new SqlCommand("SELECT RentalPrice FROM Instruments WHERE InstrumentID = @ID", con, transaction);
                        priceCmd.Parameters.AddWithValue("@ID", instrumentID);
                        decimal currentPrice = (decimal)priceCmd.ExecuteScalar();

                        string insertItemSql = "INSERT INTO BookingItems (BookingID, InstrumentID, PriceAtBooking) VALUES (@BID, @IID, @Price)";
                        SqlCommand itemCmd = new SqlCommand(insertItemSql, con, transaction);
                        itemCmd.Parameters.AddWithValue("@BID", newBookingID);
                        itemCmd.Parameters.AddWithValue("@IID", instrumentID);
                        itemCmd.Parameters.AddWithValue("@Price", currentPrice);
                        itemCmd.ExecuteNonQuery();
                    }

                    transaction.Commit();
                    //Response.Redirect("Profile.aspx");
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    lblStatus.Text = "Error saving booking: " + ex.Message;
                    lblStatus.ForeColor = System.Drawing.Color.Red;
                }
            }
        }
    }
}