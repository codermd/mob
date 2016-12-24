using System;

using Android.Support.V4.App;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using Android.Content;
using Mxp.Core.Helpers;
using Mxp.Core.Business;

namespace Mxp.Droid.Fragments
{
	public class StringPickerDialogFragment : Android.Support.V4.App.DialogFragment {
		private string[] mValues;
		private string mTitle;
		private View mView;
		private NumberPicker mNumberPicker;
		private event EventHandler<EventArgsObject<string>> mOnClickChangedHandler;

		public StringPickerDialogFragment (string[] values, string title, EventHandler<EventArgsObject<string>> onClickChangedHandler) {
			this.mValues = values;
			this.mTitle = title;
			this.mOnClickChangedHandler = onClickChangedHandler;
		}

		public override void OnCreate (Bundle savedInstanceState) {
			base.OnCreate (savedInstanceState);

			this.mView = this.Activity.LayoutInflater.Inflate (Resource.Layout.String_picker_dialog, null);

			this.mNumberPicker = this.mView.FindViewById<NumberPicker> (Resource.Id.NumberPicker);
			this.mNumberPicker.MinValue = 0;
			this.mNumberPicker.MaxValue = this.mValues.Length - 1;
			this.mNumberPicker.SetDisplayedValues (this.mValues);
		}

		public override Dialog OnCreateDialog(Bundle savedInstanceState) {
			AlertDialog.Builder builder = new AlertDialog.Builder(this.Activity);

			builder.SetTitle (this.mTitle)
				.SetView (this.mView)
				.SetNegativeButton (Labels.GetLoggedUserLabel (Labels.LabelEnum.Cancel), (object sender, DialogClickEventArgs e) => {})
				.SetPositiveButton (Labels.GetLoggedUserLabel (Labels.LabelEnum.Select), (object sender, DialogClickEventArgs e) => {
					this.mOnClickChangedHandler (this, new EventArgsObject<string> (this.mValues[this.mNumberPicker.Value]));
				});

			return builder.Create();
		}

		public override void OnPause () {
			this.Dismiss ();

			base.OnPause ();
		}
	}
}