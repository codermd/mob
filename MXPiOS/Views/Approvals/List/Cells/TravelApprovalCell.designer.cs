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
	[Register ("TravelApprovalCell")]
	partial class TravelApprovalCell
	{
		[Outlet]
		UIKit.UIView BackgroundTravelApproval { get; set; }

		[Outlet]
		UIKit.UILabel CommentLabel { get; set; }

		[Outlet]
		UIKit.UILabel DateRangeLabel { get; set; }

		[Outlet]
		UIKit.UILabel TitleLabel { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (CommentLabel != null) {
				CommentLabel.Dispose ();
				CommentLabel = null;
			}

			if (DateRangeLabel != null) {
				DateRangeLabel.Dispose ();
				DateRangeLabel = null;
			}

			if (TitleLabel != null) {
				TitleLabel.Dispose ();
				TitleLabel = null;
			}

			if (BackgroundTravelApproval != null) {
				BackgroundTravelApproval.Dispose ();
				BackgroundTravelApproval = null;
			}
		}
	}
}
