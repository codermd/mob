using System;
using Android.Gms.Common;
using Mxp.Core.Business;
using Mxp.Droid.Adapters;
using Android.OS;
using Android.Util;
using Android.Views;
using Android.Widget;
using Android.Locations;
using Android.Gms.Location;
using Android.Gms.Common.Apis;
using Mxp.Droid.Utils;
using System.ComponentModel;
using Mxp.Droid.Fragments.Headless;
using Android;
using Android.Support.Design.Widget;

namespace Mxp.Droid.Fragments
{
	public class MileageDetailsListFragment : Android.Support.V4.App.ListFragment, GoogleApiClient.IConnectionCallbacks, GoogleApiClient.IOnConnectionFailedListener, BaseDialogFragment.IDialogClickListener, HeadlessPermissionFragment.IPermissionListener
	{
		private static readonly string TAG = typeof(MileageDetailsListFragment).Name;

		public const int REQUEST_LOCATION = 0;

		public const string LOCATION_PERMISSION = Manifest.Permission.AccessFineLocation;

		private GoogleApiClient mGoogleApiClient;
		private MileageDetailsAdapter mMileageDetailsAdapter;
		private HeadlessPermissionFragment mHeadlessPermissionFragment;

		private Location mCurrentLocation;

		private Mileage Mileage {
			get {
				return ((ExpenseDetailsActivity) this.Activity).ExpenseItem.ParentExpense as Mileage;
			}
		}

		public static MileageDetailsListFragment NewInstance () {
			MileageDetailsListFragment expenseDetailsListFragment = new MileageDetailsListFragment ();

			return expenseDetailsListFragment;
		}

		public override void OnCreate (Bundle savedInstanceState) {
			base.OnCreate (savedInstanceState);

			this.mHeadlessPermissionFragment = HeadlessFragmentHelper<HeadlessPermissionFragment>.Attach (this);

			((ExpenseDetailsActivity)this.Activity).IsLoaded = false;

			Log.Debug(TAG, "OnCreate MileageDetailsList fragment");

			this.SetMileageDetailsAdapter ();

			// Segments needed by "shown"'s condition fields
			if (this.Mileage.IsNew)
				this.Activity.InvokeActionAsync (this.Mileage.Vehicles.FetchAsync, () => {
					((ExpenseDetailsActivity)this.Activity).IsLoaded = true;
					this.SetMileageDetailsAdapter ();
				}, this);
			else
				this.Activity.InvokeActionAsync (this.Mileage.MileageSegments.FetchAsync, () => {
					((ExpenseDetailsActivity)this.Activity).IsLoaded = true;
					this.mMileageDetailsAdapter.NotifyDataSetChanged ();
				}, this);

			this.BuildGoogleApiClient ();
		}

		private void SetMileageDetailsAdapter () {
			this.mMileageDetailsAdapter = new MileageDetailsAdapter (this.ChildFragmentManager, this.Activity, this.Mileage);
			((MileageSegmentsAdapter<MileageDetailsAdapter>)this.mMileageDetailsAdapter.GetSection (0)).CurrentLocation = this.mCurrentLocation;
			this.ListAdapter = this.mMileageDetailsAdapter;
		}

		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
			return inflater.Inflate (Resource.Layout.Mileage_details, null);
		}

		public override void OnStart () {
			base.OnStart ();

			this.mHeadlessPermissionFragment.CheckPermission (REQUEST_LOCATION, LOCATION_PERMISSION);
		}

		public override void OnResume () {
			base.OnResume ();

			this.Mileage.PropertyChanged += HandlePropertyChangedEventHandler;
		}

		public override void OnPause () {
			base.OnPause ();

			this.Mileage.PropertyChanged -= HandlePropertyChangedEventHandler;
		}

		public override void OnStop () {
			base.OnStop ();

			if (mGoogleApiClient.IsConnected)
				mGoogleApiClient.Disconnect ();
		}

		public override void OnListItemClick (ListView listView, View view, int position, long id) {
			this.mMileageDetailsAdapter.OnListItemClick (listView, view, position, id);
		}
			
		public void OnConnected (Bundle bundle) {
			this.mCurrentLocation = LocationServices.FusedLocationApi.GetLastLocation (this.mGoogleApiClient);
			if (this.mMileageDetailsAdapter != null)
				((MileageSegmentsAdapter<MileageDetailsAdapter>)this.mMileageDetailsAdapter.GetSection (0)).CurrentLocation = this.mCurrentLocation;
		}

		public void OnConnectionSuspended (int cause) {
			Log.Info(TAG, "Connection suspended");
			this.mGoogleApiClient.Connect ();
		}

		public void OnConnectionFailed (ConnectionResult result) {
			Log.Info(TAG, "Connection failed: ConnectionResult.getErrorCode() = " + result.ErrorCode);
		}

		private void HandlePropertyChangedEventHandler (object sender, PropertyChangedEventArgs e) {
			Log.Info (TAG, "Event " + e.PropertyName + " fired !");

			this.mMileageDetailsAdapter.NotifyDataSetChanged ();
		}

		private void BuildGoogleApiClient () {
			this.mGoogleApiClient = new GoogleApiClient.Builder (this.Activity)
				.AddConnectionCallbacks (this)
				.AddOnConnectionFailedListener (this)
				.AddApi (LocationServices.API)
				.Build ();
		}

		public void OnClickHandler<T> (int requestCode, DialogArgsObject<T> args) {

		}

		#region HeadlessPermissionFragment.IPermissionListener

		public void OnPermissionResult (int requestCode, bool granted) {
			if (!granted)
				return;

			this.mGoogleApiClient.Connect ();
		}

		public void ShouldShowPermissionRationale (int requestCode, String[] permissions) {
			if (permissions [0] == LOCATION_PERMISSION)
				Snackbar.Make (this.View, "Location access is required for enable current location action.", Snackbar.LengthIndefinite)
					.SetAction ("OK", v => this.mHeadlessPermissionFragment.RequestPermission (requestCode, permissions [0]))
					.Show ();
		}

		#endregion
	}
}