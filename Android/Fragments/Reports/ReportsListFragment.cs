using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Android.OS;
using Android.Util;
using Android.Views;
using Android.Support.V4.App;
using Android.Support.V4.Widget;

using Mxp.Core.Business;
using Android.Widget;
using Android.Content;
using Mxp.Droid.Helpers;

namespace Mxp.Droid.Fragments
{
	public class ReportsListFragment : ListFragment, BaseDialogFragment.IDialogClickListener
	{
		private const string TYPE = "com.sagacify.mxp.reports.type";

		private ReportAdapter mReportAdapter;
		private Reports mReports;
		private SwipeRefreshLayout mRefresher;

		public bool IsRefreshing {
			get {
				return this.mRefresher != null && this.mRefresher.Refreshing;
			}
		}

		public static ReportsListFragment NewInstance(Reports.ReportTypeEnum type) {
			ReportsListFragment reportsListFragment = new ReportsListFragment ();

			Bundle bundle = new Bundle ();
			bundle.PutInt (TYPE, (int) type);
			reportsListFragment.Arguments = bundle;

			return reportsListFragment;
		}

		public override void OnCreate (Bundle savedInstanceState) {
			base.OnCreate (savedInstanceState);

			switch ((Reports.ReportTypeEnum) this.Arguments.GetInt (TYPE)) {
				case Reports.ReportTypeEnum.Draft:
					this.mReports = LoggedUser.Instance.DraftReports;
					break;
				case Reports.ReportTypeEnum.Open:
					this.mReports = LoggedUser.Instance.OpenReports;
					break;
				case Reports.ReportTypeEnum.Closed:
					this.mReports = LoggedUser.Instance.ClosedReports;
					break;
			}

			this.mReportAdapter = new ReportAdapter(Activity, this.mReports);
			this.ListAdapter = this.mReportAdapter;
		}

		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
			View view = inflater.Inflate(Resource.Layout.List_reports, container, false);
			this.mRefresher = view.FindViewById<SwipeRefreshLayout> (Resource.Id.reports_refresher);
			this.mRefresher.SetColorSchemeResources(Android.Resource.Color.HoloBlueBright, Android.Resource.Color.HoloGreenLight, Android.Resource.Color.HoloOrangeLight, Android.Resource.Color.HoloRedLight);

			this.mRefresher.Refresh += (object sender, EventArgs e) => this.FetchReportsAsync ();

			return view;
		}

		public override void OnViewCreated (View view, Bundle savedInstanceState) {
			base.OnViewCreated (view, savedInstanceState);

			if (!this.mReports.Loaded)
				this.mRefresher.Post (() => this.FetchReportsAsync ());
		}

		public override void OnResume () {
			base.OnResume ();

			this.mReportAdapter.NotifyDataSetChanged ();
		}

		public override void OnListItemClick (ListView listView, View view, int position, long id) {
			Intent intent = new Intent (this.Activity, typeof(ReportDetailsActivity));

			intent.PutExtra (ReportDetailsActivity.EXTRA_REPORT_TYPE, (int) this.mReports[position].ReportType);
			intent.PutExtra (ReportDetailsActivity.EXTRA_REPORT_ID, this.mReports[position].Id.Value);

			this.StartActivity(intent);
		}

		public async void FetchReportsAsync () {
			this.mRefresher.Refreshing = true;
			await this.mReports.FetchAsync ().StartAsync (TaskConfigurator.Create (this));
			this.mRefresher.Refreshing = false;

			this.mReportAdapter.NotifyDataSetChanged ();
		}

		public void OnClickHandler<T> (int requestCode, DialogArgsObject<T> args) {
			
		}
	}
}