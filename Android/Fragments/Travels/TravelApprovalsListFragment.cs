using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Support.V4.App;
using Mxp.Core.Business;
using Android.OS;
using Android.Util;
using Android.Views;
using Android.Support.V4.Widget;
using Android.Widget;
using Mxp.Core.Services.Responses;
using Android.Content;
using Mxp.Droid.Helpers;

namespace Mxp.Droid.Fragments
{
	public class TravelApprovalsListFragment : ListFragment, BaseDialogFragment.IDialogClickListener
	{
		private TravelApprovalAdapter mApprovalAdapter;
		private TravelApprovals approvals;
		private SwipeRefreshLayout mRefresher;

		public override void OnCreate (Bundle savedInstanceState) {
			base.OnCreate (savedInstanceState);

			this.approvals = LoggedUser.Instance.TravelApprovals;

			this.mApprovalAdapter = new TravelApprovalAdapter (this.Activity, this.approvals);
			this.ListAdapter = this.mApprovalAdapter;
		}

		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
			View view = inflater.Inflate(Resource.Layout.List_approvals, container, false);
			this.mRefresher = view.FindViewById<SwipeRefreshLayout> (Resource.Id.approvals_refresher);
			this.mRefresher.SetColorSchemeResources(Android.Resource.Color.HoloBlueBright, Android.Resource.Color.HoloGreenLight, Android.Resource.Color.HoloOrangeLight, Android.Resource.Color.HoloRedLight);

			this.mRefresher.Refresh += (object sender, EventArgs e) => this.FetchTravelApprovalsAsync ();

			return view;
		}

		public override void OnViewCreated (View view, Bundle savedInstanceState) {
			base.OnViewCreated (view, savedInstanceState);

			if (!this.approvals.Loaded)
				this.mRefresher.Post (() => this.FetchTravelApprovalsAsync ());
		}

		public override void OnResume () {
			base.OnResume ();

			this.mApprovalAdapter.NotifyDataSetChanged ();
		}

		public override void OnListItemClick (ListView listview, View view, int position, long id) {
			Intent intent = new Intent (this.Activity, typeof (TravelDetailsActivity));
			intent.PutExtra (TravelDetailsActivity.EXTRA_TRAVEL_ID, this.approvals [position].Travel.Id);
			this.StartActivity(intent);
		}

		public async void FetchTravelApprovalsAsync () {
			this.mRefresher.Refreshing = true;
			await this.approvals.FetchAsync ().StartAsync (TaskConfigurator.Create (this));
			this.mRefresher.Refreshing = false;

			this.mApprovalAdapter.NotifyDataSetChanged ();
		}

		public void OnClickHandler<T> (int requestCode, DialogArgsObject<T> args) {
			
		}
	}
}