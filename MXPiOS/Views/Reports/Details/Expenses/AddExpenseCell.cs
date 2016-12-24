
using System;
using CoreGraphics;

using Foundation;
using UIKit;
using Mxp.Core.Business;

namespace Mxp.iOS
{
	public partial class AddExpenseCell : UITableViewCell
	{
		public static readonly UINib Nib = UINib.FromName ("AddExpenseCell", NSBundle.MainBundle);
		public static readonly NSString Key = new NSString ("AddExpenseCell");

		public AddExpenseCell (IntPtr handle) : base (handle)
		{
		}

		public static AddExpenseCell Create ()
		{
			return (AddExpenseCell)Nib.Instantiate (null, null) [0];
		}

		public override void AwakeFromNib ()
		{
			base.AwakeFromNib ();
			this.AddExpenseLabel.Text = Labels.GetLoggedUserLabel (Labels.LabelEnum.AddExpense);
			this.AddExpenseLabel.TextColor = UIColor.FromRGB (255, 255, 255);
			this.ButtonAddExpense.Layer.CornerRadius = 5;
			this.ButtonAddExpense.Layer.BorderWidth = 1;
			this.ButtonAddExpense.Layer.BorderColor = UIColor.FromRGB (0, 134, 158).CGColor;
		}

		public override void SetSelected (bool selected, bool animated)
		{
			base.SetSelected (selected, animated);
			this.AddExpenseLabel.TextColor = UIColor.FromRGB (255, 255, 255);
			this.ButtonAddExpense.Layer.BorderColor = UIColor.FromRGB (0, 134, 158).CGColor;
			this.ButtonAddExpense.BackgroundColor = UIColor.FromRGB (18, 152, 185);
		}


	}
}

