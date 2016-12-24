using System;
using UIKit;
using Mxp.Core.Business;
using System.Collections.ObjectModel;
using Mxp.Core.Utils;
using Foundation;

namespace Mxp.iOS
{
	public class SelectableExpensesTableViewController : MXPTableViewController
	{

		// Starting off with an empty handler avoids pesky null checks
		public event EventHandler<SelectedExpensesEventArgs> SelectedExpensesConfirm = delegate {};

		public ObservableCollection<Expense> SelectedExpenses;

		public SelectableExpensesTableViewController(Collection<Expense> selectedExpenses): base() {
			this.SelectedExpenses = new ObservableCollection<Expense> ();
			selectedExpenses.ForEach (exp => {
				this.SelectedExpenses.Add (exp);
			});
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			this.CancelButton = new UIBarButtonItem(Labels.GetLoggedUserLabel (Labels.LabelEnum.Cancel), UIBarButtonItemStyle.Plain, clickOnCancel);
		}

		void clickOnSelected (object sender, EventArgs eArgs)
		{
			SelectedExpensesEventArgs e = new SelectedExpensesEventArgs();
			e.expenses = this.SelectedExpenses;
			this.SelectedExpensesConfirm(this,e);
		}

		void clickOnCancel (object sender, EventArgs eArgs)
		{
			this.SelectedExpensesConfirm(this, new SelectedExpensesEventArgs ());
		}

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
			this.source = new Source (LoggedUser.Instance.BusinessExpenses);

			this.source.SelectExpenseEvent += (sender, e) => {
				if(!this.SelectedExpenses.Contains((Expense)e.Expense)) {
					this.SelectedExpenses.Add((Expense)e.Expense);
				}

			};

			this.source.DeselectExpenseEvent += (sender, e) => {
				this.SelectedExpenses.Remove((Expense)e.Expense);
			};

			this.SelectedExpenses.CollectionChanged += (sender, e) => {
				this.refreshTopButtons();
			};
			this.refreshTopButtons ();

			this.TableView.Source = this.source;
			this.TableView.ReloadData ();

			if (this.TableView.NumberOfSections () > 1)
				this.TableView.ScrollToRow (NSIndexPath.FromRowSection (0, 1), UITableViewScrollPosition.Top, false);

			this.TableView.AllowsMultipleSelectionDuringEditing = true;
			this.SetEditing (true, true);

			this.preselect ();
		}

		public void preselect() {
			this.SelectedExpenses.ForEach (Expense => {
				NSIndexPath path = this.source.indexPathForExpense(Expense);
				if(path != null) {
					this.TableView.SelectRow(path, false, UITableViewScrollPosition.None);
				}
			});
		}

		private UIBarButtonItem CancelButton;
		private UIBarButtonItem SelectButton;
		public void refreshTopButtons(){
			if (this.SelectedExpenses.Count > 0) {

				this.SelectButton = new UIBarButtonItem(Labels.GetLoggedUserLabel (Labels.LabelEnum.Select) + " ("+this.SelectedExpenses.Count+")", UIBarButtonItemStyle.Done, clickOnSelected);

				bool animateChange = false;
				if (this.SelectedExpenses.Count == 1) {
					animateChange = true;
				}
				this.NavigationItem.SetRightBarButtonItems(new UIBarButtonItem[]{SelectButton}, animateChange);
				this.NavigationItem.SetLeftBarButtonItems(new UIBarButtonItem[]{CancelButton}, animateChange);
				this.NavigationItem.HidesBackButton = true;

			} else {
				this.NavigationItem.SetLeftBarButtonItems(new UIBarButtonItem[]{}, true);
				this.NavigationItem.SetRightBarButtonItems(new UIBarButtonItem[]{}, true);
				this.NavigationItem.HidesBackButton = false;

				if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad) {
					this.NavigationItem.SetLeftBarButtonItems(new UIBarButtonItem[]{CancelButton}, false);
				}

			}
		}


		//Event management
		public class SelectedExpensesEventArgs : EventArgs
		{
			public Collection<Expense> expenses { get; set;}
		}


		private class Source : ExpensesTableSource
		{


			//Event management
			public class ExpenseEventArgs : EventArgs
			{
				public Expense Expense { get; set;}
			}
			public event EventHandler<ExpenseEventArgs> SelectExpenseEvent = delegate {};
			public event EventHandler<ExpenseEventArgs> DeselectExpenseEvent = delegate {};

			// Starting off with an empty handler avoids pesky null checks
			public event EventHandler<SelectedExpensesEventArgs> cellSelected = delegate {};

			public Source(Expenses expenses): base(expenses){
			}

			public override nint NumberOfSections (UITableView tableView)
			{
				return base.NumberOfSections (tableView)+1;
			}

			public override string TitleForHeader (UITableView tableView, nint section)
			{
				if (section == 0) {
					return null;
				}
				return base.TitleForHeader (tableView, section-1);
			}

			public override nint RowsInSection (UITableView tableview, nint section)
			{
				if (section == 0) {
					return 1;
				}
				return this.Expenses.getSectionnedExpenses () [this.Expenses.getOrderedKeys() [(int)section-1]].Count;
			}


			public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
			{
				if (indexPath.Section == 0) {
					SelectAllCell cell = (SelectAllCell)tableView.DequeueReusableCell (SelectAllCell.Key);
					if(cell == null) {
						cell = SelectAllCell.Create ();
					}
					return cell;
				}

				UITableViewCell exCell =  base.GetCell (tableView, NSIndexPath.FromRowSection(indexPath.Row, indexPath.Section) );
				exCell.SelectionStyle = UITableViewCellSelectionStyle.Blue;
				return exCell;
			}

			public override bool CanEditRow (UITableView tableView, NSIndexPath indexPath)
			{
				if (indexPath.Section == 0) {
					return false;
				}
				return true;
			}

			public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
			{
				if (indexPath.Section == 0) {
					this.Expenses.ForEach(exp =>{
						this.emitSelect(exp);
						var indexPathOfCurrent = this.indexPathForExpense(exp);
						tableView.SelectRow(indexPathOfCurrent, true, UITableViewScrollPosition.None);
					});
					return;
				}

				Model model = this.GetModelAtIndexPath (indexPath);

				if (model is Expense) {
					this.emitSelect (((Expense)model));
				}
			}

			public override NSIndexPath indexPathForExpense(Expense exp){
				NSIndexPath defaultIndexPath = base.indexPathForExpense(exp);
				return NSIndexPath.FromRowSection (defaultIndexPath.Row, defaultIndexPath.Section + 1);
			}

			public override Model GetModelAtIndexPath (NSIndexPath indexPath)
			{
				return base.GetModelAtIndexPath (NSIndexPath.FromRowSection (indexPath.Row, indexPath.Section - 1));
			}


			public void emitSelect(Expense exp) {
				ExpenseEventArgs e = new ExpenseEventArgs ();
				e.Expense = exp;
				this.SelectExpenseEvent (this, e);
			}

			public override NSIndexPath WillDeselectRow (UITableView tableView, NSIndexPath indexPath) {
				if (indexPath.Section == 0)
					return null;

				Model model = this.GetModelAtIndexPath (indexPath);

				if ((model is Expense && !((Expense)model).IsDeselectable)
					|| (model is ExpenseItem && !((ExpenseItem)model).ParentExpense.IsDeselectable))
					return null;
				
				return indexPath;
			}

			public override void RowDeselected (UITableView tableView, NSIndexPath indexPath)
			{
				Model model = this.GetModelAtIndexPath (indexPath);

				if (model is Expense) {
					ExpenseEventArgs e = new ExpenseEventArgs ();
					e.Expense = model as Expense;
					this.DeselectExpenseEvent (this, e);
				}
			}

			public override nfloat GetHeightForRow (UITableView tableView, NSIndexPath indexPath)
			{
				if (indexPath.Section == 0) {
					return 50;
				}

				return base.GetHeightForRow (tableView, indexPath);
			}
		}

		private Source source;

	}

}

