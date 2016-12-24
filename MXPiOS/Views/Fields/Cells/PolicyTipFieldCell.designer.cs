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
	[Register ("PolicyTipFieldCell")]
	partial class PolicyTipFieldCell
	{
		[Outlet]
		UIKit.UILabel PolicityTipLabel { get; set; }

		[Outlet]
		UIKit.UIImageView PolicyImage { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (PolicityTipLabel != null) {
				PolicityTipLabel.Dispose ();
				PolicityTipLabel = null;
			}

			if (PolicyImage != null) {
				PolicyImage.Dispose ();
				PolicyImage = null;
			}
		}
	}
}
