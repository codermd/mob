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
	[Register ("SelectAllCell")]
	partial class SelectAllCell
	{
		[Outlet]
		UIKit.UIView ButtonSelectAllLabel { get; set; }

		[Outlet]
		UIKit.UILabel SelectAllLabel { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (SelectAllLabel != null) {
				SelectAllLabel.Dispose ();
				SelectAllLabel = null;
			}

			if (ButtonSelectAllLabel != null) {
				ButtonSelectAllLabel.Dispose ();
				ButtonSelectAllLabel = null;
			}
		}
	}
}
