using System;
using Mxp.Core.Business;
using Mxp.Core;
using UIKit;

namespace Mxp.iOS
{
	public class AutocompleteStringDataFieldCell : DataFieldCell
	{
		public AutocompleteStringDataFieldCell(Field field) : base(field)
		{
			if (field.IsEditable)
			{
				this.accessory = UITableViewCellAccessory.DisclosureIndicator;
			}
			else {
				this.accessory = UITableViewCellAccessory.None;
			}
		}
		//

		public override void FieldSelected(UIViewController viewController, UITableView tableView, UITableViewCell cell)
		{
			AutocompleteStringViewController vc = new AutocompleteStringViewController();
			vc.DataField = this;

			if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad)
			{
				UIPopoverController popover = new UIPopoverController(vc);
				popover.PresentFromRect(cell.Frame, viewController.View, UIPopoverArrowDirection.Any, true);
			}
			else {
				viewController.NavigationController.PushViewController(vc, true);
			}
		}

		public override UITableViewCell GetCell(UITableView tableView)
		{
			AutocompleteStringFieldCell cell = this.GenerateCellWithNibName<AutocompleteStringFieldCell>("AutocompleteStringFieldCell", tableView);
			cell.SetField(this);
			return cell;
		}
	}
}

