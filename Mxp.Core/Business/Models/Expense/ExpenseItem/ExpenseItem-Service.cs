using System.Threading.Tasks;
using Mxp.Core.Services;
using System.Diagnostics;

namespace Mxp.Core.Business
{
    public partial class ExpenseItem
    {
        public async Task DeleteExpenseAsync () {
            this.TryDeleteValidation ();
            await ExpenseService.Instance.DeleteExpenseAsync (this);

			await this.ParentExpensesCollection.FetchAsync ();
        }

		public async Task ChangeAccountTypeAsync () {
			if (this.ParentExpense.IsFromReport && !this.ParentExpense.Report.IsNew) {
				this.AccountType = this.AccountType == ScopeTypeEnum.Company ? ScopeTypeEnum.Private : ScopeTypeEnum.Company;
				await ExpenseService.Instance.SaveExpenseAsync (this);
			} else
				await ExpenseService.Instance.ChangeAccountTypeAsync (this);

			if (!this.ParentExpense.IsFromReport)
				await this.ParentExpensesCollection?.FetchAsync ();
			else if (this.AccountType == ScopeTypeEnum.Private) {
				Expenses expenses = this.ParentExpensesCollection;

				this.ParentExpense.RemoveFromCollectionParent<Expense> ();
				this.ParentExpense.SetCollectionParent (LoggedUser.Instance.PrivateExpenses);

				if (expenses.Count == 0)
					await expenses.GetParentModel<Report> ().GetCollectionParent<Report> ().FetchAsync ();
			}

            switch (this.ParentExpensesCollection?.ExpensesType) {
                case Expenses.ExpensesTypeEnum.Business:
                    await LoggedUser.Instance.PrivateExpenses.FetchAsync ();
                    break;
                case Expenses.ExpensesTypeEnum.Private:
                    await LoggedUser.Instance.BusinessExpenses.FetchAsync ();
                    break;
            }
        }

        public void TryDeleteValidation () {
            if (this.ParentExpense == null)
                return;

            if ((this.ParentExpense.IsFromReport && !this.ParentExpense.CanRemove)
                || (!this.ParentExpense.IsFromReport && !this.ParentExpense.CanDelete))
                throw new ValidationError ("ERROR", Labels.GetLoggedUserLabel (Labels.LabelEnum.Unauthorized));
        }
    }
}