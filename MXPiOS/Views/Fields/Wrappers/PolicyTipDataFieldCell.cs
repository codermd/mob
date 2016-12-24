using System;
using Mxp.Core.Business;
using Mxp.Core;
using UIKit;

namespace Mxp.iOS
{
	public class PolicyTipDataFieldCell : DataFieldCell
	{
		public PolicyTipDataFieldCell (Field field): base(field)
		{
			this.accessory = UITableViewCellAccessory.None;
		}

		public override void FieldSelected (UIViewController viewController,UITableView tableview, UITableViewCell cell)
		{
			UIAlertView alert = new UIAlertView ("!", this.Field.extraInfo["Message"] as string, null, Labels.GetLoggedUserLabel (Labels.LabelEnum.Accept), null);
			alert.Show ();
		}


		public override UITableViewCell GetCell (UITableView tableView)
		{
			PolicyTipFieldCell cell = this.GenerateCellWithNibName<PolicyTipFieldCell> ("PolicyTipFieldCell", tableView);
			cell.SetField (this);
			return cell;
		}

	}
}