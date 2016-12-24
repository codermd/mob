using System;

using Foundation;
using UIKit;
using Mxp.Core.Business;

namespace Mxp.iOS
{
	public partial class CountryCell : UITableViewCell
	{
		public static readonly NSString Key = new NSString ("CountryCell");
		public static readonly UINib Nib;

		static CountryCell () {
			Nib = UINib.FromName ("CountryCell", NSBundle.MainBundle);
		}

		public static CountryCell Create () {
			return (CountryCell)Nib.Instantiate (null, null) [0];
		}

		public CountryCell (IntPtr handle) : base (handle) {
		}

		public void setCountry (Country c) {
			this.TitleLabel.Text = c.VName;

			if (c.IsMatched)
				this.TitleLabel.Font = UIFont.BoldSystemFontOfSize (this.TitleLabel.Font.PointSize);
			else
				this.TitleLabel.Font = UIFont.SystemFontOfSize (this.TitleLabel.Font.PointSize);

			this.LeftConstraint.Constant = c.PaddingLeft * 20;
		}
	}
}