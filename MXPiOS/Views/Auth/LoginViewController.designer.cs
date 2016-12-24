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
	[Register ("LoginViewController")]
	partial class LoginViewController
	{
		[Outlet]
		UIKit.UIButton ButtonLogin { get; set; }

		[Outlet]
		UIKit.UIButton CompanyButtonLogin { get; set; }

		[Outlet]
		UIKit.UIView companyLoginView { get; set; }

		[Outlet]
		UIKit.UISwitch companyRememberMeSwitch { get; set; }

		[Outlet]
		UIKit.UITextField emailField { get; set; }

		[Outlet]
		UIKit.UITextField passwordField { get; set; }

		[Outlet]
		UIKit.UISwitch rememberMeSwitch { get; set; }

		[Outlet]
		UIKit.UIScrollView ScrollView { get; set; }

		[Outlet]
		UIKit.UISegmentedControl segmentedControl { get; set; }

		[Outlet]
		UIKit.UIView userLoginView { get; set; }

		[Outlet]
		UIKit.UITextField usernameField { get; set; }

		[Action ("ChangeRememberPassword:")]
		partial void ChangeRememberPassword (UIKit.UISwitch sender);

		[Action ("clickOnLogin:")]
		partial void clickOnLogin (UIKit.UIButton sender);

		[Action ("clickOnLoginThroughCompany:")]
		partial void clickOnLoginThroughCompany (UIKit.UIButton sender);

		[Action ("ClickOnResetPassword:")]
		partial void ClickOnResetPassword (UIKit.UIButton sender);

		[Action ("SegmentedControlChanged:")]
		partial void SegmentedControlChanged (UIKit.UISegmentedControl sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (ButtonLogin != null) {
				ButtonLogin.Dispose ();
				ButtonLogin = null;
			}

			if (CompanyButtonLogin != null) {
				CompanyButtonLogin.Dispose ();
				CompanyButtonLogin = null;
			}

			if (companyLoginView != null) {
				companyLoginView.Dispose ();
				companyLoginView = null;
			}

			if (companyRememberMeSwitch != null) {
				companyRememberMeSwitch.Dispose ();
				companyRememberMeSwitch = null;
			}

			if (emailField != null) {
				emailField.Dispose ();
				emailField = null;
			}

			if (passwordField != null) {
				passwordField.Dispose ();
				passwordField = null;
			}

			if (rememberMeSwitch != null) {
				rememberMeSwitch.Dispose ();
				rememberMeSwitch = null;
			}

			if (ScrollView != null) {
				ScrollView.Dispose ();
				ScrollView = null;
			}

			if (segmentedControl != null) {
				segmentedControl.Dispose ();
				segmentedControl = null;
			}

			if (userLoginView != null) {
				userLoginView.Dispose ();
				userLoginView = null;
			}

			if (usernameField != null) {
				usernameField.Dispose ();
				usernameField = null;
			}
		}
	}
}
