using System;
using Mxp.Core.Services.Google;
using Android.Content;
using Android.Widget;
using System.Collections.Generic;
using Mxp.Droid.Filters;
using Android.App;
using Android.Views;
using Mxp.Core.Business;
using Mxp.Droid.Helpers;

namespace Mxp.Droid.Adapters
{
	public class FavouriteLocationsAdapter : AbstractSectionAdapter<WrappedObject, BaseSectionAdapter<WrappedObject>>, IFilterable
	{
		#pragma warning disable 0414
		private static readonly string TAG = typeof(FavouriteLocationsAdapter).Name;
		#pragma warning restore 0414

		public MileageSegments MileageSegments { get; set; }
		private Filter filter;

		public FavouriteLocationsAdapter (BaseSectionAdapter<WrappedObject> parentAdapter, Activity activity, string title) : base (parentAdapter, activity, title) {
			this.MileageSegments = new MileageSegments ();
			this.filter = new FavouriteLocationsFilter (this);
		}

		public override long GetItemId (int position) {
			return position;
		}

		public override int Count {
			get {
				return this.MileageSegments.Count;
			}
		}

		public override WrappedObject this[int index] {
			get {
				return new WrappedObject (this.MileageSegments [index]);
			}
		}

		public override View GetView (int position, View convertView, ViewGroup parent) {
			FavouriteLocationViewHolder viewHolder = null;

			if (convertView == null || convertView.Tag == null || !(convertView.Tag is FavouriteLocationViewHolder)) {
				convertView = this.Activity.LayoutInflater.Inflate (Android.Resource.Layout.SimpleDropDownItem1Line, parent, false);
				viewHolder = new FavouriteLocationViewHolder (convertView);
				convertView.Tag = viewHolder;
			} else {
				viewHolder = convertView.Tag as FavouriteLocationViewHolder;
			}

			viewHolder.BindView (this [position].GetInstance<MileageSegment> ());

			return convertView;
		}

		private class FavouriteLocationViewHolder : ViewHolder<MileageSegment> {
			private TextView TextView { get; set; }

			public FavouriteLocationViewHolder (View convertView) {
				this.TextView = convertView.FindViewById<TextView> (Android.Resource.Id.Text1);
			}

			public override void BindView (MileageSegment mileageSegment) {
				this.TextView.Text = mileageSegment.LocationAliasName;
			}
		}
			
		public Filter Filter {
			get {
				return this.filter;
			}
		}
	}
}