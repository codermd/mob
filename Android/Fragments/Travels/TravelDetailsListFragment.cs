using System;
using Android.Support.V4.App;
using Android.OS;
using Android.Util;
using Mxp.Core.Business;
using Mxp.Droid.Adapters;
using Android.Widget;
using Android.Views;
using Android.App;
using Android.Content;
using Mxp.Utils;
using Mxp.Core.Helpers;
using System.Collections.ObjectModel;
using System.Linq;

namespace Mxp.Droid.Fragments
{
	public class TravelDetailsListFragment : Android.Support.V4.App.ListFragment
	{
		private static readonly string TAG = typeof(TravelDetailsListFragment).Name;

		public static readonly string EXTRA_TRAVEL_ID = "com.sagacify.mxp.travel.id";
		public static readonly string EXTRA_TAB_POSITION = "com.sagacify.mxp.tab.position";

		private Travel mTravel;
		private TravelSectionAdapter mTravelDetailsAdapter;

		public static TravelDetailsListFragment NewInstance (int travelId, int tabPosition) {
			TravelDetailsListFragment fragment = new TravelDetailsListFragment ();

			Bundle bundle = new Bundle ();
			bundle.PutInt (EXTRA_TRAVEL_ID, travelId);
			bundle.PutInt (EXTRA_TAB_POSITION, tabPosition);
			fragment.Arguments = bundle;

			return fragment;
		}

		public override void OnCreate (Bundle savedInstanceState) {
			base.OnCreate (savedInstanceState);

			Log.Debug(TAG, "OnCreate ExpenseDetailsList fragment");

			int travelId = this.Arguments.GetInt (EXTRA_TRAVEL_ID);
			int tabPosition = this.Arguments.GetInt (EXTRA_TAB_POSITION);

			if (travelId != -1 && tabPosition != -1)
				this.mTravel = LoggedUser.Instance.TravelApprovals.Single (approval => approval.Travel.Id == travelId).Travel;

			Collection<TableSectionModel> tableSections = null;

			switch (tabPosition) {
				case 0:
					tableSections = this.mTravel.GetMainFields ();
					break;
				case 1:
					tableSections = this.mTravel.GetFlightsFields ();
					break;
				case 2:
					tableSections = this.mTravel.GetStayFields ();
					break;
				case 3:
					tableSections = this.mTravel.GetCarRentalsFields ();
					break;
			}

			this.mTravelDetailsAdapter = new TravelSectionAdapter (this.ChildFragmentManager, this.Activity, tableSections);
			this.ListAdapter = this.mTravelDetailsAdapter;
		}

		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
			return inflater.Inflate (Resource.Layout.List_sections, container, false);
		}

		public override void OnListItemClick (ListView listView, View view, int position, long id) {
			this.mTravelDetailsAdapter.OnListItemClick (listView, view, position, id);
		}
	}
}