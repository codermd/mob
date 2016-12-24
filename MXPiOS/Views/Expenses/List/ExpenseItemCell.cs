using System;
using CoreGraphics;

using Foundation;
using UIKit;
using Mxp.Core.Business;

namespace Mxp.iOS
{
	public partial class ExpenseItemCell : UITableViewCell
	{
		public static readonly UINib Nib = UINib.FromName ("ExpenseItemCell", NSBundle.MainBundle);
		public static readonly NSString Key = new NSString ("ExpenseItemCell");


		public static ExpenseItemCell Create ()
		{
			return (ExpenseItemCell)Nib.Instantiate (null, null) [0];
		}

		public ExpenseItemCell (IntPtr handle) : base (handle)
		{
		}

		public void setExpenseItem (ExpenseItem expenseItem) {
			this.TitleLabel.TextColor = expenseItem.IsHighlighted ? UIColor.Red : UIColor.FromRGB (38, 48, 60);
			this.TitleLabel.Lines = expenseItem.AreCurrenciesDifferent ? 2 : 1;
			this.TitleLabel.Text = expenseItem.VCategoryName;
			this.Amount1Label.Text = expenseItem.VAmountLC;
			this.Amount2Label.Text = expenseItem.AreCurrenciesDifferent ? expenseItem.VAmountCC : "";
			this.ContainerView.Layer.CornerRadius = 4;
			this.ContainerView.Layer.BorderWidth = 1;
			this.ContainerView.Layer.BorderColor = UIColor.FromRGBA (0,134,158, 50).CGColor;

			if (expenseItem.PolicyRule.Equals (ExpenseItem.PolicyRules.Green)) {
				this.PolicyRuleImageView.Image = UIImage.FromBundle("ExpenseIsCompliant");
				this.PolicyRuleImageView.ContentMode = UIViewContentMode.ScaleAspectFit;
			}

			if (expenseItem.PolicyRule.Equals (ExpenseItem.PolicyRules.Orange)) {
				this.PolicyRuleImageView.Image = UIImage.FromBundle("ExpenseNotCompliantPolicy");
				this.PolicyRuleImageView.ContentMode = UIViewContentMode.ScaleAspectFit;
			}

			if (expenseItem.PolicyRule.Equals (ExpenseItem.PolicyRules.Red)) {
				this.PolicyRuleImageView.Image = UIImage.FromBundle("ExpenseNotCompliant");
				this.PolicyRuleImageView.ContentMode = UIViewContentMode.ScaleAspectFit;
			}
		}

		public override void SetSelected (bool selected, bool animated)
		{
			base.SetSelected (selected, animated);
			this.ContainerView.Layer.BorderColor = UIColor.FromRGBA (0,134,158, 50).CGColor;
			this.ContainerView.BackgroundColor = UIColor.FromRGBA(18, 152, 185, 15);
		}



	}
}