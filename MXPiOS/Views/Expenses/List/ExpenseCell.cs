using System;
using CoreGraphics;
using UIKit;
using Foundation;
using System.Globalization;
using System.ComponentModel;
using Mxp.Core.Business;

namespace Mxp.iOS
{

	[Register("ExpenseCell")]
	public partial class ExpenseCell : UITableViewCell
	{
		private Expense expense;

		public static readonly UINib Nib = UINib.FromName ("ExpenseCell", NSBundle.MainBundle);

		public static readonly NSString Key = new NSString ("ExpenseCell");

		public ExpenseCell (IntPtr handle) : base (handle)
		{

		}

		public static ExpenseCell Create ()
		{
			return (ExpenseCell)Nib.Instantiate (null, null) [0];
		}

		public override void AwakeFromNib ()
		{
			base.AwakeFromNib ();

			this.container.Layer.ShadowColor = UIColor.FromRGBA (0, 0, 0, 60).CGColor;
			this.container.Layer.ShadowOffset = new CGSize (0, 1);
			this.container.Layer.ShadowOpacity = 2.0f;
			this.container.Layer.ShadowRadius = 1;
			this.container.Layer.CornerRadius = 2;

			foreach (UILabel label in this.Bullets) {
				label.Layer.BackgroundColor = UIColor.LightGray.CGColor;
				label.Layer.CornerRadius = label.Frame.Size.Width / 2;
				label.TextColor = UIColor.White;
			}

		}

		public void refreshContent () {
			this.title.TextColor = this.expense.IsHighlighted ? UIColor.Red : UIColor.FromRGB (38, 48, 60);
			this.title.Lines = this.expense.AreCurrenciesDifferent ? 2 : 1;
			this.title.Text = this.expense.VTitle;
			this.amout.Text = this.expense.VAmountLC;
			this.amountCC.Text = this.expense.AreCurrenciesDifferent ? this.expense.VAmountCC : "";
			this.date.Text = this.expense.VDate;

			this.date.TextColor = UIColor.FromRGB(0,168,198);

			if (this.expense.Country != null) {
				this.countryImage.Image = UIImage.FromBundle (this.expense.Country.VFormattedResourceName);
				if (this.countryImage.Image == null) {
					this.countryImage.Image = UIImage.FromBundle ("NoFlag.png");
				}
			}


			if (!this.expense.HasAttendees) {
				this.attendeesWidth.Constant = 0;
				this.attendeesDocMargin.Constant = 0;
				this.AttendeesBullets.Layer.Opacity = 0;
			} else {
				this.attendeesWidth.Constant = 20;
				this.attendeesDocMargin.Constant = 7;
				this.AttendeesBullets.Layer.Opacity = 1;
				this.AttendeesBullets.Layer.BackgroundColor = UIColor.FromRGB(0,168,198).CGColor;
				this.AttendeesBullets.Text = this.expense.NumberOfAttendees.ToString();
				this.AttendeesBullets.Font = UIFont.FromName ("Avenir", 11);
			}

			if (!this.expense.HasReceipts || !this.expense.CanShowReceiptIcon) {
				this.hasReceipts.Layer.Opacity = 0;
				this.ReceiptBullets.Layer.Opacity = 0;
				this.hasReceiptWidth.Constant = 0;
			} else {
				this.hasReceiptWidth.Constant = 20;
				this.hasReceipts.Layer.Opacity = 1;
				this.ReceiptBullets.Layer.Opacity = 1;
				this.ReceiptBullets.Text = this.expense.NumberReceipts.ToString();
				this.ReceiptBullets.Layer.BackgroundColor = UIColor.FromRGB(0,168,198).CGColor;
				this.ReceiptBullets.Font = UIFont.FromName ("Avenir", 11);
			}

			if (this.expense.IsTempTransaction) {
				this.cardImage.Image = UIImage.FromBundle ("CreditCardIcon.png");
				this.CardWidthConstraint.Constant = 20;
				this.cardLeftMarginConstraint.Constant = 1;
			} else if (this.expense.IsPaidByCreditCard) {
				this.cardImage.Image = UIImage.FromBundle("ExpensesIconTab.png");
				this.CardWidthConstraint.Constant = 20;
				this.cardLeftMarginConstraint.Constant = 1;
			} else {
				this.CardWidthConstraint.Constant = 0;
				this.cardLeftMarginConstraint.Constant = 0;
			}

			if (this.expense.CanShowExpenseReportStatus) {
				this.ReportStatus.Hidden = false;
				if (this.expense.ExpenseItems [0].MainStatus == ExpenseItem.Status.Accepted) {
					this.ReportStatus.Image = UIImage.FromBundle("ReportHasBeenApproved");

				} else if (this.expense.ExpenseItems [0].MainStatus == ExpenseItem.Status.Rejected) {
					this.ReportStatus.Image = UIImage.FromBundle("ReportHasBeenRefused");
				} else {
					this.ReportStatus.Image = UIImage.FromBundle("ReportApprovalIsPending");

				}
			} else {
				this.ReportStatus.Hidden = true;
			}

			this.SetNeedsUpdateConstraints ();
			this.LayoutIfNeeded ();

			if (this.expense.PolicyRule.Equals (ExpenseItem.PolicyRules.Green)) {
				this.PolicyRuleImageView.Image = UIImage.FromBundle("ExpenseIsCompliant");
				this.PolicyRuleImageView.ContentMode = UIViewContentMode.ScaleAspectFit;
			}

			if (this.expense.PolicyRule.Equals (ExpenseItem.PolicyRules.Orange)) {
				this.PolicyRuleImageView.Image = UIImage.FromBundle("ExpenseNotCompliantPolicy");
				this.PolicyRuleImageView.ContentMode = UIViewContentMode.ScaleAspectFit;
			}

			if (this.expense.PolicyRule.Equals (ExpenseItem.PolicyRules.Red)) {
				this.PolicyRuleImageView.Image = UIImage.FromBundle("ExpenseNotCompliant");
				this.PolicyRuleImageView.ContentMode = UIViewContentMode.ScaleAspectFit;
			}
		}

		public override void SetSelected (bool selected, bool animated)
		{
			base.SetSelected (selected, animated);
			this.ReceiptBullets.Layer.BackgroundColor = UIColor.FromRGB(0,168,198).CGColor;
			this.AttendeesBullets.Layer.BackgroundColor = UIColor.FromRGB(0,168,198).CGColor;
		}

		public override void SetHighlighted (bool highlighted, bool animated)
		{
			base.SetHighlighted (highlighted, animated);
			this.ReceiptBullets.Layer.BackgroundColor = UIColor.FromRGB(0,168,198).CGColor;
			this.AttendeesBullets.Layer.BackgroundColor = UIColor.FromRGB(0,168,198).CGColor;
		}


		public void setExpense (Expense anExpense) {

			if(this.expense != null) {
				this.expense.PropertyChanged -= HandlePropertyChanged;

				if (this.expense.ExpenseItems.Count != 0) {
					this.expense.ExpenseItems [0].PropertyChanged -= HandlePropertyChanged;
				}
			}

			this.expense = anExpense;
			if(this.expense != null) {
				this.expense.PropertyChanged += HandlePropertyChanged;
				if (this.expense.ExpenseItems.Count != 0) {
					this.expense.ExpenseItems[0].PropertyChanged += HandlePropertyChanged;
				}

				this.refreshContent ();
			}
		}

		protected override void Dispose (bool disposing)
		{
			base.Dispose (disposing);
			this.setExpense (null);
		}

		void HandlePropertyChanged (object sender, PropertyChangedEventArgs e)
		{
			this.refreshContent ();
		}
	}
}