using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI;
using BCrypt.Net;

namespace Music_Studio_Booking
{
    public partial class WebForm8 : System.Web.UI.Page
    {
        protected void btnReset_Click(object sender, EventArgs e)
        {
            string email = txtResetEmail.Text.Trim();
            string providedAnswer = txtSecurityAnswer.Text.Trim().ToLower(); // Matches the normalization in Signup
            string newPassword = txtNewPassword.Text.Trim();

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(providedAnswer) || string.IsNullOrEmpty(newPassword))
            {
                lblResetStatus.Text = "Please fill in all fields.";
                lblResetStatus.ForeColor = System.Drawing.Color.Red;
                return;
            }

            string connString = ConfigurationManager.ConnectionStrings["MyStudioConnString"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connString))
            {
                // 1. Fetch the stored SecurityAnswerHash for this email
                string fetchQuery = "SELECT SecurityAnswerHash FROM Users WHERE Email = @Email";
                SqlCommand cmd = new SqlCommand(fetchQuery, con);
                cmd.Parameters.AddWithValue("@Email", email);

                try
                {
                    con.Open();
                    object result = cmd.ExecuteScalar();

                    if (result != null)
                    {
                        string storedAnswerHash = result.ToString();

                        // 2. Verify the security answer using BCrypt
                        if (BCrypt.Net.BCrypt.Verify(providedAnswer, storedAnswerHash))
                        {
                            // 3. User is verified! Hash the NEW password
                            string newPasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);

                            // 4. Update the database with the new hash
                            string updateQuery = "UPDATE Users SET PasswordHash = @NewHash WHERE Email = @Email";
                            SqlCommand updateCmd = new SqlCommand(updateQuery, con);
                            updateCmd.Parameters.AddWithValue("@NewHash", newPasswordHash);
                            updateCmd.Parameters.AddWithValue("@Email", email);

                            updateCmd.ExecuteNonQuery();

                            lblResetStatus.Text = "Password updated! You can now login.";
                            lblResetStatus.ForeColor = System.Drawing.Color.Green;

                            // Clear fields for safety
                            txtResetEmail.Text = "";
                            txtSecurityAnswer.Text = "";
                            txtNewPassword.Text = "";
                        }
                        else
                        {
                            lblResetStatus.Text = "Incorrect security answer.";
                            lblResetStatus.ForeColor = System.Drawing.Color.Red;
                        }
                    }
                    else
                    {
                        lblResetStatus.Text = "Email not found.";
                        lblResetStatus.ForeColor = System.Drawing.Color.Red;
                    }
                }
                catch (Exception ex)
                {
                    lblResetStatus.Text = "Error: " + ex.Message;
                    lblResetStatus.ForeColor = System.Drawing.Color.Red;
                }
            }
        }
    }
}