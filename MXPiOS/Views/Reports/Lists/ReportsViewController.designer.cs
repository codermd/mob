// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using UIKit;
using System;
using System.CodeDom.Compiler;

namespace Mxp.iOS
{
	[Register ("ReportsViewController")]
	partial class ReportsViewController
	{
		[Outlet]
		UIKit.UISegmentedControl SegmentedFilter { get; set; }

		[Outlet]
		UIKit.UITableView TableView { get; set; }

		[Action ("ClickOnAdd:")]
		[GeneratedCode ("iOS Designer", "1.0")]
		partial void ClickOnAdd (UIBarButtonItem sender);

		[Action ("newSelectedFilter:")]
		[GeneratedCode ("iOS Designer", "1.0")]
		partial void newSelectedFilter (UISegmentedControl sender);

		void ReleaseDesignerOutlets ()
		{
		}
	}
}
