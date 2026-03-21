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
            string providedAnswer = txtSecurityAnswer.Text.Trim().ToLower(); //==========NORMALIZE ANSWER TO MATCH SIGNUP HASHING
            string newPassword = txtNewPassword.Text.Trim();

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(providedAnswer) || string.IsNullOrEmpty(newPassword))
            {
                lblResetStatus.Text = "Please fill in all fields.";
                lblResetStatus.ForeColor = System.Drawing.Color.Red;
                return;
            }

            //kinukuha yung connection sa database mula sa Web.config
            string connString = ConfigurationManager.ConnectionStrings["MyStudioConnString"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connString))
            {
                //==========FETCH STORED SECURITY ANSWER HASH FOR THIS EMAIL
                string fetchQuery = "SELECT SecurityAnswerHash FROM Users WHERE Email = @Email";
                SqlCommand cmd = new SqlCommand(fetchQuery, con);
                cmd.Parameters.AddWithValue("@Email", email);

                try
                {
                    con.Open();
                    //ginagamit ang ExecuteScalar kasi isang value lang ang kukunin mula sa database
                    object result = cmd.ExecuteScalar();

                    if (result != null)
                    {
                        string storedAnswerHash = result.ToString();

                        //==========VERIFY SECURITY ANSWER WITH BCRYPT
                        //tinitingnan ng BCrypt kung tama yung sagot — kumpara sa hash na nakalagay sa database
                        if (BCrypt.Net.BCrypt.Verify(providedAnswer, storedAnswerHash))
                        {
                            //==========ANSWER VERIFIED - HASH THE NEW PASSWORD
                            //ginagawang hash muna yung bagong password bago i-save sa database
                            string newPasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);

                            //==========UPDATE DB WITH NEW PASSWORD HASH
                            string updateQuery = "UPDATE Users SET PasswordHash = @NewHash WHERE Email = @Email";
                            SqlCommand updateCmd = new SqlCommand(updateQuery, con);
                            updateCmd.Parameters.AddWithValue("@NewHash", newPasswordHash);
                            updateCmd.Parameters.AddWithValue("@Email", email);

                            //ini-update na yung password sa database — ExecuteNonQuery kasi walang data na ibabalik
                            updateCmd.ExecuteNonQuery();

                            lblResetStatus.Text = "Password updated! You can now login.";
                            lblResetStatus.ForeColor = System.Drawing.Color.Green;

                            //==========CLEAR FIELDS FOR SECURITY AFTER RESET
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

