using System;
using System.Collections.Generic;
using System.Linq;

namespace Music_Studio_Booking
{
	public partial class StaffRequests : System.Web.UI.Page
	{
		private const string SessionKey = "StaffRequests_Data";

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
				var data = GetOrSeedRequests();
				Bind(data);
			}
		}

		protected void rptRequests_ItemCommand(object source, System.Web.UI.WebControls.RepeaterCommandEventArgs e)
		{
			var data = GetOrSeedRequests();
			if (!int.TryParse(e.CommandArgument.ToString(), out int id))
			{
				return;
			}

			var item = data.FirstOrDefault(x => x.Id == id);
			if (item == null)
			{
				return;
			}

			if (e.CommandName == "Accept")
			{
				item.Status = "Accepted";
			}
			else if (e.CommandName == "Decline")
			{
				item.Status = "Declined";
			}

			SaveRequests(data);
			Bind(data);
		}

		private void Bind(List<StaffRequestViewModel> data)
		{
			if (data == null || data.Count == 0)
			{
				PanelEmpty.Visible = true;
				rptRequests.Visible = false;
				return;
			}

			PanelEmpty.Visible = false;
			rptRequests.Visible = true;

			var ordered = data
				.OrderByDescending(x => x.RequestedAt)
				.ToList();

			foreach (var item in ordered)
			{
				item.StatusCss = GetStatusCss(item.Status);
			}

			rptRequests.DataSource = ordered;
			rptRequests.DataBind();
		}

		private static string GetStatusCss(string status)
		{
			if (string.Equals(status, "Accepted", StringComparison.OrdinalIgnoreCase))
			{
				return "request-accepted";
			}

			if (string.Equals(status, "Declined", StringComparison.OrdinalIgnoreCase))
			{
				return "request-declined";
			}

			return "request-pending";
		}

		private List<StaffRequestViewModel> GetOrSeedRequests()
		{
			if (Session[SessionKey] is List<StaffRequestViewModel> existing)
			{
				return existing;
			}

			var now = DateTime.Now;
			var seeded = new List<StaffRequestViewModel>
			{
				new StaffRequestViewModel
				{
					Id = 1,
					CustomerName = "Alex Rivera",
					CustomerEmail = "alex.rivera@example.com",
					CustomerPhone = "+63 900 111 2222",
					StudioName = "Studio A – Modern Recording Room",
					Date = now.Date.AddDays(1),
					StartTime = new TimeSpan(14, 0, 0),
					EndTime = new TimeSpan(16, 0, 0),
					Notes = "2-hour session for vocal tracking.",
					Status = "Pending",
					RequestedAt = now.AddMinutes(-15)
				},
				new StaffRequestViewModel
				{
					Id = 2,
					CustomerName = "Bella Santos",
					CustomerEmail = "bella.santos@example.com",
					CustomerPhone = "+63 900 333 4444",
					StudioName = "Studio B – Vocal Booth & Mixing Gear",
					Date = now.Date.AddDays(2),
					StartTime = new TimeSpan(10, 0, 0),
					EndTime = new TimeSpan(12, 0, 0),
					Notes = "Recording demo vocals and quick mix.",
					Status = "Pending",
					RequestedAt = now.AddHours(-2)
				},
				new StaffRequestViewModel
				{
					Id = 3,
					CustomerName = "The Night Shift Band",
					CustomerEmail = "band@example.com",
					CustomerPhone = "+63 900 555 6666",
					StudioName = "Studio C – Large Band Room",
					Date = now.Date.AddDays(3),
					StartTime = new TimeSpan(18, 0, 0),
					EndTime = new TimeSpan(21, 0, 0),
					Notes = "Full band rehearsal before live show.",
					Status = "Pending",
					RequestedAt = now.AddHours(-5)
				}
			};

			SaveRequests(seeded);
			return seeded;
		}

		private void SaveRequests(List<StaffRequestViewModel> data)
		{
			Session[SessionKey] = data;
		}

		[Serializable]
		public class StaffRequestViewModel
		{
			public int Id { get; set; }
			public string CustomerName { get; set; }
			public string CustomerEmail { get; set; }
			public string CustomerPhone { get; set; }
			public string StudioName { get; set; }
			public DateTime Date { get; set; }
			public TimeSpan StartTime { get; set; }
			public TimeSpan EndTime { get; set; }
			public string Notes { get; set; }
			public string Status { get; set; }
			public DateTime RequestedAt { get; set; }

			// Computed helpers for display
			public string DateDisplay
			{
				get { return Date.ToString("MMM dd, yyyy"); }
			}

			public string TimeRange
			{
				get
				{
					var startDateTime = Date.Date + StartTime;
					var endDateTime = Date.Date + EndTime;
					return string.Format("{0:hh:mm tt} – {1:hh:mm tt}", startDateTime, endDateTime);
				}
			}

			public string RequestedAtDisplay
			{
				get { return RequestedAt.ToString("MMM dd, yyyy • hh:mm tt"); }
			}

			public string StatusCss { get; set; }
		}
	}
}

