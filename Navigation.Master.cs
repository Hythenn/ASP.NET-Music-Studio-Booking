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
			var isAdmin = Session["IsAdmin"] != null && Session["IsAdmin"].ToString() == "True";

			// Simple toggle: only one nav visible at a time
			UserNav.Visible = !isAdmin;
			AdminNav.Visible = isAdmin;
		}
	}
}