using System;

namespace Music_Studio_Booking
{
    public partial class Profile : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            bool isAdmin = Session["IsAdmin"] != null && Session["IsAdmin"].ToString() == "True";
            if (isAdmin)
            {
                Response.Redirect("StaffRequests.aspx");
                return;
            }

            if (Session["UserID"] == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            lblName.Text = Session["UserName"] != null ? Session["UserName"].ToString() : "—";
            lblEmail.Text = Session["UserEmail"] != null ? Session["UserEmail"].ToString() : "—";

            var userIdObj = Session["UserID"];
            lblUserId.Text = userIdObj != null ? userIdObj.ToString() : "—";
        }
    }
}

