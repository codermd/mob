using System;
using Mxp.Core.Business;
using UIKit;

namespace Mxp.iOS
{
	public class AmountDataFieldCell : DataFieldCell
	{
		public AmountDataFieldCell (Field field): base(field)
		{
			if (field.IsEditable) {
				if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad) {
					this.accessory = UITableViewCellAccessory.DetailButton;
				} else {
					this.accessory = UITableViewCellAccessory.DetailDisclosureButton;
				} 			
			}
		}

		public override void AccessoryButtonTapped (UIViewController viewController,UITableView tableview)
		{
			this.showVC (viewController, tableview);
		}

		public void showVC(UIViewController viewController,UITableView tableview) {
			AmountTableViewController vc = new AmountTableViewController (this.Field.GetModel<ExpenseItem>());

			if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad) {
				UINavigationController nvc = new UINavigationController (vc);
				nvc.ModalPresentationStyle = UIModalPresentationStyle.FormSheet;
				vc.NavigationItem.SetLeftBarButtonItem(new UIBarButtonItem(Labels.GetLoggedUserLabel (Labels.LabelEnum.Close), UIBarButtonItemStyle.Done, (sender, e)=>{
					nvc.DismissViewController(true, null);
				}), true);
				viewController.PresentViewController (nvc, true, null);
			} else {
				viewController.NavigationController.PushViewController (vc, true);
			}
		}

		public override void FieldSelected (UIViewController viewController,UITableView tableview, UITableViewCell cell)
		{
			if (this.Field.IsEditable) {
				this.showVC (viewController, tableview);
			}

		}

		public override UITableViewCell GetCell (UITableView tableView)
		{
			AmountFieldCell cell = this.GenerateCellWithNibName<AmountFieldCell> ("AmountFieldCell", tableView);
			cell.SetField (this);
			return cell;
		}
	}
}