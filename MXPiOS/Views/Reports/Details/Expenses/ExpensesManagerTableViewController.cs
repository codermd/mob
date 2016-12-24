using System;

using Foundation;
using UIKit;
using Mxp.Core.Business;
using Mxp.Core.Utils;
using System.Threading.Tasks;
using Mxp.Core;
using System.ComponentModel;

namespace Mxp.iOS
{
	public partial class ExpensesManagerTableViewController : MXPTableViewController, IReportDetailsSubController
	{
		public Report Report { get; set;}

		public ExpensesManagerTableViewController (IntPtr handle) : base (handle) {
		}

		private ExpensesTableSource source;

		public override void ViewDidLoad () {
			base.ViewDidLoad ();

			ExpenseItem expenseItem = ((MainTabBarControllerView)this.TabBarController)?.ExpenseItem;
			if (expenseItem != null) {
				this.showExpense (expenseItem, false);
				((MainTabBarControllerView)this.TabBarController).ExpenseItem = null;
			}
		}

		public override void ViewDidAppear (bool animated) {
			base.ViewDidAppear (animated);

			if (this.Report.CanRemoveExpenses) {
				this.source = new EditableExpenseSectionSource (this.Report);
				((EditableExpenseSectionSource)this.source).AddExpenseEvent += (sender, e) => {
					this.showEditionExpenses ();
				};
				this.TableView.AllowsSelectionDuringEditing = true;
				this.TableView.SetEditing (true, true);

			} else {
				this.source = new ExpensesTableSource (this.Report.Expenses);
			}

			source.cellSelected += (object sender, ExpenseSelectedEventArgs e) => {
				if (e.model is Expense) {
					this.showExpense (((Expense)e.model).ExpenseItems [0]);
				} else if (e.model is ExpenseItem) {
					this.showExpense ((ExpenseItem)e.model);
				}
			};

			this.TableView.Source = this.source;
			this.Report.Expenses.DeploySplits ();
			this.TableView.ReloadData ();
		}

		public void showExpense(ExpenseItem expenseItem, bool animated = true) {
			UIViewController nextVC = null;
			if (expenseItem.ParentExpense is Mileage) {
				nextVC = new MileageViewController (expenseItem.ParentExpense as Mileage);
			} else if (expenseItem.ParentExpense is Allowance) {
				nextVC = new AllowanceViewController (expenseItem.ParentExpense as Allowance);
			} else {
				ExpenseDetailViewController vc = UIStoryboard.FromName ("ExpenseDetailsStoryboard", NSBundle.MainBundle).InstantiateInitialViewController () as ExpenseDetailViewController;
				vc.setExpenseItem (expenseItem);
				nextVC = vc;
			}
			this.NavigationController.PushViewController (nextVC, animated);
		}

		public void showEditionExpenses() {
			var expenses = this.Report.Expenses;
			if (!this.Report.IsNew) {
				expenses = new ReportExpenses ();
			}

			SelectableExpensesTableViewController vc = new SelectableExpensesTableViewController (expenses);

			vc.SelectedExpensesConfirm += async (sender, e) => {
				if (e.expenses == null)
					return;
				
				LoadingView.showMessage ();

				try {
					await this.Report.Expenses.AddSelectedExpenses (e.expenses);
				} catch (Exception error) {
					MainNavigationController.Instance.showError (error);
					return;
				} finally {
					LoadingView.hideMessage ();
				}

				this.Report.Expenses.ResetSectionnedExpenses ();
				this.Report.Expenses.DeploySplits ();
				this.TableView.ReloadData ();
			};

			if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad) {
				var nvc = new UINavigationController (vc);
				nvc.ModalPresentationStyle = UIModalPresentationStyle.FormSheet;
				this.PresentViewController (nvc, true, null);
				vc.NavigationItem.SetLeftBarButtonItem (new UIBarButtonItem (Labels.GetLoggedUserLabel (Labels.LabelEnum.Close), UIBarButtonItemStyle.Done, null), true);
				vc.SelectedExpensesConfirm += (sender, e) => {
					vc.DismissViewController(true, null);
				};
			} else {
				vc.SelectedExpensesConfirm += (sender, e) => {
					vc.NavigationController.PopViewController(true);
				};
				this.NavigationController.PushViewController (vc, true);
			}
		}
	}
}