using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace Music_Studio_Booking
{
    public partial class StaffMessages : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var isAdmin = Session["IsAdmin"] != null && Session["IsAdmin"].ToString() == "True";
            if (!isAdmin)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                LoadMessages();
            }
        }

        private void LoadMessages()
        {
            var messages = new List<MessageViewModel>();
            string connString = ConfigurationManager.ConnectionStrings["MyStudioConnString"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connString))
            {
                con.Open();
                string sql = @"SELECT MessageID, SenderName, SenderEmail, UserMessage, SubmittedAt
                               FROM ContactMessages
                               ORDER BY SubmittedAt DESC";

                SqlCommand cmd = new SqlCommand(sql, con);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    messages.Add(new MessageViewModel
                    {
                        MessageID   = reader.GetInt32(reader.GetOrdinal("MessageID")),
                        SenderName  = reader["SenderName"].ToString(),
                        SenderEmail = reader["SenderEmail"].ToString(),
                        UserMessage = reader["UserMessage"].ToString(),
                        SubmittedAt = Convert.ToDateTime(reader["SubmittedAt"])
                    });
                }
            }

            if (messages.Count == 0)
            {
                PanelEmpty.Visible = true;
                rptMessages.Visible = false;
                return;
            }

            PanelEmpty.Visible = false;
            rptMessages.Visible = true;
            rptMessages.DataSource = messages;
            rptMessages.DataBind();
        }

        public class MessageViewModel
        {
            public int      MessageID   { get; set; }
            public string   SenderName  { get; set; }
            public string   SenderEmail { get; set; }
            public string   UserMessage { get; set; }
            public DateTime SubmittedAt { get; set; }

            public string SubmittedAtDisplay => SubmittedAt.ToString("MMM dd, yyyy • hh:mm tt");

            //==========TRUNCATE MESSAGE TO 80 CHARS FOR CARD PREVIEW
            public string Preview
            {
                get
                {
                    if (UserMessage != null && UserMessage.Length > 80)
                        return UserMessage.Substring(0, 80) + "...";
                    return UserMessage;
                }
            }
        }
    }
}


