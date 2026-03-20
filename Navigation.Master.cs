using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Music_Studio_Booking
{
	public partial class Navigation : System.Web.UI.MasterPage
	{
        protected void Page_Load(object sender, EventArgs e)
        {
            bool isAdmin = Session["IsAdmin"] != null && Session["IsAdmin"].ToString() == "True";
            bool isUser = Session["UserID"] != null;

            phGuest.Visible = !isAdmin && !isUser;
            phUser.Visible = !isAdmin && isUser;
            phAdmin.Visible = isAdmin;
            phPublicLinks.Visible = !isAdmin;
        }

    }
}