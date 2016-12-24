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
	[Register ("AddReceiptCell")]
	partial class AddReceiptCell
	{
		[Outlet]
		UIKit.UIView ButtonAddReceipt { get; set; }

		[Outlet]
		UIKit.UILabel FirstPartLabel { get; set; }

		[Outlet]
		UIKit.UILabel messageLabel { get; set; }

		[Outlet]
		UIKit.UILabel SecondPartLabel { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (ButtonAddReceipt != null) {
				ButtonAddReceipt.Dispose ();
				ButtonAddReceipt = null;
			}

			if (FirstPartLabel != null) {
				FirstPartLabel.Dispose ();
				FirstPartLabel = null;
			}

			if (SecondPartLabel != null) {
				SecondPartLabel.Dispose ();
				SecondPartLabel = null;
			}

			if (messageLabel != null) {
				messageLabel.Dispose ();
				messageLabel = null;
			}
		}
	}
}
