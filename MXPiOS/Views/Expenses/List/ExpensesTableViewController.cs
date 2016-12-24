using System;
using Foundation;
using UIKit;
using System.CodeDom.Compiler;
using Mxp.Core.Business;
using System.Linq;
using System.Collections.ObjectModel;
using Mxp.Core.Utils;
using System.Diagnostics;
using CoreGraphics;
using Mxp.Core;
using MXPiOS;
using System.ComponentModel;
using System.Threading.Tasks;

namespace Mxp.iOS
{
	partial class ExpensesTableViewController : MXPViewController
	{
		public enum SelectedSourceEnum {
			BusinessExpenses,
			PrivateExpenses,
			SpendCatcherExpences
		}

		public Expenses BusinessExpenses {
			get { 
				return LoggedUser.Instance.BusinessExpenses;
			}
		}

		public Expenses PrivateExpenses {
			get {
				return LoggedUser.Instance.PrivateExpenses;
			}
		}

		public SpendCatcherExpenses SpendCatcherExpenses { 
			get { 
				return LoggedUser.Instance.SpendCatcherExpenses;
			}
		}

		public ExpensesTableViewController (IntPtr handle) : base (handle) {
			
		}

		private SpendCatcherTableSource _scSource;
		protected SpendCatcherTableSource SCSource {
			get {
				if (this._scSource == null) {
					this._scSource = new SpendCatcherTableSource (this.SpendCatcherExpenses);
					this._scSource.spendCatcherSelected += (object sender, EventArgs e) => {
						this.ShowModel (sender as SpendCatcherExpense);
					};
				}

				return this._scSource;
			}
		}

		private ExpensesTableSource _expensesSource;
		protected ExpensesTableSource ExpensesSource {
			get {
				if (_expensesSource == null) {
					this._expensesSource = new ExpensesTableSource (this.BusinessExpenses);
					this._expensesSource.cellSelected += (object sender, ExpenseSelectedEventArgs e) => {
						if (e.model is Expense) {
							Expense expense = e.model as Expense;

							if (expense.IsSplit){
								if (expense.IsOpen)
									this.removeExpenseItem (this._expensesSource.Expenses);
								else
									this.unsplitExp (expense);

								expense.IsOpen = !expense.IsOpen;

								return;
							} else {
								this.ShowModel (expense.ExpenseItems[0]);
								return;
							}
						}

						if (e.model is ExpenseItem)
							this.ShowModel (e.model as ExpenseItem);
					};
				}

				return this._expensesSource;
			}
		}

		private UIRefreshControl refreshControl = new UIRefreshControl ();
		private SelectedSourceEnum selectedSource = SelectedSourceEnum.BusinessExpenses;

		private ExpenseItem selectedBusinessExpenseItem;
		private ExpenseItem selectedPrivateExpenseItem;
		private SpendCatcherExpense selectedSpendCatcherExpense;
			
		public override void ViewDidLoad () {
			base.ViewDidLoad ();

			this.TableView.AddSubview (this.refreshControl);

			this.Title = Labels.GetLoggedUserLabel (Labels.LabelEnum.Expenses);

			this.SegmentedControl.SetTitle (Labels.GetLoggedUserLabel (Labels.LabelEnum.BusinessDistance), 0);
			this.SegmentedControl.SetTitle (Labels.GetLoggedUserLabel (Labels.LabelEnum.Private), 1);
			this.SegmentedControl.SetTitle (Labels.GetLoggedUserLabel (Labels.LabelEnum.SpendCatcher), 2);

			if (!Preferences.Instance.IsSpendCatcherEnable)
				this.SegmentedControl.RemoveSegmentAtIndex (2, false);

			this.SegmentedControl.SelectedSegment = (nint) ((MainTabBarControllerView)this.TabBarController)?.SelectedCategory;
			this.SegmentedControl.SendActionForControlEvents (UIControlEvent.ValueChanged);

			this.ConfigureSource ();

			ExpenseItem expenseItem = ((MainTabBarControllerView)this.TabBarController)?.ExpenseItem;
			if (expenseItem != null)
				this.SetSelectedExpense (expenseItem);
		}

		public override void ViewWillAppear (bool animated) {
			base.ViewWillAppear (animated);

			this.BusinessExpenses.PropertyChanged += HandlePropertyChanged;
			this.PrivateExpenses.PropertyChanged += HandlePropertyChanged;
			this.SpendCatcherExpenses.PropertyChanged += HandlePropertyChanged;

			this.refreshControl.ValueChanged += HandleRefreshControlChanged;

			this.ReloadData ();
		}

		public override void ViewWillDisappear (bool animated) {
			base.ViewWillDisappear (animated);

			this.BusinessExpenses.PropertyChanged -= HandlePropertyChanged;
			this.PrivateExpenses.PropertyChanged -= HandlePropertyChanged;
			this.SpendCatcherExpenses.PropertyChanged -= HandlePropertyChanged;

			this.refreshControl.ValueChanged -= HandleRefreshControlChanged;
		}

		private void ConfigureSource () {
			switch (this.selectedSource) {
				case SelectedSourceEnum.BusinessExpenses:
					this.ExpensesSource.Expenses = this.BusinessExpenses;
					this.TableView.Source = this.ExpensesSource;
					if (!this.BusinessExpenses.Loaded)
						this.RefreshExpenses ();
					break;
				case SelectedSourceEnum.PrivateExpenses:
					this.ExpensesSource.Expenses = this.PrivateExpenses;
					this.TableView.Source = this.ExpensesSource;
					if (!this.PrivateExpenses.Loaded)
						this.RefreshExpenses ();
					break;
				case SelectedSourceEnum.SpendCatcherExpences:
					this.TableView.Source = this.SCSource;
					if (!this.SpendCatcherExpenses.Loaded)
						this.RefreshExpenses ();
					break;
			}

			this.ReloadData ();
		}
						
		partial void ChangeSegmentedControl (UISegmentedControl sender) {
			switch (sender.SelectedSegment) {
				case 0:
					this.selectedSource = SelectedSourceEnum.BusinessExpenses;
					break;
				case 1:
					this.selectedSource = SelectedSourceEnum.PrivateExpenses;
					break;
				case 2:
					this.selectedSource = SelectedSourceEnum.SpendCatcherExpences;
					break;
			}

			this.ConfigureSource ();
		}

		private void HandleRefreshControlChanged (object sender, EventArgs e) {
			this.RefreshExpenses ();
		}

		private void HandlePropertyChanged (object sender, PropertyChangedEventArgs e) {
			this.ReloadData ();
		}

		private void ReloadData () {
			this.TableView.ReloadData ();

			switch (selectedSource) {
				case SelectedSourceEnum.BusinessExpenses:
					this.ShowModel ((this.PresentedExpenseExists ? this.selectedBusinessExpenseItem : null), false);
					break;
				case SelectedSourceEnum.PrivateExpenses:
					this.ShowModel ((this.PresentedExpenseExists ? this.selectedPrivateExpenseItem : null), false);
					break;
				case SelectedSourceEnum.SpendCatcherExpences:
					this.ShowModel ((this.PresentedExpenseExists ? this.selectedSpendCatcherExpense : null), false);
					break;
			}
			
			this.HighlightSelectedExpense ();
		}

		private bool PresentedExpenseExists {
			get {
				switch (this.selectedSource) {
					case SelectedSourceEnum.BusinessExpenses:
						return this.selectedBusinessExpenseItem != null && this.BusinessExpenses.Contains (this.selectedBusinessExpenseItem.ParentExpense);
					case SelectedSourceEnum.PrivateExpenses:
						return this.selectedPrivateExpenseItem != null && this.PrivateExpenses.Contains (this.selectedPrivateExpenseItem.ParentExpense);
					case SelectedSourceEnum.SpendCatcherExpences:
						return this.selectedSpendCatcherExpense != null && this.SpendCatcherExpenses.Contains (this.selectedSpendCatcherExpense);
					default:
						return false;
				}
			}
		}

		public async void RefreshExpenses () {
			this.refreshControl.BeginRefreshing ();

			try {
				switch (this.selectedSource) {
					case SelectedSourceEnum.BusinessExpenses:
						await this.BusinessExpenses.FetchAsync ();
						break;
					case SelectedSourceEnum.PrivateExpenses:
						await this.PrivateExpenses.FetchAsync ();
						break;
					case SelectedSourceEnum.SpendCatcherExpences:
						await this.SpendCatcherExpenses.FetchAsync ();
						break;
				}
			} catch (Exception error) {
				MainNavigationController.Instance.showError (error);
				return;
			} finally {
				this.refreshControl.EndRefreshing ();
			}
		}

		private void HighlightSelectedExpense () {
			if (this.SelectedModel == null || !this.PresentedExpenseExists)
				return;

			NSIndexPath indexPath = null;

			switch (this.selectedSource) {
				case SelectedSourceEnum.BusinessExpenses:
					indexPath = this.ExpensesSource.indexPathForExpense (this.selectedBusinessExpenseItem.ParentExpense);
					break;
				case SelectedSourceEnum.PrivateExpenses:
					indexPath = this.ExpensesSource.indexPathForExpense (this.selectedPrivateExpenseItem.ParentExpense);
					break;
				case SelectedSourceEnum.SpendCatcherExpences:
					indexPath = NSIndexPath.FromRowSection (this.SpendCatcherExpenses.IndexOf (this.selectedSpendCatcherExpense), 1);
					break;
			}

			this.TableView.SelectRow (indexPath, false, UITableViewScrollPosition.Middle);
		}

		private Model SelectedModel {
			get {
				switch (this.selectedSource) {
					case SelectedSourceEnum.BusinessExpenses:
						return this.selectedBusinessExpenseItem;
					case SelectedSourceEnum.PrivateExpenses:
						return this.selectedPrivateExpenseItem;
					case SelectedSourceEnum.SpendCatcherExpences:
						return this.selectedSpendCatcherExpense;
					default:
						return null;
				}
			}
		}

		protected void SetSelectedExpense (Model model) {
			switch (this.selectedSource) {
				case SelectedSourceEnum.BusinessExpenses:
					this.selectedBusinessExpenseItem = (ExpenseItem) model;
					break;
				case SelectedSourceEnum.PrivateExpenses:
					this.selectedPrivateExpenseItem = (ExpenseItem)model;
					break;
				case SelectedSourceEnum.SpendCatcherExpences:
					this.selectedSpendCatcherExpense = (SpendCatcherExpense) model;
					break;
			}
		}

		public void unsplitExp (Expense exp) {
			Collection<Expenses.RowSection> indexToRemove = exp.GetCollectionParent<Expenses, Expense> ().removeExpenseItems();
			Collection<NSIndexPath> pathsToRemove = new Collection<NSIndexPath> ();
			indexToRemove.ForEach (rowSection => {
				pathsToRemove.Add(NSIndexPath.FromRowSection(rowSection.Row, rowSection.Section));
			});

			Collection<Expenses.RowSection> indexToAdd = exp.GetCollectionParent<Expenses, Expense> ().unsplitExpense (exp);
			Collection<NSIndexPath> pathsToAdd = new Collection<NSIndexPath> ();
			indexToAdd.ForEach (rowSection => {
				pathsToAdd.Add(NSIndexPath.FromRowSection(rowSection.Row, rowSection.Section));
			});

			this.TableView.BeginUpdates ();
			TableView.DeleteRows (pathsToRemove.ToArray (), UITableViewRowAnimation.Automatic);
			this.TableView.InsertRows (pathsToAdd.ToArray (), UITableViewRowAnimation.Automatic);
			this.TableView.EndUpdates ();
		}

		public void removeExpenseItem (Expenses expenses) {
			Collection<Expenses.RowSection> indexToRemove = expenses.removeExpenseItems();
			this.TableView.BeginUpdates ();
			Collection<NSIndexPath> paths = new Collection<NSIndexPath> ();
			indexToRemove.ForEach (rowSection => {
				paths.Add(NSIndexPath.FromRowSection(rowSection.Row, rowSection.Section));
			});
			TableView.DeleteRows (paths.ToArray (), UITableViewRowAnimation.Top);
			this.TableView.EndUpdates ();
		}
			
		partial void ClickOnAddButton (UIBarButtonItem sender) {
			Actionables actionables = Expense.ListShowAddExpenses (
				actionExpense: () => this.createNewExpense (),
				actionMileage: () => this.createNewMileage (),
				actionAllowance: () => this.CreateNewAllowance ()
			);

			new ActionnablesWrapper(actionables, this, this.NavigationItem.RightBarButtonItem).show();
		}

		public void createNewExpense(){
			UIViewController vc = new ChooseCategoryTableViewControllerController ();
			this.ShowCreateViewController(vc);
		}

		public void createNewMileage(){
			Mileage newMileage = Mileage.NewInstance ();

			if (newMileage.CanShowJourneysList) {
				Action finishAction = () => {
					MileageViewController vc = new MileageViewController (newMileage);
					this.ShowCreateViewController(vc);
				};

				new ActionnablesWrapper (newMileage.ListJourneys(finishAction), this, this.NavigationItem.RightBarButtonItem).show ();
			} else {
				MileageViewController vc = new MileageViewController (newMileage);
				this.ShowCreateViewController(vc);
			}				
		}

		public void CreateNewAllowance () {
			AllowanceCreationTableViewController vc = new AllowanceCreationTableViewController ();
			this.ShowCreateViewController(vc);
		}
			
		public virtual void ShowCreateViewController (UIViewController vc) {}

		public virtual void ShowModel (Model model, bool animated = true) {}
	}
}