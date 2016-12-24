using System;
using System.Linq;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections.Generic;
using RestSharp.Portable;

using Mxp.Core.Business;
using Mxp.Core.Services.Responses;
using Mxp.Core.Utils;
using System.Threading;

namespace Mxp.Core.Services
{
	public static class LookupServiceExtensions {
		public static string GetRoute (this LookupService.ApiEnum route) {
			switch (route) {
				case LookupService.ApiEnum.GetLookupItems:
					return "APIField/GetLookupItems";
				case LookupService.ApiEnum.GetLookupVehicle:
					return "APIField/Vehicle";
				case LookupService.ApiEnum.GetLookupProject:
					return "APIField/GetLookupProject";
				case LookupService.ApiEnum.GetLookupDepartment:
					return "APIField/GetLookupDepartment";
				case LookupService.ApiEnum.GetLookupTravelRequests:
					return "APIField/GetLookupTravelRequests";
				case LookupService.ApiEnum.GetLookUpEmployee:
					return "APIField/GetLookUpEmployee";
				default:
					return null;
			}
		}
	}

	public class LookupService : Service
	{
		public static readonly LookupService Instance = new LookupService ();

		public enum ApiEnum {
			GetLookupItems,
			GetLookupVehicle,
			GetLookupProject,
			GetLookupDepartment,
			GetLookupTravelRequests,
			GetLookUpEmployee
		}

		private LookupService () : base () {

		}
			
		public async Task FetchLookUp (ApiEnum value, LookupItems lookupItems, String searchText) {
			RestRequest request = new RestRequest(value.GetRoute ());

			request.AddParameter ("searchString", searchText);

			List<LookupItemResponse> items = await this.ExecuteAsync<List<LookupItemResponse>> (request);

			lookupItems.Populate (items);
		}

		public async Task FetchLookUp (LookupField lookupField, String searchText, CancellationToken token) {
			RestRequest request = new RestRequest (lookupField.LookupKey.GetRoute ());

			if (lookupField is DynamicLookupField
				&& ((DynamicLookupField)lookupField).DynamicFieldHolder.ComboTypeId != 0)
				request.AddParameter ("comboTypeId", ((DynamicLookupField)lookupField).DynamicFieldHolder.ComboTypeId.ToString ());
			else if (lookupField.Model is ExpenseItem) {
				ExpenseItem expenseItem = lookupField.GetModel<ExpenseItem> ();
				request.AddParameter ("countryID", expenseItem.ParentExpense.Country != null ? expenseItem.ParentExpense.Country.Id : (object)null);
				request.AddParameter ("productID", expenseItem.Product != null ? expenseItem.Product.Id : (object)null);
			}

			request.AddParameter ("searchString", searchText);
			request.AddParameter ("mandatory", lookupField.Permission == FieldPermissionEnum.Mandatory);
			
			List<LookupItemResponse> lookupItemResponses = await this.ExecuteAsync<List<LookupItemResponse>> (request, TIMEOUT_IN_SECONDS, token);

			lookupField.Results.Populate (lookupItemResponses);
		}

		public async Task<LookupItem> FetchLookUp (LookupField lookupField) {
			RestRequest request = new RestRequest(lookupField.LookupKey.GetRoute ());

			request.AddParameter ("lookupItemId", lookupField.Value);
			request.AddParameter ("mandatory", lookupField.Permission == FieldPermissionEnum.Mandatory);

			if (lookupField is DynamicLookupField)
				request.AddParameter("comboTypeId", ((DynamicLookupField)lookupField).DynamicFieldHolder.ComboTypeId);

			List<LookupItemResponse> lookupItemResponse = await this.ExecuteAsync<List<LookupItemResponse>> (request);

			return lookupItemResponse == null ? null : new LookupItem (lookupItemResponse [0]);
		}

		// FIXME Kind of useless method
		public async Task<List<Attendee>> FetchEmployee(String searchText) {
			RestRequest request = new RestRequest (ApiEnum.GetLookUpEmployee.GetRoute ());

			request.AddParameter ("searchString", searchText);

			List<LookupItemResponse> responses = await this.ExecuteAsync<List<LookupItemResponse>> (request);
			List<Attendee> result = new List<Attendee> ();

			responses.ForEach (response => {
				result.Add (new Attendee (response));
			});

			return result;
		}
	}
}