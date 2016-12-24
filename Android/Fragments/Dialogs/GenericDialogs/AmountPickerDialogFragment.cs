using System;

using Android.Support.V4.App;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Content;
using Android.Widget;
using Android.Util;
using Mxp.Core.Business;
using Mxp.Core.Helpers;
using Mxp.Droid.Adapters;
using Mxp.Droid.Helpers;

namespace Mxp.Droid.Fragments
{
	public class AmountPickerDialogFragment : Android.Support.V4.App.DialogFragment
	{
		#pragma warning disable 0414
		private static readonly string TAG = typeof(AmountPickerDialogFragment).Name;
		#pragma warning restore 0414

		private ExpenseItem mExpenseItem;
		private ListView mListView;
		private BaseAdapter<WrappedObject> mParentAdapter;
		private event EventHandler<EventArgsObject<ExpenseItem>> mOnClickHandler;

		public AmountPickerDialogFragment (ExpenseItem expenseItem, BaseAdapter<WrappedObject> parentAdapter, EventHandler<EventArgsObject<ExpenseItem>> onClickHandler) {
			this.mExpenseItem = expenseItem;
			this.mParentAdapter = parentAdapter;
			this.mOnClickHandler = onClickHandler;
		}

		public override void OnCreate (Bundle savedInstanceState) {
			base.OnCreate (savedInstanceState);

			this.mListView = (ListView) this.Activity.LayoutInflater.Inflate (Resource.Layout.Amount_details_item, null);
			FieldsAdapter fieldsAdapter = new FieldsAdapter (this.ChildFragmentManager, this.Activity, this.mExpenseItem.AmountFields);
			this.mListView.Adapter = fieldsAdapter;
			this.mListView.ItemClick += (object sender, AdapterView.ItemClickEventArgs e) => {
				fieldsAdapter.OnListItemClick (this.mListView, e.View, e.Position, e.Id);
			};
		}

		public override Dialog OnCreateDialog (Bundle savedInstanceState) {
			AlertDialog.Builder builder = new AlertDialog.Builder(this.Activity);

			builder.SetTitle (Labels.GetLoggedUserLabel (Labels.LabelEnum.Amount))
				.SetView (this.mListView)
				.SetPositiveButton (Labels.GetLoggedUserLabel (Labels.LabelEnum.Done), (object sender, DialogClickEventArgs e) => {
					if (this.mOnClickHandler != null)
						this.mOnClickHandler (this, new EventArgsObject<ExpenseItem> (this.mExpenseItem));
				});

			return builder.Create();
		}

		public override void OnPause () {
			this.Dismiss ();

			base.OnPause ();
		}

		public override void OnDismiss (IDialogInterface dialog) {
			this.mParentAdapter?.NotifyDataSetChanged ();

			base.OnDismiss (dialog);
		}
	}
}