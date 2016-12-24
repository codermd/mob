using System;
using Mxp.Core.Business;
using Mxp.Core;
using UIKit;

namespace Mxp.iOS
{
	public class LongStringDataFieldCell : DataFieldCell
	{
		public LongStringDataFieldCell (Field field): base(field)
		{
			this.accessory = UITableViewCellAccessory.DisclosureIndicator;
		}

		public override void FieldSelected (UIViewController viewController, UITableView tableview, UITableViewCell cell)
		{
			LongStringViewController vc = new LongStringViewController ();
			vc.setDataField (this);

			if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad) {
				UINavigationController nvc = new UINavigationController (vc);
				UIPopoverController popover = new UIPopoverController (nvc);
				popover.PresentFromRect (cell.Frame, viewController.View, UIPopoverArrowDirection.Any, true);
			} else {
				viewController.NavigationController.PushViewController (vc, true);
			}
		}

		public override UITableViewCell GetCell (UITableView tableView)
		{
			LongStringFieldCell cell = this.GenerateCellWithNibName<LongStringFieldCell> ("LongStringFieldCell", tableView);
			cell.SetField (this);
			return cell;
		}

	}
}