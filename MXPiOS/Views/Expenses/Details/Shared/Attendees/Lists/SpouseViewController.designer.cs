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
	[Register ("SpouseViewController")]
	partial class SpouseViewController
	{
		[Outlet]
		UIKit.UILabel Firstname { get; set; }

		[Outlet]
		UIKit.UITextField FirstnameField { get; set; }

		[Outlet]
		UIKit.UILabel Lastname { get; set; }

		[Outlet]
		UIKit.UITextField LastnameField { get; set; }

		[Action ("ClickOnCreate:")]
		partial void ClickOnCreate (Foundation.NSObject sender);

		[Action ("ValueChange:")]
		partial void ValueChange (Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (Firstname != null) {
				Firstname.Dispose ();
				Firstname = null;
			}

			if (FirstnameField != null) {
				FirstnameField.Dispose ();
				FirstnameField = null;
			}

			if (Lastname != null) {
				Lastname.Dispose ();
				Lastname = null;
			}

			if (LastnameField != null) {
				LastnameField.Dispose ();
				LastnameField = null;
			}
		}
	}
}
