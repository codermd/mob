
using System;

using Foundation;
using UIKit;

namespace Mxp.iOS
{
	public partial class DefaultCell : UITableViewCell
	{
		public static readonly UINib Nib = UINib.FromName ("DefaultCell", NSBundle.MainBundle);
		public static readonly NSString Key = new NSString ("DefaultCell");

		public DefaultCell (IntPtr handle) : base (handle)
		{
		}

		public static DefaultCell Create ()
		{
			return (DefaultCell)Nib.Instantiate (null, null) [0];
		}

		public override UILabel TextLabel {
			get {
				return this.ContentLabel;
			}
		}

	}
}

