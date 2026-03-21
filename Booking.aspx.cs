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
            //kung wala pang naka-login, ipinapadala agad sa login page
            if (Session["UserID"] == null)
            {
                Response.Redirect("Login.aspx");
            }

            //ang IsPostBack ay naging true kapag ni-click ng user ang isang button — first load lang tayo gumagalaw dito
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
            //kinukuha yung email ng naka-login mula sa Session
            string email = Session["UserEmail"]?.ToString();
            string room = ddlStudio.SelectedValue;
            string dateInput = txtDate.Text;

            if (string.IsNullOrEmpty(email)) { Response.Redirect("Login.aspx"); return; }

            //==========VALIDATE SELECTED DATE IS NOT IN THE PAST
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

            //==========COLLECT ALL SELECTED TIME SLOTS FROM CHECKBOX LIST
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

            //==========COMPUTE TOTAL PRICE AND COLLECT SELECTED INSTRUMENTS
            decimal totalPrice = UpdateRunningTotal();

            List<string> selectedInstruments = new List<string>();
            foreach (ListItem item in cblInstruments.Items)
            {
                if (item.Selected) selectedInstruments.Add(item.Text);
            }
            string instrumentsString = string.Join(", ", selectedInstruments);

            //==========OPEN DB CONNECTION AND PROCESS BOOKING
            //kukuha ng connection string sa Web.config para makakonekta sa database
            string connString = ConfigurationManager.ConnectionStrings["MyStudioConnString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connString))
            {
                con.Open();

                //==========CHECK EACH TIME SLOT FOR CONFLICTS IN SAME ROOM AND DATE
                foreach (string timeSlot in selectedTimes)
                {
                    string checkQuery = "SELECT COUNT(*) FROM Bookings WHERE StudioRoom = @Room AND BookingDate = @Date AND BookingTime LIKE '%' + @Slot + '%'";
                    SqlCommand checkCmd = new SqlCommand(checkQuery, con);
                    //ginagamit ang parameters para safe — hindi makapasok ang malicious SQL
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

                //isinasave na sa database yung booking — ExecuteNonQuery kasi walang data na ibabalik
                cmd.ExecuteNonQuery();

                lblStatus.Text = $"Booking Successful for {selectedTimes.Count} hours! Total: P{totalPrice}";
                lblStatus.ForeColor = System.Drawing.Color.Green;
            }
        }
    }
}

