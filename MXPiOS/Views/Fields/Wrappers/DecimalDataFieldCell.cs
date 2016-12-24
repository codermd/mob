using System;
using Mxp.Core.Business;
using UIKit;

namespace Mxp.iOS
{
	public class DecimalDataFieldCell : DataFieldCell
	{
		public DecimalDataFieldCell (Field field): base(field)
		{
			this.accessory = UITableViewCellAccessory.None;
		}

		public override void FieldSelected (UIViewController viewController,UITableView tableview, UITableViewCell cell)
		{

		}

		public override UITableViewCell GetCell (UITableView tableView)
		{
			DecimalFieldCell cell = this.GenerateCellWithNibName<DecimalFieldCell> ("DecimalFieldCell", tableView);
			cell.SetField (this);
			return cell;
		}
	}
}