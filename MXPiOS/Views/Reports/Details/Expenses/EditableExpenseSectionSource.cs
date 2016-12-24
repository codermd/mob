using System;
using UIKit;
using Mxp.Core.Business;
using Foundation;
using Mxp.Core.Utils;

namespace Mxp.iOS
{
	public class EditableExpenseSectionSource : ExpensesTableSource
	{
		public event EventHandler AddExpenseEvent = delegate {};
		private Report report;

		public EditableExpenseSectionSource (Report report): base (report.Expenses) {
			this.report = report;
		}

		public override nint NumberOfSections (UITableView tableView) {
			return base.NumberOfSections (tableView) + (this.report.CanAddExpenses ? 1 : 0);
		}

		public override string TitleForHeader (UITableView tableView, nint section) {
			if (this.report.CanAddExpenses && section == 0)
				return "";
			
			return base.TitleForHeader (tableView, section - (this.report.CanAddExpenses ? 1 : 0));
		}

		public override nint RowsInSection (UITableView tableview, nint section) {
			if (this.report.CanAddExpenses && section == 0)
				return 1;
			
			return base.RowsInSection (tableview, section - (this.report.CanAddExpenses ? 1 : 0));
		}

		public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath) {
			if (this.report.CanAddExpenses && indexPath.Section == 0) {
				AddExpenseCell cell = (AddExpenseCell)tableView.DequeueReusableCell (AddExpenseCell.Key);
				if (cell == null)
					cell = AddExpenseCell.Create ();
				return cell;
			}

			return base.GetCell (tableView, indexPath);
		}
			
		public override void RowSelected (UITableView tableView, NSIndexPath indexPath) {
			if (this.report.CanAddExpenses && indexPath.Section == 0) {
				this.AddExpenseEvent (this, new EventArgs ());
				return;
			}
				
			Model model = this.GetModelAtIndexPath (indexPath);
			if(model is ExpenseItem || (model is Expense && !((Expense)model).IsSplit))
				base.RowSelected (tableView, indexPath);
		}

		public override UITableViewCellEditingStyle EditingStyleForRow (UITableView tableView, NSIndexPath indexPath) {
			if (this.report.CanAddExpenses && indexPath.Section == 0)
				return UITableViewCellEditingStyle.None;

			return this.GetModelAtIndexPath (indexPath) is Expense && ((Expense)this.GetModelAtIndexPath (indexPath)).CanRemove
				? UITableViewCellEditingStyle.Delete
					: UITableViewCellEditingStyle.None;
		}

		public override bool CanEditRow (UITableView tableView, NSIndexPath indexPath) {
			if (this.report.CanAddExpenses && indexPath.Section == 0)
				return false;

			Model model = this.GetModelAtIndexPath (indexPath);
			if (model is Expense)
				return ((Expense)model).CanRemove;

			return true;
		}

		public override Model GetModelAtIndexPath (NSIndexPath indexPath) {
			if (this.report.CanAddExpenses && indexPath.Section == 0)
				return null;
			
			return base.GetModelAtIndexPath (NSIndexPath.FromRowSection (indexPath.Row, indexPath.Section - (this.report.CanAddExpenses ? 1 : 0)));
		}

		public override async void CommitEditingStyle (UITableView tableView, UITableViewCellEditingStyle editingStyle, NSIndexPath indexPath) {
			Model model = this.GetModelAtIndexPath (indexPath);
			if (model is Expense) {
				LoadingView.showMessage ();

				try {
					await ((ReportExpenses)this.Expenses).RemoveReportExpenseAsync ((Expense)model);
				} catch (Exception error) {
					MainNavigationController.Instance.showError (error);
					return;
				} finally {
					LoadingView.hideMessage ();
				}

				this.Expenses.ResetSectionnedExpenses ();
				this.Expenses.DeploySplits ();
				tableView.ReloadData ();
			}
		}

		public override nfloat GetHeightForRow (UITableView tableView, NSIndexPath indexPath) {
			if (this.report.CanAddExpenses && indexPath.Section == 0)
				return 56;
			
			return base.GetHeightForRow (tableView, indexPath);
		}
	}
}