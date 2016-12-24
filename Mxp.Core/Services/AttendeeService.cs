using System;
using System.Linq;
using System.Text;
using System.Net.Http;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using RestSharp.Portable;
using Newtonsoft.Json;

using Mxp.Core.Business;
using Mxp.Core.Services.Responses;
using Mxp.Core.Utils;

namespace Mxp.Core.Services
{
	public static class AttendeeServiceExtensions {
		public static string GetRoute (this AttendeeService.ApiEnum route) {
			switch (route) {
				case AttendeeService.ApiEnum.GetItemAttendees:
					return "attendees/GetItemAttendees";
				case AttendeeService.ApiEnum.AddItemAttendee:
					return "attendees/AddItemAttendee";
				case AttendeeService.ApiEnum.RemoveItemAttendee:
					return "attendees/RemoveItemAttendee";
				case AttendeeService.ApiEnum.GetRecentAttendees:
					return "attendees/GetRecentAttendees";
				case AttendeeService.ApiEnum.SearchHCOAttendees:
					return "attendees/SearchHCOAttendees";
				case AttendeeService.ApiEnum.SearchHCPAttendees:
					return "attendees/SearchHCPAttendees";
				case AttendeeService.ApiEnum.SwitchNoShow:
					return "attendees/SwitchNoShow";
				default:
					return null;
			}
		}
	}

	public class AttendeeService : Service
	{
		public static readonly AttendeeService Instance = new AttendeeService ();

		public enum ApiEnum {
			GetItemAttendees,
			AddItemAttendee,
			RemoveItemAttendee,
			GetRecentAttendees,
			SearchHCOAttendees,
			SearchHCPAttendees,
			SwitchNoShow
		}

		private AttendeeService () : base () {

		}

		public async Task AddAttendeeAsync (Attendees attendees, Attendee attendee) {
			RestRequest request = new RestRequest(ApiEnum.AddItemAttendee.GetRoute ());

			attendee.Serialize (request, attendees);

			List<AttendeeResponse> attendeesResponse = await this.ExecuteAsync<List<AttendeeResponse>> (request);

			attendees.Populate (attendeesResponse);
		}

		public async Task RemoveAttendeeAsync (Attendees attendees, Attendee attendee) {
			RestRequest request = new RestRequest(ApiEnum.RemoveItemAttendee.GetRoute ());

			request.AddParameter ("AttendeeID", attendee.Id.ToString ());
			request.AddParameter ("ItemID", attendees.GetParentModel<ExpenseItem>().ParentExpense.Id.ToString());

			await this.ExecuteAsync (request);

			attendees.Remove (attendee);
		}

		public async Task FetchRelatedAttendeesAsync (Attendees attendees, Attendee relatedAttendee) {
			String url;
			if (relatedAttendee.Type == AttendeeTypeEnum.HCO) {
				url = ApiEnum.SearchHCOAttendees.GetRoute ();
			} else {
				url = ApiEnum.SearchHCPAttendees.GetRoute ();
			}

			RestRequest request = new RestRequest (url);

			relatedAttendee.SerializeRelatedAttendee (request);
			List<AttendeeResponse> attendeesResponses = await this.ExecuteAsync<List<AttendeeResponse>> (request);
			attendees.Populate (attendeesResponses);
			attendees.ForEach (at => {
				at.FromRelatedMode = true;
			});
		}

		// FIXME
		public async Task<List<Attendee>> FetchRecentAttendeesAsync () {
			RestRequest request = new RestRequest(ApiEnum.GetRecentAttendees.GetRoute ());
			List<AttendeeResponse> responses =  await this.ExecuteAsync<List<AttendeeResponse>> (request);

			List<Attendee> result = new List<Attendee> ();

			responses.ForEach (response => {
				result.Add(new Attendee(response));
			});

			return result;
		}

		public async Task ChangeSwitchNoShowAsync (Attendee attendee) {
			RestRequest request = new RestRequest (ApiEnum.SwitchNoShow.GetRoute ());

			request.AddParameter ("AttendeeID", attendee.Id);
			request.AddParameter ("ItemID", attendee.ItemId);

			await this.ExecuteAsync (request);
		}
	}
}