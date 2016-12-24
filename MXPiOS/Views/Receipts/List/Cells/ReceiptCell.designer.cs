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
	[Register ("ReceiptCell")]
	partial class ReceiptCell
	{
		[Outlet]
		UIKit.UIImageView ReceiptImage { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (ReceiptImage != null) {
				ReceiptImage.Dispose ();
				ReceiptImage = null;
			}
		}
	}
}
