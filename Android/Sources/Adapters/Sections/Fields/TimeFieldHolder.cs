using System;

using Android.Views;
using Android.Widget;
using Android.App;

using Mxp.Core.Business;
using Mxp.Droid.Fragments;
using Mxp.Core.Helpers;
using Mxp.Droid.Adapters;
using Mxp.Droid.Helpers;
using Android.OS;
using BetterPickers.CalendarDatePickers;
using Android.Support.V7.App;
using Mxp.Droid.Utils;
using System.Text;
using BetterPickers.RadialTimePickers;

namespace Mxp.Droid
{
	public class TimeFieldHolder : AbstractFieldHolder
	{
		public TimeFieldHolder (BaseAdapter<WrappedObject> parentAdapter, Activity activity) : base (parentAdapter, activity) {

		}

		public override void OnListItemClick (ListView listView, View view, int position, long id) {
			if (!this.Field.IsEditable)
				return;

			if (Build.VERSION.SdkInt < BuildVersionCodes.Lollipop) {
				RadialTimePickerDialog radialTimePickerDialog = RadialTimePickerDialog.NewInstance (null, this.Field.GetValue<TimeSpan> ().Hours, this.Field.GetValue<TimeSpan> ().Minutes, true);
				radialTimePickerDialog.TimeSet += (object sender, RadialTimePickerDialog.TimeSetEventArgs e) => {
					TimeSpan time = new TimeSpan (e.P1, e.P2, 0);
		
					if (time != this.Field.GetValue<TimeSpan> ()) {
						this.Field.Value = time;
						this.ParentAdapter.NotifyDataSetChanged ();
					}
				};
				radialTimePickerDialog.Show (((IChildFragmentManager)this.ParentAdapter).GetChildFragmentManager (), null);
			} else {
				Android.Support.V4.App.DialogFragment dialogFragment = new TimePickerDialogFragment (
					this.Field.GetValue<TimeSpan> (),
					(object sender, TimePickerDialog.TimeSetEventArgs e) => {
						TimeSpan time = new TimeSpan (e.HourOfDay, e.Minute, 0);

						if (time != this.Field.GetValue<TimeSpan> ()) {
							this.Field.Value = time;
							this.ParentAdapter.NotifyDataSetChanged ();
						}
					}
				);
				dialogFragment.Show (((IChildFragmentManager)this.ParentAdapter).GetChildFragmentManager (), null);
			}
		}
	}
}