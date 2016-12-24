using System;
using UIKit;
using Mxp.Core.Business;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;

namespace Mxp.iOS
{
	public class CountriesSectionSource : UITableViewSource
	{
		public class CountrySelectedEventArgs : EventArgs
		{
			public Country Country { get; }

			public CountrySelectedEventArgs (Country country) {
				this.Country = country;
			}
		}

		public event EventHandler<CountrySelectedEventArgs> cellSelected = delegate {};

		public Countries Countries { get; private set; }
		private bool whileSearching;

		public void SetCountries (Countries countries, bool whileSearching = false) {
			this.Countries = countries;
			this.whileSearching = whileSearching;

			this.Countries.ResetGroups ();
		}

		public CountriesSectionSource (Countries countries) {
			this.Countries = countries;
		}

		public override nint NumberOfSections (UITableView tableView) {
			return this.Countries.GetGroupedCountries (this.whileSearching).Count;
		}

		public override nint RowsInSection (UITableView tableview, nint section) {
			return this.Countries.GetGroupedCountries (this.whileSearching) [(int)section].Count ();
		}

		public override UITableViewCell GetCell (UITableView tableView, Foundation.NSIndexPath indexPath) {
			CountryCell cell = (CountryCell)tableView.DequeueReusableCell (CountryCell.Key);

			if (cell == null)
				cell = CountryCell.Create();

			Country country = this.Countries.GetGroupedCountries (this.whileSearching) [indexPath.Section].ElementAt (indexPath.Row);
			cell.setCountry(country);

			return cell;
		}

		public override string TitleForHeader (UITableView tableView, nint section) {
			return this.Countries.GetGroupedCountries (this.whileSearching) [(int)section].Key;
		}

		public override String[] SectionIndexTitles (UITableView tableView) {
			return this.Countries.GetGroupedCountries (this.whileSearching).Select (grouping => grouping.Key.Substring (0, 1)).ToArray ();
		}

		public override nint SectionFor (UITableView tableView, string title, nint atIndex) {
			return atIndex;
		}

		public override void RowSelected (UITableView tableView, Foundation.NSIndexPath indexPath) {
			if (this.cellSelected == null)
				return;

			this.cellSelected (this, new CountrySelectedEventArgs (this.Countries.GetGroupedCountries (this.whileSearching) [indexPath.Section].ElementAt (indexPath.Row)));
		}
	}
}