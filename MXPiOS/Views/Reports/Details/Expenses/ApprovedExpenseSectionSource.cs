using System;
using Mxp.Core.Business;
using UIKit;
using Mxp.Core.Utils;
using Foundation;
using CoreGraphics;

namespace Mxp.iOS
{
	public class ApprovedExpenseSectionSource : ExpensesTableSource
	{
		public ApprovedExpenseSectionSource (Report report): base (report.Expenses) {
			this.Expenses.ForEach (expense => {
				if(expense.IsSplit) {
					this.Expenses.unsplitExpense(expense);
				}
			});
		}

		public override void RowSelected (UITableView tableView, NSIndexPath indexPath) {
			Model model = this.GetModelAtIndexPath (indexPath);
			if(model is ExpenseItem || (model is Expense && !((Expense)model).IsSplit)) {
				base.RowSelected (tableView, indexPath);
			}
		}
			
		public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath) {
			UITableViewCell cell = base.GetCell (tableView, indexPath);

			Model model = this.GetModelAtIndexPath (indexPath);

			if (model is Expense && ((Expense)model).IsSplit) {
				cell.SelectionStyle = UITableViewCellSelectionStyle.None;
				if (cell.AccessoryView is ModelButton)
					((ModelButton)cell.AccessoryView).Unbind ();
				cell.AccessoryView = null;
			} else {
				cell.SelectionStyle = UITableViewCellSelectionStyle.Blue;

				ExpenseItem expenseItem = model is Expense ? ((Expense)model).ExpenseItems [0] : (ExpenseItem)model;

				if (cell.AccessoryView == null) {
					cell.AccessoryView = new ModelButton ();
					((ModelButton)cell.AccessoryView).Bind ();
				}

				((ModelButton)cell.AccessoryView).Model = expenseItem;
			}

			return cell;
		}

		private class ModelButton : UIButton 
		{
			UIImage imageSelected = UIImage.FromBundle ("CheckmarkSelected");
			UIImage imageUnselected = UIImage.FromBundle ("CheckmarkUnselected");

			private Model _model;
			public Model Model {
				get {
					return this._model;
				} 
				set {
					this._model = value;

					imageSelected = UIImage.FromBundle ("CheckmarkSelected");
					imageUnselected = UIImage.FromBundle ("CheckmarkUnselected");
					this.SetBackgroundImage (((ExpenseItem)this.Model).StatusForApprovalReport == ExpenseItem.Status.Accepted ? imageSelected : imageUnselected, UIControlState.Normal);
				}
			}

			public ModelButton() : base (new CGRect (0, 0, 48, 48)) { 
				
			}

			public void Bind () {
				this.TouchUpInside += TouchInsideButtonHandler;
			}

			public void Unbind () {
				this.TouchUpInside -= TouchInsideButtonHandler;
			}

			private void TouchInsideButtonHandler (object sender, EventArgs e) {
				if (this.Model is Expense) {
					Expense exp = this.Model as Expense;
					exp.ExpenseItems.ForEach (item => {
						item.Toggle ();
					});
				} else if (this.Model is ExpenseItem) {
					ExpenseItem expItem = this.Model as ExpenseItem;
					expItem.Toggle ();
				}

				((UIButton)sender).SetBackgroundImage (((ExpenseItem)this.Model).StatusForApprovalReport == ExpenseItem.Status.Accepted ? imageSelected : imageUnselected, UIControlState.Normal);	
			}
		}
	}
}