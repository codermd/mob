using System;
using Mxp.Core.Business;
using Mxp.Core;
using UIKit;

namespace Mxp.iOS
{
	public class BooleanDataFieldCell : DataFieldCell
	{
		public BooleanDataFieldCell (Field field): base(field)
		{
			this.accessory = UITableViewCellAccessory.None;
		}

		public override UITableViewCell GetCell (UITableView tableView)
		{
			BooleanFieldCell cell = this.GenerateCellWithNibName<BooleanFieldCell> ("BooleanFieldCell", tableView);
			cell.SetField (this);
			return cell;
		}
	}
}

