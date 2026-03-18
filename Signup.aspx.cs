using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using BCrypt.Net;

namespace Music_Studio_Booking
{
	public partial class WebForm6 : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{

		}
        protected void btnSignUp_Click(object sender, EventArgs e)
        {
            // 2. Validate the fields (Simplified for clarity)
            // You should have front-end validation (CompareValidator for passwords),
            // but always validate on the back-end too.
            if (string.IsNullOrWhiteSpace(signupName.Text) ||
                string.IsNullOrWhiteSpace(signupEmail.Text) ||
                string.IsNullOrWhiteSpace(signupPassword.Text))
            {
                lblErrorMessage.Text = "All fields are required.";
                return;
            }

            if (signupPassword.Text != signupconfirmPassword.Text)
            {
                lblErrorMessage.Text = "Passwords do not match.";
                return;
            }

            string name = signupName.Text.Trim();
            string email = signupEmail.Text.Trim();
            string password = signupPassword.Text; // Use the raw password ONLY for hashing

            // 3. SECURE HASHING: Do this BEFORE touching the database
            // This converts "mysecret123" into a safe, random-looking string
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(password);

            // 4. Connect to SQL
            // Make sure "MyStudioConnString" is in your Web.config
            string connString = ConfigurationManager.ConnectionStrings["MyStudioConnString"].ConnectionString;

            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    // Use a dynamic query using Parameters for protection
                    string query = "INSERT INTO Users (FullName, Email, PasswordHash) VALUES (@Name, @Email, @PassHash)";
                    SqlCommand cmd = new SqlCommand(query, con);

                    // 5. SECURITY: Always use Parameters to prevent SQL Injection
                    cmd.Parameters.AddWithValue("@Name", name);
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@PassHash", passwordHash); // Safe, hashed string

                    con.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        // Success! Redirect to the Login page
                        Response.Redirect("Login.aspx?signup=success");
                    }
                    else
                    {
                        lblErrorMessage.Text = "Registration failed. Please try again.";
                    }
                }
            }
            catch (SqlException ex)
            {
                // Catch specific SQL errors, like trying to insert a duplicate Email
                if (ex.Number == 2627 || ex.Number == 2601) // Violation of UNIQUE KEY constraint
                {
                    lblErrorMessage.Text = "This email address is already in use.";
                }
                else
                {
                    // In a real app, log this error for your review
                    lblErrorMessage.Text = "A database error occurred: " + ex.Message;
                }
            }
            catch (Exception ex)
            {
                lblErrorMessage.Text = "An unexpected error occurred.";
            }
        }
    }
}