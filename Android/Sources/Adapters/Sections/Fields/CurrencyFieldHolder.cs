using System;
using Android.App;
using Mxp.Core.Business;
using Android.Views;
using Android.Widget;
using Mxp.Droid.Fragments;
using Mxp.Core.Helpers;
using Mxp.Droid.Adapters;
using Mxp.Droid.Helpers;
using Mxp.Droid.Pagers;
using Android.Content;

namespace Mxp.Droid
{
	public class CurrencyFieldHolder : AbstractFieldHolder
	{
		public CurrencyFieldHolder (BaseAdapter<WrappedObject> parentAdapter, Activity activity) : base (parentAdapter, activity) {

		}

		public override void OnListItemClick (ListView listView, View view, int position, long id) {
			if (!this.Field.IsEditable)
				return;

			Android.Support.V4.App.DialogFragment dialogFragment = new CurrencyPickerDialogFragment (this.Field.GetValue<Currency> (), (object resender, EventArgsObject<Currency> re) => {
				((Android.Support.V4.App.DialogFragment) resender).Dismiss ();
				this.Field.Value = re.Object;
				this.ParentAdapter.NotifyDataSetChanged ();
			});

			dialogFragment.Show (((IChildFragmentManager) this.ParentAdapter).GetChildFragmentManager (), null);
		}
	}
}