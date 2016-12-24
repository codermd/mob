// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace MXPiOS
{
	[Register ("ProductsViewController")]
	partial class ProductsViewController
	{
		[Outlet]
		UIKit.UISearchBar SearchBar { get; set; }

		[Outlet]
		UIKit.NSLayoutConstraint SegmentBottomMargin { get; set; }

		[Outlet]
		UIKit.UISegmentedControl SegmentedControl { get; set; }

		[Outlet]
		UIKit.NSLayoutConstraint SegmentHeight { get; set; }

		[Outlet]
		UIKit.NSLayoutConstraint SegmentTopMargin { get; set; }

		[Outlet]
		UIKit.UITableView TableView { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (SearchBar != null) {
				SearchBar.Dispose ();
				SearchBar = null;
			}

			if (SegmentedControl != null) {
				SegmentedControl.Dispose ();
				SegmentedControl = null;
			}

			if (SegmentHeight != null) {
				SegmentHeight.Dispose ();
				SegmentHeight = null;
			}

			if (SegmentTopMargin != null) {
				SegmentTopMargin.Dispose ();
				SegmentTopMargin = null;
			}

			if (SegmentBottomMargin != null) {
				SegmentBottomMargin.Dispose ();
				SegmentBottomMargin = null;
			}

			if (TableView != null) {
				TableView.Dispose ();
				TableView = null;
			}
		}
	}
}
