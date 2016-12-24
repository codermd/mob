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
	[Register ("ForgotPasswordViewController")]
	partial class ForgotPasswordViewController
	{
		[Outlet]
		UIKit.UIView FormContainer { get; set; }

		[Outlet]
		UIKit.UIButton SendButton { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (FormContainer != null) {
				FormContainer.Dispose ();
				FormContainer = null;
			}

			if (SendButton != null) {
				SendButton.Dispose ();
				SendButton = null;
			}
		}
	}
}
