
using System;
using CoreGraphics;

using Foundation;
using UIKit;
using Mxp.Core.Business;

namespace Mxp.iOS
{
	public partial class MileageShowMapCell : UITableViewCell
	{
		public static readonly UINib Nib = UINib.FromName ("MileageShowMapCell", NSBundle.MainBundle);
		public static readonly NSString Key = new NSString ("MileageShowMapCell");

		public MileageShowMapCell (IntPtr handle) : base (handle)
		{
		}

		public override void AwakeFromNib ()
		{
			base.AwakeFromNib ();
			this.ShowMapText.Text = Labels.GetLoggedUserLabel (Labels.LabelEnum.ShowMap).ToUpper();
		}

		public static MileageShowMapCell Create ()
		{
			return (MileageShowMapCell)Nib.Instantiate (null, null) [0];
		}
	}
}

