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
	[Register ("ApprovalTravelComment")]
	partial class ApprovalTravelComment
	{
		[Outlet]
		UIKit.UIButton CancelButton { get; set; }

		[Outlet]
		UIKit.UILabel CommentLabel { get; set; }

		[Outlet]
		UIKit.UIButton ProcessButton { get; set; }

		[Outlet]
		UIKit.UIScrollView ScrollView { get; set; }

		[Outlet]
		UIKit.UITextView TextView { get; set; }

		[Action ("ClickOnCancel:")]
		partial void ClickOnCancel (Foundation.NSObject sender);

		[Action ("ClickOnProcess:")]
		partial void ClickOnProcess (Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (CancelButton != null) {
				CancelButton.Dispose ();
				CancelButton = null;
			}

			if (CommentLabel != null) {
				CommentLabel.Dispose ();
				CommentLabel = null;
			}

			if (ProcessButton != null) {
				ProcessButton.Dispose ();
				ProcessButton = null;
			}

			if (TextView != null) {
				TextView.Dispose ();
				TextView = null;
			}

			if (ScrollView != null) {
				ScrollView.Dispose ();
				ScrollView = null;
			}
		}
	}
}
