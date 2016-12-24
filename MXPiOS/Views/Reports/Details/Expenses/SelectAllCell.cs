using System;
using CoreGraphics;
using Foundation;
using UIKit;
using Mxp.Core.Business;

namespace Mxp.iOS
{
	public partial class SelectAllCell : UITableViewCell
	{
		public static readonly UINib Nib = UINib.FromName ("SelectAllCell", NSBundle.MainBundle);
		public static readonly NSString Key = new NSString ("SelectAllCell");

		public SelectAllCell (IntPtr handle) : base (handle)
		{
		}

		public static SelectAllCell Create ()
		{
			return (SelectAllCell)Nib.Instantiate (null, null) [0];
		}

		public override void AwakeFromNib ()
		{
			base.AwakeFromNib ();
			this.configureView ();
		}


		private void configureView(){
			this.SelectAllLabel.Text = Labels.GetLoggedUserLabel (Labels.LabelEnum.SelectAll);
			this.SelectAllLabel.TextColor = UIColor.White;
			this.ButtonSelectAllLabel.Layer.BorderWidth = 1;
			this.ButtonSelectAllLabel.Layer.BorderColor = UIColor.FromRGB (154,94,100).CGColor;
			UIColor backgroundColor =  UIColor.FromRGB (22, 184, 99);
			this.ButtonSelectAllLabel.BackgroundColor = backgroundColor;
		}

		public override void SetHighlighted (bool highlighted, bool animated)
		{
			base.SetHighlighted (highlighted, animated);
			this.configureView ();
		}

		public override void SetSelected (bool selected, bool animated)
		{
			base.SetSelected (selected, animated);
			this.configureView ();
		}
	}
}