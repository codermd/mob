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
	[Register ("MileageAddSegmentCell")]
	partial class MileageAddSegmentCell
	{
		[Outlet]
		UIKit.UIView AddMapButton { get; set; }

		[Outlet]
		UIKit.UILabel ShowMapText { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (ShowMapText != null) {
				ShowMapText.Dispose ();
				ShowMapText = null;
			}

			if (AddMapButton != null) {
				AddMapButton.Dispose ();
				AddMapButton = null;
			}
		}
	}
}
