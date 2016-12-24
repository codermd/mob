using System;
using Mxp.Core.Business;
using UIKit;

namespace Mxp.iOS
{
	public class LookupDataFieldCell : DataFieldCell
	{
		private LookupField lookupField;

		public LookupDataFieldCell (LookupField field): base(field)
		{
			if (field.IsEditable) {
				this.accessory = UITableViewCellAccessory.DisclosureIndicator;
			}

			this.lookupField = field;
		}

		public override void FieldSelected (UIViewController viewController,UITableView tableview, UITableViewCell cell)
		{
			if (!this.Field.IsEditable) {
				return;
			}

			LookupViewController vc = new LookupViewController ();
			vc.LookupField = this.lookupField;
			vc.cellSelected += (sender, e) => {
				this.Field.Value = e.LookupItem;
			};

			if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad) {
				UIPopoverController popover = new UIPopoverController (vc);
				popover.PresentFromRect (cell.ConvertRectToView (cell.Subviews [0].Subviews [1].Frame, viewController.View), viewController.View, UIPopoverArrowDirection.Any, true);
				vc.cellSelected += (sender, e) => {
					popover.Dismiss(true);
				};
			} else {
				viewController.NavigationController.PushViewController (vc, true);
				vc.cellSelected += (sender, e) => {
					vc.NavigationController.PopViewController(true);
				};
			}

		}

		public override UITableViewCell GetCell (UITableView tableView)
		{
			LookupFieldCell cell = this.GenerateCellWithNibName<LookupFieldCell> ("LookupFieldCell", tableView);
			cell.SetField (this);
			return cell;
		}

	}
}