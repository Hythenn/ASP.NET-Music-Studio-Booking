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

            //kunukuha yung connection string sa Web.config para makakonekta sa database
            string connString = ConfigurationManager.ConnectionStrings["MyStudioConnString"].ConnectionString;
            string storedHash = "";
            string userName = "";
            int userId = 0;
            string userEmail = "";

            //binubuksan yung connection sa database, at awtomatiko itong sasarahin pagkatapos ng using block
            using (SqlConnection con = new SqlConnection(connString))
            {
                string query = "SELECT UserID, FullName, PasswordHash, Email FROM Users WHERE Email = @Email";
                SqlCommand cmd = new SqlCommand(query, con);
                //ginagamit ang parameter para hindi makapasok ang SQL injection
                cmd.Parameters.AddWithValue("@Email", email);

                con.Open();
                //dito nababasa ang resulta ng query row by row
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    userId = Convert.ToInt32(reader["UserID"]);
                    userName = reader["FullName"].ToString();
                    storedHash = reader["PasswordHash"].ToString();
                    userEmail = reader["Email"].ToString();
                }
            }

            //tinitingnan ng BCrypt kung tama yung password na ni-type — hindi ito plain text comparison kundi hash comparison
            if (!string.IsNullOrEmpty(storedHash) && BCrypt.Net.BCrypt.Verify(passwordAttempt, storedHash))
            {
                //iniimbak sa Session yung info ng naka-login na user para magamit sa ibang pages
                Session["UserID"] = userId;
                Session["UserName"] = userName;
                Session["UserEmail"] = userEmail;
                Session["IsAdmin"] = IsAdminUser(userEmail, userName);

                //depende kung admin o regular user, dini-direct sila sa tamang page
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


