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
	public class ReportApprovalsListFragment : ListFragment, BaseDialogFragment.IDialogClickListener
	{
		private ReportApprovalAdapter mApprovalAdapter;
		private ReportApprovals approvals;
		private SwipeRefreshLayout mRefresher;

		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
			View view = inflater.Inflate(Resource.Layout.List_approvals, container, false);
			this.mRefresher = view.FindViewById<SwipeRefreshLayout> (Resource.Id.approvals_refresher);
			this.mRefresher.SetColorSchemeResources(Android.Resource.Color.HoloBlueBright, Android.Resource.Color.HoloGreenLight, Android.Resource.Color.HoloOrangeLight, Android.Resource.Color.HoloRedLight);

			this.mRefresher.Refresh += (object sender, EventArgs e) => this.FetchApprovalsAsync ();

			return view;
		}

		public override void OnCreate (Bundle savedInstanceState) {
			base.OnCreate (savedInstanceState);

			this.approvals = LoggedUser.Instance.ReportApprovals;

			this.mApprovalAdapter = new ReportApprovalAdapter (this.Activity, this.approvals);
			this.ListAdapter = this.mApprovalAdapter;
		}

		public override void OnViewCreated (View view, Bundle savedInstanceState) {
			base.OnViewCreated (view, savedInstanceState);

			if (!this.approvals.Loaded)
				this.mRefresher.Post (() => this.FetchApprovalsAsync ());
		}

		public override void OnResume () {
			base.OnResume ();

			this.mApprovalAdapter.NotifyDataSetChanged ();
		}

		public override void OnListItemClick (ListView listView, View view, int position, long id) {
			Intent intent = new Intent (this.Activity, typeof(ReportDetailsActivity));

			intent.PutExtra (ReportDetailsActivity.EXTRA_REPORT_TYPE, (int) this.approvals[position].Report.ReportType);
			intent.PutExtra (ReportDetailsActivity.EXTRA_REPORT_ID, this.approvals[position].Report.Id.Value);

			this.StartActivity (intent);
		}

		public async void FetchApprovalsAsync () {
			this.mRefresher.Refreshing = true;
			await this.approvals.FetchAsync ().StartAsync (TaskConfigurator.Create (this));
			this.mRefresher.Refreshing = false;

			this.mApprovalAdapter.NotifyDataSetChanged ();
		}

		public void OnClickHandler<T> (int requestCode, DialogArgsObject<T> args) {
			
		}
	}
}