using System;
using Mxp.Core.Business;
using Mxp.Core;
using UIKit;

namespace Mxp.iOS
{
	public class StringDataFieldCell : DataFieldCell
	{
		public StringDataFieldCell (Field field): base(field)
		{
			this.accessory = UITableViewCellAccessory.None;
		}

		public override void FieldSelected (UIViewController viewController,UITableView tableview, UITableViewCell cell)
		{

		}


		public override UITableViewCell GetCell (UITableView tableView)
		{
			StringFieldCell cell = this.GenerateCellWithNibName<StringFieldCell> ("StringFieldCell", tableView);
			cell.SetField (this);
			return cell;
		}

	}
}