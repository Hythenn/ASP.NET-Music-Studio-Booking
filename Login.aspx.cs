using BCrypt.Net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Music_Studio_Booking
{
    public partial class WebForm5 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void btnlogin_Click(object sender, EventArgs e)
        {
            string email = loginEmail.Text.Trim();
            string passwordAttempt = loginPassword.Text;

            string connString = ConfigurationManager.ConnectionStrings["MyStudioConnString"].ConnectionString;
            string storedHash = "";
            string userName = "";
            int userId = 0;
            string userEmail = ""; 

            using (SqlConnection con = new SqlConnection(connString))
            {
                string query = "SELECT UserID, FullName, PasswordHash, Email FROM Users WHERE Email = @Email";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Email", email);

                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    userId = Convert.ToInt32(reader["UserID"]);
                    userName = reader["FullName"].ToString();
                    storedHash = reader["PasswordHash"].ToString();
                    userEmail = reader["Email"].ToString(); 
                }
            }

            if (!string.IsNullOrEmpty(storedHash) && BCrypt.Net.BCrypt.Verify(passwordAttempt, storedHash))
            {
                Session["UserID"] = userId;
                Session["UserName"] = userName;
                Session["UserEmail"] = userEmail; 

                Response.Redirect("Home.aspx");
            }
            else
                {
                    
                    lblLoginError.Text = "Invalid email or password.";
                }
            }
        }
    }
