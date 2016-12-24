using System;
using CoreGraphics;

using Foundation;
using UIKit;
using Mxp.Core.Business;

namespace Mxp.iOS
{
	public partial class PolicyTipFieldCell : UITableViewCell
	{
		public static readonly UINib Nib = UINib.FromName ("PolicyTipFieldCell", NSBundle.MainBundle);
		public static readonly NSString Key = new NSString ("PolicyTipFieldCell");


		public PolicyTipFieldCell (IntPtr handle) : base (handle)
		{
		}

		public static StringFieldCell Create ()
		{
			return (StringFieldCell)Nib.Instantiate (null, null) [0];
		}


		public override bool CanBecomeFirstResponder {
			get {
				return false;
			}
		}

		public DataFieldCell _DataField;

		public void SetField (DataFieldCell aField){
			this._DataField = aField;
			this.refresh ();
		}


		public void refresh() {
			if (this._DataField.Field.GetModel<ExpenseItem> ().PolicyRule == ExpenseItem.PolicyRules.Orange) {
				this.PolicyImage.Highlighted = true;
				this.PolicityTipLabel.TextColor = UIColor.Orange;
			}
			if (this._DataField.Field.GetModel<ExpenseItem> ().PolicyRule == ExpenseItem.PolicyRules.Red) {
				this.PolicyImage.Highlighted = false;
				this.PolicityTipLabel.TextColor = UIColor.Red;
			}
			this.PolicityTipLabel.Text = this._DataField.Field.GetModel<ExpenseItem> ().PolicyRuleTip;

		}
	}
}


