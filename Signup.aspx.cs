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
            // 1. Validation First
            if (string.IsNullOrWhiteSpace(signupName.Text) ||
                string.IsNullOrWhiteSpace(signupEmail.Text) ||
                string.IsNullOrWhiteSpace(signupPassword.Text) ||
                string.IsNullOrWhiteSpace(signupAnswer.Text))
            {
                lblErrorMessage.Text = "All fields, including the security answer, are required.";
                return;
            }

            if (signupPassword.Text != signupconfirmPassword.Text)
            {
                lblErrorMessage.Text = "Passwords do not match.";
                return;
            }

            // 2. Prepare Data
            string name = signupName.Text.Trim();
            string email = signupEmail.Text.Trim();
            string password = signupPassword.Text;
            string securityQuestion = ddlSecurityQuestion.SelectedValue;
            string securityAnswer = signupAnswer.Text.Trim().ToLower();

            // 3. Hashing (Using BCrypt)
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(password);
            string answerHash = BCrypt.Net.BCrypt.HashPassword(securityAnswer);

            // 4. Database Operation
            string connString = ConfigurationManager.ConnectionStrings["MyStudioConnString"].ConnectionString;

            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    // Define the query
                    string query = "INSERT INTO Users (FullName, Email, PasswordHash, SecurityQuestion, SecurityAnswerHash) " +
                                   "VALUES (@Name, @Email, @PassHash, @Question, @AnswerHash)";

                    // NOW create the cmd object
                    SqlCommand cmd = new SqlCommand(query, con);

                    // Add all parameters
                    cmd.Parameters.AddWithValue("@Name", name);
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@PassHash", passwordHash);
                    cmd.Parameters.AddWithValue("@Question", securityQuestion);
                    cmd.Parameters.AddWithValue("@AnswerHash", answerHash);

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
                // 2627 and 2601 are SQL codes for "Duplicate Entry" (Email already exists)
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
                lblErrorMessage.Text = "An unexpected error occurred: " + ex.Message;
            }
        }
    }
}