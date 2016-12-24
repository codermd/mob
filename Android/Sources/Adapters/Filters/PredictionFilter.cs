using Android.Support.V7.App;
using Android.Util;
using Android.Widget;
using Java.Lang;
using Mxp.Core.Business;
using Mxp.Core.Services;
using Mxp.Droid.Fragments;

namespace Mxp.Droid.Sources.Adapters.Filters
{
	public class PredictionFilter : Filter
	{
		private readonly GoogleService.PlaceTypeEnum mPlaceType;

		private readonly PredictionAdapter mAdapter;

		public PredictionFilter (PredictionAdapter adapter, GoogleService.PlaceTypeEnum placeType) {
			this.mAdapter = adapter;
			this.mPlaceType = placeType;
		}

		protected override FilterResults PerformFiltering (ICharSequence constraint) {
			return null;
		}

		protected override async void PublishResults (ICharSequence constraint, FilterResults results) {
			if (constraint == null)
				return;

			try {
				this.mAdapter.Predictions = (await GoogleService.Instance.FetchPlacesLocationsAsync (constraint.ToString () ?? string.Empty, this.mPlaceType, this.mAdapter.Country)).predictions;

				if (this.mAdapter.Predictions?.Count > 0)
					this.mAdapter.NotifyDataSetChanged ();
				else
					this.mAdapter.NotifyDataSetInvalidated ();
			} catch (Exception) { }
		}
	}
}