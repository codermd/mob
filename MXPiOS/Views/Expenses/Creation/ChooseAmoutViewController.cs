using System;
using CoreGraphics;

using Foundation;
using UIKit;
using Mxp.Core.Business;

namespace Mxp.iOS
{
	public partial class ChooseAmoutViewController : AmountTableViewController
	{
		public ChooseAmoutViewController (ExpenseItem expenseItem) : base (expenseItem) {
			
		}

		public override void ViewDidLoad () {
			base.ViewDidLoad ();
			this.Title = "3. "+Labels.GetLoggedUserLabel (Labels.LabelEnum.Amount);
		}

		public override void configureTopBar(){
			UIBarButtonItem saveButton = new UIBarButtonItem(Labels.GetLoggedUserLabel (Labels.LabelEnum.Save), UIBarButtonItemStyle.Done, (object sender, EventArgs e) => {
				this.CreateExpense();
			});

			this.NavigationItem.SetRightBarButtonItem (saveButton, true);
		}

		public void CreateExpense () {
			UIStoryboard storyBoard = UIStoryboard.FromName ("ExpenseDetailsStoryboard", NSBundle.MainBundle);
			ExpenseDetailViewController vc =  storyBoard.InstantiateInitialViewController () as ExpenseDetailViewController;
			vc.setExpenseItem (this.expenseItem);
			UIViewController[] viewControllers = new UIViewController[]{this.NavigationController.ViewControllers[0], vc};
			this.NavigationController.SetViewControllers (viewControllers, true);
		}
	}
}