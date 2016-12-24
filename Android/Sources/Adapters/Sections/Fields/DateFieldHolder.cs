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
	public class DateFieldHolder : AbstractFieldHolder
	{
		public DateFieldHolder (BaseAdapter<WrappedObject> parentAdapter, Activity activity) : base (parentAdapter, activity) {

		}

		public override void OnListItemClick (ListView listView, View view, int position, long id) {
			if (!this.Field.IsEditable)
				return;

			if (Build.VERSION.SdkInt < BuildVersionCodes.Lollipop) {
				CalendarDatePickerDialog calendarDatePickerDialog = BetterPickers.CalendarDatePickers.CalendarDatePickerDialog.NewInstance (null, this.Field.GetValue<DateTime> ().Year, this.Field.GetValue<DateTime> ().Month - 1, this.Field.GetValue<DateTime> ().Day);
				calendarDatePickerDialog.DateSet += (sender, e) => {
					DateTime date = new DateTime (
						e.P1,
						e.P2 + 1,
						e.P3,
						this.Field.GetValue<DateTime> ().Hour,
						this.Field.GetValue<DateTime> ().Minute,
						this.Field.GetValue<DateTime> ().Second
					);

					StringBuilder builder = new StringBuilder ();

					if (this.Field.extraInfo.ContainsKey ("Min-Range") && date < (DateTime)this.Field.extraInfo ["Min-Range"])
						builder.AppendLine ("The date can't be selected before " + (DateTime)this.Field.extraInfo ["Min-Range"]);

					if (this.Field.extraInfo.ContainsKey ("Max-Range") && date > (DateTime)this.Field.extraInfo ["Max-Range"])
						builder.AppendLine ("The date can't be selected after " + (DateTime)this.Field.extraInfo ["Max-Range"]);

					if (builder.Length > 0) {
						Android.Support.V4.App.DialogFragment errorDialogFragment = BaseDialogFragment.NewInstance (this.mActivity, BaseDialogFragment.DialogTypeEnum.ErrorDialog, builder.ToString ());
						errorDialogFragment.Show (((IChildFragmentManager)this.ParentAdapter).GetChildFragmentManager (), null);
						return;
					}

					if (date != this.Field.GetValue<DateTime> ()) {
						this.Field.Value = date;
						this.ParentAdapter.NotifyDataSetChanged ();
					}

					if (this.Field.extraInfo.ContainsKey ("Type") && (string) this.Field.extraInfo ["Type"] == "DATE-TIME")
						this.ShowTimePickerDialog ();
				};
				calendarDatePickerDialog.Show (((IChildFragmentManager)this.ParentAdapter).GetChildFragmentManager (), null);
			} else {
				Android.Support.V4.App.DialogFragment dialogFragment = new DatePickerDialogFragment (
					this.Field.GetValue<DateTime> (),
					(object sender, DatePickerDialog.DateSetEventArgs e) => {
						DateTime date = new DateTime (
							e.Date.Year,
							e.Date.Month,
							e.Date.Day,
							this.Field.GetValue<DateTime> ().Hour,
							this.Field.GetValue<DateTime> ().Minute,
							this.Field.GetValue<DateTime> ().Second
						);	

						if (date != this.Field.GetValue<DateTime> ()) {
							this.Field.Value = date;
							this.ParentAdapter.NotifyDataSetChanged ();
						}

						if (this.Field.extraInfo.ContainsKey ("Type") && (string) this.Field.extraInfo ["Type"] == "DATE-TIME")
							this.ShowTimePickerDialog ();
					},
					this.Field.extraInfo.ContainsKey ("Min-Range") ? (DateTime?)this.Field.extraInfo ["Min-Range"] : null,
					this.Field.extraInfo.ContainsKey ("Max-Range") ? (DateTime?)this.Field.extraInfo ["Max-Range"] : null
				);
				dialogFragment.Show (((IChildFragmentManager)this.ParentAdapter).GetChildFragmentManager (), null);
			}
		}

		private void ShowTimePickerDialog () {
			if (Build.VERSION.SdkInt < BuildVersionCodes.Lollipop) {
				RadialTimePickerDialog radialTimePickerDialog = RadialTimePickerDialog.NewInstance (null, this.Field.GetValue<DateTime> ().Hour, this.Field.GetValue<DateTime> ().Minute, true);
				radialTimePickerDialog.TimeSet += (object sender, RadialTimePickerDialog.TimeSetEventArgs e) => {
					DateTime date = new DateTime (
						this.Field.GetValue<DateTime> ().Year,
						this.Field.GetValue<DateTime> ().Month,
						this.Field.GetValue<DateTime> ().Day,
						e.P1,
						e.P2,
						0
					);

					if (!this.TryValidatingTime (date))
						return;

					if (date != this.Field.GetValue<DateTime> ()) {
						this.Field.Value = date;
						this.ParentAdapter.NotifyDataSetChanged ();
					}
				};
				radialTimePickerDialog.Show (((IChildFragmentManager)this.ParentAdapter).GetChildFragmentManager (), null);
			} else {
				Android.Support.V4.App.DialogFragment dialogFragment = new TimePickerDialogFragment (
					this.Field.GetValue<DateTime> ().TimeOfDay,
					(object sender, TimePickerDialog.TimeSetEventArgs e) => {
						DateTime date = new DateTime (
							this.Field.GetValue<DateTime> ().Year,
							this.Field.GetValue<DateTime> ().Month,
							this.Field.GetValue<DateTime> ().Day,
							e.HourOfDay,
							e.Minute,
							0
						);

						if (!this.TryValidatingTime (date))
							return;

						if (date != this.Field.GetValue<DateTime> ()) {
							this.Field.Value = date;
							this.ParentAdapter.NotifyDataSetChanged ();
						}
					}
				);
				dialogFragment.Show (((IChildFragmentManager)this.ParentAdapter).GetChildFragmentManager (), null);
			}
		}

		private bool TryValidatingTime (DateTime time) {
			StringBuilder builder = new StringBuilder ();

			if (this.Field.extraInfo.ContainsKey ("Min-Range") && time < (DateTime)this.Field.extraInfo ["Min-Range"])
				builder.AppendLine ("The time can't be selected before " + (DateTime)this.Field.extraInfo ["Min-Range"]);

			if (this.Field.extraInfo.ContainsKey ("Max-Range") && time > (DateTime)this.Field.extraInfo ["Max-Range"])
				builder.AppendLine ("The time can't be selected after " + (DateTime)this.Field.extraInfo ["Max-Range"]);

			if (builder.Length > 0) {
				Android.Support.V4.App.DialogFragment errorDialogFragment = BaseDialogFragment.NewInstance (this.mActivity, BaseDialogFragment.DialogTypeEnum.ErrorDialog, builder.ToString ());
				errorDialogFragment.Show (((IChildFragmentManager)this.ParentAdapter).GetChildFragmentManager (), null);
				return false;
			}

			return true;
		}
	}
}