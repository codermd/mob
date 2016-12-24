using System;
using System.Threading.Tasks;

using RestSharp.Portable;

using Mxp.Core.Services.Responses;
using Mxp.Core.Business;
using System.Collections.Generic;

namespace Mxp.Core.Services
{
	public static class AllowanceServiceExtensions {
		public static string GetRoute (this AllowanceService.ApiEnum route) {
			switch (route) {
				case AllowanceService.ApiEnum.CreateAllowance:
					return "APIAllowance/CreateJourneyAllowance";
				case AllowanceService.ApiEnum.CalculateAllowance:
					return "APIAllowance/CalculateJourneyAllowance";
				case AllowanceService.ApiEnum.AddAllowance:
					return "APIAllowance/AddJourneyAllowance";
				case AllowanceService.ApiEnum.GetAllowance:
					return "APIAllowance/GetJourneyAllowance";
				case AllowanceService.ApiEnum.DeleteAllowance:
					return "APIAllowance/DeleteJourneyAllowance";
				default:
					return null;
			}
		}
	}

	public class AllowanceService : Service
	{
		public static readonly AllowanceService Instance = new AllowanceService ();

		public enum ApiEnum {
			CreateAllowance,
			CalculateAllowance,
			AddAllowance,
			GetAllowance,
			DeleteAllowance
		}

		private AllowanceService () : base () {

		}

		public async Task CalculateAllowanceAsync (Allowance allowance) {
			RestRequest request = new RestRequest(ApiEnum.CalculateAllowance.GetRoute ());

			// FIXME
			Dictionary<string, object> dict = new Dictionary<string, object> ();
			allowance.AddJourneyAllowanceSerialization (dict);

			AllowanceResponse allowanceResponse = await this.ExecuteAsync<AllowanceResponse> (request, dict);

			allowance.Populate (allowanceResponse, true);
		}

		public async Task FetchAllowance (Allowance allowance) {
			RestRequest request = new RestRequest(ApiEnum.GetAllowance.GetRoute ());

			request.AddParameter ("itemID", allowance.Id);
			AllowanceResponse allowanceResponse = await this.ExecuteAsync<AllowanceResponse> (request);

			allowance.Populate (allowanceResponse);
		}
			
		public async Task PartiallyCreateAllowanceAsync (Allowance allowance) {
			RestRequest request = new RestRequest (ApiEnum.CreateAllowance.GetRoute ());

			allowance.CreateJourneyAllowanceSerialization (request);
			this.PrintParameters (request);
			AllowanceResponse response = await this.ExecuteAsync<AllowanceResponse> (request);

			allowance.Populate (response);
		}

		public async Task AddAllowanceAsync (Allowance allowance) {
			RestRequest request = new RestRequest (ApiEnum.AddAllowance.GetRoute ());

			// FIXME
			Dictionary<string, object> dict = new Dictionary<string, object> ();
			allowance.AddJourneyAllowanceSerialization (dict);

			await this.ExecuteAsync<AllowanceResponse> (request, dict);

			// Response is inconsistent.
			// allowance.Populate (response);
		}
	}
}