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
	public class ReportDetailsListFragment : Android.Support.V4.App.ListFragment
	{
		#pragma warning disable 0414
		private static readonly string TAG = typeof(ReportDetailsListFragment).Name;
		#pragma warning restore 0414

		private Report mReport;
		private ReportDetailsAdapter mExpenseDetailsAdapter;

		public static ReportDetailsListFragment NewInstance () {
			ReportDetailsListFragment reportDetailsListFragment = new ReportDetailsListFragment ();

			return reportDetailsListFragment;
		}

		public override void OnCreate (Bundle savedInstanceState) {
			base.OnCreate (savedInstanceState);

			this.mReport = ((IReportListener)this.Activity).GetReport ();

			this.mExpenseDetailsAdapter = new ReportDetailsAdapter (this.ChildFragmentManager, this.Activity, this.mReport);
			this.ListAdapter = this.mExpenseDetailsAdapter;
		}

		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
			return inflater.Inflate (Resource.Layout.List_sections, container, false);
		}

		public override void OnListItemClick (ListView listView, View view, int position, long id) {
			this.mExpenseDetailsAdapter.OnListItemClick (listView, view, position, id);
		}
	}
}