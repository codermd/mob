using System;
using Android.Widget;
using Mxp.Droid.Adapters;
using Mxp.Core.Services;
using Android.Util;

namespace Mxp.Droid.Filters
{
	public class FavouriteLocationsFilter : Filter
	{
		#pragma warning disable 0414
		private static readonly string TAG = typeof(FavouriteLocationsFilter).Name;
		#pragma warning restore 0414

		private readonly FavouriteLocationsAdapter mAdapter;

		public FavouriteLocationsFilter (FavouriteLocationsAdapter adapter) {
			this.mAdapter = adapter;
		}

		protected override FilterResults PerformFiltering (Java.Lang.ICharSequence constraint) {
			return null;
		}

		protected override async void PublishResults (Java.Lang.ICharSequence constraint, FilterResults results) {
			if (constraint == null)
				return;

			try {
				await MileageService.Instance.GetFavouriteLocations (this.mAdapter.MileageSegments, constraint.ToString ());

				if (this.mAdapter.MileageSegments != null && this.mAdapter.MileageSegments.Count > 0)
					this.mAdapter.NotifyDataSetChanged ();
				else
					this.mAdapter.NotifyDataSetInvalidated ();
			} catch (Exception e) {
				Log.Error (TAG, e.Message);
			}
		}
	}
}