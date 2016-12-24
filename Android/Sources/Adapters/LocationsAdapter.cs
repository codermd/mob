using Android.App;
using Android.Widget;
using Mxp.Core.Services;
using Mxp.Droid.Helpers;
using Mxp.Droid.Sources.Adapters;
using Mxp.Core.Business;

namespace Mxp.Droid.Adapters
{		
	public class LocationsAdapter : BaseSectionAdapter<WrappedObject>, IFilterable
	{
		#pragma warning disable 0414
		private static readonly string TAG = typeof(LocationsAdapter).Name;
		#pragma warning restore 0414

		private readonly Filter mFilter;
		private readonly GoogleService.PlaceTypeEnum mPlaceType;
		private readonly Country mCountry;

		public LocationsAdapter (Activity activity, GoogleService.PlaceTypeEnum placeType, Country country = null) : base (activity) {
			this.mFilter = new MultiFilter (this);
            this.mPlaceType = placeType;
			this.mCountry = country;
        }

		public override BaseAdapter<WrappedObject> InstantiateSection (int position) {
			switch (position) {
				case 0:
					return new FavouriteLocationsAdapter (this, this.mActivity, "Favourite locations");
				case 1:
					return new PredictionAdapter (this, this.mActivity, "Google Places", this.mPlaceType, this.mCountry);
				default:
					return null;
			}
		}

		public override int SectionCount => 2;

	    public void InvokeFilterOnEachSection (string search) {
			for (int i = 0; i < this.SectionCount; i++) {
				((IFilterable)this.GetSection (i)).Filter.InvokeFilter (search);
			}
		}

		public new Filter Filter => this.mFilter;

	    private class MultiFilter : Filter
		{
			private readonly LocationsAdapter mAdapter;

			public MultiFilter (LocationsAdapter adapter) {
				this.mAdapter = adapter;
			}

			protected override FilterResults PerformFiltering (Java.Lang.ICharSequence constraint) {
				if (constraint == null)
					return null;
				
				this.mAdapter.mActivity.RunOnUiThread (() => this.mAdapter.InvokeFilterOnEachSection (constraint.ToString ()));

				return null;
			}

			protected override void PublishResults (Java.Lang.ICharSequence constraint, FilterResults results) {
				// Nothing to do
			}
		}
	}
}