using System;
using Mxp.Core.Business;
using MXPiOS;
using UIKit;

namespace Mxp.iOS
{
	public class CategoryDataFieldCell : DataFieldCell
	{
		public CategoryDataFieldCell (Field field): base(field)
		{
			if (field.IsEditable) {
				this.accessory = UITableViewCellAccessory.DisclosureIndicator;
			}
		}

		public override void FieldSelected (UIViewController viewController,UITableView tableview, UITableViewCell cell)
		{
			if (!this.Field.IsEditable) {
				return;
			}
			ProductsViewController vc = new ProductsViewController ();
			vc.setDataField (this);


			vc.cellSelected += (sender, e) => {
				this.Field.Value = e.Product;
			};


			if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad) {
				UIPopoverController popover = new UIPopoverController (vc);
				popover.PresentFromRect (cell.ConvertRectToView (cell.Subviews [0].Subviews [1].Frame, viewController.View), viewController.View, UIPopoverArrowDirection.Any, true);
				vc.cellSelected += (sender, e) => {
					popover.Dismiss(true);
				};
			} else {
				vc.cellSelected += (sender, e) => {
					vc.NavigationController.PopViewController(true);
				};
				viewController.NavigationController.PushViewController (vc, true);
			}
		}

		public override UITableViewCell GetCell (UITableView tableView)
		{
			CategoryFieldCell cell = this.GenerateCellWithNibName<CategoryFieldCell> ("CategoryFieldCell", tableView);
			cell.SetField (this);
			return cell;
		}

		public override int HeightForCell ()
		{
			if (this.Field.VValue.Length < 22) {
				return base.HeightForCell ();
			}
			int total =70;

			int size = (this.Field.VValue.Length / 24);

			total += size * 21;
			return total;
		}

	}
}