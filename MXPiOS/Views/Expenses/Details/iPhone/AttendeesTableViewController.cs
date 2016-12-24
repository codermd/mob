using System;

using Foundation;
using UIKit;
using Mxp.Core.Business;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Mxp.iOS
{
	public partial class AttendeesTableViewController : MXPTableViewController, IExpenseDetailsSubController
	{
		private ExpenseItem expenseItem;

		public AttendeesTableViewController (IntPtr handle) : base (handle) {
			
		}

		public void setExpenseItem (ExpenseItem expenseItem) {
			this.expenseItem = expenseItem;
		}

		public override void ViewWillAppear (bool animated) {
			base.ViewWillAppear (animated);

			this.TableView.Source = new AttendeesTableSource (this.expenseItem, this);

			this.TableView.ReloadData ();

			this.expenseItem.PropertyChanged += ExpenseItemPropertyChangedHandler;
		}

		private void ExpenseItemPropertyChangedHandler (object sender, PropertyChangedEventArgs e) {
			this.TableView.ReloadData ();
		}

		public override void ViewWillDisappear (bool animated) {
			this.expenseItem.PropertyChanged -= ExpenseItemPropertyChangedHandler;

			base.ViewWillDisappear (animated);
		}
	}
}