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
			// Simple hard-coded admin credentials for now.
			// In a real system you would store this in the database or a config/roles table.
			private const string AdminEmail = "admin@gstudio.local";
			private const string AdminPassword = "Admin123!";

        protected void Page_Load(object sender, EventArgs e)
        {

        }
			protected void btnlogin_Click(object sender, EventArgs e)
			{
				string email = loginEmail.Text.Trim();
				string passwordAttempt = loginPassword.Text;

				// First, check if this is the special admin login.
				if (string.Equals(email, AdminEmail, StringComparison.OrdinalIgnoreCase) &&
				    passwordAttempt == AdminPassword)
				{
					Session["IsAdmin"] = true;
					Session["UserID"] = null;
					Session["UserName"] = "Admin";
					Response.Redirect("StaffRequests.aspx");
					return;
				}

				// Normal user login flow (non-admin)
				string connString = ConfigurationManager.ConnectionStrings["MyStudioConnString"].ConnectionString;
				string storedHash = "";
				string userName = "";
				int userId = 0;

				using (SqlConnection con = new SqlConnection(connString))
				{
					// We only look for the record with the matching email
					string query = "SELECT UserID, FullName, PasswordHash FROM Users WHERE Email = @Email";
					SqlCommand cmd = new SqlCommand(query, con);
					cmd.Parameters.AddWithValue("@Email", email);

					con.Open();
					SqlDataReader reader = cmd.ExecuteReader();

					if (reader.Read())
					{
						// We found a user with that email
						userId = Convert.ToInt32(reader["UserID"]);
						userName = reader["FullName"].ToString();
						storedHash = reader["PasswordHash"].ToString();
					}
					con.Close();
					if (!string.IsNullOrEmpty(storedHash) && BCrypt.Net.BCrypt.Verify(passwordAttempt, storedHash))
					{
						// SUCCESS!
						// 1. Create a Session to remember the user
						Session["UserID"] = userId;
						Session["UserName"] = userName;
						Session["IsAdmin"] = false;

						// 2. Send them to their dashboard or studio schedule
						Response.Redirect("Home.aspx");
					}
					else
					{
						// FAIL!
						lblLoginError.Text = "Invalid email or password.";
					}
				}
			}
    }
}