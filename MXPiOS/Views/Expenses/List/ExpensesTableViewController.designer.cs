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
	[Register ("ExpensesTableViewController")]
	partial class ExpensesTableViewController
	{
		[Outlet]
		UIKit.UISegmentedControl SegmentedControl { get; set; }

		[Outlet]
		UIKit.UITableView TableView { get; set; }

		[Action ("ChangeSegmentedControl:")]
		partial void ChangeSegmentedControl (UIKit.UISegmentedControl sender);

		[Action ("ClickOnAddButton:")]
		partial void ClickOnAddButton (UIKit.UIBarButtonItem sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (SegmentedControl != null) {
				SegmentedControl.Dispose ();
				SegmentedControl = null;
			}

			if (TableView != null) {
				TableView.Dispose ();
				TableView = null;
			}
		}
	}
}
