using Java.Interop;
﻿using System;

using Android.Support.V7.App;
using Android.OS;
using Android.Support.V4.View;
using Mxp.Core.Business;
using Mxp.Droid.Helpers;
using Mxp.Droid.Fragments;
using Android.Content;
using Android.Support.V4.App;
using Mxp.Droid.Adapters;
using System.Linq;
using Android.Views;
using System.Threading.Tasks;
using Mxp.Droid.Utils;
using Android.App;
using Mxp.Core.Helpers;
using Android.Support.V7.Widget;
using Android.Support.Design.Widget;

namespace Mxp.Droid
{		
	public class ReportDetailsActivity : BaseActivity, IReportListener
	{
		#pragma warning disable 0414
		private static readonly string TAG = typeof(ReportDetailsActivity).Name;
		#pragma warning restore 0414

		public static readonly string EXTRA_REPORT_TYPE = "com.sagacify.mxp.report.type";
		public static readonly string EXTRA_REPORT_ID = "com.sagacify.mxp.report.id";

		private const int deleteConfirmDialogRequestCode = 0;
		private const int cancelConfirmDialogRequestCode = 1;
		private const int submitConfirmDialogRequestCode = 2;

		private enum ModeEnum {
			Creating,
			Reading
		}

		private ModeEnum mMode = ModeEnum.Reading;
		private ViewPager mViewPager;
		private Report mReport;

		protected override void OnCreate (Bundle savedInstanceState) {
			int reportId = this.Intent.GetIntExtra (EXTRA_REPORT_ID, -1);
			int reportType = this.Intent.GetIntExtra (EXTRA_REPORT_TYPE, -1);

			if (reportId != -1 && reportType != -1)
				this.mReport = LoggedUser.Instance.GetReport ((Reports.ReportTypeEnum)reportType, reportId);
			else {
				this.mMode = ModeEnum.Creating;

				this.mReport = LoggedUser.Instance.DraftReports.Single (report => report.IsNew);
			}

			base.OnCreate (savedInstanceState);

			this.SetContentView (Resource.Layout.Pager_generic);

			Toolbar toolbar = this.FindViewById<Toolbar> (Resource.Id.Toolbar);
			this.SetSupportActionBar (toolbar);

			this.SupportActionBar.SetDisplayHomeAsUpEnabled (true);

			this.Title = this.mReport.IsNew ? Labels.GetLoggedUserLabel (Labels.LabelEnum.CreateReport) : this.mReport.VDetailsBarTitle;

			TabLayout tabLayout = this.FindViewById<TabLayout> (Resource.Id.TabLayout);
			this.mViewPager = this.FindViewById<ViewPager> (Resource.Id.ViewPager);
			this.mViewPager.Adapter = new ReportDetailsFragmentPagerAdapter (this.SupportFragmentManager, this.mReport);
			tabLayout.SetupWithViewPager (this.mViewPager);
		}

		protected override void OnResume () {
			base.OnResume ();

			if (this.mReport.CanBeClosed) {
				this.Finish ();
				return;
			}

			this.ConfigureActionBar ();
		}

		public override void OnBackPressed () {
			this.OnCancel ();
		}

		public override bool OnCreateOptionsMenu (IMenu menu) {
			if (this.mReport.IsDraft && !this.mReport.IsNew)
				this.MenuInflater.Inflate (Resource.Menu.Report_draft_menu, menu);
			else if (this.mReport.IsOpen)
				this.MenuInflater.Inflate (Resource.Menu.Report_open_menu, menu);
			else if (this.mReport.IsFromApproval)
				this.MenuInflater.Inflate (Resource.Menu.Approval_report_menu, menu);

			return base.OnCreateOptionsMenu (menu);
		}

		public override bool OnOptionsItemSelected (IMenuItem item) {
			switch (item.ItemId) {
				case Android.Resource.Id.Home:
					// Navigate up to the track associated with this event
					Intent upIntent = new Intent (this, typeof(MainActivity));
					if (this.mReport.IsFromApproval) {
						upIntent.PutExtra (MainActivity.EXTRA_SELECTED_TAB, 2);
						upIntent.PutExtra (MainActivity.EXTRA_SELECTED_CATEGORY, 0);
					} else {
						upIntent.PutExtra (MainActivity.EXTRA_SELECTED_TAB, 1);
						upIntent.PutExtra (MainActivity.EXTRA_SELECTED_CATEGORY, (int)this.mReport.ReportType);
					}

					if (NavUtils.ShouldUpRecreateTask (this, upIntent)) {
						Android.Support.V4.App.TaskStackBuilder.Create (this)
							.AddNextIntent (upIntent)
							.StartActivities ();
					} else {
						// Replicate the compatibility implementation of NavUtils.navigateUpTo()
						// to ensure the parent Activity is always launched
						// even if not present on the back stack.
						upIntent.AddFlags (ActivityFlags.ClearTop);
						this.StartActivity (upIntent);
					}
					this.OnCancel ();
					return true;
				case Resource.Id.Action_submit:
					if (this.mReport.IsFromApproval)
						this.SubmitApprovalReport ();
					else
						this.InvokeActionAsync (this.mReport.SubmitAsync);
					return true;
				case Resource.Id.Action_delete:
					this.ConfirmAction (deleteConfirmDialogRequestCode, Labels.GetLoggedUserLabel (Labels.LabelEnum.DoYouConfirm), Labels.GetLoggedUserLabel (Labels.LabelEnum.Delete) + " " + Labels.GetLoggedUserLabel (Labels.LabelEnum.Report));
					return true;
				case Resource.Id.Action_cancel:
					this.ConfirmAction (cancelConfirmDialogRequestCode, Labels.GetLoggedUserLabel (Labels.LabelEnum.DoYouConfirm), Labels.GetLoggedUserLabel (Labels.LabelEnum.CancelReport));
					return true;
			}

			return base.OnOptionsItemSelected (item);
		}

		public Report GetReport () {
			return this.mReport;
		}

		public void RefreshIntentExtras () {
			this.Intent.PutExtra (EXTRA_REPORT_ID, this.mReport.Id.Value);
			this.Intent.PutExtra (EXTRA_REPORT_TYPE, (int) this.mReport.ReportType);
		}

		private void SubmitApprovalReport () {
			Android.Support.V4.App.DialogFragment dialogFragment = new ApprovalCommentDialogFragment (this.mReport, (object sender, EventArgsObject<String> e) => {
				this.mReport.Approval.Comment = e.Object;

				this.InvokeActionAsync (this.mReport.Approval.SaveAsync);
			});
			dialogFragment.Show (this.SupportFragmentManager, null);
		}

		public override void OnClickHandler<T> (int requestCode, DialogArgsObject<T> args) {
			base.OnClickHandler (requestCode, args);

			switch (requestCode) {
				case cancelConfirmDialogRequestCode:
					if (args.ButtonType == DialogButtonType.Positive)
						this.InvokeActionAsync (this.mReport.CancelAsync);
					break;
				case deleteConfirmDialogRequestCode:
					if (args.ButtonType == DialogButtonType.Positive)
						this.InvokeActionAsync (this.mReport.DeleteAsync);
					break;
			}
		}

		private void ConfigureActionBar () {
			if (!this.mReport.IsNew)
				return;

			this.SupportActionBar.SetDisplayHomeAsUpEnabled (false);

			View customActionBarView = this.LayoutInflater.Inflate (Resource.Layout.Actionbar_custom_view_done_cancel, null);
			customActionBarView.FindViewById (Resource.Id.Actionbar_cancel).Click += (object sender, EventArgs e) => {
				this.OnCancel ();
			};
			customActionBarView.FindViewById (Resource.Id.Actionbar_done).Click += (object sender, EventArgs e) => {
				this.OnSave ();
			};

			this.SupportActionBar.SetDisplayOptions (
				(int)ActionBarDisplayOptions.ShowCustom,
				(int)(ActionBarDisplayOptions.ShowCustom
					| ActionBarDisplayOptions.ShowHome
					| ActionBarDisplayOptions.ShowTitle));
			this.SupportActionBar.SetCustomView (
				customActionBarView,
				new Android.Support.V7.App.ActionBar.LayoutParams (
					Android.Support.V7.App.ActionBar.LayoutParams.MatchParent,
					Android.Support.V7.App.ActionBar.LayoutParams.MatchParent));
		}

		private void OnSave () {
			bool wasNew = this.mReport.IsNew;
			this.InvokeActionAsync (this.mReport.SaveAsync, () => {
				this.Title = this.mReport.VDetailsBarTitle;

				if (wasNew) {
					this.SupportActionBar.SetDisplayShowCustomEnabled (false);
					this.SupportActionBar.SetDisplayHomeAsUpEnabled (true);
					this.SupportActionBar.SetDisplayShowTitleEnabled (true);

					this.mMode = ModeEnum.Reading;

					this.RefreshExpensesFragment ();

					this.InvalidateOptionsMenu ();
				} else
					this.Finish ();
			});
		}

		private void OnCancel () {
			switch (this.mMode) {
				case ModeEnum.Creating:
					this.mReport.RemoveFromCollectionParent<Report> ();
					this.Finish ();
					break;
				case ModeEnum.Reading:
					this.Finish ();
					break;
			}
		}

		private void RefreshExpensesFragment () {
			ReportExpensesListFragment fragment = (ReportExpensesListFragment) this.SupportFragmentManager.FindFragmentByTag (
				"android:switcher:" + this.mViewPager.Id + ":"
				+ ((ReportDetailsFragmentPagerAdapter)this.mViewPager.Adapter).GetItemId (1));

			fragment.Refresh ();
		}

        [Export("AddReicept")]
        public void AddReicept()
        {
            var frag = SupportFragmentManager.Fragments.First(d => d is ExpenseReceiptsFragment) as ExpenseReceiptsFragment;

            frag.AddImage();
        }
	}
}