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
	public partial class WebForm4 : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{

		}
        protected void btnSendMessage_Click(object sender, EventArgs e)
        {
            string connString = ConfigurationManager.ConnectionStrings["MyStudioConnString"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connString))
            {
                string query = "INSERT INTO ContactMessages (SenderName, SenderEmail, UserMessage) VALUES (@Name, @Email, @Message)";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Name", txtName.Text.Trim());
                cmd.Parameters.AddWithValue("@Email", txtEmail.Text.Trim());
                cmd.Parameters.AddWithValue("@Message", txtMessage.Text.Trim());

                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();

                    lblStatus.Text = "Message sent successfully! We'll get back to you soon.";
                    lblStatus.ForeColor = System.Drawing.Color.Green;

                    // Clear the form
                    txtName.Text = "";
                    txtEmail.Text = "";
                    txtMessage.Text = "";
                }
                catch (Exception ex)
                {
                    lblStatus.Text = "Error: " + ex.Message;
                    lblStatus.ForeColor = System.Drawing.Color.Red;
                }
            }
        }
    }
}