
using System;
using CoreGraphics;

using Foundation;
using UIKit;
using Mxp.Core.Business;

namespace Mxp.iOS
{
	public partial class AddReturningCell : UITableViewCell
	{
		public static readonly UINib Nib = UINib.FromName ("AddReturningCell", NSBundle.MainBundle);
		public static readonly NSString Key = new NSString ("AddReturningCell");

		public AddReturningCell (IntPtr handle) : base (handle)
		{
		}

		public static AddReturningCell Create ()
		{
			return (AddReturningCell)Nib.Instantiate (null, null) [0];
		}

		public override void AwakeFromNib ()
		{
			base.AwakeFromNib ();
			this.TitleLabel.Text = Labels.GetLoggedUserLabel (Labels.LabelEnum.BackHome);
		}
	}
}

