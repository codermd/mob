using System;
using CoreGraphics;
using Foundation;
using UIKit;
using Mxp.Core.Business;

namespace Mxp.iOS
{
	public partial class AddReceiptCell : UICollectionViewCell
	{
		public static readonly UINib Nib = UINib.FromName ("AddReceiptCell", NSBundle.MainBundle);
		public static readonly NSString Key = new NSString ("AddReceiptCell");


		public AddReceiptCell (IntPtr handle) : base (handle)
		{
		}

		public static AddReceiptCell Create ()
		{
			return (AddReceiptCell)Nib.Instantiate (null, null) [0];
		}


		public override void AwakeFromNib ()
		{
			base.AwakeFromNib ();
			this.SecondPartLabel.TextColor = UIColor.FromRGB(255, 255, 255);
			this.SecondPartLabel.Layer.CornerRadius = 5;
			this.SecondPartLabel.Layer.BorderWidth = 1;
			this.SecondPartLabel.Layer.BorderColor = UIColor.FromRGB (204,151,64).CGColor;
			this.SecondPartLabel.Text = "+ " + Labels.GetLoggedUserLabel (Labels.LabelEnum.AddReceipt);
		}

		public void setExpense(Expense expense) {
//			this.messageLabel.Text = expense.VReceiptMessage;
		}
	}
}