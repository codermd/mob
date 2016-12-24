using System;

using Android.Views;
using Android.Widget;
using Android.App;

using Mxp.Core.Business;
using Mxp.Droid.Fragments;
using Mxp.Core.Helpers;
using Mxp.Droid.Adapters;
using Mxp.Droid.Helpers;

namespace Mxp.Droid
{
	public class AmountFieldHolder : AbstractFieldHolder
	{
		public AmountFieldHolder (BaseAdapter<WrappedObject> parentAdapter, Activity activity) : base (parentAdapter, activity) {

		}

		public override void OnListItemClick (ListView listView, View view, int position, long id) {
			Android.Support.V4.App.DialogFragment dialogFragment = new AmountPickerDialogFragment (this.Field.GetModel<ExpenseItem> (), this.ParentAdapter, null);
			dialogFragment.Show (((IChildFragmentManager)this.ParentAdapter).GetChildFragmentManager (), null);
		}
	}
}