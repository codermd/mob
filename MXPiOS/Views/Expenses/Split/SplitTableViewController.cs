using System;
using UIKit;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using Foundation;
using Mxp.Core.Business;

namespace Mxp.iOS
{
	public class SplitTableViewController : MXPTableViewController
	{

		protected ExpenseItem expenseItem;
		public SplitTableViewController (ExpenseItem expenseItem) : base()
		{
			this.expenseItem = expenseItem;
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			this.NavigationItem.SetHidesBackButton (true, true);
		}


		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);

			this.expenseItem.SanitizeInnerSplits ();

			this.TableView.Source = new Source(this.expenseItem, (sender, e)=>{
				this.showExpenceItem(e.ExpenseItem);
			});
			this.TableView.ReloadData ();
			this.SetEditing (true, false);

			this.TableView.AllowsSelectionDuringEditing = true;

			this.expenseItem.InnerSplittedItems.CollectionChanged += HandleCollectionChanged;

			this.configureTopBar ();
		}

		void HandleCollectionChanged (object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
		{
			this.configureTopBar ();
		}

		public override void ViewWillDisappear (bool animated)
		{
			base.ViewWillDisappear (animated);
			this.expenseItem.InnerSplittedItems.CollectionChanged -= HandleCollectionChanged;
		}

		public void configureTopBar(){	

			List<UIBarButtonItem> buttons = new  List<UIBarButtonItem> ();

			if (this.expenseItem.IsReadyToSplit) {
				buttons.Add (new UIBarButtonItem (Labels.GetLoggedUserLabel (Labels.LabelEnum.Split), UIBarButtonItemStyle.Done, (sender, args) => {
					this.splitProcess ();
				}));
			}

			buttons.Add (new UIBarButtonItem (UIBarButtonSystemItem.Add, (sender, e)=>{
				this.showExpenceItem(this.expenseItem.AddExpenseItem ());
			}));

			this.NavigationItem.SetRightBarButtonItems (buttons.ToArray(), true);


			this.NavigationItem.SetLeftBarButtonItem (new UIBarButtonItem (Labels.GetLoggedUserLabel (Labels.LabelEnum.Cancel), UIBarButtonItemStyle.Plain, (sender, args)=>{
				this.cancelProcess();
			}), true);
		}

		public void showExpenceItem(ExpenseItem expenseItem){
			this.NavigationController.PushViewController (new CategoriesChooserSplit (expenseItem), true);
		}


		public void cancelProcess(){
			this.expenseItem.ClearSplittedItems ();
			if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad) {
				this.DismissViewController (true, null);
			} else {
				this.NavigationController.PopToViewController (this.NavigationController.ViewControllers [1], true);
			}

		}

		public async void splitProcess(){
			LoadingView.showMessage (Labels.GetLoggedUserLabel (Labels.LabelEnum.Splitting) + "...");
			try {
				await this.expenseItem.SplitAsync ();
			} catch (Exception e){
				MainNavigationController.Instance.showError (e);
				return;
			} finally {
				LoadingView.hideMessage ();
			}

			this.HidesBottomBarWhenPushed = false;

			if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad) {
				this.DismissViewController (true, null);
			} else {
				this.NavigationController.PopToRootViewController (true);
			}
		}


		private class Source : UITableViewSource 
		{

			//Event management
			public class SelectedEventArgs : EventArgs
			{
				public ExpenseItem ExpenseItem{ get; set;}
			}

			// Starting off with an empty handler avoids pesky null checks
			public event EventHandler<SelectedEventArgs> cellSelected = delegate {};


			public ExpenseItem expenseItem;

			public Source(ExpenseItem expenseItem, EventHandler<SelectedEventArgs> handler): base(){
				this.cellSelected = handler;
				this.expenseItem = expenseItem;
			}

			public override nint NumberOfSections (UITableView tableView)
			{
				return 1;
			}

			public override nint RowsInSection (UITableView tableview, nint section)
			{
				return expenseItem.InnerSplittedItems.Count;
			}

			public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
			{
				SplitExpenceItemCell cell = tableView.DequeueReusableCell (SplitExpenceItemCell.Key) as SplitExpenceItemCell;
				if (cell == null) {
					cell = SplitExpenceItemCell.Create ();
				}

				cell.setExpenseItem (expenseItem.InnerSplittedItems [indexPath.Row]);

				return cell;
			}

			public override bool CanEditRow (UITableView tableView, NSIndexPath indexPath)
			{
				return true;
			}
			public override UITableViewCellEditingStyle EditingStyleForRow (UITableView tableView, NSIndexPath indexPath)
			{
				return UITableViewCellEditingStyle.Delete;
			}

			public override void CommitEditingStyle (UITableView tableView, UITableViewCellEditingStyle editingStyle, NSIndexPath indexPath)
			{
				this.expenseItem.InnerSplittedItems.RemoveAt (indexPath.Row);
				tableView.BeginUpdates ();
				tableView.DeleteRows(new NSIndexPath[]{
					indexPath
				}, UITableViewRowAnimation.Automatic);

				tableView.EndUpdates();
			}

			public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
			{
				SelectedEventArgs e = new SelectedEventArgs ();
				e.ExpenseItem = this.expenseItem.InnerSplittedItems [indexPath.Row];
				cellSelected (this, e);
			}

			public override string TitleForHeader (UITableView tableView, nint section)
			{
				return this.expenseItem.VTitleSplit;
			}

		}
	}
}

