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
	[Register ("ReportCell")]
	partial class ReportCell
	{
		[Outlet]
		UIKit.UILabel AmoutLabel { get; set; }

		[Outlet]
		UIKit.UILabel DateRangeLabel { get; set; }

		[Outlet]
		UIKit.UIImageView DocumentImage { get; set; }

		[Outlet]
		UIKit.UILabel NameLabel { get; set; }

		[Outlet]
		UIKit.UIImageView PolicyRuleImage { get; set; }

		[Outlet]
		UIKit.UIView ReportCellBackground { get; set; }

		[Outlet]
		UIKit.UIImageView StatusImage { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (AmoutLabel != null) {
				AmoutLabel.Dispose ();
				AmoutLabel = null;
			}

			if (DateRangeLabel != null) {
				DateRangeLabel.Dispose ();
				DateRangeLabel = null;
			}

			if (DocumentImage != null) {
				DocumentImage.Dispose ();
				DocumentImage = null;
			}

			if (NameLabel != null) {
				NameLabel.Dispose ();
				NameLabel = null;
			}

			if (PolicyRuleImage != null) {
				PolicyRuleImage.Dispose ();
				PolicyRuleImage = null;
			}

			if (ReportCellBackground != null) {
				ReportCellBackground.Dispose ();
				ReportCellBackground = null;
			}

			if (StatusImage != null) {
				StatusImage.Dispose ();
				StatusImage = null;
			}
		}
	}
}
