using System;
using UIKit;
using System.Collections.Generic;
using Foundation;

namespace sc
{
	public class ProductsTableViewController : UITableViewController
	{


		public Product PreselectedProduct { get; set; }

		public class SelectedEventArgs : EventArgs
		{
			public Product Product { get; set; }
		}

		public event EventHandler<SelectedEventArgs> cellSelected;

		public ProductsTableViewController (UITableViewStyle withStyle) : base (withStyle) {
		}

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
			this.configureTable ();

			this.NavigationItem.SetLeftBarButtonItem(new UIBarButtonItem("Close", UIBarButtonItemStyle.Plain, (sender, e) =>
			{
				this.DismissViewController(true, null);
			}), true);

			this.Title = "Select a category";
		}
			
		public void configureTable() {
			try {
				this.TableView.WeakDataSource = new TableDataSource(Context.Instance.Products, this.PreselectedProduct);
				var tableViewDelegate = new TableViewDelegate();
				tableViewDelegate.cellSelected += (sender, agrs) =>
				{
					this.cellSelected(this, agrs);
				};
				this.TableView.WeakDelegate = tableViewDelegate;
			} catch(Exception e) {
			}
		}

		public class TableDataSource : UITableViewDataSource 
		{

			List<Product> items;
			Product PreselectedProduct;

			public TableDataSource(List<Product> someItems, Product preselectedProduct) {
				this.items = someItems;
				this.PreselectedProduct = preselectedProduct;
			}

			public override nint NumberOfSections (UITableView tableView)
			{
				return 1;
			}

			public override nint RowsInSection (UITableView tableView, nint section)
			{
				return this.items.Count;
			}

			public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
			{
				UITableViewCell cell =  tableView.DequeueReusableCell ("CELL");
				if (cell == null) {
					cell = new UITableViewCell (UITableViewCellStyle.Default, "CELL");
				}

				if (this.items [indexPath.Row] == this.PreselectedProduct) {
					cell.Accessory = UITableViewCellAccessory.Checkmark;
				} else {
					cell.Accessory = UITableViewCellAccessory.None;
				}

				cell.TextLabel.Text = this.items [indexPath.Row].title;
				return cell;
			}
		}

		public class TableViewDelegate : UITableViewDelegate
		{
			public TableViewDelegate() {

			}

			public event EventHandler<SelectedEventArgs> cellSelected;

			public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
			{
				this.cellSelected(this, new SelectedEventArgs() { Product = Context.Instance.Products[indexPath.Row] });
			}
		}
	}
}

