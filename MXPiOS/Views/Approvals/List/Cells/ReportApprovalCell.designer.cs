// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace Mxp.iOS
{
	[Register ("ReportApprovalCell")]
	partial class ReportApprovalCell
	{
		[Outlet]
		UIKit.UILabel CommentLabel { get; set; }

		[Outlet]
		UIKit.UILabel DateRangeLabel { get; set; }

		[Outlet]
		UIKit.UILabel PriceLabel { get; set; }

		[Outlet]
		UIKit.UIView ReportApprovalBackground { get; set; }

		[Outlet]
		UIKit.UILabel TitleLabel { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (ReportApprovalBackground != null) {
				ReportApprovalBackground.Dispose ();
				ReportApprovalBackground = null;
			}

			if (CommentLabel != null) {
				CommentLabel.Dispose ();
				CommentLabel = null;
			}

			if (DateRangeLabel != null) {
				DateRangeLabel.Dispose ();
				DateRangeLabel = null;
			}

			if (PriceLabel != null) {
				PriceLabel.Dispose ();
				PriceLabel = null;
			}

			if (TitleLabel != null) {
				TitleLabel.Dispose ();
				TitleLabel = null;
			}
		}
	}
}
