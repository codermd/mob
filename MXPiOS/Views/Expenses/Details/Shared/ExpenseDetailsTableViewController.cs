using System;
using Mxp.Core.Business;
using System.Linq;
using System.ComponentModel;

namespace Mxp.iOS
{
	partial class ExpenseDetailsTableViewController : MXPTableViewController, IExpenseDetailsSubController {

		private ExpenseItem expenseItem;

		public ExpenseDetailsTableViewController (IntPtr handle) : base (handle) {
		
		}

		public void setExpenseItem (ExpenseItem expenseItem) {
			this.expenseItem = expenseItem;
		}

		public override void ViewDidLoad () {
			base.ViewDidLoad ();

			this.TableView.Source = new SectionedFieldsSource (this.expenseItem.DetailsFields, this);
		}

		public override void ViewWillAppear (bool animated) {
			base.ViewWillAppear (animated);

			if (this.expenseItem.modifiedFields.Any (field => field.Title.Equals ("Product")))
				this.reload ();

			this.expenseItem.PropertyChanged += HandlePropertyChanged;
			this.expenseItem.ParentExpense.PropertyChanged += HandlePropertyChanged;
		}

		public override void ViewWillDisappear (bool animated) {
			base.ViewWillDisappear (animated);

			this.expenseItem.PropertyChanged -= HandlePropertyChanged;
			this.expenseItem.ParentExpense.PropertyChanged -= HandlePropertyChanged;
		}

		private void HandlePropertyChanged (object sender, PropertyChangedEventArgs e) {
			if (e.PropertyName.Equals ("Product"))
				this.reload ();
			else
				this.TableView.ReloadData ();
		}

		public void reload () {
			this.TableView.Source = new SectionedFieldsSource (this.expenseItem.DetailsFields, this);
			this.TableView.ReloadData ();
		}
	}
}