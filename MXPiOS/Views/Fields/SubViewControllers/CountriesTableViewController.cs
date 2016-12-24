using System;
using Foundation;
using Mxp.Core.Business;
using System.Collections.Generic;
using UIKit;
using CoreGraphics;
using System.Linq;

namespace Mxp.iOS
{
	public partial class CountriesTableViewController : MXPTableViewController
	{
		public event EventHandler<CountriesSectionSource.CountrySelectedEventArgs> cellSelected = delegate {};

		private Country country;
		private Countries countries;

		public CountriesTableViewController (Countries countries) : base () {
			this.countries = countries;
		}

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);
			_hidenotification = UIKeyboard.Notifications.ObserveDidHide(HideCallback);
			_shownotification = UIKeyboard.Notifications.ObserveWillShow(ShowCallback);
		}

		public override void ViewWillDisappear(bool animated)
		{
			base.ViewWillDisappear(animated);
			// Unregister the callbacks
			if (_shownotification != null)
				_shownotification.Dispose();
			if (_hidenotification != null)
				_hidenotification.Dispose();
		}

		void ShowCallback(object obj, UIKit.UIKeyboardEventArgs args)
		{
			this.TableView.ContentInset = new UIEdgeInsets(0.0f, 0.0f, args.FrameEnd.Size.Height, 0.0f);
		}

		void HideCallback(object obj, UIKit.UIKeyboardEventArgs args)
		{
			this.TableView.ContentInset = new UIEdgeInsets(0.0f, 0.0f, 0.0f, 0.0f);
		}

		NSObject _shownotification;
		NSObject _hidenotification;

		public override void ViewDidLoad () {
			base.ViewDidLoad ();

			this.Title = Labels.GetLoggedUserLabel (Labels.LabelEnum.Countries);

			UISearchBar sb = new UISearchBar(CGRect.FromLTRB(0, 0,320,44));
			sb.SearchButtonClicked += (object sender, EventArgs e) => this.searchWith(sb.Text);
			sb.TextChanged += (object sender, UISearchBarTextChangedEventArgs e) => this.searchWith(sb.Text);

			this.TableView.TableHeaderView = sb;

			this.TableView.Source = new CountriesSectionSource (this.countries);
			((CountriesSectionSource)TableView.Source).cellSelected += (sender, e) => {
				if (this.cellSelected != null)
					this.cellSelected (this, e);
			};

			this.highlightSelectedCountry ();
		}

		public void searchWith(string text) {
			((CountriesSectionSource)this.TableView.Source).SetCountries (this.countries.SearchWith (text), !String.IsNullOrEmpty (text));
			this.TableView.ReloadData ();
		}

		private void highlightSelectedCountry () {
			if (this.country == null)
				return;

			IGrouping<string, Country> selectedGroup = ((CountriesSectionSource)this.TableView.Source).Countries.GroupedCountries.LastOrDefault (grouping => grouping.Contains (this.country));
			if (selectedGroup != null) {
				NSIndexPath indexPath = NSIndexPath.FromRowSection (selectedGroup.ToList ().IndexOf (this.country), ((CountriesSectionSource)this.TableView.Source).Countries.GroupedCountries.IndexOf (selectedGroup));
				this.TableView.SelectRow (indexPath, false, UITableViewScrollPosition.Middle);
			}
		}

		public void setDataField (DataFieldCell dataField) {
			this.country = dataField.Field.GetValue<Country> ();
		}
	}
}