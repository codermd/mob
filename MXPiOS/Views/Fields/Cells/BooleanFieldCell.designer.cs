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
	[Register ("BooleanFieldCell")]
	partial class BooleanFieldCell
	{
		[Outlet]
		UIKit.UISwitch Switcher { get; set; }

		[Outlet]
		UIKit.UILabel TitleLabel { get; set; }

		[Action ("ClickOnSwitch:")]
		partial void ClickOnSwitch (Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (Switcher != null) {
				Switcher.Dispose ();
				Switcher = null;
			}

			if (TitleLabel != null) {
				TitleLabel.Dispose ();
				TitleLabel = null;
			}
		}
	}
}
