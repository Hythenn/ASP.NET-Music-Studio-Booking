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
        string connString = ConfigurationManager.ConnectionStrings["MyStudioConnString"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            // Security Check
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

        private void BindGrid()
        {
            using (SqlConnection con = new SqlConnection(connString))
            {
                // Select all columns so we have access to ID, Name, Price, and Quantities
                string sql = "SELECT InstrumentID, InstrumentName, RentalPrice, TotalQuantity, AvailableQuantity, IsActive FROM Instruments";
                SqlDataAdapter da = new SqlDataAdapter(sql, con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                gvInstruments.DataSource = dt;
                gvInstruments.DataBind();
            }
        }

        protected void btnAddInstrument_Click(object sender, EventArgs e)
        {
            // Validation: Prevent crashes on empty inputs
            if (string.IsNullOrWhiteSpace(txtNewName.Text) ||
                string.IsNullOrWhiteSpace(txtNewPrice.Text) ||
                string.IsNullOrWhiteSpace(txtNewQty.Text))
            {
                // You could add a label for errors here
                return;
            }

            using (SqlConnection con = new SqlConnection(connString))
            {
                // We set both Available and Total quantity to the same value on creation
                string sql = @"INSERT INTO Instruments (InstrumentName, RentalPrice, TotalQuantity, AvailableQuantity, IsActive) 
                               VALUES (@Name, @Price, @Qty, @Qty, 1)";

                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@Name", txtNewName.Text.Trim());
                cmd.Parameters.AddWithValue("@Price", decimal.Parse(txtNewPrice.Text));
                cmd.Parameters.AddWithValue("@Qty", int.Parse(txtNewQty.Text));

                con.Open();
                cmd.ExecuteNonQuery();
            }
            BindGrid();
            txtNewName.Text = ""; txtNewPrice.Text = ""; txtNewQty.Text = "";
        }

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
            // Ensure DataKeyNames="InstrumentID" is set in your ASPX GridView tag
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
            int id = Convert.ToInt32(gvInstruments.DataKeys[e.RowIndex].Value);

            // Extracting values from EditItemTemplate TextBoxes
            // Use FindControl if you used Templates, otherwise index into Cells
            string name = ((TextBox)gvInstruments.Rows[e.RowIndex].Cells[0].Controls[0]).Text.Trim();
            decimal price = decimal.Parse(((TextBox)gvInstruments.Rows[e.RowIndex].Cells[1].Controls[0]).Text);
            int qty = int.Parse(((TextBox)gvInstruments.Rows[e.RowIndex].Cells[2].Controls[0]).Text);

            using (SqlConnection con = new SqlConnection(connString))
            {
                // IMPORTANT: When updating TotalQuantity, we usually update AvailableQuantity too 
                // unless you have a separate logic for currently rented items.
                string sql = @"UPDATE Instruments 
                               SET InstrumentName=@Name, RentalPrice=@Price, TotalQuantity=@Qty, AvailableQuantity=@Qty 
                               WHERE InstrumentID=@Id";

                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@Name", name);
                cmd.Parameters.AddWithValue("@Price", price);
                cmd.Parameters.AddWithValue("@Qty", qty);
                cmd.Parameters.AddWithValue("@Id", id);

                con.Open();
                cmd.ExecuteNonQuery();
            }

            gvInstruments.EditIndex = -1;
            BindGrid();
        }
    }
}