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
using Mxp.Droid.Helpers;
using Mxp.Utils;
using System.Collections.Generic;
using Mxp.Core.Services;
using Mxp.Core.Helpers;
using Mxp.Core.Utils;
using Android.Gms.Maps;
using Android.Gms.Common;
using System.ComponentModel;

namespace Mxp.Droid.Fragments
{
	public class MileageMapFragment : Android.Support.V4.App.Fragment, IOnMapReadyCallback
	{
		#pragma warning disable 0414
		private static readonly string TAG = typeof(MileageMapFragment).Name;
		#pragma warning restore 0414

		private View mView;
		private GoogleMapHelper googleMapHelper;

		private Mileage Mileage {
			get {
				return ((ExpenseDetailsActivity) this.Activity).ExpenseItem.ParentExpense as Mileage;
			}
		}

		public static MileageMapFragment NewInstance () {
			MileageMapFragment expenseAttendeesListFragment = new MileageMapFragment ();

			return expenseAttendeesListFragment;
		}

		public override void OnCreate (Bundle savedInstanceState) {
			base.OnCreate (savedInstanceState);

			this.googleMapHelper = new GoogleMapHelper (this.Activity, this.Mileage);
		}

		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
			// cf. http://stackoverflow.com/questions/14083950/duplicate-id-tag-null-or-parent-id-with-another-fragment-for-com-google-androi/14695397#14695397
			if (this.mView != null) {
				ViewGroup parent = (ViewGroup) this.mView.Parent;
				if (parent != null)
					parent.RemoveView (this.mView);
			}
			try {
				this.mView = inflater.Inflate (Resource.Layout.Mileage_map_fragment, container, false);
			} catch (InflateException) {
				/* map is already there, just return view as it is */
			}

			try {
				// Prevent "CameraUpdateFactory is not initialized" such Exception
				MapsInitializer.Initialize (this.Activity);
			} catch (GooglePlayServicesNotAvailableException e) {
				Log.Error(TAG, "Could not initialize Google Play Services", e);
			}

			int isGooglePlayServicesAvailable = GoogleApiAvailability.Instance.IsGooglePlayServicesAvailable (this.Activity);
			switch (isGooglePlayServicesAvailable) {
				case ConnectionResult.Success:
					((SupportMapFragment)ChildFragmentManager.FindFragmentById (Resource.Id.Map)).GetMapAsync (this);
					break;
				case ConnectionResult.ServiceMissing: 
					Toast.MakeText (this.Activity, "SERVICE MISSING", ToastLength.Short).Show ();
					break;
				case ConnectionResult.ServiceVersionUpdateRequired: 
					Toast.MakeText (this.Activity, "UPDATE REQUIRED", ToastLength.Short).Show ();
					break;
				default:
					if (GoogleApiAvailability.Instance.IsUserResolvableError (isGooglePlayServicesAvailable)) {
						Toast.MakeText (this.Activity, "There is a problem with Google Play Services on this device: " + GoogleApiAvailability.Instance.GetErrorString (isGooglePlayServicesAvailable), ToastLength.Short).Show ();
						break;
					}

					Toast.MakeText (this.Activity, "Google Play Services is not installed: " + isGooglePlayServicesAvailable, ToastLength.Short).Show ();
					break;
			}

			return this.mView;
		}

		public override void OnResume () {
			base.OnResume ();

			this.Mileage.PropertyChanged += HandlePropertyChangedEventHandler;
		}

		public override void OnPause () {
			base.OnPause ();

			this.Mileage.PropertyChanged -= HandlePropertyChangedEventHandler;
		}

		private void HandlePropertyChangedEventHandler (object sender, PropertyChangedEventArgs e) {
			Log.Info (TAG, "Event " + e.PropertyName + " fired !");

			if (e.PropertyName == "Directions") {
				this.googleMapHelper.RefreshPolylinesOnMap ();
			} else
				this.googleMapHelper.RefreshMarkersOnMap ();
		}

		public void OnMapReady (GoogleMap googleMap) {
			this.googleMapHelper.OnMapReady (googleMap);
		}
	}
}