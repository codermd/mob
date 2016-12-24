using Android.OS;
using Mxp.Core.Business;
using Mxp.Droid.Adapters;
using Android.Widget;
using Android.Views;
using System.ComponentModel;

namespace Mxp.Droid.Fragments
{
	public class ExpenseDetailsListFragment : Android.Support.V4.App.ListFragment, INotifyFragmentDataSetRefreshed
	{
		#pragma warning disable 0414
		private static readonly string TAG = typeof(ExpenseDetailsListFragment).Name;
		#pragma warning restore 0414

		private ExpenseDetailsAdapter mExpenseDetailsAdapter;

		private ExpenseItem ExpenseItem {
			get {
				return ((ExpenseDetailsActivity) this.Activity).ExpenseItem;
			}
		}

		public static ExpenseDetailsListFragment NewInstance () {
			ExpenseDetailsListFragment expenseDetailsListFragment = new ExpenseDetailsListFragment ();

			return expenseDetailsListFragment;
		}

		public override void OnCreate (Bundle savedInstanceState) {
			base.OnCreate (savedInstanceState);

			this.NotifyDataSetRefreshed ();
		}

		public override void OnResume () {
			base.OnResume ();

			this.ExpenseItem.PropertyChanged += ExpenseItemPropertyChangedHandler;
		}

		public override void OnStop () {
			this.ExpenseItem.PropertyChanged -= ExpenseItemPropertyChangedHandler;

			base.OnStop ();
		}

		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
			return inflater.Inflate (Resource.Layout.List_sections, container, false);
		}

		public override void OnListItemClick (ListView listView, View view, int position, long id) {
			this.mExpenseDetailsAdapter.OnListItemClick (listView, view, position, id);
		}

		#region INotifyFragmentDataSetRefreshed

		public void NotifyDataSetRefreshed () {
			this.mExpenseDetailsAdapter = new ExpenseDetailsAdapter (this.ChildFragmentManager, this.Activity, this.ExpenseItem);
			this.ListAdapter = this.mExpenseDetailsAdapter;
		}

		#endregion

		private void ExpenseItemPropertyChangedHandler (object sender, PropertyChangedEventArgs e) {
			this.mExpenseDetailsAdapter.NotifyDataSetChanged ();
		}
	}
}