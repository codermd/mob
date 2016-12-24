using Mxp.Core.Utils;
using Mxp.Core.Services;
using System.Threading.Tasks;
using RestSharp.Portable;
using System.Collections.ObjectModel;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace Mxp.Core.Business
{
	public class ReportExpenses : Expenses
	{
		public ReportExpenses (Report report = null) : base (ExpensesTypeEnum.Report, report) {

		}

		public ReportExpenses (IEnumerable<Expense> collection, Model model) : base (collection, model) {

		}

		public override async Task FetchAsync () {
			await ExpenseService.Instance.FetchReportExpensesAsync (this, this.GetParentModel<Report> ());
			this.ReplaceWith (this.OrderByDescending (expense => expense.Date).ToList ());
			this.ResetSectionnedExpenses ();
		}

		public void Serialize (RestRequest request) {
			string result = "";

			Collection<ExpenseItem> allItems = new Collection<ExpenseItem> ();

			this.ForEach ((expense, index) => {
				expense.ExpenseItems.ForEach (item => {
					allItems.Add (item);
				});
			});

			allItems.ForEach ((item, index) => {
				if (index < allItems.Count - 1) {
					result += item.Id.ToString () + ",";
				} else {
					result += item.Id.ToString ();
				}
			});

			request.AddParameter ("selectedItems", result);
		}

		#region iOS only

		public async Task AddSelectedExpenses (Collection<Expense> expenses) {
			if (this.GetParentModel<Report> ().IsNew)
				this.Clear ();

			expenses.ForEach (exp => {
				this.AddItem (exp);
			});

			if (!this.GetParentModel<Report> ().IsNew)
				await this.GetParentModel<Report> ().SaveAsync ();
		}

		#endregion

		public async Task RemoveReportExpenseAsync (Expense expense) {
			if (!this.CanRemove)
				return;

			this.Remove (expense);

			if (!this.GetParentModel<Report> ().IsNew)
				await ReportService.Instance.RemoveReportExpenseAsync (this.GetParentModel<Report> (), expense);
		}

		public override void TryValidate () {
			if (this.Count == 0) {
				throw new ValidationError ("ERROR", Labels.GetLoggedUserLabel (Labels.LabelEnum.MissingExpense));
			}
		}

		public bool CanRemove {
			get {
				return this.Count > 1;
			}
		}
	}
}