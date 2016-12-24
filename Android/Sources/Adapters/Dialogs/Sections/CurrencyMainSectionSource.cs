using System;
using Mxp.Droid.Adapters;
using Mxp.Core.Business;
using Android.App;
using Android.Views;
using Mxp.Droid.Helpers;
using Android.Widget;
using Mxp.Droid.ViewHolders;
using com.refractored.components.stickylistheaders;
using System.Linq;

namespace Mxp.Droid
{
	public class CurrencyMainSectionSource : AbstractSectionAdapter<Currency, CurrencyDialogAdapter>
	{
		#pragma warning disable 0414
		private static readonly string TAG = typeof(CurrencyMainSectionSource).Name;
		#pragma warning restore 0414

		public Currency MainCurrency {
			get {
				return LoggedUser.Instance.Currencies.Single (currency => currency.Id == LoggedUser.Instance.Preferences.FldCurrencyId);
			}
		}

		public CurrencyMainSectionSource (CurrencyDialogAdapter parentAdapter, Activity activity) : base (parentAdapter, activity) {

		}

		public override long GetItemId (int position) {
			return position;
		}

		public override Currency this[int index] {
			get {
				return this.MainCurrency;
			}
		}

		public override int Count {
			get {
				return 1;
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
			MainCurrencyHeaderViewHolder headerViewHolder;

			if (convertView == null || convertView.Tag == null || !(convertView.Tag is MainCurrencyHeaderViewHolder)) {
				convertView = this.Activity.LayoutInflater.Inflate (Resource.Layout.List_currencies_header, parent, false);
				headerViewHolder = new MainCurrencyHeaderViewHolder (convertView);
				convertView.Tag = headerViewHolder;
			} else {
				headerViewHolder = convertView.Tag as MainCurrencyHeaderViewHolder;
			}
				
			return convertView;
		}

		public override long GetHeaderId(int position) {
			return 0;
		}

		private class MainCurrencyHeaderViewHolder : Java.Lang.Object {
			private TextView Text { get; set; }

			public MainCurrencyHeaderViewHolder (View convertView) {
				this.Text = convertView.FindViewById<TextView> (Resource.Id.Text);
				this.Text.Text = Labels.GetLoggedUserLabel(Labels.LabelEnum.CustomerCurrency);
			}
		}
	}
}