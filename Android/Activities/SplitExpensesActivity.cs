using System;
using Mxp.Droid.Fragments;
using Android.OS;
using Mxp.Core.Business;
using Mxp.Core.Utils;
using System.Linq;
using Mxp.Droid.Adapters;
using Mxp.Droid.Utils;
using Mxp.Core.Helpers;
using Android.Widget;
using Android.Views;
using Android.App;
using Mxp.Droid.Helpers;

namespace Mxp.Droid
{		
	public class SplitExpensesActivity : BaseActivity
	{
		#pragma warning disable 0414
		private static readonly string TAG = typeof(SplitExpensesActivity).Name;
		#pragma warning restore 0414

		public static readonly string EXTRA_EXPENSE_ITEM_ID = "com.sagacify.mxp.expense.item.id";
		public static readonly string EXTRA_EXPENSES_TYPE = "com.sagacify.mxp.expenses.type";
		public static readonly string EXTRA_EXPENSE_REPORT_TYPE = "com.sagacify.mxp.expense.report.id";
		public static readonly string EXTRA_EXPENSE_REPORT_ID = "com.sagacify.mxp.expense.report.type";

		private ExpenseItem mExpenseItem;
		private Products mProducts;
		private SplitExpenseItemAdapter mSplitExpenseItemAdapter;

		private FrameLayout mDoneLayout;

		protected override void OnCreate (Bundle bundle) {
			base.OnCreate (bundle);

			this.SetContentView (Resource.Layout.List_expenses_splitted);

			Android.Support.V7.Widget.Toolbar toolbar = this.FindViewById<Android.Support.V7.Widget.Toolbar> (Resource.Id.Toolbar);
			this.SetSupportActionBar (toolbar);

			View customActionBarView = this.LayoutInflater.Inflate (Resource.Layout.Actionbar_custom_view_done_cancel, null);
			customActionBarView.FindViewById (Resource.Id.Actionbar_cancel).Click += (object sender, EventArgs e) => {
				this.ClickOnCancel ();
			};
			this.mDoneLayout = customActionBarView.FindViewById<FrameLayout> (Resource.Id.Actionbar_done);
			this.mDoneLayout.Enabled = false;
			this.mDoneLayout.Click += (object sender, EventArgs e) => {
				this.ClickOnValidate ();
			};

			this.SupportActionBar.SetDisplayOptions (
				(int) ActionBarDisplayOptions.ShowCustom,
				(int) (ActionBarDisplayOptions.ShowCustom
					| ActionBarDisplayOptions.ShowHome
					| ActionBarDisplayOptions.ShowTitle));
			this.SupportActionBar.SetCustomView (
				customActionBarView,
				new Android.Support.V7.App.ActionBar.LayoutParams (
					ViewGroup.LayoutParams.MatchParent,
					ViewGroup.LayoutParams.MatchParent));

			SplitToolbar splitToolbar = this.FindViewById<SplitToolbar> (Resource.Id.SplitToolbar);
			splitToolbar.InflateMenu (Resource.Menu.Toolbar_expenses_splitted_menu);
			splitToolbar.MenuItemClick += (object sender, Android.Support.V7.Widget.Toolbar.MenuItemClickEventArgs e) => {
				switch (e.Item.ItemId) {
					case Resource.Id.Action_new:
						this.AddSplittedExpenseItem ();
						break;
				}
			};

			int expenseItemId = this.Intent.GetIntExtra (EXTRA_EXPENSE_ITEM_ID, -1);
			int reportId = this.Intent.GetIntExtra (EXTRA_EXPENSE_REPORT_ID, -1);
			int reportType = this.Intent.GetIntExtra (EXTRA_EXPENSE_REPORT_TYPE, -1);
			Expenses.ExpensesTypeEnum expenseType = (Expenses.ExpensesTypeEnum)this.Intent.GetIntExtra (EXTRA_EXPENSES_TYPE, (int)Expenses.ExpensesTypeEnum.Business);
			Expenses expenses = expenseType == Expenses.ExpensesTypeEnum.Business
				? LoggedUser.Instance.BusinessExpenses
				: LoggedUser.Instance.PrivateExpenses;

			if (expenseItemId != -1) {
				if (reportId != -1 && reportType != -1)
					this.mExpenseItem = LoggedUser.Instance.GetReport ((Reports.ReportTypeEnum) reportType, reportId).Expenses.SelectSingle (expense => expense.ExpenseItems.SingleOrDefault (expenseItem => expenseItem.Id == expenseItemId));
				else
					this.mExpenseItem = expenses.SelectSingle (expense => expense.ExpenseItems.SingleOrDefault (expenseItem => expenseItem.Id == expenseItemId));
			}

			this.Title = Labels.GetLoggedUserLabel (Labels.LabelEnum.Split);

			ListView listView = this.FindViewById<ListView> (Resource.Id.List);
			listView.ItemClick += (object sender, AdapterView.ItemClickEventArgs e) => {
				this.AddSplittedExpenseItem (this.mSplitExpenseItemAdapter [e.Position]);
			};
			this.mSplitExpenseItemAdapter = new SplitExpenseItemAdapter (this, this.mExpenseItem.InnerSplittedItems, (object sender, EventArgsObject<ExpenseItem> e) => {
				e.Object.RemoveFromCollectionParent<ExpenseItem> ();
				this.NotififyDataSetChanged ();
			});
			listView.Adapter = this.mSplitExpenseItemAdapter;

			this.SplitExpense ();
		}

		public override void OnBackPressed () {
			this.ClickOnCancel ();
		}

		public void SplitExpense () {
			if (this.mProducts == null)
				this.InvokeActionAsync (async () => this.mProducts = await this.mExpenseItem.FetchAvailableProductsAsync (), () => this.AddSplittedExpenseItem ());
		}

		public void AddSplittedExpenseItem (ExpenseItem innerExpenseItem = null) {
			if (innerExpenseItem == null) {
				innerExpenseItem = new ExpenseItem {
					Quantity = 1
				};
				// Dependency needed
				innerExpenseItem.SetCollectionParent (this.mExpenseItem.InnerSplittedItems);
			}

			Android.Support.V4.App.DialogFragment categoryPickerDialogFragment = new CategoryPickerDialogFragment (this.mProducts, (object sender, EventArgsObject<Product> e) => {
				innerExpenseItem.Product = e.Object;

				Android.Support.V4.App.DialogFragment amountPickerDialogFragment = new AmountPickerDialogFragment (innerExpenseItem, null, (object resender, EventArgsObject<ExpenseItem> re) => {
					((Android.Support.V4.App.DialogFragment)sender).Dismiss ();
					if (!this.mExpenseItem.InnerSplittedItems.Contains (innerExpenseItem))
						this.mExpenseItem.InnerSplittedItems.AddItem (innerExpenseItem);
					this.NotififyDataSetChanged ();
				});
				amountPickerDialogFragment.Show (this.SupportFragmentManager, null);
			}, false);
			categoryPickerDialogFragment.Show (this.SupportFragmentManager, null);
		}

		private void ClickOnValidate () {
			this.InvokeActionAsync (this.mExpenseItem.SplitAsync, () => {
				this.SetResult (Result.Ok);
				this.Finish ();
			});
		}

		private void ClickOnCancel () {
			this.mExpenseItem.ClearSplittedItems ();
			this.SetResult (Result.Canceled);
			this.Finish ();
		}

		public void NotififyDataSetChanged () {
			this.mDoneLayout.Enabled = this.mExpenseItem.IsReadyToSplit;
			this.mSplitExpenseItemAdapter.NotifyDataSetChanged ();
		}
	}
}