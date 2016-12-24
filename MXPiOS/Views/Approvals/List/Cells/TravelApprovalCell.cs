using System;
using CoreGraphics;

using Foundation;
using UIKit;
using Mxp.Core.Business;

namespace Mxp.iOS
{
	public partial class TravelApprovalCell : UITableViewCell
	{
		public static readonly UINib Nib = UINib.FromName ("TravelApprovalCell", NSBundle.MainBundle);
		public static readonly NSString Key = new NSString ("TravelApprovalCell");

		public TravelApproval TravelApproval;

		public TravelApprovalCell (IntPtr handle) : base (handle)
		{
		}

		public static TravelApprovalCell Create ()
		{
			return (TravelApprovalCell)Nib.Instantiate (null, null) [0];
		}

		public void setTravelApproval(TravelApproval approval){
			this.TravelApproval = approval;
			this.refreshView ();
		}

		public void refreshView() {
			this.TitleLabel.Text = this.TravelApproval.VEmployeeFullname;
			this.CommentLabel.Text = this.TravelApproval.Travel.Name;
			this.DateRangeLabel.Text = this.TravelApproval.VDateRange;

			this.DateRangeLabel.TextColor = UIColor.FromRGB (0, 168, 198);

			this.BackgroundTravelApproval.Layer.ShadowColor = UIColor.FromRGBA (0, 0, 0, 60).CGColor;
			this.BackgroundTravelApproval.Layer.ShadowOffset = new CGSize (0, 1);
			this.BackgroundTravelApproval.Layer.ShadowOpacity = 2.0f;
			this.BackgroundTravelApproval.Layer.ShadowRadius = 1;
			this.BackgroundTravelApproval.Layer.CornerRadius = 2;
		}
	}
}