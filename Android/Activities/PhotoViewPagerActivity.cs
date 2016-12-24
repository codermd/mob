using System;

using Android.OS;
using Android.App;
using Android.Support.V4.View;
using Mxp.Droid.Adapters;
using Mxp.Core.Business;
using System.Linq;
using DK.Ostebaronen.Droid.ViewPagerIndicator;
using Android.Views;
using Android.Support.V7.App;

namespace Mxp.Droid
{
	public class PhotoViewPagerActivity : BaseActivity
	{
		#pragma warning disable 0414
		private static readonly string TAG = typeof(PhotoViewPagerActivity).Name;
		#pragma warning restore 0414

		public static readonly string EXTRA_EXPENSE_ID = "com.sagacify.mxp.expense.id";

		public static readonly string EXTRA_REPORT_ID = "com.sagacify.mxp.report.id";
		public static readonly string EXTRA_REPORT_TYPE = "com.sagacify.mxp.report.type";

		public static readonly string EXTRA_RECEIPT_ID = "com.sagacify.mxp.receipt.id";
		public static readonly string EXTRA_RECEIPT_POSITION = "com.sagacify.mxp.receipt.position";

		private ViewPager mViewPager;

		private Report mReport;
		private Expense mExpense;

		private Receipts mReceipts {
			get {
				if (this.mExpense != null)
					return this.mExpense.Receipts;
				else if (this.mReport != null)
					return this.mReport.Receipts;
				else
					return null;
			}
		}

		protected override void OnCreate (Bundle savedInstanceState) {
			base.OnCreate (savedInstanceState);

			this.SetContentView (Resource.Layout.Pager_photo_view);

			Android.Support.V7.Widget.Toolbar toolbar = this.FindViewById<Android.Support.V7.Widget.Toolbar> (Resource.Id.Toolbar);
			this.SetSupportActionBar (toolbar);

			this.SupportActionBar.SetDisplayHomeAsUpEnabled (true);

			this.Title = Labels.GetLoggedUserLabel (Labels.LabelEnum.Receipt);

			this.mViewPager = this.FindViewById<ViewPager> (Resource.Id.Pager);

			int expenseId = this.Intent.GetIntExtra (EXTRA_EXPENSE_ID, -1);
			int reportId = this.Intent.GetIntExtra (EXTRA_REPORT_ID, -1);
			int reportType = this.Intent.GetIntExtra (EXTRA_REPORT_TYPE, -1);

			if (reportId != -1 && reportType != -1) {
				this.mReport = LoggedUser.Instance.GetReport ((Reports.ReportTypeEnum)reportType, reportId);
				if (expenseId != -1)
					this.mExpense = this.mReport.Expenses.Single (expense => expense.Id == expenseId);
			} else if (reportType != -1)
				this.mReport = LoggedUser.Instance.DraftReports.Single (report => report.IsNew);
			else if (expenseId != -1)
				this.mExpense = LoggedUser.Instance.BusinessExpenses.Single (expense => expense.Id == expenseId);
			else
				this.mExpense = LoggedUser.Instance.BusinessExpenses.Single (expense => expense.IsNew);

			this.mViewPager.Adapter = new PhotoPagerAdapter (this.mReceipts);

			CirclePageIndicator indicator = FindViewById<CirclePageIndicator>(Resource.Id.Indicator);
			indicator.SetViewPager(this.mViewPager);

			int receiptId = this.Intent.GetIntExtra (EXTRA_RECEIPT_ID, -1);
			int position = this.Intent.GetIntExtra (EXTRA_RECEIPT_POSITION, -1);

			if (receiptId != -1) {
				Receipt mReceipt = this.mReceipts.Single (receipt => receipt.AttachmentId == receiptId);
				this.mViewPager.CurrentItem = this.mReceipts.IndexOf (mReceipt);
			} else if (position != -1) {
				Receipt mReceipt = this.mReceipts.ElementAt (position);
				this.mViewPager.CurrentItem = this.mReceipts.IndexOf (mReceipt);
			}
		}

		public override bool OnOptionsItemSelected (IMenuItem item) {
			switch (item.ItemId) {
			case Android.Resource.Id.Home:
				this.Finish ();
				return true;
			}

			return base.OnOptionsItemSelected (item);
		}
	}
}