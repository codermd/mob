using System;
using Android.Widget;
using Mxp.Core.Business;
using Android.App;
using Android.Views;
using Mxp.Droid.Helpers;
using com.refractored.components.stickylistheaders;
using System.Linq;
using Mxp.Droid.ViewHolders;
using Mxp.Droid.Filters;
using Android.Util;

namespace Mxp.Droid.Adapters
{
	public class CurrencyListSectionSource : AbstractSectionAdapter<Currency, CurrencyDialogAdapter>, IFilterable
	{
		#pragma warning disable 0414
		private static readonly string TAG = typeof(CurrencyListSectionSource).Name;
		#pragma warning restore 0414

		private readonly Filter mCurrencyFilter;

		public Currencies FilteredCurrencies { get; set; }
		public Currencies OriginalCurrencies {
			get {
				return LoggedUser.Instance.Currencies;
			}
		}

		public CurrencyListSectionSource (CurrencyDialogAdapter parentAdapter, Activity activity) : base (parentAdapter, activity) {
			this.mCurrencyFilter = new CurrencyFilter (this);
			this.FilteredCurrencies = this.OriginalCurrencies;
		}

		public override long GetItemId (int position) {
			return position;
		}

		public override int Count {
			get {
				return this.FilteredCurrencies.Count;
			}
		}

		public override Currency this[int index] {
			get {
				return this.FilteredCurrencies [index];
			}
		}

		public override View GetView (int position, View convertView, ViewGroup parent) {
			CurrencyViewHolder viewHolder = null;

			if (convertView == null || convertView.Tag == null) {
				convertView = this.Activity.LayoutInflater.Inflate (Resource.Layout.List_currencies_item, parent, false);
				viewHolder = new CurrencyViewHolder (convertView);
				convertView.Tag = viewHolder;
			} else {
				viewHolder = convertView.Tag as CurrencyViewHolder;
			}

			//				if (position == LoggedUser.Instance.Countries.IndexOf (this.country)) {
			//					convertView.SetBackgroundColor (Android.Graphics.Color.LightGray);
			//				}

			viewHolder.BindView (this[position]);

			return convertView;
		}

		public override View GetHeaderView(int position, View convertView, ViewGroup parent) {
			CurrencyHeaderViewHolder headerViewHolder;

			if (convertView == null || convertView.Tag == null || !(convertView.Tag is CurrencyHeaderViewHolder)) {
				convertView = this.Activity.LayoutInflater.Inflate (Resource.Layout.List_categories_header, parent, false);
				headerViewHolder = new CurrencyHeaderViewHolder (convertView);
				convertView.Tag = headerViewHolder;
			} else {
				headerViewHolder = convertView.Tag as CurrencyHeaderViewHolder;
			}

			headerViewHolder.BindView (this[position]);

			return convertView;
		}

		public override long GetHeaderId(int position) {
			return this [position].Name.Substring (0, 1)[0];
		}

		public Filter Filter {
			get {
				return this.mCurrencyFilter;
			}
		}

		private class CurrencyHeaderViewHolder : ViewHolder<Currency>
		{
			private TextView Text { get; set; }

			public CurrencyHeaderViewHolder (View convertView) {
				this.Text = convertView.FindViewById<TextView> (Resource.Id.Text);
			}

			public override void BindView (Currency currency) {
				this.Text.Text = currency.Name.Substring (0, 1);
			}
		}
	}
}