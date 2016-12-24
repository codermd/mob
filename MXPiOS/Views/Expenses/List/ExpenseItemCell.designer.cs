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
	[Register ("ExpenseItemCell")]
	partial class ExpenseItemCell
	{
		[Outlet]
		UIKit.UILabel Amount1Label { get; set; }

		[Outlet]
		UIKit.UILabel Amount2Label { get; set; }

		[Outlet]
		UIKit.UIView ContainerView { get; set; }

		[Outlet]
		UIKit.UIImageView PolicyRuleImageView { get; set; }

		[Outlet]
		UIKit.UILabel TitleLabel { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (Amount1Label != null) {
				Amount1Label.Dispose ();
				Amount1Label = null;
			}

			if (Amount2Label != null) {
				Amount2Label.Dispose ();
				Amount2Label = null;
			}

			if (ContainerView != null) {
				ContainerView.Dispose ();
				ContainerView = null;
			}

			if (TitleLabel != null) {
				TitleLabel.Dispose ();
				TitleLabel = null;
			}

			if (PolicyRuleImageView != null) {
				PolicyRuleImageView.Dispose ();
				PolicyRuleImageView = null;
			}
		}
	}
}
