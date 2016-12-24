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
	[Register ("HCUViewController")]
	partial class HCUViewController
	{
		[Outlet]
		UIKit.UILabel Address { get; set; }

		[Outlet]
		UIKit.UITextField AddressField { get; set; }

		[Outlet]
		UIKit.UILabel City { get; set; }

		[Outlet]
		UIKit.UITextField CityField { get; set; }

		[Outlet]
		UIKit.UILabel Company { get; set; }

		[Outlet]
		UIKit.UITextField CompanyField { get; set; }

		[Outlet]
		UIKit.UILabel Firstname { get; set; }

		[Outlet]
		UIKit.UITextField FirstnameField { get; set; }

		[Outlet]
		UIKit.UITextField[] inputs { get; set; }

		[Outlet]
		UIKit.UILabel Lastname { get; set; }

		[Outlet]
		UIKit.UITextField LastnameField { get; set; }

		[Outlet]
		UIKit.UIView MainContainer { get; set; }

		[Outlet]
		UIKit.UILabel NPI { get; set; }

		[Outlet]
		UIKit.UITextField NPIField { get; set; }

		[Outlet]
		UIKit.UIScrollView ScrollView { get; set; }

		[Outlet]
		UIKit.UILabel Speciality { get; set; }

		[Outlet]
		UIKit.UITextField SpecialityField { get; set; }

		[Outlet]
		UIKit.UILabel State { get; set; }

		[Outlet]
		UIKit.UITextField StateField { get; set; }

		[Outlet]
		UIKit.UILabel Zip { get; set; }

		[Outlet]
		UIKit.UITextField ZipField { get; set; }

		[Action ("ClickOnCreate:")]
		partial void ClickOnCreate (Foundation.NSObject sender);

		[Action ("textFieldChange:")]
		partial void textFieldChange (Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (Address != null) {
				Address.Dispose ();
				Address = null;
			}

			if (AddressField != null) {
				AddressField.Dispose ();
				AddressField = null;
			}

			if (City != null) {
				City.Dispose ();
				City = null;
			}

			if (CityField != null) {
				CityField.Dispose ();
				CityField = null;
			}

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

			if (MainContainer != null) {
				MainContainer.Dispose ();
				MainContainer = null;
			}

			if (NPI != null) {
				NPI.Dispose ();
				NPI = null;
			}

			if (NPIField != null) {
				NPIField.Dispose ();
				NPIField = null;
			}

			if (ScrollView != null) {
				ScrollView.Dispose ();
				ScrollView = null;
			}

			if (Speciality != null) {
				Speciality.Dispose ();
				Speciality = null;
			}

			if (SpecialityField != null) {
				SpecialityField.Dispose ();
				SpecialityField = null;
			}

			if (State != null) {
				State.Dispose ();
				State = null;
			}

			if (StateField != null) {
				StateField.Dispose ();
				StateField = null;
			}

			if (Zip != null) {
				Zip.Dispose ();
				Zip = null;
			}

			if (ZipField != null) {
				ZipField.Dispose ();
				ZipField = null;
			}
		}
	}
}
