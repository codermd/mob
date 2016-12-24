using CoreGraphics;

using Foundation;
using UIKit;
using Mxp.Core.Business;
using System;

namespace Mxp.iOS
{
	public partial class ReportApprovalCell : UITableViewCell
	{
		public static readonly UINib Nib = UINib.FromName ("ReportApprovalCell", NSBundle.MainBundle);
		public static readonly NSString Key = new NSString ("ReportApprovalCell");

		public ReportApproval Approval;

		public ReportApprovalCell (IntPtr handle) : base (handle)
		{
		}

		public static ReportApprovalCell Create ()
		{
			return (ReportApprovalCell)Nib.Instantiate (null, null) [0];
		}

		public void setApproval(ReportApproval approval){
			this.Approval = approval;
			this.refreshView ();
		}

		public void refreshView(){
			this.TitleLabel.Text = Approval.VEmployeeFullname;
			this.CommentLabel.Text = Approval.Report.Name;

			this.PriceLabel.Text = Approval.Report.VAmount;
			this.DateRangeLabel.Text = Approval.VDateRange;
			this.DateRangeLabel.TextColor = UIColor.FromRGB (0, 168, 198);

			this.ReportApprovalBackground.Layer.ShadowColor = UIColor.FromRGBA (0, 0, 0, 60).CGColor;
			this.ReportApprovalBackground.Layer.ShadowOffset = new CGSize (0, 1);
			this.ReportApprovalBackground.Layer.ShadowOpacity = 2.0f;
			this.ReportApprovalBackground.Layer.ShadowRadius = 1;
			this.ReportApprovalBackground.Layer.CornerRadius = 2;
		}

	}
}