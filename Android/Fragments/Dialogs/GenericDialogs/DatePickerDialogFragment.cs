using System;

using Android.Support.V4.App;
using Android.App;
using Android.OS;
using Android.Content;
using Mxp.Core.Utils;

namespace Mxp.Droid.Fragments
{
	public class DatePickerDialogFragment : Android.Support.V4.App.DialogFragment {

		private DateTime mDate;
		private DateTime? mMinDate;
		private DateTime? mMaxDate;
		private event EventHandler<DatePickerDialog.DateSetEventArgs> mOnDateChangedHandler;

		public DatePickerDialogFragment (DateTime date, EventHandler<DatePickerDialog.DateSetEventArgs> onDateChangedHandler, DateTime? minDate = null, DateTime? maxDate = null) {
			this.mDate = date;
			this.mMinDate = minDate;
			this.mMaxDate = maxDate;
			this.mOnDateChangedHandler = onDateChangedHandler;
		}

		public override Dialog OnCreateDialog(Bundle savedInstanceState) {
			DatePickerDialog datePickerDialog = new DatePickerDialog (this.Activity, this.mOnDateChangedHandler, this.mDate.Year, this.mDate.Month - 1, this.mDate.Day);
			if (this.mMinDate.HasValue)
				datePickerDialog.DatePicker.MinDate = new Java.Util.Date (this.mMinDate.Value.Year - 1900, this.mMinDate.Value.Month - 1, this.mMinDate.Value.Day).Time;
			if (this.mMaxDate.HasValue)
				datePickerDialog.DatePicker.MaxDate = new Java.Util.Date (this.mMaxDate.Value.Year - 1900, this.mMaxDate.Value.Month - 1, this.mMaxDate.Value.Day).Time;
			return datePickerDialog;
		}

		public override void OnPause () {
			this.Dismiss ();

			base.OnPause ();
		}
	}
}