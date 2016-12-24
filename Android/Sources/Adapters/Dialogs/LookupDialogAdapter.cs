using System;
using Android.Widget;
using Mxp.Core.Business;
using Android.App;
using com.refractored.components.stickylistheaders;
using Android.Views;
using Mxp.Droid.Utils;
using Mxp.Droid.Helpers;
using Android.Util;
using System.Linq;
using Mxp.Droid.Filters;
using Mxp.Core.Services;

namespace Mxp.Droid.Adapters
{		
	public class LookupDialogAdapter : BaseAdapter<LookupItem>, IFilterable
	{
		#pragma warning disable 0414
		private static readonly string TAG = typeof(LookupDialogAdapter).Name;
		#pragma warning restore 0414

		private Activity mActivity;

		public Filter mLookupFilter { get; private set; }

		public LookupItems mLookupItems { get; set; }

		public LookupDialogAdapter (Activity activity, LookupService.ApiEnum value, LookupItems lookupItems) : base () {
			this.mActivity = activity;
			this.mLookupItems = lookupItems;
			this.mLookupFilter = new LookupFilter (this, value);
		}

		public override int Count {
			get {
				return this.mLookupItems.Count;
			}
		}

		public override LookupItem this[int index] {
			get {
				return this.mLookupItems [index];
			}
		}

		public override long GetItemId (int position) {
			return position;
		}

		public override View GetView (int position, View convertView, ViewGroup parent) {
			LookupItemViewHolder viewHolder = null;

			if (convertView == null || convertView.Tag == null) {
				convertView = this.mActivity.LayoutInflater.Inflate (Resource.Layout.List_lookups_item, parent, false);
				viewHolder = new LookupItemViewHolder (convertView);
				convertView.Tag = viewHolder;
			} else {
				viewHolder = convertView.Tag as LookupItemViewHolder;
			}

//				if (position == LoggedUser.Instance.Countries.IndexOf (this.country)) {
//					convertView.SetBackgroundColor (Android.Graphics.Color.LightGray);
//				}

			viewHolder.BindView (this[position]);

			return convertView;
		}

		public Filter Filter {
			get {
				return this.mLookupFilter;
			}
		}

		private class LookupItemViewHolder : ViewHolder<LookupItem>
		{
			private TextView Text { get; set; }

			public LookupItemViewHolder (View convertView) {
				this.Text = convertView.FindViewById<TextView> (Android.Resource.Id.Text1);
			}

			public override void BindView (LookupItem lookupItem) {
				this.Text.Text = lookupItem.Description;
			}
		}
	}
}