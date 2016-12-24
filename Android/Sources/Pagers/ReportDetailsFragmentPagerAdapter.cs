using System;
using Android.Support.V4.App;
using Mxp.Droid.Fragments;
using Mxp.Core.Business;

namespace Mxp.Droid.Adapters
{
	public class ReportDetailsFragmentPagerAdapter : FragmentPagerAdapter
	{
		private Report mReport;

		public ReportDetailsFragmentPagerAdapter (FragmentManager fm, Report report) : base (fm) {
			this.mReport = report;
		}

		public override int Count {
			get {
				return 2
					+ (this.mReport.CanShowReceipts ? 1 : 0)
					+ (this.mReport.CanShowHistory ? 1 : 0);
			}
		}

		public override Fragment GetItem (int position) {
			switch (position) {
				case 0:
					return ReportDetailsListFragment.NewInstance ();
				case 1:
					return ReportExpensesListFragment.NewInstance ();
				case 2:
					if (this.mReport.CanShowReceipts)
						return ReportReceiptsFragment.NewInstance ();
					else
						return ReportHistoryListFragment.NewInstance ();
				case 3:
					return ReportHistoryListFragment.NewInstance ();
				default:
					return null;
			}
		}

		public override Java.Lang.ICharSequence GetPageTitleFormatted (int position) {
			switch (position) {
				case 0:
					return new Java.Lang.String (Labels.GetLoggedUserLabel (Labels.LabelEnum.Details));
				case 1:
					return new Java.Lang.String (Labels.GetLoggedUserLabel (Labels.LabelEnum.Expenses));
				case 2:
					return new Java.Lang.String (Labels.GetLoggedUserLabel (Labels.LabelEnum.Receipts));
				case 3:
					return new Java.Lang.String (Labels.GetLoggedUserLabel (Labels.LabelEnum.History));
				default:
					return null;
			}
		}
	}
}