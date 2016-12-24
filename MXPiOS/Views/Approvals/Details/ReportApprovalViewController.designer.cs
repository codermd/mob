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
	[Register ("ReportApprovalViewController")]
	partial class ReportApprovalViewController
	{
		[Outlet]
		UIKit.UIButton AcceptButton { get; set; }

		[Outlet]
		UIKit.UIView AcceptButtonBackground { get; set; }

		[Outlet]
		UIKit.UILabel AcceptedLabel { get; set; }

		[Outlet]
		UIKit.UILabel AcceptedNumbr { get; set; }

		[Outlet]
		UIKit.UIButton CancelButton { get; set; }

		[Outlet]
		UIKit.UILabel CommentLabel { get; set; }

		[Outlet]
		UIKit.UITextView CommentTextView { get; set; }

		[Outlet]
		UIKit.UILabel RejectedLabel { get; set; }

		[Outlet]
		UIKit.UILabel RejectedNumber { get; set; }

		[Outlet]
		UIKit.UIScrollView ScrollView { get; set; }

		[Outlet]
		UIKit.UILabel TitleLabel { get; set; }

		[Action ("ClickOnAccept:")]
		partial void ClickOnAccept (Foundation.NSObject sender);

		[Action ("ClickOnCancel:")]
		partial void ClickOnCancel (Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (AcceptButton != null) {
				AcceptButton.Dispose ();
				AcceptButton = null;
			}

			if (AcceptedLabel != null) {
				AcceptedLabel.Dispose ();
				AcceptedLabel = null;
			}

			if (AcceptedNumbr != null) {
				AcceptedNumbr.Dispose ();
				AcceptedNumbr = null;
			}

			if (CancelButton != null) {
				CancelButton.Dispose ();
				CancelButton = null;
			}

			if (CommentLabel != null) {
				CommentLabel.Dispose ();
				CommentLabel = null;
			}

			if (CommentTextView != null) {
				CommentTextView.Dispose ();
				CommentTextView = null;
			}

			if (RejectedLabel != null) {
				RejectedLabel.Dispose ();
				RejectedLabel = null;
			}

			if (RejectedNumber != null) {
				RejectedNumber.Dispose ();
				RejectedNumber = null;
			}

			if (ScrollView != null) {
				ScrollView.Dispose ();
				ScrollView = null;
			}

			if (TitleLabel != null) {
				TitleLabel.Dispose ();
				TitleLabel = null;
			}

			if (AcceptButtonBackground != null) {
				AcceptButtonBackground.Dispose ();
				AcceptButtonBackground = null;
			}
		}
	}
}
