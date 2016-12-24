using System;
using Foundation;
using UIKit;
using Mxp.Core.Business;
using ObjCRuntime;
using System.ComponentModel;
using MXPiOS;

namespace Mxp.iOS
{
	public partial class IPadSplitExpenseViewController : MXPSplitViewController
	{
		private Model selectedExpense;

		public IPadSplitExpenseViewController (IntPtr handle) : base (handle) {
			
		}

		public override void ViewDidLoad () {
			base.ViewDidLoad ();

			((IPadExpensesTableViewController)this.listViewController).cellSelected += (sender, e) => {
				this.ShowModel (e.Model);
			};

			ExpenseItem expenseItem = ((MainTabBarControllerView)this.TabBarController)?.ExpenseItem;
			if (expenseItem != null) {
				this.ShowModel (expenseItem);
				((MainTabBarControllerView)this.TabBarController).ExpenseItem = null;
			}
		}

		public override void ViewWillAppear (bool animated) {
			base.ViewWillAppear (animated);

			LoggedUser.Instance.BusinessExpenses.PropertyChanged += HandlePropertyChanged;
			LoggedUser.Instance.PrivateExpenses.PropertyChanged += HandlePropertyChanged;
			LoggedUser.Instance.SpendCatcherExpenses.PropertyChanged += HandlePropertyChanged;
		}

		private void HandlePropertyChanged (object sender, PropertyChangedEventArgs e) {
			if ((e.PropertyName.Equals ("IsChanged") || e.PropertyName.Equals ("Removed"))
			    && this.selectedExpense != null
			    && ((this.selectedExpense is Expense
			         && !LoggedUser.Instance.BusinessExpenses.Contains ((Expense)this.selectedExpense)
			         && !LoggedUser.Instance.PrivateExpenses.Contains ((Expense)this.selectedExpense))
			        || (this.selectedExpense is SpendCatcherExpense && !LoggedUser.Instance.SpendCatcherExpenses.Contains ((SpendCatcherExpense)this.selectedExpense))))
					this.ShowModel (null);
		}

		public override void ViewWillDisappear (bool animated) {
			base.ViewWillDisappear (animated);

			LoggedUser.Instance.BusinessExpenses.PropertyChanged -= HandlePropertyChanged;
			LoggedUser.Instance.PrivateExpenses.PropertyChanged -= HandlePropertyChanged;
			LoggedUser.Instance.SpendCatcherExpenses.PropertyChanged -= HandlePropertyChanged;
		}

		public void ShowModel (Model model) {
			if (model == null || (model is ExpenseItem && ((ExpenseItem)model).ParentExpense == null)) {
				this.selectedExpense = null;
				this.ShouldShowDetailViewController (this.Storyboard.InstantiateViewController ("NOEXPENSE"), this);
				return;
			}

			UIViewController vc = null;

			if (model is ExpenseItem) {
				ExpenseItem expenseItem = (ExpenseItem)model;

				this.selectedExpense = expenseItem.ParentExpense;

				if (this.selectedExpense != null && this.selectedExpense is Mileage)
					vc = new MileageViewController (this.selectedExpense as Mileage);
				else if (this.selectedExpense != null && this.selectedExpense is Allowance)
					vc = new AllowanceViewController (this.selectedExpense as Allowance);
				else {
					vc = UIStoryboard.FromName ("ExpenseDetailsStoryboard", NSBundle.MainBundle).InstantiateInitialViewController () as ExpenseDetailViewController;
					((ExpenseDetailViewController)vc).setExpenseItem (expenseItem);
				}
			} else if (model is SpendCatcherExpense) {
				this.selectedExpense = model;

				vc = new SpendCatcherViewController ((SpendCatcherExpense)model);
			}

			this.ShouldShowDetailViewController (vc, this);
		}
	}
}