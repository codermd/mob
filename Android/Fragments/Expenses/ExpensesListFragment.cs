using System;
using Android.OS;
using Android.Util;
using Android.Views;
using Android.Support.V4.Widget;
using Mxp.Core.Business;
using Android.Widget;
using Android.Content;
using Mxp.Droid.Adapters;
using Android.Support.V4.App;
using Mxp.Droid.Helpers;

namespace Mxp.Droid.Fragments
{
	public class ExpensesListFragment : Fragment, BaseDialogFragment.IDialogClickListener
	{
		private static readonly string TAG = typeof(ExpensesListFragment).Name;

		public static readonly string EXTRA_EXPENSES_TYPE = "com.sagacify.mxp.expenses.type";

		private SwipeRefreshLayout mRefresher;

		protected ExpenseAdapter mExpenseAdapter;
		protected Report mReport;
		protected ListView mListView;

		private Expenses.ExpensesTypeEnum mExpensesTypeEnum;

		protected virtual Expenses mExpenses {
			get {
				switch (this.mExpensesTypeEnum) {
					case Expenses.ExpensesTypeEnum.Business:
						return LoggedUser.Instance.BusinessExpenses;
					case Expenses.ExpensesTypeEnum.Private:
						return LoggedUser.Instance.PrivateExpenses;
					default:
						return null;
				}
			}
		}

		public static ExpensesListFragment NewInstance (Expenses.ExpensesTypeEnum expensesType) {
			ExpensesListFragment fragment = new ExpensesListFragment ();

			Bundle bundle = new Bundle ();
			bundle.PutInt (EXTRA_EXPENSES_TYPE, (int)expensesType);
			fragment.Arguments = bundle;

			return fragment;
		}

		public override void OnCreate (Bundle savedInstanceState) {
			base.OnCreate (savedInstanceState);

			if (this.Arguments != null)
				this.mExpensesTypeEnum = (Expenses.ExpensesTypeEnum) this.Arguments.GetInt (EXTRA_EXPENSES_TYPE);

			this.mExpenseAdapter = new ExpenseAdapter (Activity, this.mExpenses, this.mReport);
		}

		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
			Log.Debug(TAG, "OnCreateView expenses list fragment");

			View view = inflater.Inflate(Resource.Layout.List_expenses_sticky_headers, container, false);

			this.ConfigureListView (view);

			this.mRefresher = view.FindViewById<SwipeRefreshLayout> (Resource.Id.expenses_refresher);
			this.mRefresher.SetColorSchemeResources(Android.Resource.Color.HoloBlueBright, Android.Resource.Color.HoloGreenLight, Android.Resource.Color.HoloOrangeLight, Android.Resource.Color.HoloRedLight);

			this.mRefresher.Refresh += (object sender, EventArgs e) => this.FetchExpensesAsync ();

			return view;
		}

		public override void OnViewCreated (View view, Bundle savedInstanceState) {
			base.OnViewCreated (view, savedInstanceState);

			if (!this.mExpenses.Loaded)
				this.mRefresher.Post (() => this.FetchExpensesAsync ());
		}

		public override void OnResume () {
			base.OnResume ();

			this.mExpenseAdapter.NotifyDataSetChanged ();
		}

		protected virtual void OnListItemClick (ListView listView, View view, int position, long id) {
			if (this.mExpenses [position].IsSplit) {
				LinearLayout itemsLayout = view.FindViewById<LinearLayout> (Resource.Id.Items);
				itemsLayout.Visibility = itemsLayout.Visibility == ViewStates.Gone ? ViewStates.Visible : ViewStates.Gone;
			} else {
				Intent intent = new Intent (this.Activity, typeof(ExpenseDetailsActivity));
				intent.PutExtra (ExpenseDetailsActivity.EXTRA_EXPENSE_ITEM_ID, this.mExpenses[position].ExpenseItems [0].Id.Value);
				intent.PutExtra (ExpenseDetailsActivity.EXTRA_EXPENSES_TYPE, (int)this.mExpenses.ExpensesType);
				this.StartActivity (intent);
			}
		}

		protected void ConfigureListView (View view) {
			this.mListView = view.FindViewById<ListView> (Android.Resource.Id.List);
			this.mListView.ItemClick += (object sender, AdapterView.ItemClickEventArgs e) => this.OnListItemClick (this.mListView, e.View, e.Position, e.Id);
			this.mListView.Adapter = this.mExpenseAdapter;
		}

		public bool IsRefreshing {
			get {
				return this.mRefresher.Refreshing;
			}
		}

		private async void FetchExpensesAsync () {
			this.mRefresher.Refreshing = true;
			await this.mExpenses.FetchAsync ().StartAsync (TaskConfigurator.Create (this));
			this.mRefresher.Refreshing = false;

			Log.Debug (TAG, "expenses fetched");

			this.mExpenseAdapter.NotifyDataSetChanged ();
		}

		public void OnClickHandler<T> (int requestCode, DialogArgsObject<T> args) {
			
		}
	}
}