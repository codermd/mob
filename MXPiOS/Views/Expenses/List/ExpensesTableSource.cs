using System;
using CoreGraphics;

using Foundation;
using UIKit;
using ObjCRuntime;
using Mxp.Core.Business;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using Mxp.Core.Utils;

namespace Mxp.iOS
{
	public class ExpensesTableSource : UITableViewSource
	{
		// Starting off with an empty handler avoids pesky null checks
		public event EventHandler<ExpenseSelectedEventArgs> cellSelected = delegate {};

		public Expenses Expenses { get; set; }

		public ExpensesTableSource (Expenses expenses) {
			this.Expenses = expenses;
		}

		public override nint NumberOfSections (UITableView tableView) {
			if (this.Expenses.getOrderedKeys() == null)
				return 0;
			
			return this.Expenses.getOrderedKeys().Count;
		}

		public override nint RowsInSection (UITableView tableview, nint section) {
			return this.Expenses.getSectionnedExpenses () [this.Expenses.getOrderedKeys () [(int)section]].Count;
		}

		public virtual Model GetModelAtIndexPath(NSIndexPath indexPath){
			int key = this.Expenses.getOrderedKeys () [indexPath.Section];
			var section = this.Expenses.getSectionnedExpenses () [key];
			return section[indexPath.Row];
		}

		public virtual NSIndexPath indexPathForExpense(Expense exp){

			foreach(int key in this.Expenses.getSectionnedExpenses ().Keys) {
				int index = this.Expenses.getSectionnedExpenses () [key].IndexOf (exp);
				if (index != -1) {
					int section = this.Expenses.getOrderedKeys ().IndexOf (key);
					return NSIndexPath.FromRowSection(index, section);
				}
			}
			return null;
		}

		public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath) {
			Model model = this.GetModelAtIndexPath (indexPath);

			if (model is Expense) {
				var cell = tableView.DequeueReusableCell ("ExpenseCell") as ExpenseCell;
				if (cell == null) {
					var views = NSBundle.MainBundle.LoadNib ("ExpenseCell", cell, null);
					cell = Runtime.GetNSObject (views.ValueAt (0)) as ExpenseCell;
				}

				cell.setExpense (model as Expense);

				return cell;
			}

			if (model is ExpenseItem) {
				ExpenseItemCell cell = tableView.DequeueReusableCell (ExpenseItemCell.Key) as ExpenseItemCell;
				if (cell == null) {
					cell = ExpenseItemCell.Create ();
				}
				cell.setExpenseItem (model as ExpenseItem);
				return cell;
			}

			return null;
		}

		public override void RowSelected (UITableView tableView, NSIndexPath indexPath) {
			ExpenseSelectedEventArgs e = new ExpenseSelectedEventArgs ();
			e.model = this.GetModelAtIndexPath (indexPath);
			this.cellSelected (this, e);
		}

		public override string TitleForHeader (UITableView tableView, nint section) {
			return ((Expense)this.Expenses.getSectionnedExpenses () [this.Expenses.getOrderedKeys () [(int)section]] [0]).VDateHeader;
		}

		public override nfloat GetHeightForRow (UITableView tableView, NSIndexPath indexPath) {
			Model model = this.GetModelAtIndexPath (indexPath);

			if (model is Expense)
				return ((Expense)model).AreCurrenciesDifferent ? 100 : 80;
			else if (model is ExpenseItem)
				return ((ExpenseItem)model).AreCurrenciesDifferent ? 88 : 68;
			
			return 0;
		}
	}

	public class ExpenseSelectedEventArgs : EventArgs
	{
		public Model model { get; set;}
	}
}