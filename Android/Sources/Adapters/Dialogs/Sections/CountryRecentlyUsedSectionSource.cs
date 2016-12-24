using System;
using Mxp.Droid.Adapters;
using Mxp.Core.Business;
using Android.App;
using Android.Views;
using Mxp.Droid.Helpers;
using Android.Widget;
using Mxp.Droid.ViewHolders;
using com.refractored.components.stickylistheaders;

namespace Mxp.Droid
{
	public class CountryRecentlyUsedSectionSource : AbstractSectionAdapter<Country, CountryDialogAdapter>
	{
		#pragma warning disable 0414
		private static readonly string TAG = typeof(CountryRecentlyUsedSectionSource).Name;
		#pragma warning restore 0414

		private Countries mCountries;

		public Countries RecentlyUsedCountries {
			get {
				return this.mCountries.RecentlyUsedCountry;
			}
		}

		public CountryRecentlyUsedSectionSource (CountryDialogAdapter parentAdapter, Activity activity, Countries countries) : base (parentAdapter, activity) {
			this.mCountries = countries;
		}

		public override long GetItemId (int position) {
			return position;
		}

		public override Country this[int index] {
			get {
				return this.RecentlyUsedCountries [index];
			}
		}

		public override int Count {
			get {
				return this.RecentlyUsedCountries.Count;
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

		public override View GetHeaderView(int position, View convertView, ViewGroup parent) {
			RecentlyUsedHeaderViewHolder headerViewHolder;

			if (convertView == null || convertView.Tag == null || !(convertView.Tag is RecentlyUsedHeaderViewHolder)) {
				convertView = this.Activity.LayoutInflater.Inflate (Resource.Layout.List_countries_header, parent, false);
				headerViewHolder = new RecentlyUsedHeaderViewHolder (convertView);
				convertView.Tag = headerViewHolder;
			} else {
				headerViewHolder = convertView.Tag as RecentlyUsedHeaderViewHolder;
			}
				
			return convertView;
		}

		public override long GetHeaderId(int position) {
			return 0;
		}

		private class RecentlyUsedHeaderViewHolder : Java.Lang.Object {
			private TextView Text { get; set; }

			public RecentlyUsedHeaderViewHolder (View convertView) {
				this.Text = convertView.FindViewById<TextView> (Resource.Id.Text);
				this.Text.Text = Labels.GetLoggedUserLabel(Labels.LabelEnum.Recents);
			}
		}
	}
}