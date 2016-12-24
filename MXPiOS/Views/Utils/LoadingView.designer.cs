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
	[Register ("LoadingView")]
	partial class LoadingView
	{
		[Outlet]
		UIKit.UIView Container { get; set; }

		[Outlet]
		UIKit.UIActivityIndicatorView Loader { get; set; }

		[Outlet]
		UIKit.UILabel MessageLabel { get; set; }

		[Outlet]
		UIKit.UILabel LoadingText { get; set; }

		void ReleaseDesignerOutlets ()
		{
			if (LoadingText != null) {
				LoadingText.Dispose ();
				LoadingText = null;
			}

			if (Container != null) {
				Container.Dispose ();
				Container = null;
			}

			if (Loader != null) {
				Loader.Dispose ();
				Loader = null;
			}

			if (MessageLabel != null) {
				MessageLabel.Dispose ();
				MessageLabel = null;
			}
		}
	}
}
