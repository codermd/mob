using System;
using System.Threading.Tasks;
using RestSharp.Portable;
using Mxp.Core.Business;
using System.Collections.Generic;
using Mxp.Core.Services.Responses;
using Mxp.Core.Utils;
using System.Linq;

namespace Mxp.Core.Services
{
	public static class SpendCatcherServiceExtensions {
		public static string GetRoute(this SpendCatcherService.ApiEnum route) {
			switch (route) {
				case SpendCatcherService.ApiEnum.Send:
					return "SpendCatcher/InitiateSpendCatcher";
				case SpendCatcherService.ApiEnum.Fetch:
					return "SpendCatcher/SpendCatcherStatus";
				default:
					return null;
			}
		}
	}

	public class SpendCatcherService : Service
	{
		public static readonly SpendCatcherService Instance = new SpendCatcherService ();

		public enum ApiEnum {
			Send,
			Fetch
		}

		private SpendCatcherService () : base () {

		}

		public async Task AddExpenseAsync (SpendCatcherExpense spendCatcherExpense) {
			RestRequest request = new RestRequest (ApiEnum.Send.GetRoute ());

			request.AddParameter ("ObjectType", "tempTrx");
			request.AddParameter ("fileType", "image/jpeg");
			request.AddParameter ("ImageData", spendCatcherExpense.Base64Image);
			request.AddParameter ("ImageName", "mobileUpload.jpg");

			if (spendCatcherExpense.Country != null)
				request.AddParameter ("CountryID", spendCatcherExpense.Country.Id);

			if (spendCatcherExpense.Product != null)
				request.AddParameter ("ProductID", spendCatcherExpense.Product.Id);

			request.AddParameter ("IsPaidByCC", spendCatcherExpense.IsPaidByCC);

			await this.ExecuteAsync (request);
		}

		public async Task FetchExpensesAsync (SpendCatcherExpenses expenses) {
			RestRequest request = new RestRequest (ApiEnum.Fetch.GetRoute ());
			List<SpendCatcherResponse> expensesResponses = await this.ExecuteAsync<List<SpendCatcherResponse>> (request);
			expenses.Populate (expensesResponses);
			expenses.ReplaceWith (expenses.OrderByDescending (expense => expense.Date).ToList ());
		}
	}
}
