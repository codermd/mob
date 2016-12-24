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
	[Register ("StringFieldCell")]
	partial class StringFieldCell
	{
		[Outlet]
		UIKit.UITextField InputText { get; set; }

		[Outlet]
		UIKit.UIActivityIndicatorView Loader { get; set; }

		[Outlet]
		UIKit.UILabel TitleLabel { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (Loader != null) {
				Loader.Dispose ();
				Loader = null;
			}

			if (InputText != null) {
				InputText.Dispose ();
				InputText = null;
			}

			if (TitleLabel != null) {
				TitleLabel.Dispose ();
				TitleLabel = null;
			}
		}
	}
}
