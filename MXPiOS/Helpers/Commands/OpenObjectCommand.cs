using System;
using System.Threading.Tasks;
using Mxp.Core.Business;
using UIKit;
using Foundation;
using Mxp.Core.Utils;
using System.Linq;

namespace Mxp.iOS.Helpers
{
	public class OpenObjectCommand : OpenObjectAbstractCommand
	{
		private UIViewController viewController;

		public OpenObjectCommand (UIViewController viewController, Uri uri) : base (uri) {
			this.viewController = viewController;
		}

		public OpenObjectCommand (UIViewController viewController, OpenObject openObject) {
			this.viewController = viewController;
			this.openObject = openObject;
		}

		#region SAMLAbstractCommand

		protected override void RedirectToListView (ValidationError error) {
			LoadingView.hideMessage ();

			MainTabBarControllerView mainTabBar = this.viewController.Storyboard.InstantiateViewController ("MainTabBar") as MainTabBarControllerView;
			MainNavigationController.Instance.PushViewController (mainTabBar, true);

			MainNavigationController.Instance.showError (error);
		}

		protected override void RedirectToDetailsView () {
			LoadingView.hideMessage ();

			MainTabBarControllerView mainTabBar = this.viewController.Storyboard.InstantiateViewController ("MainTabBar") as MainTabBarControllerView;

			switch (this.MetaOpenObject.Location) {
				case MetaOpenObject.LocationEnum.PendingExpenses: {
						ExpenseItem expenseItem = LoggedUser.Instance.BusinessExpenses.SelectSingle (expense => expense.ExpenseItems.SingleOrDefault (item => item.Id == this.MetaOpenObject.Id));

						mainTabBar.ExpenseItem = expenseItem;
						break;
					}
				case MetaOpenObject.LocationEnum.DraftReports: {
						if (this.MetaOpenObject.HasFatherId) {
							Report report = LoggedUser.Instance.GetReport (Reports.ReportTypeEnum.Draft, this.MetaOpenObject.FatherId);
							ExpenseItem expenseItem = report.Expenses.SelectSingle (expense => expense.ExpenseItems.SingleOrDefault (item => item.Id == this.MetaOpenObject.Id));

							mainTabBar.ExpenseItem = expenseItem;
						} else {

						}
						break;
					}
				case MetaOpenObject.LocationEnum.OpenReports: {
						if (this.MetaOpenObject.HasFatherId) {
							Report report = LoggedUser.Instance.GetReport (Reports.ReportTypeEnum.Open, this.MetaOpenObject.FatherId);
							ExpenseItem expenseItem = report.Expenses.SelectSingle (expense => expense.ExpenseItems.SingleOrDefault (item => item.Id == this.MetaOpenObject.Id));

							mainTabBar.ExpenseItem = expenseItem;
						} else {
						
						}
						break;
					}
				case MetaOpenObject.LocationEnum.ClosedReports: {
					if (this.MetaOpenObject.HasFatherId) {
						Report report = LoggedUser.Instance.GetReport (Reports.ReportTypeEnum.Closed, this.MetaOpenObject.FatherId);
						ExpenseItem expenseItem = report.Expenses.SelectSingle (expense => expense.ExpenseItems.SingleOrDefault (item => item.Id == this.MetaOpenObject.Id));

						mainTabBar.ExpenseItem = expenseItem;
					} else {

					}
					break;
				}
				case MetaOpenObject.LocationEnum.ApprovalReports:
					break;
				case MetaOpenObject.LocationEnum.ApprovalTravelRequests:
					break;
			}

			MainNavigationController.Instance.PushViewController (mainTabBar, true);
		}

		public override void RedirectToLoginView (ValidationError error = null) {
			LoginViewController loginViewController = MainNavigationController.Instance.TopViewController as LoginViewController;

			if (loginViewController == null)
				loginViewController = this.viewController.Storyboard.InstantiateViewController ("LoginViewController") as LoginViewController;
			
			loginViewController.OpenObject = this.openObject;
			
			if (!(MainNavigationController.Instance.TopViewController is LoginViewController))
				MainNavigationController.Instance.PushViewController (loginViewController, true);

			if (error != null)
				MainNavigationController.Instance.showError (error);
		}

		#endregion
	}
}