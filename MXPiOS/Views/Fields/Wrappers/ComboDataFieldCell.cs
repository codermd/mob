using System;
using UIKit;
using Mxp.Core.Business;
using Mxp.iOS;

namespace Mxp.iOS
{
	public class ComboDataFieldCell : DataFieldCell
	{


		public ComboDataFieldCell (Field field): base(field)
		{
			this.accessory = UITableViewCellAccessory.None;
		}


		public override UITableViewCell GetCell (UIKit.UITableView tableView)
		{
			ComboFieldCell cell = this.GenerateCellWithNibName<ComboFieldCell> ("ComboFieldCell", tableView);

			cell.SetField (this);
			return cell;
		}

		public override void FieldSelected (UIViewController viewController, UITableView tableView, UITableViewCell cell)
		{
			if (!this.Field.IsEditable) {
				return;
			}

//			if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad) {
//
//			} else {
				cell.BecomeFirstResponder ();
//			}
		}

	}
}

