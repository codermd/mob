using System;
using UIKit;
using Mxp.Core.Business;

namespace Mxp.iOS
{
	public class ChooseAmoutSplitViewController : ChooseAmoutViewController
	{

		public ChooseAmoutSplitViewController(ExpenseItem expenseItem) : base(expenseItem){}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			this.Title = this.Title = "2. " + Labels.GetLoggedUserLabel (Labels.LabelEnum.Amount);
		}


		public override void ViewWillAppear (bool animated) {
			base.ViewWillAppear (animated);

//			this.ProtectedAmountLabel.Text = this.expenseItem.GetModelParent<ExpenseItem, ExpenseItem> ().VRemainingAmount;
//
//			this.ProtectedAmountField.EditingDidEnd += (sender, e) => {
//				try {
//					if(this.expenseItem.GetModelParent<ExpenseItem, ExpenseItem> ().RemainingAmount < Convert.ToDouble(this.ProtectedAmountField.Text)) {
//						this.ProtectedAmountField.Text = "0";//this.expenseItem.GetModelParent<ExpenseItem, ExpenseItem> ().RemainingAmount.ToString();
//					}
//				} catch(Exception ex){
//
//				}
//			};
		}

		public override void configureTopBar () {
			UIBarButtonItem saveButton = new UIBarButtonItem(Labels.GetLoggedUserLabel (Labels.LabelEnum.Next), UIBarButtonItemStyle.Done, (object sender, EventArgs e) => {
				this.GoToSplitedViewController();
			});

			this.NavigationItem.SetRightBarButtonItem (saveButton, true);
		}

		public void GoToSplitedViewController () {
			this.NavigationController.PushViewController(new SplitTableViewController(this.expenseItem.GetModelParent<ExpenseItem>() as ExpenseItem), true);
		}
	}
}