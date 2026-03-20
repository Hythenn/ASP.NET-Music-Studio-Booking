using System;

namespace Music_Studio_Booking
{
	public partial class Logout : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			// Abandon first, then clear remaining session state.
			Session.Abandon();
			Session.Clear();
			Response.Redirect("Home.aspx");
		}
	}
}

