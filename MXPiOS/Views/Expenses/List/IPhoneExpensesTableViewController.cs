using System;
using Mxp.Core.Business;
using UIKit;
using Foundation;
using MXPiOS;
using Mxp.Core;

namespace Mxp.iOS
{
	[Foundation.Register ("IPhoneExpensesTableViewController")]
	partial class IPhoneExpensesTableViewController : ExpensesTableViewController
	{
		public IPhoneExpensesTableViewController (IntPtr handle) : base (handle) {

		}

		public override void ShowModel (Model model, bool animated = true) {
			this.SetSelectedExpense (null);

			if (model == null)
				return;

			UIViewController vc = null;

			if (model is SpendCatcherExpense)
				vc = new SpendCatcherViewController ((SpendCatcherExpense)model);
			else if (model is ExpenseItem) {
				ExpenseItem expenseItem = (ExpenseItem)model;

				if (expenseItem.ParentExpense is Mileage)
					vc = new MileageViewController (expenseItem.ParentExpense as Mileage);
				else if (expenseItem.ParentExpense is Allowance) {
					vc = new AllowanceViewController (expenseItem.ParentExpense as Allowance);
				} else if (expenseItem.ParentExpense is Expense) {
					UIStoryboard storyBoard = UIStoryboard.FromName ("ExpenseDetailsStoryboard", NSBundle.MainBundle);
					ExpenseDetailViewController evc = (ExpenseDetailViewController)storyBoard.InstantiateInitialViewController ();
					evc.setExpenseItem (expenseItem);
					vc = evc;
				}
			}

			this.NavigationController.PushViewController(vc, animated);
		}

		public override void ShowCreateViewController (UIViewController vc) {
			this.NavigationController.PushViewController (vc, true);
		}
	}
}