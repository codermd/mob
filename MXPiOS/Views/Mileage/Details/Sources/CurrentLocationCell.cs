
using System;
using CoreGraphics;

using Foundation;
using UIKit;
using Mxp.Core.Business;

namespace Mxp.iOS
{
	public partial class CurrentLocationCell : UITableViewCell
	{
		public static readonly UINib Nib = UINib.FromName ("CurrentLocationCell", NSBundle.MainBundle);
		public static readonly NSString Key = new NSString ("CurrentLocationCell");

		public CurrentLocationCell (IntPtr handle) : base (handle)
		{
		}

		public static CurrentLocationCell Create ()
		{
			return (CurrentLocationCell)Nib.Instantiate (null, null) [0];
		}

		public override void AwakeFromNib ()
		{
			base.AwakeFromNib ();
			this.TitleLabel.Text = Labels.GetLoggedUserLabel (Labels.LabelEnum.AddCurrentLocation);
		}
	}
}

