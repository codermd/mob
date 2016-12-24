using System.Linq;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections.Generic;
using Mxp.Core.Services.Responses;
using RestSharp.Portable;
using Mxp.Core.Business;

namespace Mxp.Core.Services
{
	public static class ExpenseServiceExtensions
	{
		public static string GetRoute (this ExpenseService.ApiEnum route) {
			switch (route) {
				case ExpenseService.ApiEnum.GetBusinessExpenses:
					return "APIEWF/getWaitingExpenses";
				case ExpenseService.ApiEnum.GetPrivateExpenses:
					return "APIEWF/getPrivateExpenses";
				case ExpenseService.ApiEnum.GetReportExpenses:
					return "APIEWF/getReportExpenses";
				case ExpenseService.ApiEnum.AddExpense:
					return "APIEWF/addExpense";
				case ExpenseService.ApiEnum.EditExpense:
					return "APIEWF/EditExpense";
				case ExpenseService.ApiEnum.DeleteExpense:
					return "APIEWF/deleteExpense";
				case ExpenseService.ApiEnum.Split:
					return "APIEWF/SplitExpense";
				case ExpenseService.ApiEnum.Unsplit:
					return "APIEWF/UnsplitExpense";
				case ExpenseService.ApiEnum.SetAccountBusiness:
					return "APIEWF/setItemComp";
				case ExpenseService.ApiEnum.SetAccountPrivate:
					return "APIEWF/setItemPriv";
				default:
					return null;
			}
		}
	}

	public class ExpenseService : Service
	{
		public static readonly ExpenseService Instance = new ExpenseService ();

		public enum ApiEnum {
			GetBusinessExpenses,
			GetPrivateExpenses,
			GetReportExpenses,
			AddExpense,
			EditExpense,
			DeleteExpense,
			Split,
			Unsplit,
			FetchSpendCather,
			SetAccountPrivate,
			SetAccountBusiness
		}

		private ExpenseService () : base () {

		}

		public async Task FetchExpensesAsync (Expenses expenses) {
			ApiEnum route;

			switch (expenses.ExpensesType) {
				case Expenses.ExpensesTypeEnum.Business:
					route = ApiEnum.GetBusinessExpenses;
					break;
				case Expenses.ExpensesTypeEnum.Private:
					route = ApiEnum.GetPrivateExpenses;
					break;
				default:
					return;
			}

			RestRequest request = new RestRequest (route.GetRoute ());

			List<ExpenseResponse> expenseResponses = await this.ExecuteAsync<List<ExpenseResponse>> (request);

			Debug.WriteLine ("{0} expenseResponses", expenseResponses.Count);

			expenses.Populate (expenseResponses);
		}

		public async Task FetchReportExpensesAsync (Expenses expenses, Report report) {
			RestRequest request = new RestRequest (ApiEnum.GetReportExpenses.GetRoute ());

			request.AddParameter ("ReportID", report.Id.ToString ());

			List<ExpenseResponse> expenseResponses = await this.ExecuteAsync<List<ExpenseResponse>> (request);

			Debug.WriteLine ("{0} expenseResponses", expenseResponses.Count);

			expenses.Populate (expenseResponses);
		}

		public async Task UnsplitExpenseAsync (ExpenseItem expenseItem) {
			RestRequest request = new RestRequest (ApiEnum.Unsplit.GetRoute ());
			request.AddParameter ("itemId", expenseItem.Id.ToString ());
			await this.ExecuteAsync (request);
		}

		public async Task SplitExpenseAsync (ExpenseItem expenseItem) {
			RestRequest request = new RestRequest (ApiEnum.Split.GetRoute ());
			await this.ExecuteAsync<ExpenseItemResponse> (request, expenseItem.SplitSerialize ());
		}

		public async Task ChangeAccountTypeAsync (ExpenseItem expenseItem) {
			ApiEnum route;

			switch (expenseItem.AccountType) {
				case ExpenseItem.ScopeTypeEnum.Company:
					route = ApiEnum.SetAccountBusiness;
					break;
				case ExpenseItem.ScopeTypeEnum.Private:
					route = ApiEnum.SetAccountPrivate;
					break;
				default:
					return;
			}

			RestRequest request = new RestRequest (route.GetRoute ());

			request.AddParameter ("itemId", expenseItem.Id.ToString ());

			await this.ExecuteAsync (request);

			expenseItem.AccountType = expenseItem.AccountType == ExpenseItem.ScopeTypeEnum.Company ? ExpenseItem.ScopeTypeEnum.Private : ExpenseItem.ScopeTypeEnum.Company;
		}

		public async Task SaveExpenseAsync (ExpenseItem expenseItem) {
			RestRequest request;

			if (expenseItem.ParentExpense != null && expenseItem.ParentExpense.IsNew)
				request = new RestRequest (ApiEnum.AddExpense.GetRoute ());
			else
				request = new RestRequest (ApiEnum.EditExpense.GetRoute ());

			expenseItem.ParentExpense.EditCreateSerialize (request, expenseItem);

			expenseItem.ParentExpense.Populate (await this.ExecuteAsync<ExpenseResponse> (request));
		}

		public async Task DeleteExpenseAsync (ExpenseItem expenseItem) {
			RestRequest request = new RestRequest (ApiEnum.DeleteExpense.GetRoute ());
			request.AddParameter ("ItemID", expenseItem.Id.ToString ());
			await this.ExecuteAsync (request);
		}
	}
}