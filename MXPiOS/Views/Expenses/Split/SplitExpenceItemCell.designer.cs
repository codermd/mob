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
	[Register ("SplitExpenceItemCell")]
	partial class SplitExpenceItemCell
	{
		[Outlet]
		UIKit.UILabel AmountLabel { get; set; }

		[Outlet]
		UIKit.UILabel CategoryLabel { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (CategoryLabel != null) {
				CategoryLabel.Dispose ();
				CategoryLabel = null;
			}

			if (AmountLabel != null) {
				AmountLabel.Dispose ();
				AmountLabel = null;
			}
		}
	}
}
