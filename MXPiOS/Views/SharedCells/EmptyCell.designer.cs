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
	[Register ("EmptyCell")]
	partial class EmptyCell
	{
		[Outlet]
		UIKit.UILabel EmptyLabel { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (EmptyLabel != null) {
				EmptyLabel.Dispose ();
				EmptyLabel = null;
			}
		}
	}
}
