using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web.UI.WebControls;

namespace Music_Studio_Booking
{
    public partial class WebForm7 : System.Web.UI.Page
    {
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
                string query = "SELECT InstrumentID, InstrumentName, RentalPrice FROM Instruments";
                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    string name = reader["InstrumentName"].ToString();
                    decimal price = Convert.ToDecimal(reader["RentalPrice"]);
                    string id = reader["InstrumentID"].ToString(); // We store ID in Value

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

            switch (ddlStudio.SelectedValue)
            {
                case "studio-a": hourlyRate = 300; break;
                case "studio-b": hourlyRate = 600; break;
                case "studio-c": hourlyRate = 900; break;
            }

            // Optimize: One DB call to get all prices for selected IDs
            var selectedIds = cblInstruments.Items.Cast<ListItem>()
                                .Where(i => i.Selected)
                                .Select(i => i.Value).ToList();

            if (selectedIds.Any())
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    string query = $"SELECT SUM(RentalPrice) FROM Instruments WHERE InstrumentID IN ({string.Join(",", selectedIds)})";
                    SqlCommand cmd = new SqlCommand(query, con);
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
            // ... Keep your existing validation logic ...

            using (SqlConnection con = new SqlConnection(connString))
            {
                con.Open();

                // PHASE 1 & 2: Capacity Checks (Keep your existing loop logic here)
                // ... 

                // PHASE 3: Insertion
                decimal finalPrice = UpdateRunningTotal();
                string instList = string.Join(", ", cblInstruments.Items.Cast<ListItem>().Where(i => i.Selected).Select(i => i.Text));
                string times = string.Join(", ", cblTime.Items.Cast<ListItem>().Where(i => i.Selected).Select(i => i.Text));

                string insertQuery = @"INSERT INTO Bookings (UserEmail, StudioRoom, BookingDate, BookingTime, SelectedInstruments, TotalPrice, Status, CreatedAt) 
                                       VALUES (@Email, @Room, @Date, @Time, @Instruments, @Price, 'Confirmed', GETDATE())";

                SqlCommand cmd = new SqlCommand(insertQuery, con);
                cmd.Parameters.AddWithValue("@Email", Session["UserEmail"].ToString());
                cmd.Parameters.AddWithValue("@Room", ddlStudio.SelectedValue);
                cmd.Parameters.AddWithValue("@Date", txtDate.Text);
                cmd.Parameters.AddWithValue("@Time", times);
                cmd.Parameters.AddWithValue("@Instruments", instList);
                cmd.Parameters.AddWithValue("@Price", finalPrice);

                cmd.ExecuteNonQuery();
                lblStatus.Text = "Booking Successful!";
                lblStatus.ForeColor = System.Drawing.Color.Green;
            }
        }
    }
}