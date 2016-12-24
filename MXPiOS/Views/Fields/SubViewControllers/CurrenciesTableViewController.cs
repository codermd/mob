using System;
using Foundation;
using Mxp.Core.Business;
using System.Collections.Generic;
using UIKit;
using CoreGraphics;
using System.Linq;

namespace Mxp.iOS
{
	public partial class CurrenciesTableViewController : MXPTableViewController
	{
		public event EventHandler<CurrenciesSectionSource.CurrencySelectedEventArgs> cellSelected = delegate {};

		private Currency currency;

		public override void ViewDidLoad () {
			base.ViewDidLoad ();

			this.Title = Labels.GetLoggedUserLabel (Labels.LabelEnum.Currencies);

			UISearchBar sb = new UISearchBar(CGRect.FromLTRB(0,0,320,44));
			sb.SearchButtonClicked += (object sender, EventArgs e) => this.searchWith(sb.Text);
			sb.TextChanged += (object sender, UISearchBarTextChangedEventArgs e) => this.searchWith(sb.Text);

			this.TableView.TableHeaderView = sb;

			this.TableView.Source = new CurrenciesSectionSource (LoggedUser.Instance.Currencies);
			((CurrenciesSectionSource)TableView.Source).cellSelected += (sender, e) => {
				if (this.cellSelected != null)
					this.cellSelected (this, e);
			};

//			this.highlightSelectedCurrency ();
		}

		public void searchWith(string text) {
			((CurrenciesSectionSource)this.TableView.Source).SetCurrencies (LoggedUser.Instance.Currencies.SearchWith (text), !String.IsNullOrEmpty (text));
			this.TableView.ReloadData ();
		}

		private void highlightSelectedCurrency () {
			if (this.currency == null)
				return;

			IGrouping<string, Currency> selectedGroup = ((CurrenciesSectionSource)this.TableView.Source).Currencies.GroupedCurrencies.LastOrDefault (grouping => grouping.Contains (this.currency));
			if (selectedGroup != null) {
				NSIndexPath indexPath = NSIndexPath.FromRowSection (selectedGroup.ToList ().IndexOf (this.currency), ((CurrenciesSectionSource)this.TableView.Source).Currencies.GroupedCurrencies.IndexOf (selectedGroup));
				this.TableView.SelectRow (indexPath, false, UITableViewScrollPosition.Middle);
			}
		}
			
		public void setDataField(DataFieldCell dataField){
			this.currency = dataField.Field.GetValue<Currency> ();
		}
	}
}