
using System;
using UIKit;
using Mxp.Core.Business;
using Foundation;
using System.Linq;

namespace Mxp.iOS
{
	public class CurrenciesSectionSource : UITableViewSource
	{
		public class CurrencySelectedEventArgs : EventArgs
		{
			public Currency Currency { get; }

			public CurrencySelectedEventArgs (Currency currency) {
				this.Currency = currency;
			}
		}

		public event EventHandler<CurrencySelectedEventArgs> cellSelected = delegate {};

		public Currencies Currencies { get; private set; }
		private bool whileSearching;

		public void SetCurrencies (Currencies currencies, bool whileSearching = false) {
			this.Currencies = currencies;
			this.whileSearching = whileSearching;

			this.Currencies.ResetGroups ();
		}

		public CurrenciesSectionSource (Currencies currencies) {
			this.Currencies = currencies;
		}

		public override nint NumberOfSections (UITableView tableView) {
			return this.Currencies.GetGroupedCurrencies (this.whileSearching).Count;
		}
			
		public override nint RowsInSection (UITableView tableview, nint section) {
			return this.Currencies.GetGroupedCurrencies (this.whileSearching) [(int)section].Count ();
		}

		public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath) {
			DefaultCell cell = (DefaultCell)tableView.DequeueReusableCell (DefaultCell.Key);

			if (cell == null)
				cell = DefaultCell.Create();

			Currency currency = this.Currencies.GetGroupedCurrencies (this.whileSearching) [indexPath.Section].ElementAt (indexPath.Row);

			cell.TextLabel.Text = currency.VName;

			return cell;
		}

		public override string TitleForHeader (UITableView tableView, nint section) {
			return this.Currencies.GetGroupedCurrencies (this.whileSearching) [(int)section].Key;
		}

		public override String[] SectionIndexTitles (UITableView tableView) {
			return this.Currencies.GetGroupedCurrencies (this.whileSearching).Select (grouping => grouping.Key.Substring (0, 1)).ToArray ();
		}

		public override nint SectionFor (UITableView tableView, string title, nint atIndex) {
			return atIndex;
		}

		public override void RowSelected (UITableView tableView, Foundation.NSIndexPath indexPath) {
			if (this.cellSelected == null)
				return;

			this.cellSelected (this, new CurrencySelectedEventArgs (this.Currencies.GetGroupedCurrencies (this.whileSearching) [indexPath.Section].ElementAt (indexPath.Row)));
		}
	}
}