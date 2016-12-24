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
	[Register ("SpendCatcherViewController")]
	partial class SpendCatcherViewController
	{
		[Outlet]
		UIKit.NSLayoutConstraint ImageHeightConstraint { get; set; }

		[Outlet]
		UIKit.UIImageView ImageView { get; set; }

		[Outlet]
		UIKit.NSLayoutConstraint ImageWidthConstraint { get; set; }

		[Outlet]
		UIKit.UIScrollView ScrollView { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (ImageHeightConstraint != null) {
				ImageHeightConstraint.Dispose ();
				ImageHeightConstraint = null;
			}

			if (ImageView != null) {
				ImageView.Dispose ();
				ImageView = null;
			}

			if (ImageWidthConstraint != null) {
				ImageWidthConstraint.Dispose ();
				ImageWidthConstraint = null;
			}

			if (ScrollView != null) {
				ScrollView.Dispose ();
				ScrollView = null;
			}
		}
	}
}
