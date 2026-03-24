using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace Music_Studio_Booking
{
    public partial class StaffInstruments : System.Web.UI.Page
    {
        // Fix for CS0103: Declare this at the class level
        string connString = ConfigurationManager.ConnectionStrings["MyStudioConnString"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["IsAdmin"] == null || Session["IsAdmin"].ToString() != "True")
            {
                Response.Redirect("Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                BindGrid();
            }
        }

        // Fix for CS0103: This method refreshes your table
        private void BindGrid()
        {
            using (SqlConnection con = new SqlConnection(connString))
            {
                string sql = "SELECT * FROM Instruments";
                SqlDataAdapter da = new SqlDataAdapter(sql, con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                gvInstruments.DataSource = dt;
                gvInstruments.DataBind();
            }
        }

        protected void btnAddInstrument_Click(object sender, EventArgs e)
        {
            using (SqlConnection con = new SqlConnection(connString))
            {
                // Note: Ensure your txtNewName, txtNewPrice, and txtNewQty IDs match your ASPX file
                string sql = "INSERT INTO Instruments (InstrumentName, RentalPrice, AvailableQuantity, TotalQuantity, IsActive) VALUES (@Name, @Price, @Qty, @Qty, 1)";
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@Name", txtNewName.Text);
                cmd.Parameters.AddWithValue("@Price", decimal.Parse(txtNewPrice.Text));
                cmd.Parameters.AddWithValue("@Qty", int.Parse(txtNewQty.Text));

                con.Open();
                cmd.ExecuteNonQuery();
            }
            BindGrid(); // Refresh after adding
            txtNewName.Text = ""; txtNewPrice.Text = ""; txtNewQty.Text = ""; // Clear form
        }

        // Add these to handle the GridView buttons
        protected void gvInstruments_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvInstruments.EditIndex = e.NewEditIndex;
            BindGrid();
        }

        protected void gvInstruments_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvInstruments.EditIndex = -1;
            BindGrid();
        }

        protected void gvInstruments_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int id = Convert.ToInt32(gvInstruments.DataKeys[e.RowIndex].Value);
            using (SqlConnection con = new SqlConnection(connString))
            {
                SqlCommand cmd = new SqlCommand("DELETE FROM Instruments WHERE InstrumentID=@Id", con);
                cmd.Parameters.AddWithValue("@Id", id);
                con.Open();
                cmd.ExecuteNonQuery();
            }
            BindGrid();
        }
        protected void gvInstruments_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            // 1. Get the ID of the instrument being edited
            int id = Convert.ToInt32(gvInstruments.DataKeys[e.RowIndex].Value);

            // 2. Extract the new values from the GridView TextBoxes
            // Note: Cells[0] is Name, Cells[1] is Price, Cells[2] is Quantity
            string name = ((TextBox)gvInstruments.Rows[e.RowIndex].Cells[0].Controls[0]).Text;
            decimal price = decimal.Parse(((TextBox)gvInstruments.Rows[e.RowIndex].Cells[1].Controls[0]).Text);
            int qty = int.Parse(((TextBox)gvInstruments.Rows[e.RowIndex].Cells[2].Controls[0]).Text);

            // 3. Update the Database
            using (SqlConnection con = new SqlConnection(connString))
            {
                string sql = @"UPDATE Instruments 
                       SET InstrumentName=@Name, RentalPrice=@Price, AvailableQuantity=@Qty, TotalQuantity=@Qty 
                       WHERE InstrumentID=@Id";

                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@Name", name);
                cmd.Parameters.AddWithValue("@Price", price);
                cmd.Parameters.AddWithValue("@Qty", qty);
                cmd.Parameters.AddWithValue("@Id", id);

                con.Open();
                cmd.ExecuteNonQuery();
            }

            // 4. Exit Edit Mode and Refresh the list
            gvInstruments.EditIndex = -1;
            BindGrid();
        }
    }
}