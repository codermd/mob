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
	public class CountryFieldHolder : AbstractFieldHolder
	{
		public CountryFieldHolder (BaseAdapter<WrappedObject> parentAdapter, Activity activity) : base (parentAdapter, activity) {

		}

		public override void OnListItemClick (ListView listView, View view, int position, long id) {
			if (!this.Field.IsEditable)
				return;

			Android.Support.V4.App.DialogFragment dialogFragment = new CountryPickerDialogFragment (this.Field.GetValue<Country> (), ((ICountriesFor)this.Field.Model).Countries, (object sender, EventArgsObject<Country> e) => {
				((Android.Support.V4.App.DialogFragment)sender).Dismiss ();
				// TODO Compare Country -> override == or use Equals method
				this.Field.Value = e.Object;
				this.ParentAdapter.NotifyDataSetChanged ();
			});
			dialogFragment.Show (((IChildFragmentManager)this.ParentAdapter).GetChildFragmentManager (), null);
		}
	}
}