using System;
using UIKit;
using Mxp.Core.Business;

namespace Mxp.iOS
{
	public class DateDataFieldCell : DataFieldCell
	{


		public DateDataFieldCell (Field field): base(field)
		{
			this.accessory = UITableViewCellAccessory.None;
		}


		public override UITableViewCell GetCell (UIKit.UITableView tableView)
		{
			DateFieldCell cell = this.GenerateCellWithNibName<DateFieldCell> ("DateFieldCell", tableView);

			cell.SetField (this);
			return cell;
		}

		public override void FieldSelected (UIViewController viewController, UITableView tableView, UITableViewCell cell)
		{
			cell.BecomeFirstResponder ();
		}

	}
}

