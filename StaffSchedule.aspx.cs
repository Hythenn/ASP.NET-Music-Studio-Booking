using System;
using System.Collections.Generic;
using System.Linq;

namespace Music_Studio_Booking
{
	public partial class StaffSchedule : System.Web.UI.Page
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
				var requests = GetRequestsFromSession();

				// Only show accepted bookings in the schedule
				var accepted = requests
					.Where(r => string.Equals(r.Status, "Accepted", StringComparison.OrdinalIgnoreCase))
					.ToList();

				if (accepted.Count == 0)
				{
					PanelEmptySchedule.Visible = true;
					rptSchedule.Visible = false;
					return;
				}

				PanelEmptySchedule.Visible = false;
				rptSchedule.Visible = true;

				var grouped = accepted
					.GroupBy(r => r.Date.Date)
					.OrderBy(g => g.Key)
					.Select(g => new ScheduleDayViewModel
					{
						Date = g.Key,
						Bookings = g
							.OrderBy(b => b.StartTime)
							.Select(b => new ScheduleBookingViewModel
							{
								CustomerName = b.CustomerName,
								StudioName = b.StudioName,
								Notes = b.Notes,
								TimeRange = b.TimeRange
							})
							.ToList()
					})
					.ToList();

				rptSchedule.DataSource = grouped;
				rptSchedule.DataBind();
			}
		}

		protected void rptSchedule_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
		{
			if (e.Item.ItemType != System.Web.UI.WebControls.ListItemType.Item &&
			    e.Item.ItemType != System.Web.UI.WebControls.ListItemType.AlternatingItem)
			{
				return;
			}

			var day = e.Item.DataItem as ScheduleDayViewModel;
			if (day == null || day.Bookings == null)
			{
				return;
			}

			var inner = (System.Web.UI.WebControls.Repeater)e.Item.FindControl("rptBookings");
			if (inner == null)
			{
				return;
			}

			inner.DataSource = day.Bookings;
			inner.DataBind();
		}

		private List<StaffRequests.StaffRequestViewModel> GetRequestsFromSession()
		{
			// Reuse the same session data seeded by StaffRequests page if available.
			var sessionKey = "StaffRequests_Data";

			if (Session[sessionKey] is List<StaffRequests.StaffRequestViewModel> existing)
			{
				return existing;
			}

			return new List<StaffRequests.StaffRequestViewModel>();
		}

		private class ScheduleDayViewModel
		{
			public DateTime Date { get; set; }
			public List<ScheduleBookingViewModel> Bookings { get; set; }

			public string DateDisplay
			{
				get { return Date.ToString("MMM dd, yyyy (dddd)"); }
			}

			public string CountLabel
			{
				get
				{
					int count = Bookings != null ? Bookings.Count : 0;
					return count == 1 ? "1 booking" : count + " bookings";
				}
			}
		}

		private class ScheduleBookingViewModel
		{
			public string CustomerName { get; set; }
			public string StudioName { get; set; }
			public string Notes { get; set; }
			public string TimeRange { get; set; }
		}
	}
}

