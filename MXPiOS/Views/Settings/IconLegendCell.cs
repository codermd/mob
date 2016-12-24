using System;
using CoreGraphics;
using UIKit;
using Foundation;
using System.Globalization;
using System.ComponentModel;
using Mxp.Core.Business;
using Mxp.Core.Helpers;

namespace Mxp.iOS
{
	[Register("IconLegendCell")]
	public partial class IconLegendCell : UITableViewCell
	{
		public static readonly UINib Nib = UINib.FromName ("IconLegendCell", NSBundle.MainBundle);
		public static readonly NSString Key = new NSString ("IconLegendCell");

		private IconLegend _iconLegend;
		public IconLegend IconLegend {
			get {
				return this._iconLegend;
			}
			set {
				this._iconLegend = value;

				if (this._iconLegend != null)
					this.RefreshContent ();
			}
		}

		public IconLegendCell (IntPtr handle) : base (handle) {

		}

		public static IconLegendCell Create () {
			return (IconLegendCell)Nib.Instantiate (null, null) [0];
		}

		public void RefreshContent () {
			switch (this.IconLegend.Icon) {
				case IconLegend.IconsEnum.Compliant:
					this.Icon.Image = UIImage.FromBundle ("ExpenseIsCompliant");
					break;
				case IconLegend.IconsEnum.NotCompliantPolicy:
					this.Icon.Image = UIImage.FromBundle ("ExpenseNotCompliantPolicy");
					break;
				case IconLegend.IconsEnum.NotCompliant:
					this.Icon.Image = UIImage.FromBundle ("ExpenseNotCompliant");
					break;
				case IconLegend.IconsEnum.ReceiptsAttached:
					this.Icon.Image = UIImage.FromBundle ("ReportIsPending");
					break;

				case IconLegend.IconsEnum.Accepted:
					this.Icon.Image = UIImage.FromBundle ("ReportAcceptedByController");
					break;
				case IconLegend.IconsEnum.Rejected:
					this.Icon.Image = UIImage.FromBundle ("ReportRejectedByController");
					break;
				case IconLegend.IconsEnum.Pending:
					this.Icon.Image = UIImage.FromBundle ("ReportIsPending");
					break;

				case IconLegend.IconsEnum.Approved:
					this.Icon.Image = UIImage.FromBundle ("ReportHasBeenApproved");
					break;
				case IconLegend.IconsEnum.Refused:
					this.Icon.Image = UIImage.FromBundle ("ReportHasBeenRefused");
					break;
				case IconLegend.IconsEnum.PendingSchedule:
					this.Icon.Image = UIImage.FromBundle ("ReportApprovalIsPending");
					break;
			}

			this.Legend.Text = this.IconLegend.Legend;
		}
			
		protected override void Dispose (bool disposing) {
			base.Dispose (disposing);
			this.IconLegend = null;
		}
	}
}