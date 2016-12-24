
using System;
using CoreGraphics;

using Foundation;
using UIKit;
using Mxp.Core.Business;

using Mxp.Utils;

namespace Mxp.iOS
{
	public partial class EmptyCell : UITableViewCell
	{
		public static readonly UINib Nib = UINib.FromName ("EmptyCell", NSBundle.MainBundle);
		public static readonly NSString Key = new NSString ("EmptyCell");

		public EmptyCell (IntPtr handle) : base (handle)
		{
		}

		public static EmptyCell Create ()
		{
			return (EmptyCell)Nib.Instantiate (null, null) [0];
		}

		public override void AwakeFromNib ()
		{
			base.AwakeFromNib ();
			this.EmptyLabel.Text = LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.Empty).ToTitleCase ();
		}


	}
}
