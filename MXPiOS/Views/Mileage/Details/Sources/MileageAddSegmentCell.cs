
using System;
using CoreGraphics;

using Foundation;
using UIKit;
using Mxp.Core.Business;

namespace Mxp.iOS
{
	public partial class MileageAddSegmentCell : UITableViewCell
	{
		public static readonly UINib Nib = UINib.FromName ("MileageAddSegmentCell", NSBundle.MainBundle);
		public static readonly NSString Key = new NSString ("MileageAddSegmentCell");

		public MileageAddSegmentCell (IntPtr handle) : base (handle)
		{
		}

		public static MileageAddSegmentCell Create ()
		{
			return (MileageAddSegmentCell)Nib.Instantiate (null, null) [0];
		}

		public override void AwakeFromNib ()
		{
			base.AwakeFromNib ();
			this.ShowMapText.TextColor = UIColor.White;
			this.AddMapButton.Layer.CornerRadius = 5;
			this.AddMapButton.Layer.BorderWidth = 1;
			this.AddMapButton.Layer.BorderColor = UIColor.FromRGB (0, 134, 158).CGColor;
			this.ShowMapText.Text = Labels.GetLoggedUserLabel(Labels.LabelEnum.Add);

		}
	}
}

