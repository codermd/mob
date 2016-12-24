using System;
using CoreGraphics;

using Foundation;
using UIKit;
using Mxp.Core.Business;

namespace Mxp.iOS
{
	public partial class SplitExpenceItemCell : UITableViewCell
	{
		public static readonly UINib Nib = UINib.FromName ("SplitExpenceItemCell", NSBundle.MainBundle);
		public static readonly NSString Key = new NSString ("SplitExpenceItemCell");

		public SplitExpenceItemCell (IntPtr handle) : base (handle)
		{
		}

		public static SplitExpenceItemCell Create ()
		{
			return (SplitExpenceItemCell)Nib.Instantiate (null, null) [0];
		}

		private ExpenseItem ExpenseItem;

		public void setExpenseItem(ExpenseItem expenseItem){
			this.ExpenseItem = expenseItem;
			this.CategoryLabel.Text = expenseItem.VCategoryName;
			this.AmountLabel.Text = expenseItem.VAmountLC;
		}

	}
}

