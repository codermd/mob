// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace Mxp.iOS
{
	[Register ("ReportDetailsViewController")]
	partial class ReportDetailsViewController
	{
		[Outlet]
		UIKit.UISegmentedControl SegmentedControl { get; set; }

		[Outlet]
		UIKit.UIView ViewContainer { get; set; }

		[Action ("actionButton:")]
		partial void actionButton (UIKit.UIBarButtonItem sender);

		[Action ("SelectionChange:")]
		[GeneratedCode ("iOS Designer", "1.0")]
		partial void SelectionChange (UISegmentedControl sender);

		void ReleaseDesignerOutlets ()
		{
		}
	}
}
