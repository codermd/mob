using System;
using CoreGraphics;
using Foundation;
using UIKit;
using Mxp.Core.Business;

namespace Mxp.iOS
{
	public partial class ReportCell : UITableViewCell
	{
		public static readonly UINib Nib = UINib.FromName ("ReportCell", NSBundle.MainBundle);
		public static readonly NSString Key = new NSString ("ReportCell");

		private Report report;
		public void setReport(Report report) {
			this.report = report;

			this.NameLabel.Text = this.report.Name;
			this.AmoutLabel.Text = this.report.VAmount;

			this.DateRangeLabel.Text = this.report.VDateRange;
			this.DateRangeLabel.TextColor = UIColor.FromRGB (0, 168, 198);

			this.ReportCellBackground.Layer.ShadowColor = UIColor.FromRGBA (0, 0, 0, 60).CGColor;
			this.ReportCellBackground.Layer.ShadowOffset = new CGSize (0, 1);
			this.ReportCellBackground.Layer.ShadowOpacity = 2.0f;
			this.ReportCellBackground.Layer.ShadowRadius = 1;
			this.ReportCellBackground.Layer.CornerRadius = 2;

			this.configurePolicyRule();
			this.configureReportStatus ();
			this.configureDocumentColor ();

		}

		public void configurePolicyRule(){

			if (this.report.PolicyRule.Equals (Report.PolicyRulesEnum.Green) && this.PolicyRuleImage != null) {
				this.PolicyRuleImage.Image = UIImage.FromBundle ("ExpenseIsCompliant");
			}

			if (this.report.PolicyRule.Equals (Report.PolicyRulesEnum.Orange) && this.PolicyRuleImage != null) {
				this.PolicyRuleImage.Image = UIImage.FromBundle ("ExpenseNotCompliantPolicy");
			}

			if (this.report.PolicyRule.Equals (Report.PolicyRulesEnum.Red) && this.PolicyRuleImage != null) {
				this.PolicyRuleImage.Image = UIImage.FromBundle ("ExpenseNotCompliant");
			}

		}

		public void configureReportStatus(){

			this.StatusImage.Hidden = !this.report.CanShowApprovalStatus;

			if(this.report.ApprovalStatus.Equals(Report.ApprovalStatusEnum.Accepted)){
				this.StatusImage.Image = UIImage.FromBundle("ReportHasBeenApproved");
			}
			if(this.report.ApprovalStatus.Equals(Report.ApprovalStatusEnum.Rejected)){
				this.StatusImage.Image = UIImage.FromBundle("ReportHasBeenRefused");
			}
			if(this.report.ApprovalStatus.Equals(Report.ApprovalStatusEnum.Waiting)){
				this.StatusImage.Image = UIImage.FromBundle("ReportApprovalIsPending");
			}
		}

		public void configureDocumentColor(){ 

			this.DocumentImage.Hidden = !this.report.CanShowReceiptStatus;

			if(this.report.ReceiptStatus.Equals(Report.ReceiptStatusEnum.Black)){
				this.DocumentImage.Image = UIImage.FromBundle ("ReportIsPending");
			}
			if(this.report.ReceiptStatus.Equals(Report.ReceiptStatusEnum.Green)){
				this.DocumentImage.Image = UIImage.FromBundle ("ReportAcceptedByController");
			}
			if(this.report.ReceiptStatus.Equals(Report.ReceiptStatusEnum.Orange)){
				this.DocumentImage.Image = UIImage.FromBundle ("ReportHasBeenOrange");
			}
			if(this.report.ReceiptStatus.Equals(Report.ReceiptStatusEnum.Red)){
				this.DocumentImage.Image = UIImage.FromBundle ("ReportRejectedByController");
			}
		}

		public ReportCell (IntPtr handle) : base (handle)
		{

		}

		public static ReportCell Create ()
		{
			return (ReportCell)Nib.Instantiate (null, null) [0];
		}
	}
}