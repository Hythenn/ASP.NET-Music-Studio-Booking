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
            ValidationSettings.UnobtrusiveValidationMode = UnobtrusiveValidationMode.None;
		}
        protected void btnSignUp_Click(object sender, EventArgs e)
        {
            //==========VALIDATE ALL REQUIRED FIELDS ARE FILLED AND PASSWORDS MATCH
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

            //==========PREPARE AND NORMALIZE USER INPUT DATA
            string name = signupName.Text.Trim();
            string email = signupEmail.Text.Trim();
            string password = signupPassword.Text;
            string securityQuestion = ddlSecurityQuestion.SelectedValue;
            string securityAnswer = signupAnswer.Text.Trim().ToLower();

            //==========HASH PASSWORD AND SECURITY ANSWER WITH BCRYPT
            //not storing the actual password — doing a BCrypt hash para secure
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(password);
            string answerHash = BCrypt.Net.BCrypt.HashPassword(securityAnswer);

            //==========INSERT NEW USER INTO DATABASE
            //getting the connection string sa Web.config — that's where the DB address is
            string connString = ConfigurationManager.ConnectionStrings["MyStudioConnString"].ConnectionString;

            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    //==========DEFINE INSERT QUERY FOR NEW USER
                    string query = "INSERT INTO Users (FullName, Email, PasswordHash, SecurityQuestion, SecurityAnswerHash) " +
                                   "VALUES (@Name, @Email, @PassHash, @Question, @AnswerHash)";

                    //==========CREATE SQL COMMAND WITH QUERY
                    SqlCommand cmd = new SqlCommand(query, con);

                    //==========BIND ALL PARAMETERS TO COMMAND
                    cmd.Parameters.AddWithValue("@Name", name);
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@PassHash", passwordHash);
                    cmd.Parameters.AddWithValue("@Question", securityQuestion);
                    cmd.Parameters.AddWithValue("@AnswerHash", answerHash);

                    //opening the connection and saving yung bagong user
                    con.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();

                    //if the insert is successful, ire-redirect to the login page
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
                //==========SQL ERROR 2627/2601 = DUPLICATE EMAIL ENTRY
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

