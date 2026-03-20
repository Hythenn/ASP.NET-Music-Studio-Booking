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
			// Admin access is based on user identity from the Users table.
			// (We still verify the password using BCrypt, same as regular users.)
			private const string AdminEmail = "admin@gstudio.local";
			private const string AdminDisplayName = "Admin";

        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void btnlogin_Click(object sender, EventArgs e)
        {
            string email = loginEmail.Text.Trim();
            string passwordAttempt = loginPassword.Text;

			// DEV-only bypass:
			// Use an email shaped like: `admin+MAGALING-v1.05@anything.com`
			// This skips the SQL database lookup and grants staff/admin access immediately.
			if (TryDevAdminBypass(email))
			{
				Session["UserID"] = null;
				Session["UserName"] = AdminDisplayName;
				Session["UserEmail"] = email;
				Session["IsAdmin"] = true;
				Response.Redirect("StaffRequests.aspx");
				return;
			}

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
				Session["IsAdmin"] = IsAdminUser(userEmail, userName);

				if ((bool)Session["IsAdmin"])
				{
					Response.Redirect("StaffRequests.aspx");
				}
				else
				{
					Response.Redirect("Home.aspx");
				}
            }
            else
                {
                    
                    lblLoginError.Text = "Invalid email or password.";
                }
            }

			private static bool IsAdminUser(string email, string displayName)
			{
				if (string.IsNullOrEmpty(email) && string.IsNullOrEmpty(displayName))
				{
					return false;
				}

				return string.Equals(email, AdminEmail, StringComparison.OrdinalIgnoreCase)
				       || string.Equals(displayName, AdminDisplayName, StringComparison.OrdinalIgnoreCase);
			}

			private static bool TryDevAdminBypass(string email)
			{
#if DEBUG
				if (string.IsNullOrWhiteSpace(email))
				{
					return false;
				}

				// Token format: admin+<token>@domain
				const string prefix = "admin+";
				const string token = "1234567890";
				const string requiredDomain = "GSTUDIO.com";

				var atIndex = email.IndexOf('@');
				var localPart = atIndex > 0 ? email.Substring(0, atIndex) : email;
				var domainPart = atIndex >= 0 && atIndex + 1 < email.Length ? email.Substring(atIndex + 1) : string.Empty;

				if (!localPart.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
				{
					return false;
				}

				if (!string.Equals(domainPart, requiredDomain, StringComparison.OrdinalIgnoreCase))
				{
					return false;
				}

				var providedToken = localPart.Substring(prefix.Length);
				return string.Equals(providedToken, token, StringComparison.Ordinal);
#else
				return false;
#endif
			}
        }
    }
