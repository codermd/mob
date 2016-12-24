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
	[Register ("ApprovalsViewController")]
	partial class ApprovalsViewController
	{
		[Outlet]
		UIKit.UISegmentedControl SegmentedControl { get; set; }

		[Outlet]
		UIKit.UITableView TableView { get; set; }

		[Action ("SegmentedControllerChange:")]
		[GeneratedCode ("iOS Designer", "1.0")]
		partial void SegmentedControllerChange (UISegmentedControl sender);

		void ReleaseDesignerOutlets ()
		{
		}
	}
}
