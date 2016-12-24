using System;
using UIKit;
using System.Collections.Generic;
using Foundation;
using Social;
using System.Linq;

namespace sc
{
	public class CountriesTableViewController : UITableViewController
	{

		public CountriesTableViewController (UITableViewStyle withStyle) : base (withStyle) {
			
		}

		public class SelectedEventArgs : EventArgs
		{
			public Country Country { get; set; }
		}

		public event EventHandler<SelectedEventArgs> cellSelected;

		public Country PreselectedCountry { get; set; }
			
		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
			this.configureTable ();

			this.NavigationItem.SetLeftBarButtonItem(new UIBarButtonItem("Close", UIBarButtonItemStyle.Plain, (sender, e) =>
			{
				this.DismissViewController(true, null);
			}), true);

			this.Title = "Select a country";
				
		}

		public void configureTable() {

			this.groupedCountries = Context.Instance.Countries
				.GroupBy(country => country.Name.Substring(0, 1))
				.OrderBy(grouping => grouping.Key).ToList();
						IEnumerable<IGrouping<String, Country>> recentlyUsedCountries = Context.Instance.Countries.Where(country => country.Recent).GroupBy(country => "Recent");
						this.groupedCountries = recentlyUsedCountries.Concat(groupedCountries).ToList();

			try {
				var dataSource = new TableDataSource(Context.Instance.Countries, this.PreselectedCountry);
				this.TableView.WeakDataSource = dataSource;
				dataSource.groupedCountries = this.groupedCountries;
				var tableViewDelegate = new TableViewDelegate();
				tableViewDelegate.groupedCountries = this.groupedCountries;
				tableViewDelegate.cellSelected += (sender, agrs) =>
				{
					this.cellSelected(this, agrs);
				};
				this.TableView.WeakDelegate = tableViewDelegate;
			} catch(Exception) {
			}
		}

		IEnumerable<IGrouping<String, Country>> groupedCountries;

		public class TableDataSource : UITableViewDataSource 
		{

			public IEnumerable<IGrouping<String, Country>> groupedCountries;
			public Country PreselectedCountry { get; set; }

			public TableDataSource(List<Country> someItems, Country preselectedCountry) {
				this.PreselectedCountry = preselectedCountry;
			}

			public override nint NumberOfSections (UITableView tableView)
			{
				return this.groupedCountries.Count();
			}

			public override nint RowsInSection (UITableView tableView, nint section)
			{
				return this.groupedCountries.ElementAt((int)section).Count();
			}

			public override string TitleForHeader(UITableView tableView, nint section)
			{
				return this.groupedCountries.ElementAt((int)section).Key;
			}

			public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
			{
				UITableViewCell cell =  tableView.DequeueReusableCell ("CELL");
				if (cell == null) {
					cell = new UITableViewCell (UITableViewCellStyle.Default, "CELL");
				}
				var country = this.groupedCountries.ElementAt(indexPath.Section).ElementAt(indexPath.Row);

				if (country == this.PreselectedCountry) {
					cell.Accessory = UITableViewCellAccessory.Checkmark;
				} else {
					cell.Accessory = UITableViewCellAccessory.None;
				}

				cell.TextLabel.Text = country.title;
				return cell;
			}
		}

		public class TableViewDelegate : UITableViewDelegate
		{

			public IEnumerable<IGrouping<String, Country>> groupedCountries;
			
			public TableViewDelegate() {
			}

			public event EventHandler<SelectedEventArgs> cellSelected;

			public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
			{
				var country = this.groupedCountries.ElementAt(indexPath.Section).ElementAt(indexPath.Row);
				this.cellSelected(this, new SelectedEventArgs() { Country = country });
			}
		}
	}
}

