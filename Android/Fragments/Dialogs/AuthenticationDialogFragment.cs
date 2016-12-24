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
	public class AuthenticationDialogFragment : Android.Support.V4.App.DialogFragment
	{
		#pragma warning disable 0414
		private static readonly string TAG = typeof(AuthenticationDialogFragment).Name;
		#pragma warning restore 0414

		private const string HOST_EXTRA = "host";

		private View mView;
		private EditText mUsernameEditText;
		private EditText mPasswordEditText;

		public event EventHandler<DialogArgsObject<AuthWrapper>> OnClickHandler;

		public static AuthenticationDialogFragment NewInstance (string host) {
			AuthenticationDialogFragment fragment = new AuthenticationDialogFragment ();

			Bundle bundle = new Bundle ();
			bundle.PutString (HOST_EXTRA, host);
			fragment.Arguments = bundle;

			return fragment;
		}

		public override void OnCreate (Bundle savedInstanceState) {
			base.OnCreate (savedInstanceState);

			this.mView = this.Activity.LayoutInflater.Inflate (Resource.Layout.Dialog_basic_auth, null);
			this.mUsernameEditText = mView.FindViewById<EditText> (Resource.Id.UsernameEditText);
			this.mPasswordEditText = mView.FindViewById<EditText> (Resource.Id.PasswordEditText);
		}

		public override Dialog OnCreateDialog(Bundle savedInstanceState) {
			AlertDialog.Builder builder = new AlertDialog.Builder (this.Activity);

			builder.SetView (this.mView)
				.SetTitle (Resource.String.auth_title)
				.SetMessage (this.GetString (Resource.String.auth_message, this.Arguments.GetString (HOST_EXTRA)))
				.SetNegativeButton (Resource.String.cancel, (object sender, DialogClickEventArgs e) => {
					this.OnClickHandler (this, new DialogArgsObject<AuthWrapper> (this, DialogButtonType.Negative));
				}).SetPositiveButton (Resource.String.auth_login, (object sender, DialogClickEventArgs e) => {
					AuthWrapper authWrapper = new AuthWrapper () {
						Username = this.mUsernameEditText.Text,
						Password = this.mPasswordEditText.Text
					};
					this.OnClickHandler (this, new DialogArgsObject<AuthWrapper> (this, DialogButtonType.Positive, authWrapper));
				});

			return builder.Create ();
		}

		public override void OnPause () {
			this.Dismiss ();

			base.OnPause ();
		}

		public class AuthWrapper {
			public string Username { get; set; }
			public string Password { get; set; }
		}
	}
}