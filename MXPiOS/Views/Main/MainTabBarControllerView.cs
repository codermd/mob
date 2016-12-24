using System;

using Foundation;
using UIKit;
using Mxp.Core.Business;

namespace Mxp.iOS
{
	public partial class MainTabBarControllerView : UITabBarController
	{
		private ExpenseItem _expenseItem;
		public ExpenseItem ExpenseItem {
			get {
				ExpenseItem expenseItem = this._expenseItem;
				this._expenseItem = null;
				return expenseItem;
			}
			set {
				this._expenseItem = value;
			}
		}

		public int SelectedTab {
			get {
				if (this._expenseItem == null)
					return 0;
				
				if (this._expenseItem.ParentExpense.IsFromApproval)
					return 2;
				else if (this._expenseItem.ParentExpense.IsFromReport)
					return 1;
				else
					return 0;
			}
		}

		public int SelectedCategory {
			get {
				if (this._expenseItem == null || !this._expenseItem.ParentExpense.IsFromReport)
					return 0;
				
				return (int)this._expenseItem.ParentExpense.Report.ReportType;
			}
		}

		public MainTabBarControllerView (IntPtr handle) : base (handle) {
			
		}

		public override void ViewDidLoad () {
			base.ViewDidLoad ();

			this.TabBar.Items [0].Title = Labels.GetLoggedUserLabel(Labels.LabelEnum.Expenses);
			this.TabBar.Items [1].Title = Labels.GetLoggedUserLabel(Labels.LabelEnum.Reports);
			this.TabBar.Items [2].Title = Labels.GetLoggedUserLabel(Labels.LabelEnum.Approvals);
			this.TabBar.Items [3].Title = Labels.GetLoggedUserLabel(Labels.LabelEnum.Settings);

			this.SelectedIndex = this.SelectedTab;
		}
	}
}