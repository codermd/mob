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
	[Register ("SettingsViewController")]
	partial class SettingsViewController
	{
		[Outlet]
		UIKit.UIButton BrowseDesktopVersionButton { get; set; }

		[Outlet]
		UIKit.UIButton IconsLegendButton { get; set; }

		[Outlet]
		UIKit.UIButton LogoutButton { get; set; }

		[Outlet]
		UIKit.UILabel VersionLabel { get; set; }

		[Action ("ClickOnBrowserVersion:")]
		partial void ClickOnBrowserVersion (UIKit.UIButton sender);

		[Action ("ClickOnLogout:")]
		partial void ClickOnLogout (UIKit.UIButton sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (IconsLegendButton != null) {
				IconsLegendButton.Dispose ();
				IconsLegendButton = null;
			}

			if (BrowseDesktopVersionButton != null) {
				BrowseDesktopVersionButton.Dispose ();
				BrowseDesktopVersionButton = null;
			}

			if (LogoutButton != null) {
				LogoutButton.Dispose ();
				LogoutButton = null;
			}

			if (VersionLabel != null) {
				VersionLabel.Dispose ();
				VersionLabel = null;
			}
		}
	}
}
