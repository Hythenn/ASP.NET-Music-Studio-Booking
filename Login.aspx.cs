using BCrypt.Net;
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI;

namespace Music_Studio_Booking
{
    public partial class WebForm5 : System.Web.UI.Page
    {
        //==========ADMIN IDENTITY CONSTANTS
        private const string AdminEmail = "admin@gstudio.local";
        private const string AdminDisplayName = "Admin";

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnlogin_Click(object sender, EventArgs e)
        {
            string email = loginEmail.Text.Trim();
            string passwordAttempt = loginPassword.Text;

            //==========BYPASS DATABASE FOR ADMIN LOGIN
            if (email.ToLower() == "admin+1234567890@gstudio.com")
            {
                Session["UserID"] = 0;
                Session["UserName"] = "Admin";
                Session["UserEmail"] = email;
                Session["IsAdmin"] = true;
                Response.Redirect("StaffRequests.aspx");
                return;
            }

            //getting the connection string from Web.config to connect to the database
            string connString = ConfigurationManager.ConnectionStrings["MyStudioConnString"].ConnectionString;
            string storedHash = "";
            string userName = "";
            int userId = 0;
            string userEmail = "";

            //opening the database connection, it will automatically close pagtapos ng using block
            using (SqlConnection con = new SqlConnection(connString))
            {
                string query = "SELECT UserID, FullName, PasswordHash, Email FROM Users WHERE Email = @Email";
                SqlCommand cmd = new SqlCommand(query, con);
                //using parameters here para safe from SQL injection
                cmd.Parameters.AddWithValue("@Email", email);

                con.Open();
                //reading the query results dito row by row
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    userId = Convert.ToInt32(reader["UserID"]);
                    userName = reader["FullName"].ToString();
                    storedHash = reader["PasswordHash"].ToString();
                    userEmail = reader["Email"].ToString();
                }
            }

            //BCrypt verifies kung tama yung password — this compares the hash, not the plain text
            if (!string.IsNullOrEmpty(storedHash) && BCrypt.Net.BCrypt.Verify(passwordAttempt, storedHash))
            {
                //storing the logged-in user info sa Session to use across other pages
                Session["UserID"] = userId;
                Session["UserName"] = userName;
                Session["UserEmail"] = userEmail;
                Session["IsAdmin"] = IsAdminUser(userEmail, userName);

                //redirecting them to diff pages depending on their admin status
                if ((bool)Session["IsAdmin"])
                {
                    Response.Redirect("StaffRequests.aspx");
                }
                else
                {
                    Response.Redirect("Profile.aspx");
                }
            }
            else
            {
                lblLoginError.Text = "Invalid email or password.";
            }
        }

        //==========CHECK IF LOGGED IN USER IS ADMIN BY EMAIL OR NAME
        private static bool IsAdminUser(string email, string displayName)
        {
            if (email.ToLower() == AdminEmail || displayName == AdminDisplayName)
                return true;
            return false;
        }
    }
}


