using Android.OS;
using Android.Views;
using Mxp.Core.Business;
using Android.Widget;
using Android.Content;
using Mxp.Core.Helpers;
using Mxp.Droid.Utils;

namespace Mxp.Droid.Fragments
{
	public class ReportExpensesListFragment : ExpensesListFragment
	{
		#pragma warning disable 0414
		private static readonly string TAG = typeof(ReportExpensesListFragment).Name;
		#pragma warning disable 0414

		private Android.Support.V7.Widget.Toolbar mToolbar;

		protected override Expenses mExpenses {
			get {
				return this.mReport.IsNew ? LoggedUser.Instance.BusinessExpenses : this.mReport.Expenses;
			}
		}

		public static ReportExpensesListFragment NewInstance () {
			ReportExpensesListFragment reportExpensesListFragment = new ReportExpensesListFragment ();

			return reportExpensesListFragment;
		}

		public override void OnCreate (Bundle savedInstanceState) {
			this.mReport = ((IReportListener)this.Activity).GetReport ();

			base.OnCreate (savedInstanceState);

			this.HasOptionsMenu = false;
		}

		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
			View view = inflater.Inflate(Resource.Layout.List_report_expenses, container, false);

			this.ConfigureListView (view);

			this.mToolbar = view.FindViewById<Android.Support.V7.Widget.Toolbar> (Resource.Id.Toolbar);
			this.mToolbar.InflateMenu (Resource.Menu.Expenses_menu);
			this.mToolbar.MenuItemClick += (object sender, Android.Support.V7.Widget.Toolbar.MenuItemClickEventArgs e) => {
				switch (e.Item.ItemId) {
					case Resource.Id.Action_new:
						this.ChooseExpense ();
						break;
				}
			};

			return view;
		}

		public override void OnViewCreated (View view, Bundle savedInstanceState) {
			base.OnViewCreated (view, savedInstanceState);

			this.ConfigureToolbar ();
		}

		protected override void OnListItemClick (ListView listView, View view, int position, long id) {
			if (this.mExpenses [position].IsSplit) {
				LinearLayout ItemsLayout = view.FindViewById<LinearLayout> (Resource.Id.Items);
				ItemsLayout.Visibility = ItemsLayout.Visibility == ViewStates.Gone ? ViewStates.Visible : ViewStates.Gone;
			} else {
				Intent intent = new Intent (this.Activity, typeof(ExpenseDetailsActivity));

				intent.PutExtra (ExpenseDetailsActivity.EXTRA_EXPENSE_ITEM_ID, this.mExpenses [position].ExpenseItems [0].Id.Value);
				if (!this.mReport.IsNew) {
					intent.PutExtra (ExpenseDetailsActivity.EXTRA_REPORT_TYPE, (int)this.mReport.ReportType);
					intent.PutExtra (ExpenseDetailsActivity.EXTRA_REPORT_ID, this.mReport.Id.Value);
				}
				this.StartActivity (intent);
			}
		}

		private void ChooseExpense () {
			int count = this.mReport.Expenses.Count;
			Android.Support.V4.App.DialogFragment selectableExpensesDialogFragment = new SelectableExpensesDialogFragment (this.mReport, (object sender, EventArgsObject<Report> e) => {
				if (count == this.mReport.Expenses.Count)
					return;

				// FIXME If error, expenses in report are not refresh
				this.Activity.InvokeActionAsync (this.mReport.SaveAsync, () => {
					this.mExpenseAdapter.NotifyDataSetChanged ();
				});
			});
			selectableExpensesDialogFragment.Show (this.ChildFragmentManager, null);
		}

		private void ConfigureToolbar () {
			this.mToolbar.Visibility = (!this.mReport.IsNew && this.mReport.IsDraft) ? ViewStates.Visible : ViewStates.Gone;
		}

		public void Refresh () {
			((IReportListener)this.Activity).RefreshIntentExtras ();

			this.ConfigureToolbar ();

			this.mExpenseAdapter.Expenses = this.mExpenses;

			// Trick to clear the views, "Invalidate" and "InvalidateViews" don't work
			this.mListView.Adapter = this.mExpenseAdapter;
		}
	}
}