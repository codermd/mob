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
	[Register ("AttendeeItemCell")]
	partial class AttendeeItemCell
	{
		[Outlet]
		UIKit.UIView ContainerView { get; set; }

		[Outlet]
		UIKit.UILabel PriceLabel { get; set; }

		[Outlet]
		UIKit.UIButton ShownButton { get; set; }

		[Outlet]
		UIKit.UILabel TitleLabel { get; set; }

		[Outlet]
		UIKit.UILabel TypeLabel { get; set; }

		[Action ("ClickOnAttendee:")]
		partial void ClickOnAttendee (UIKit.UIButton sender);

		[Action ("ClickOnCell:")]
		partial void ClickOnCell (Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (ContainerView != null) {
				ContainerView.Dispose ();
				ContainerView = null;
			}

			if (PriceLabel != null) {
				PriceLabel.Dispose ();
				PriceLabel = null;
			}

			if (TitleLabel != null) {
				TitleLabel.Dispose ();
				TitleLabel = null;
			}

			if (TypeLabel != null) {
				TypeLabel.Dispose ();
				TypeLabel = null;
			}

			if (ShownButton != null) {
				ShownButton.Dispose ();
				ShownButton = null;
			}
		}
	}
}
