using System;
using Mxp.Core.Business;
using Mxp.Core;
using UIKit;

namespace Mxp.iOS
{
	public class IntegerDataFieldCell : DataFieldCell
	{
		public IntegerDataFieldCell (Field field): base(field)
		{
			this.accessory = UITableViewCellAccessory.None;
		}

		public override void FieldSelected (UIViewController viewController,UITableView tableview, UITableViewCell cell)
		{

		}


		public override UITableViewCell GetCell (UITableView tableView)
		{
			IntegerFieldCell cell = this.GenerateCellWithNibName<IntegerFieldCell> ("IntegerFieldCell", tableView);
			cell.SetField (this);
			return cell;
		}

	}
}