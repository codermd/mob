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
	[Register ("ReceiptGalleryViewController")]
	partial class ReceiptGalleryViewController
	{
		[Outlet]
		UIKit.UIButton CloseButton { get; set; }

		[Outlet]
		UIKit.UIButton DeleteButton { get; set; }

		[Outlet]
		UIKit.UIImageView ImageView { get; set; }

		[Outlet]
		UIKit.UIButton NextButton { get; set; }

		[Outlet]
		UIKit.UIButton PreviousButton { get; set; }

		[Outlet]
		UIKit.UILabel ReceiptPositionLabel { get; set; }

		[Outlet]
		UIKit.UIScrollView ScrollView { get; set; }

		[Action ("ClickOnClose:")]
		partial void ClickOnClose (Foundation.NSObject sender);

		[Action ("ClickOnDelete:")]
		partial void ClickOnDelete (Foundation.NSObject sender);

		[Action ("ClickOnNext:")]
		partial void ClickOnNext (Foundation.NSObject sender);

		[Action ("ClickOnPrevious:")]
		partial void ClickOnPrevious (Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (CloseButton != null) {
				CloseButton.Dispose ();
				CloseButton = null;
			}

			if (DeleteButton != null) {
				DeleteButton.Dispose ();
				DeleteButton = null;
			}

			if (ImageView != null) {
				ImageView.Dispose ();
				ImageView = null;
			}

			if (NextButton != null) {
				NextButton.Dispose ();
				NextButton = null;
			}

			if (PreviousButton != null) {
				PreviousButton.Dispose ();
				PreviousButton = null;
			}

			if (ScrollView != null) {
				ScrollView.Dispose ();
				ScrollView = null;
			}

			if (ReceiptPositionLabel != null) {
				ReceiptPositionLabel.Dispose ();
				ReceiptPositionLabel = null;
			}
		}
	}
}
