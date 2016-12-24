using Mxp.Core.Business;
using Mxp.Droid.Adapters;
using Android.OS;
using Android.Views;
using Android.Widget;
using Mxp.Droid.Utils;
using System.ComponentModel;

namespace Mxp.Droid.Fragments
{
	public class AllowanceDetailsListFragment : Android.Support.V4.App.ListFragment, BaseDialogFragment.IDialogClickListener
	{
		#pragma warning disable 0414
		private static readonly string TAG = typeof(AllowanceDetailsListFragment).Name;
		#pragma warning restore 0414

		private AllowanceDetailsAdapter mAllowanceDetailsAdapter;

		private Allowance Allowance {
			get {
				return ((ExpenseDetailsActivity) this.Activity).ExpenseItem.ParentExpense as Allowance;
			}
		}

		public static AllowanceDetailsListFragment NewInstance () {
			AllowanceDetailsListFragment allowanceDetailsListFragment = new AllowanceDetailsListFragment ();

			return allowanceDetailsListFragment;
		}

		public override void OnCreate (Bundle savedInstanceState) {
			base.OnCreate (savedInstanceState);

			this.Allowance.PropertyChanged += HandlePropertyChanged;

			this.Activity.InvokeActionAsync (this.Allowance.FetchAsync, () => {
				this.mAllowanceDetailsAdapter = new AllowanceDetailsAdapter (this.ChildFragmentManager, this.Activity, this.Allowance);
				this.ListAdapter = this.mAllowanceDetailsAdapter;
			}, this);
		}

		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
			return inflater.Inflate (Resource.Layout.List_sections, container, false);
		}

		public override void OnListItemClick (ListView listView, View view, int position, long id) {
			this.mAllowanceDetailsAdapter.OnListItemClick (listView, view, position, id);
		}

		private void HandlePropertyChanged (object sender, PropertyChangedEventArgs e) {
			if (e.PropertyName == "IsChanged")
				this.mAllowanceDetailsAdapter?.NotifyDataSetChanged ();
		}

		public void OnClickHandler<T> (int requestCode, DialogArgsObject<T> args) {

		}
	}
}