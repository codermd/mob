// WARNING
//
// This file has been generated automatically by Xamarin Studio Indie to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace Mxp.iOS
{
	[Register ("IPhoneHCPAttendeeCell")]
	partial class IPhoneHCPAttendeeCell
	{
		[Outlet]
		UIKit.UILabel AdressLabel { get; set; }

		[Outlet]
		UIKit.UILabel CountryLabel { get; set; }

		[Outlet]
		UIKit.UILabel TitleLabel { get; set; }

		[Outlet]
		UIKit.UILabel ZipLabel { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (TitleLabel != null) {
				TitleLabel.Dispose ();
				TitleLabel = null;
			}

			if (ZipLabel != null) {
				ZipLabel.Dispose ();
				ZipLabel = null;
			}

			if (AdressLabel != null) {
				AdressLabel.Dispose ();
				AdressLabel = null;
			}

			if (CountryLabel != null) {
				CountryLabel.Dispose ();
				CountryLabel = null;
			}
		}
	}
}
