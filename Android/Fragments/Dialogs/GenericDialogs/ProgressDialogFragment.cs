using System;
using Android.Views;
using Android.OS;
using Android.Widget;
using Android.Util;
using Android.App;
using Mxp.Core.Business;
using Mxp.Droid.Adapters;
using Android.Text;
using Android.Content;
using Mxp.Core.Helpers;

namespace Mxp.Droid.Fragments
{
	public class ProgressDialogFragment : Android.Support.V4.App.DialogFragment
	{
		#pragma warning disable 0414
		private static readonly string TAG = typeof(ProgressDialogFragment).Name;
		#pragma warning restore 0414

		public static readonly string EXTRA_PROGRESS_DIALOG_TITLE = "com.sagacify.mxp.android.dialog.title";
		public static readonly string EXTRA_PROGRESS_DIALOG_MESSAGE = "com.sagacify.mxp.android.dialog.message";

		private ProgressDialogFragment () : base () {

		}

		public static ProgressDialogFragment NewInstance (string title = null, string message = null) {
			ProgressDialogFragment fragment = new ProgressDialogFragment ();

			Bundle bundle = new Bundle ();
			bundle.PutString (EXTRA_PROGRESS_DIALOG_TITLE, title);
			bundle.PutString (EXTRA_PROGRESS_DIALOG_MESSAGE, message);
			fragment.Arguments = bundle;

			return fragment;
		}

		public override void OnCreate (Bundle savedInstanceState) {
			base.OnCreate (savedInstanceState);

			this.Cancelable = false;
		}

		public override Dialog OnCreateDialog (Bundle savedInstanceState) {
			ProgressDialog dialog = new ProgressDialog (this.Activity, this.Theme);

			dialog.SetTitle (String.IsNullOrWhiteSpace (this.Arguments.GetString (EXTRA_PROGRESS_DIALOG_TITLE)) ? this.GetString (Resource.String.title_progress_dialog) : this.Arguments.GetString (EXTRA_PROGRESS_DIALOG_TITLE));
			dialog.SetMessage (String.IsNullOrWhiteSpace (this.Arguments.GetString (EXTRA_PROGRESS_DIALOG_MESSAGE)) ? this.GetString (Resource.String.message_progress_dialog) : this.Arguments.GetString (EXTRA_PROGRESS_DIALOG_MESSAGE));
			dialog.Indeterminate = true;
			dialog.SetProgressStyle (ProgressDialogStyle.Spinner);

			return dialog;
		}
	}
}