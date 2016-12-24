using System;

using Android.Views;
using Android.Widget;
using Android.App;

using Mxp.Core.Business;
using Mxp.Droid.Fragments;
using Mxp.Core.Helpers;
using Mxp.Droid.Adapters;
using Mxp.Droid.Helpers;
using Mxp.Droid.Pagers;

namespace Mxp.Droid
{
	public class DecimalFieldHolder : AbstractFieldHolder
	{
		public DecimalFieldHolder (BaseAdapter<WrappedObject> parentAdapter, Activity activity) : base (parentAdapter, activity) {

		}

		public override void OnListItemClick (ListView listView, View view, int position, long id) {
			if (!this.Field.IsEditable)
				return;

			new BetterPickers.NumberPickers.NumberPickerBuilder ()
				.SetFragmentManager (((IChildFragmentManager)this.ParentAdapter).GetChildFragmentManager ())
				.SetStyleResId (Resource.Style.BetterPickersDialogFragment_Light)
				.AddNumberPickerDialogHandler ((int reference, int number, double decimalNumber, bool isNegative, double fullNumber) => {
					if (fullNumber != this.Field.GetValue<double> ()) {
						this.Field.Value = fullNumber;
						this.ParentAdapter.NotifyDataSetChanged ();
					}
				})
				.SetPlusMinusVisibility ((int)ViewStates.Gone)
				.Show ();
		}
	}
}