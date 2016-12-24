using System;

using Foundation;
using UIKit;
using Mxp.Core.Business;

namespace Mxp.iOS
{
	public partial class IPadApprovalsSplitViewController : MXPSplitViewController
	{
		public IPadApprovalsSplitViewController (IntPtr handle) : base (handle) {
			
		}

		public override void ViewDidLoad () {
			base.ViewDidLoad ();

			((ApprovalsViewController)this.listViewController).cellSelected += (sender, e) => this.showApproval (e.approval);
		}

		public void showApproval (Approval approval) {
			if (approval == null) {
				this.ShouldShowDetailViewController (this.Storyboard.InstantiateViewController ("NONAPPROVAL"), this);
				return;
			}

			if (approval is ReportApproval) {
				UIStoryboard storyBoard =  UIStoryboard.FromName ("ReportDetails", NSBundle.MainBundle);
				ReportDetailsViewController vc = storyBoard.InstantiateInitialViewController () as ReportDetailsViewController;
				vc.Report = ((ReportApproval)approval).Report;
				this.ShouldShowDetailViewController (vc, this);
			} else if (approval is TravelApproval) {
				TravelViewController vc = new TravelViewController ();
				vc.Travel = ((TravelApproval)approval).Travel;
				this.ShouldShowDetailViewController (vc, this);
			}
		}
	}
}