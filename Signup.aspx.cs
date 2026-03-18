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
            string password = signupPassword.Text; 

            
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(password);

            // 4. Connect to SQL
            // Make sure "MyStudioConnString" is in your Web.config
            string connString = ConfigurationManager.ConnectionStrings["MyStudioConnString"].ConnectionString;

            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    
                    string query = "INSERT INTO Users (FullName, Email, PasswordHash) VALUES (@Name, @Email, @PassHash)";
                    SqlCommand cmd = new SqlCommand(query, con);

                    
                    cmd.Parameters.AddWithValue("@Name", name);
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@PassHash", passwordHash); 

                    con.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        
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
                
                if (ex.Number == 2627 || ex.Number == 2601) 
                {
                    lblErrorMessage.Text = "This email address is already in use.";
                }
                else
                {
                    
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