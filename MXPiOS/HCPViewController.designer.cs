// WARNING
//
// This file has been generated automatically by Xamarin Studio Indie to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace MXPiOS
{
	[Register ("HCPViewController")]
	partial class HCPViewController
	{
		[Outlet]
		UIKit.UIButton SearchButton { get; set; }

		[Outlet]
		UIKit.UITableView TableView { get; set; }

		[Action ("ClickOnSearch:")]
		partial void ClickOnSearch (Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (SearchButton != null) {
				SearchButton.Dispose ();
				SearchButton = null;
			}

			if (TableView != null) {
				TableView.Dispose ();
				TableView = null;
			}
		}
	}
}
