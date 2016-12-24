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
	[Register ("BusinessFormViewController")]
	partial class BusinessFormViewController
	{
		[Outlet]
		UIKit.UILabel Company { get; set; }

		[Outlet]
		UIKit.UITextField CompanyField { get; set; }

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

		[Action ("companyNameChange:")]
		partial void companyNameChange (Foundation.NSObject sender);

		[Action ("FirstnameChange:")]
		partial void FirstnameChange (Foundation.NSObject sender);

		[Action ("LastnameChange:")]
		partial void LastnameChange (Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (Company != null) {
				Company.Dispose ();
				Company = null;
			}

			if (CompanyField != null) {
				CompanyField.Dispose ();
				CompanyField = null;
			}

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
