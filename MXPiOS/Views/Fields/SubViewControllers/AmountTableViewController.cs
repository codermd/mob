using System;

using Foundation;
using UIKit;
using Mxp.Core.Business;
using System.Collections.ObjectModel;
using CoreGraphics;

namespace Mxp.iOS
{
	public partial class AmountTableViewController : MXPViewController
	{
		protected ExpenseItem expenseItem;

		public AmountTableViewController (ExpenseItem expenseItem) : base ("AmountTableViewController", null) {
			this.expenseItem = expenseItem;
		}

		public override void ViewDidLoad () {
			base.ViewDidLoad ();

			this.TableView.Source = new SectionedFieldsSource (new Collection<TableSectionModel> () {
				new TableSectionModel (this.expenseItem.AmountFields)
			}, this);
		}

		public override void ViewWillAppear (bool animated) {
			base.ViewWillAppear (animated);

			this.configureTopBar ();
		}

		public virtual void configureTopBar () {
			this.Title = Labels.GetLoggedUserLabel (Labels.LabelEnum.Amount);

			this.NavigationItem.SetHidesBackButton (false, false);
		}
	}
}