using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Support.V4.App;
using Mxp.Core.Business;
using Android.OS;
using Android.Util;
using Android.Views;
using Android.Support.V4.Widget;
using Android.Widget;
using Mxp.Core.Services.Responses;
using Android.Content;
using Mxp.Droid.Helpers;

namespace Mxp.Droid.Fragments
{
	public class SpendCatcherExpensesListFragment : ListFragment, BaseDialogFragment.IDialogClickListener
	{
		private SpendCatcherExpensesAdapter mSpendCatcherExpensesAdapter;
		private SwipeRefreshLayout mRefresher;

		private SpendCatcherExpenses mSpendCatcherExpenses {
			get {
				return LoggedUser.Instance.SpendCatcherExpenses;
			}
		}

		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
			View view = inflater.Inflate(Resource.Layout.List_spendcatcher_expenses, container, false);

			this.mRefresher = view.FindViewById<SwipeRefreshLayout> (Resource.Id.spendcater_expenses_refresher);
			this.mRefresher.SetColorSchemeResources(Android.Resource.Color.HoloBlueBright, Android.Resource.Color.HoloGreenLight, Android.Resource.Color.HoloOrangeLight, Android.Resource.Color.HoloRedLight);

			this.mRefresher.Refresh += (object sender, EventArgs e) => this.FetchSpendCatcherExpensesAsync ();

			return view;
		}

		public override void OnCreate (Bundle savedInstanceState) {
			base.OnCreate (savedInstanceState);

			this.mSpendCatcherExpensesAdapter = new SpendCatcherExpensesAdapter (this.Activity, this.mSpendCatcherExpenses);
		}

		public override void OnViewCreated (View view, Bundle savedInstanceState) {
			base.OnViewCreated (view, savedInstanceState);

			TextView textView = (TextView) LayoutInflater.From (this.Context).Inflate (Resource.Layout.List_header_item, null);
			textView.Text = Labels.GetLoggedUserLabel (Labels.LabelEnum.SpendCatcherHeaderMessage);
			this.ListView.AddHeaderView (textView);

			this.ListAdapter = this.mSpendCatcherExpensesAdapter;

			if (!this.mSpendCatcherExpenses.Loaded)
				this.mRefresher.Post (() => this.FetchSpendCatcherExpensesAsync ());
		}

		public override void OnResume () {
			base.OnResume ();

			this.mSpendCatcherExpensesAdapter.NotifyDataSetChanged ();
		}

		public override void OnListItemClick (ListView listView, View view, int position, long id) {
			if (position < this.ListView.HeaderViewsCount)
				return;
			
			position -= this.ListView.HeaderViewsCount;

			Intent intent = new Intent (this.Activity, typeof (SpendCatcherPhotoViewActivity));

			intent.PutExtra (SpendCatcherPhotoViewActivity.EXTRA_EXPENSE_ID, this.mSpendCatcherExpenses [position].Id);

			this.StartActivity (intent);
		}

		public async void FetchSpendCatcherExpensesAsync () {
			this.mRefresher.Refreshing = true;
			await this.mSpendCatcherExpenses.FetchAsync ().StartAsync (TaskConfigurator.Create (this));
			this.mRefresher.Refreshing = false;

			this.mSpendCatcherExpensesAdapter.NotifyDataSetChanged ();
		}

		public void OnClickHandler<T> (int requestCode, DialogArgsObject<T> args) {
			
		}
	}
}