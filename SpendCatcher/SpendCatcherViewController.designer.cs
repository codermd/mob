// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace SpendCatcher
{
	[Register ("SpendCatcherViewController")]
	partial class SpendCatcherViewController
	{
		[Outlet]
		UIKit.UIButton CountryButton { get; set; }

		[Outlet]
		UIKit.UILabel CountryLabel { get; set; }

		[Outlet]
		UIKit.UIImageView ImageView { get; set; }

		[Outlet]
		UIKit.UIButton ProductButton { get; set; }

		[Outlet]
		UIKit.UILabel ProductLabel { get; set; }

		[Outlet]
		UIKit.UILabel TransactionLabel { get; set; }

		[Outlet]
		UIKit.UISwitch TransactionSwitch { get; set; }

		[Action ("ClickOnCountry:")]
		partial void ClickOnCountry (Foundation.NSObject sender);

		[Action ("ClickOnProduct:")]
		partial void ClickOnProduct (Foundation.NSObject sender);

		[Action ("SwitchTransaction:")]
		partial void SwitchTransaction (Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (CountryButton != null) {
				CountryButton.Dispose ();
				CountryButton = null;
			}

			if (CountryLabel != null) {
				CountryLabel.Dispose ();
				CountryLabel = null;
			}

			if (ImageView != null) {
				ImageView.Dispose ();
				ImageView = null;
			}

			if (ProductButton != null) {
				ProductButton.Dispose ();
				ProductButton = null;
			}

			if (ProductLabel != null) {
				ProductLabel.Dispose ();
				ProductLabel = null;
			}

			if (TransactionLabel != null) {
				TransactionLabel.Dispose ();
				TransactionLabel = null;
			}

			if (TransactionSwitch != null) {
				TransactionSwitch.Dispose ();
				TransactionSwitch = null;
			}
		}
	}
}
