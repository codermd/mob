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
using Mxp.Droid.Adapters;
using System.Threading.Tasks;

namespace Mxp.Droid.Fragments
{
	public class ReportHistoryListFragment : ListFragment
	{
		#pragma warning disable 0414
		private static readonly string TAG = typeof(ReportHistoryListFragment).Name;
		#pragma warning restore 0414

		private Report mReport;
		private HistoryAdapter mHistoryAdapter;

		private ReportHistoryItems mReportHistoryItems {
			get {
				return this.mReport.History;
			}
		}

		public static ReportHistoryListFragment NewInstance () {
			ReportHistoryListFragment reportHistoryListFragment = new ReportHistoryListFragment ();

			return reportHistoryListFragment;
		}

		public override void OnCreate (Bundle savedInstanceState) {
			base.OnCreate (savedInstanceState);

			this.mReport = ((IReportListener)this.Activity).GetReport ();

			this.mHistoryAdapter = new HistoryAdapter (Activity, this.mReportHistoryItems);
			this.ListAdapter = this.mHistoryAdapter;
		}
	}
}