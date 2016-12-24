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
	[Register ("CurrencyFieldCell")]
	partial class CurrencyFieldCell
	{
		[Outlet]
		UIKit.UILabel CurrencyTitle { get; set; }

		[Outlet]
		UIKit.UILabel CurrencyValue { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (CurrencyTitle != null) {
				CurrencyTitle.Dispose ();
				CurrencyTitle = null;
			}

			if (CurrencyValue != null) {
				CurrencyValue.Dispose ();
				CurrencyValue = null;
			}
		}
	}
}
