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
	partial class ExpenseCell
	{
		[Outlet]
		UIKit.UILabel amountCC { get; set; }

		[Outlet]
		UIKit.UILabel amout { get; set; }

		[Outlet]
		UIKit.UILabel AttendeesBullets { get; set; }

		[Outlet]
		UIKit.NSLayoutConstraint attendeesDocMargin { get; set; }

		[Outlet]
		UIKit.NSLayoutConstraint attendeesWidth { get; set; }

		[Outlet]
		UIKit.UILabel[] Bullets { get; set; }

		[Outlet]
		UIKit.UIImageView cardImage { get; set; }

		[Outlet]
		UIKit.NSLayoutConstraint cardLeftMarginConstraint { get; set; }

		[Outlet]
		UIKit.NSLayoutConstraint CardWidthConstraint { get; set; }

		[Outlet]
		UIKit.UIView container { get; set; }

		[Outlet]
		UIKit.UIImageView countryImage { get; set; }

		[Outlet]
		UIKit.UILabel date { get; set; }

		[Outlet]
		UIKit.UIImageView HasAttendees { get; set; }

		[Outlet]
		UIKit.UIImageView hasReceipts { get; set; }

		[Outlet]
		UIKit.NSLayoutConstraint hasReceiptWidth { get; set; }

		[Outlet]
		UIKit.UIImageView PolicyRuleImageView { get; set; }

		[Outlet]
		UIKit.UILabel ReceiptBullets { get; set; }

		[Outlet]
		UIKit.UIImageView ReportStatus { get; set; }

		[Outlet]
		UIKit.UILabel title { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (amountCC != null) {
				amountCC.Dispose ();
				amountCC = null;
			}

			if (amout != null) {
				amout.Dispose ();
				amout = null;
			}

			if (AttendeesBullets != null) {
				AttendeesBullets.Dispose ();
				AttendeesBullets = null;
			}

			if (attendeesDocMargin != null) {
				attendeesDocMargin.Dispose ();
				attendeesDocMargin = null;
			}

			if (attendeesWidth != null) {
				attendeesWidth.Dispose ();
				attendeesWidth = null;
			}

			if (cardImage != null) {
				cardImage.Dispose ();
				cardImage = null;
			}

			if (cardLeftMarginConstraint != null) {
				cardLeftMarginConstraint.Dispose ();
				cardLeftMarginConstraint = null;
			}

			if (CardWidthConstraint != null) {
				CardWidthConstraint.Dispose ();
				CardWidthConstraint = null;
			}

			if (container != null) {
				container.Dispose ();
				container = null;
			}

			if (countryImage != null) {
				countryImage.Dispose ();
				countryImage = null;
			}

			if (date != null) {
				date.Dispose ();
				date = null;
			}

			if (HasAttendees != null) {
				HasAttendees.Dispose ();
				HasAttendees = null;
			}

			if (hasReceipts != null) {
				hasReceipts.Dispose ();
				hasReceipts = null;
			}

			if (hasReceiptWidth != null) {
				hasReceiptWidth.Dispose ();
				hasReceiptWidth = null;
			}

			if (PolicyRuleImageView != null) {
				PolicyRuleImageView.Dispose ();
				PolicyRuleImageView = null;
			}

			if (ReceiptBullets != null) {
				ReceiptBullets.Dispose ();
				ReceiptBullets = null;
			}

			if (ReportStatus != null) {
				ReportStatus.Dispose ();
				ReportStatus = null;
			}

			if (title != null) {
				title.Dispose ();
				title = null;
			}
		}
	}
}
