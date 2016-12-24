// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace MXPiOS
{
	[Register ("HistoryCommentCell")]
	partial class HistoryCommentCell
	{
		[Outlet]
		UIKit.NSLayoutConstraint CommentHeightConstraint { get; set; }

		[Outlet]
		UIKit.UILabel CommentLabel { get; set; }

		[Outlet]
		UIKit.UILabel DateLabel { get; set; }

		[Outlet]
		UIKit.NSLayoutConstraint TitleHeightConstraint { get; set; }

		[Outlet]
		UIKit.UILabel TitleLabel { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (CommentLabel != null) {
				CommentLabel.Dispose ();
				CommentLabel = null;
			}

			if (DateLabel != null) {
				DateLabel.Dispose ();
				DateLabel = null;
			}

			if (TitleLabel != null) {
				TitleLabel.Dispose ();
				TitleLabel = null;
			}

			if (TitleHeightConstraint != null) {
				TitleHeightConstraint.Dispose ();
				TitleHeightConstraint = null;
			}

			if (CommentHeightConstraint != null) {
				CommentHeightConstraint.Dispose ();
				CommentHeightConstraint = null;
			}
		}
	}
}
