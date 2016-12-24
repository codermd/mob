using System;

using Android.Support.V4.App;
using Android.App;
using Android.OS;
using Android.Content;
using Mxp.Core.Utils;

namespace Mxp.Droid.Fragments
{
	public class TimePickerDialogFragment : Android.Support.V4.App.DialogFragment {

		private TimeSpan mTime;
		private event EventHandler<TimePickerDialog.TimeSetEventArgs> mOnTimeChangedHandler;

		public TimePickerDialogFragment (TimeSpan time, EventHandler<TimePickerDialog.TimeSetEventArgs> onTimeChangedHandler) {
			this.mTime = time;
			this.mOnTimeChangedHandler = onTimeChangedHandler;
		}

		public override Dialog OnCreateDialog(Bundle savedInstanceState) {
			return new TimePickerDialog (this.Activity, this.mOnTimeChangedHandler, this.mTime.Hours, this.mTime.Minutes, true);
		}

		public override void OnPause () {
			this.Dismiss ();

			base.OnPause ();
		}
	}
}