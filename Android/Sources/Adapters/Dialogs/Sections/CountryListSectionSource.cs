using Android.Widget;
using Mxp.Core.Business;
using Android.App;
using Android.Views;
using Mxp.Droid.Helpers;
using Mxp.Droid.ViewHolders;
using Mxp.Droid.Filters;

namespace Mxp.Droid.Adapters
{
	public class CountryListSectionSource : AbstractSectionAdapter<Country, CountryDialogAdapter>, IFilterable
	{
		#pragma warning disable 0414
		private static readonly string TAG = typeof(CountryListSectionSource).Name;
		#pragma warning restore 0414

		public Filter mCountryFilter { get; private set; }

		public Countries FilteredCountries { get; set; }
		public Countries OriginalCountries { get; private set; }

		public CountryListSectionSource (CountryDialogAdapter parentAdapter, Activity activity, Countries countries) : base (parentAdapter, activity) {
			this.mCountryFilter = new CountryFilter (this);
			this.OriginalCountries = countries;
			this.FilteredCountries = this.OriginalCountries;
		}

		public override long GetItemId (int position) {
			return position;
		}

		public override Country this[int index] {
			get {
				return this.FilteredCountries [index];
			}
		}

		public override int Count {
			get {
				return this.FilteredCountries.Count;
			}
		}

		public override View GetView (int position, View convertView, ViewGroup parent) {
			CountryViewHolder viewHolder = null;

			if (convertView == null || convertView.Tag == null) {
				convertView = this.Activity.LayoutInflater.Inflate (Resource.Layout.List_countries_item, parent, false);
				viewHolder = new CountryViewHolder (convertView);
				convertView.Tag = viewHolder;
			} else {
				viewHolder = convertView.Tag as CountryViewHolder;
			}

//				if (position == LoggedUser.Instance.Countries.IndexOf (this.country)) {
//					convertView.SetBackgroundColor (Android.Graphics.Color.LightGray);
//				}

			viewHolder.BindView (this[position]);

			return convertView;
		}

		public Filter Filter {
			get {
				return this.mCountryFilter;
			}
		}

		public override View GetHeaderView(int position, View convertView, ViewGroup parent) {
			CountryHeaderViewHolder headerViewHolder;

			if (convertView == null || convertView.Tag == null || !(convertView.Tag is CountryHeaderViewHolder)) {
				convertView = this.Activity.LayoutInflater.Inflate (Resource.Layout.List_countries_header, parent, false);
				headerViewHolder = new CountryHeaderViewHolder (convertView);
				convertView.Tag = headerViewHolder;
			} else
				headerViewHolder = convertView.Tag as CountryHeaderViewHolder;

			headerViewHolder.BindView (this[position]);

			return convertView;
		}

		public override long GetHeaderId(int position) {
			return this [position].IndexName[0];
		}

		private class CountryHeaderViewHolder : ViewHolder<Country> {
			private TextView Text { get; set; }

			public CountryHeaderViewHolder (View convertView) {
				this.Text = convertView.FindViewById<TextView> (Resource.Id.Text);
			}

			public override void BindView (Country country) {
				this.Text.Text = country.IndexName;
			}
		}
	}
}