using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Music_Studio_Booking
{
	public partial class WebForm2 : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{

		}
        protected void btnBook_Click(object sender, EventArgs e)
        {
            if (Session["UserID"] == null)
            {
                Response.Redirect("Login.aspx?msg=login_required");
            }
            else
            {
                LinkButton btn = (LinkButton)sender;
                string selectedRoom = btn.CommandArgument;

                Response.Redirect("Booking.aspx");
            }
        }
    }
}