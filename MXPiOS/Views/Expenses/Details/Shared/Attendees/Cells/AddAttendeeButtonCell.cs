
using System;
using CoreGraphics;

using Foundation;
using UIKit;
using Mxp.Core.Business;

namespace Mxp.iOS
{
	public partial class AddAttendeeButtonCell : UITableViewCell
	{
		public static readonly UINib Nib = UINib.FromName ("AddAttendeeButtonCell", NSBundle.MainBundle);
		public static readonly NSString Key = new NSString ("AddAttendeeButtonCell");

		public AddAttendeeButtonCell (IntPtr handle) : base (handle)
		{
		}

		public override void AwakeFromNib ()
		{
			base.AwakeFromNib ();
			this.MainLabel.Layer.CornerRadius = 4;
			this.MainLabel.Layer.MasksToBounds = true;
			this.MainLabel.Text = LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.NewAttendees);
			this.MainLabel.Layer.BorderWidth = 1;
			this.MainLabel.Layer.BorderColor =  UIColor.FromRGB (0, 134, 158).CGColor;
			this.MainLabel.TextColor = UIColor.White;
		}

		public static AddAttendeeButtonCell Create ()
		{
			return (AddAttendeeButtonCell)Nib.Instantiate (null, null) [0];
		}
	}
}

