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
	[Register ("TravelViewController")]
	partial class TravelViewController
	{
		[Outlet]
		UIKit.UISegmentedControl SegmentView { get; set; }

		[Outlet]
		UIKit.UITableView TableView { get; set; }

		[Action ("ClickOnSegment:")]
		partial void ClickOnSegment (Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (SegmentView != null) {
				SegmentView.Dispose ();
				SegmentView = null;
			}

			if (TableView != null) {
				TableView.Dispose ();
				TableView = null;
			}
		}
	}
}
