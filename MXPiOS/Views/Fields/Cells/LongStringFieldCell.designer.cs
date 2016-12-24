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
	[Register ("LongStringFieldCell")]
	partial class LongStringFieldCell
	{
		[Outlet]
		UIKit.UILabel LongStringLabel { get; set; }

		[Outlet]
		UIKit.UILabel TitleLabel { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (TitleLabel != null) {
				TitleLabel.Dispose ();
				TitleLabel = null;
			}

			if (LongStringLabel != null) {
				LongStringLabel.Dispose ();
				LongStringLabel = null;
			}
		}
	}
}
