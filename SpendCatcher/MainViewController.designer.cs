// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace sc
{
	[Register ("MainViewController")]
	partial class MainViewController
	{
		[Outlet]
		UIKit.UIButton CancelButton { get; set; }

		[Outlet]
		UIKit.UIView ContentView { get; set; }

		[Outlet]
		UIKit.UIActivityIndicatorView Loading { get; set; }

		[Outlet]
		UIKit.UITextView LogTextArea { get; set; }

		[Outlet]
		UIKit.UIPageControl PageControl { get; set; }

		[Outlet]
		UIKit.UIScrollView Scrollview { get; set; }

		[Outlet]
		UIKit.UIView SpendCatcherSelectorView { get; set; }

		[Outlet]
		UIKit.UILabel SuccessAddLabel { get; set; }

		[Outlet]
		UIKit.UIButton SuccessCloseButton { get; set; }

		[Outlet]
		UIKit.UIButton SuccessShowButton { get; set; }

		[Outlet]
		UIKit.UIView SuccessView { get; set; }

		[Outlet]
		UIKit.UIView TransparentView { get; set; }

		[Outlet]
		UIKit.UIButton UploadButton { get; set; }

		[Action ("ClickOnCancel:")]
		partial void ClickOnCancel (Foundation.NSObject sender);

		[Action ("ClickOnShow:")]
		partial void ClickOnShow (Foundation.NSObject sender);

		[Action ("ClickOnUpload:")]
		partial void ClickOnUpload (Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (ContentView != null) {
				ContentView.Dispose ();
				ContentView = null;
			}

			if (Loading != null) {
				Loading.Dispose ();
				Loading = null;
			}

			if (LogTextArea != null) {
				LogTextArea.Dispose ();
				LogTextArea = null;
			}

			if (PageControl != null) {
				PageControl.Dispose ();
				PageControl = null;
			}

			if (Scrollview != null) {
				Scrollview.Dispose ();
				Scrollview = null;
			}

			if (SpendCatcherSelectorView != null) {
				SpendCatcherSelectorView.Dispose ();
				SpendCatcherSelectorView = null;
			}

			if (SuccessAddLabel != null) {
				SuccessAddLabel.Dispose ();
				SuccessAddLabel = null;
			}

			if (SuccessCloseButton != null) {
				SuccessCloseButton.Dispose ();
				SuccessCloseButton = null;
			}

			if (SuccessShowButton != null) {
				SuccessShowButton.Dispose ();
				SuccessShowButton = null;
			}

			if (SuccessView != null) {
				SuccessView.Dispose ();
				SuccessView = null;
			}

			if (TransparentView != null) {
				TransparentView.Dispose ();
				TransparentView = null;
			}

			if (UploadButton != null) {
				UploadButton.Dispose ();
				UploadButton = null;
			}

			if (CancelButton != null) {
				CancelButton.Dispose ();
				CancelButton = null;
			}
		}
	}
}
