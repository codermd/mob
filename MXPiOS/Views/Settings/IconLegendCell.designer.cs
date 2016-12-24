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
	partial class IconLegendCell
	{
		[Outlet]
		UIKit.UIImageView Icon { get; set; }

		[Outlet]
		UIKit.UILabel Legend { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (Icon != null) {
				Icon.Dispose ();
				Icon = null;
			}

			if (Legend != null) {
				Legend.Dispose ();
				Legend = null;
			}
		}
	}
}
