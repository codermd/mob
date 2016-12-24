using System;
using Android.OS;
using Android.Support.V4.View;
using Mxp.Core.Business;
using Mxp.Droid.Fragments;
using Android.Content;
using Android.Support.V4.App;
using Mxp.Droid.Adapters;
using System.Linq;
using Android.Views;
using Mxp.Droid.Utils;
using Android.App;
using System.ComponentModel;
using Android.Util;
using Mxp.Core.Utils;
using Mxp.Droid.Pagers;
using Android.Support.Design.Widget;
using Java.Interop;
using Mxp.Core;

namespace Mxp.Droid
{		
	public class ExpenseDetailsActivity : BaseActivity, IPagerAdapter
	{
		private static readonly string TAG = typeof (ExpenseDetailsActivity).Name;

		public static readonly string EXTRA_EXPENSE_ITEM_ID = "com.sagacify.mxp.expense.id";
		public static readonly string EXTRA_REPORT_TYPE = "com.sagacify.mxp.report.type";
		public static readonly string EXTRA_REPORT_ID = "com.sagacify.mxp.report.id";
		public static readonly string EXTRA_EXPENSES_TYPE = "com.sagacify.mxp.expenses.type";

		public const int SPLIT_EXPENSE_REQUEST_CODE = 0;

		private const int mDeleteConfirmDialogRequestCode = 0;
		private const int mUnsplitConfirmDialogRequestCode = 1;

		private Android.Support.V4.App.DialogFragment mProgressDialog;

		private enum ModeEnum {
			Creating,
			Reading,
			Editing
		}

		public bool IsLoaded = true;

		private ModeEnum mMode { get; set; } = ModeEnum.Reading;

		public ExpenseItem ExpenseItem { get; set; }
		private Expense mExpense {
			get {
				return this.ExpenseItem.ParentExpense;
			}
		}

		private ViewPager mViewPager;
		private ExpenseDetailsFragmentPagerAdapter mPagerAdapter;

		#region IPagerAdapter

		public PagerAdapter PagerAdapter {
			get {
				return this.mPagerAdapter;
			}
		}

		#endregion

		protected override void OnCreate (Bundle savedInstanceState) {
			int expenseItemId = this.Intent.GetIntExtra (EXTRA_EXPENSE_ITEM_ID, -1);
			int reportId = this.Intent.GetIntExtra (EXTRA_REPORT_ID, -1);
			int reportType = this.Intent.GetIntExtra (EXTRA_REPORT_TYPE, -1);
            Expenses.ExpensesTypeEnum expenseType = (Expenses.ExpensesTypeEnum)this.Intent.GetIntExtra (EXTRA_EXPENSES_TYPE, (int)Expenses.ExpensesTypeEnum.Business);
			Expenses expenses = expenseType == Expenses.ExpensesTypeEnum.Business
		        ? LoggedUser.Instance.BusinessExpenses
		        : LoggedUser.Instance.PrivateExpenses;

			if (expenseItemId != -1) {
				if (reportId != -1 && reportType != -1)
					this.ExpenseItem = LoggedUser.Instance.GetReport ((Reports.ReportTypeEnum)reportType, reportId).Expenses.SelectSingle (expense => expense.ExpenseItems.SingleOrDefault (expenseItem => expenseItem.Id == expenseItemId));
				else {
				    this.ExpenseItem = expenses.SelectSingle (expense => expense.ExpenseItems.SingleOrDefault (expenseItem => expenseItem.Id == expenseItemId));
                }
			} else {
				if (reportId != -1 && reportType != -1)
					this.ExpenseItem = LoggedUser.Instance.GetReport ((Reports.ReportTypeEnum)reportType, reportId).Expenses.Single (expense => expense.IsNew).ExpenseItems.First ();
				else 
					this.ExpenseItem = expenses.Single (expense => expense.IsNew).ExpenseItems.First ();
			}

			base.OnCreate (savedInstanceState);

			this.SetContentView (Resource.Layout.Pager_generic);

			Android.Support.V7.Widget.Toolbar toolbar = this.FindViewById<Android.Support.V7.Widget.Toolbar> (Resource.Id.Toolbar);
			this.SetSupportActionBar (toolbar);

			this.SupportActionBar.SetDisplayHomeAsUpEnabled (true);

			this.Title = this.mExpense.VDetailsBarTitle;

			TabLayout tabLayout = this.FindViewById<TabLayout> (Resource.Id.TabLayout);
			this.mViewPager = this.FindViewById<ViewPager> (Resource.Id.ViewPager);
			this.mPagerAdapter = new ExpenseDetailsFragmentPagerAdapter (tabLayout, this.ExpenseItem, this.SupportFragmentManager);
			this.mViewPager.Adapter = this.mPagerAdapter;
			tabLayout.SetupWithViewPager (this.mViewPager);
			this.mPagerAdapter.SynchronizeActionBar ();
		}

		protected override void OnResume () {
			base.OnResume ();

			this.mExpense.PropertyChanged += HandlePropertyChangedEventHandler;
			LoggedUser.Instance.AlertMessageChanged += HandleAlertMessageChangedEvent;

			if (this.IsLoaded)
				this.ConfigureActionBar ();
		}

		protected override void OnPause () {
			base.OnPause ();

			this.mExpense.PropertyChanged -= HandlePropertyChangedEventHandler;
			LoggedUser.Instance.AlertMessageChanged -= HandleAlertMessageChangedEvent;
		}

		public override bool OnCreateOptionsMenu (IMenu menu) {
			this.MenuInflater.Inflate (Resource.Menu.Expense_menu, menu);

			menu.FindItem (Resource.Id.Action_split).SetVisible (this.ExpenseItem.CanShowSplit);
			menu.FindItem (Resource.Id.Action_unsplit).SetVisible (this.ExpenseItem.CanShowUnsplit);
			menu.FindItem (Resource.Id.Action_delete).SetVisible (this.ExpenseItem.CanShowDelete);

			menu.FindItem (Resource.Id.Action_copy).SetVisible (this.ExpenseItem.CanCopy);
			menu.FindItem (Resource.Id.Action_copy).SetTitle (Labels.GetLoggedUserLabel (Labels.LabelEnum.SaveAndCopy));

			menu.FindItem (Resource.Id.Action_change_type).SetVisible (this.ExpenseItem.CanChangeAccountType);
			menu.FindItem (Resource.Id.Action_change_type).SetTitle (this.ExpenseItem.VChangeAccountType);

            return base.OnCreateOptionsMenu (menu);
		}

		public override bool OnOptionsItemSelected (IMenuItem item) {
			switch (item.ItemId) {
				case Android.Resource.Id.Home:
					// Navigate up to the track associated with this event
					Intent upIntent = null;

					if (this.mExpense.IsFromReport) {
						upIntent = new Intent (this, typeof(ReportDetailsActivity));
						upIntent.PutExtra (ReportDetailsActivity.EXTRA_REPORT_ID, this.mExpense.Report.Id.Value);
						upIntent.PutExtra (ReportDetailsActivity.EXTRA_REPORT_TYPE, (int)this.mExpense.Report.ReportType);
					} else if (LoggedUser.Instance.DraftReports.Count > 0 && LoggedUser.Instance.DraftReports [0].IsNew) {
						upIntent = new Intent (this, typeof(ReportDetailsActivity));
					} else
						upIntent = new Intent (this, typeof(MainActivity));

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
				case Resource.Id.Action_delete:
					this.ConfirmAction (mDeleteConfirmDialogRequestCode, Labels.GetLoggedUserLabel (Labels.LabelEnum.DoYouConfirm), Labels.GetLoggedUserLabel (Labels.LabelEnum.Delete) + " " + Labels.GetLoggedUserLabel (Labels.LabelEnum.Expense));
					return true;
				case Resource.Id.Action_split:
					this.SplitExpense ();
					return true;
				case Resource.Id.Action_unsplit:
					this.ConfirmAction (mUnsplitConfirmDialogRequestCode, Labels.GetLoggedUserLabel (Labels.LabelEnum.DoYouConfirm), Labels.GetLoggedUserLabel (Labels.LabelEnum.Unsplit) + " " + Labels.GetLoggedUserLabel (Labels.LabelEnum.Expense));
					return true;
				case Resource.Id.Action_copy:
					this.SaveAndCopyExpense ();
					return true;
                case Resource.Id.Action_change_type:
					this.InvokeActionAsync (this.ExpenseItem.ChangeAccountTypeAsync);
                    return true;
            }

			return base.OnOptionsItemSelected (item);
		}

		public override void OnBackPressed () {
			this.OnCancel ();
		}

		private void HandlePropertyChangedEventHandler (object sender, PropertyChangedEventArgs e) {
			Log.Info (TAG, "Event " + e.PropertyName + " fired by " + sender.GetType ().Name + " !");

			if (!this.IsLoaded)
				return;

			this.RunOnUiThread (this.ConfigureActionBar);
		}

		private void HandleAlertMessageChangedEvent (object sender, AlertMessage.AlertMessageEventArgs e) {
			this.mProgressDialog?.Dismiss ();

			switch (e.AlertMessage.MessageType) {
				case AlertMessage.MessageTypeEnum.StartLoading:
					this.mProgressDialog = ProgressDialogFragment.NewInstance (e.AlertMessage.Title, e.AlertMessage.Message);
						this.mProgressDialog.Show (this.SupportFragmentManager, null);
					break;
				case AlertMessage.MessageTypeEnum.Error:
					Android.Support.V4.App.DialogFragment errorDialog = BaseDialogFragment.NewInstance (this, this.GetErrorDialogRequestCode (), BaseDialogFragment.DialogTypeEnum.ErrorDialog, e.AlertMessage.Message, e.AlertMessage.Title);
					errorDialog.Show (this.SupportFragmentManager, null);
					break;
			}
		}

		public override void OnClickHandler<T> (int requestCode, DialogArgsObject<T> args) {
			base.OnClickHandler (requestCode, args);

			switch (requestCode) {
				case mDeleteConfirmDialogRequestCode:
					if (args.ButtonType == DialogButtonType.Positive)
						this.InvokeActionAsync (this.ExpenseItem.DeleteExpenseAsync);
					break;
				case mUnsplitConfirmDialogRequestCode:
					if (args.ButtonType == DialogButtonType.Positive)
						this.InvokeActionAsync (this.ExpenseItem.UnsplitAsync);
					break;
			}
		}

		private void SplitExpense () {
			Intent intent = new Intent (this, typeof(SplitExpensesActivity));

			intent.PutExtra (SplitExpensesActivity.EXTRA_EXPENSE_ITEM_ID, this.ExpenseItem.Id.Value);

			intent.PutExtra (SplitExpensesActivity.EXTRA_EXPENSES_TYPE, (int)this.ExpenseItem.ParentExpensesCollection.ExpensesType);

			if (this.mExpense.IsFromReport) {
				intent.PutExtra (SplitExpensesActivity.EXTRA_EXPENSE_REPORT_ID, this.mExpense.Report.Id.Value);
				intent.PutExtra (SplitExpensesActivity.EXTRA_EXPENSE_REPORT_TYPE, (int) this.mExpense.Report.ReportType);
			}

			this.StartActivityForResult (intent, SPLIT_EXPENSE_REQUEST_CODE);
		}

		protected override void OnActivityResult (int requestCode, Result resultCode, Intent data) {
			base.OnActivityResult (requestCode, resultCode, data);

			if (requestCode == SPLIT_EXPENSE_REQUEST_CODE && resultCode == Result.Ok)
				this.Finish ();
		}

		private void ClickOnSave () {
			if (this.mExpense.IsNew) {
				if (this.mExpense is Mileage)
					this.InvokeActionAsync (() => ((Mileage)this.mExpense).CreateAsync ());
				else if (this.mExpense is Allowance)
					this.InvokeActionAsync (((Allowance)this.mExpense).SaveAsync);
				else
					this.InvokeAsync (() => this.mExpense.CreateAsync ());
			} else {
				if (this.mExpense is Mileage)
					this.InvokeActionAsync (((Mileage)this.mExpense).SaveAsync);
				else if (this.mExpense is Allowance)
					this.InvokeActionAsync (((Allowance)this.mExpense).SaveAsync);
				else
					this.InvokeAsync (() => this.mExpense.SaveAsync (this.ExpenseItem));
			}
		}

		private void ConfigureActionBar () {
			if (!this.mExpense.IsChanged && this.mMode != ModeEnum.Reading) {
				this.SupportActionBar.SetDisplayShowCustomEnabled (false);
				this.SupportActionBar.SetDisplayHomeAsUpEnabled (true);
				this.SupportActionBar.SetDisplayShowTitleEnabled (true);

				this.InvalidateOptionsMenu ();

				this.mMode = ModeEnum.Reading;

				int position = this.mViewPager.CurrentItem;

				((IPagerAdapter)this).PagerAdapter.NotifyDataSetChanged ();

				this.mViewPager.SetCurrentItem (position, false);
			}

			if ((!this.mExpense.IsNew && !this.mExpense.IsChanged)
				|| this.mMode == ModeEnum.Creating
				|| this.mMode == ModeEnum.Editing)
				return;

			this.mMode = this.mExpense.IsNew ? ModeEnum.Creating : ModeEnum.Editing;

			this.SupportActionBar.SetDisplayHomeAsUpEnabled (false);

			View customActionBarView = this.LayoutInflater.Inflate (Resource.Layout.Actionbar_custom_view_done_cancel, null);
			customActionBarView.FindViewById (Resource.Id.Actionbar_cancel).Click += (object sender, EventArgs e) => {
				this.OnCancel ();
			};
			customActionBarView.FindViewById (Resource.Id.Actionbar_done).Click += (object sender, EventArgs e) => {
				this.ClickOnSave ();
			};

			this.SupportActionBar.SetDisplayOptions (
				(int)ActionBarDisplayOptions.ShowCustom,
				(int)(ActionBarDisplayOptions.ShowCustom
				| ActionBarDisplayOptions.ShowHome
				| ActionBarDisplayOptions.ShowTitle));
			this.SupportActionBar.SetCustomView (
				customActionBarView,
				new Android.Support.V7.App.ActionBar.LayoutParams (ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent));
		}

		private void OnCancel () {
			switch (this.mMode) {
				case ModeEnum.Creating:
					this.mExpense.RemoveFromCollectionParent<Expense> ();
					this.Finish ();
					break;
				case ModeEnum.Reading:
					this.Finish ();
					break;
				case ModeEnum.Editing:
					this.InvokeActionAsync (LoggedUser.Instance.BusinessExpenses.FetchAsync);
					break;
			}
		}

		private void SaveAndCopyExpense () {
			Action callback = new Action (() => {
				Expense expense = (Expense) this.mExpense.Clone ();

				this.mExpense.GetCollectionParent<Expenses, Expense> ().AddItem (expense);

				Intent intent = new Intent (this, typeof (ExpenseDetailsActivity));
				intent.PutExtra (ExpenseDetailsActivity.EXTRA_EXPENSES_TYPE, (int)expense.GetCollectionParent<Expenses, Expense> ().ExpensesType);
				this.StartActivity (intent);

				this.Finish ();
			});

			if (this.mExpense.IsNew)
				this.InvokeAsync (() => this.mExpense.CreateAsync (), callback);
			else if (this.mExpense.IsChanged)
				this.InvokeAsync (() => this.mExpense.SaveAsync (this.ExpenseItem), callback);
			else
				callback ();
		}
		
        [Export("AddReicept")]
	    public void AddReicept()
	    {
	        var frag = SupportFragmentManager.Fragments.First(d => d is ExpenseReceiptsFragment) as ExpenseReceiptsFragment;

            frag.AddImage();
	    }
	}
}