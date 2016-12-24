using System;
using Android.Support.V4.App;
using Android.OS;
using Android.Views;
using Mxp.Core.Business;
using Android.Widget;
using System.Threading.Tasks;
using Mxp.Droid.Fragments;
using Mxp.Droid.Fragments.Headless;
using Mxp.Droid.Helpers;

namespace Mxp.Droid
{
	public class DefaultLoginFragment : Fragment, BaseDialogFragment.IDialogClickListener, HeadlessCustomTabsFragment.ICustomTabsServiceListener
	{
		private Button mLoginButton;
		private HeadlessCustomTabsFragment mHeadlessCustomTabsFragment;

		private string ForgotPasswordUrl {
			get {
				return LoggedUser.Instance.ForgotPasswordUrl;
			}
		}

		public static DefaultLoginFragment NewInstance () {
			return new DefaultLoginFragment ();
		}

		public override void OnCreate (Bundle savedInstanceState) {
			base.OnCreate (savedInstanceState);

			this.mHeadlessCustomTabsFragment = HeadlessFragmentHelper<HeadlessCustomTabsFragment>.Attach (this);
		}

		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
			View view = inflater.Inflate(Resource.Layout.Login, container, false);

			EditText username = view.FindViewById<EditText> (Resource.Id.Username);
			EditText password = view.FindViewById<EditText> (Resource.Id.Password);
			CheckBox remember = view.FindViewById<CheckBox> (Resource.Id.RememberMe);

			username.Text = LoggedUser.Instance.VUsername;
			password.Text = LoggedUser.Instance.Password;
			remember.Checked = LoggedUser.Instance.AutoLogin;

			TextView resetPassword = view.FindViewById<TextView> (Resource.Id.Reset);
			resetPassword.Click += (object sender, EventArgs e) => this.mHeadlessCustomTabsFragment.OpenCustomTab (this.ForgotPasswordUrl);

			remember.CheckedChange += (object sender, CompoundButton.CheckedChangeEventArgs e) => {
				LoggedUser.Instance.AutoLogin = e.IsChecked;
			};

			this.mLoginButton = view.FindViewById<Button> (Resource.Id.Login);
			this.mLoginButton.Click += (object sender, EventArgs e) => {
				this.mLoginButton.Enabled = false;

				this.LoginAsync (username.Text, password.Text);
			};

			return view;
		}

		private void LoginAsync (string username, string password) {
			LoggedUser.Instance.Username = username;
			LoggedUser.Instance.Password = password;

			LoggedUser.Instance.Email = null;

			TaskConfigurator.Create (this)
			                .SetWithProgress (true)
			                .Catch (error => this.mLoginButton.Enabled = true)
			                .Finally (() => ((LoginActivity)this.Activity).StartNextActivityStrategy ())
			                .Start (Task.Run (async () => {
								await LoggedUser.Instance.LoginAsync ();
								await LoggedUser.Instance.RefreshCacheAsync ();
							}));
		}

		#region HeadlessCustomTabsFragment.ICustomTabsServiceListener

		public void OnHeadlessCustomTabsFragmentAttached () {
			this.mHeadlessCustomTabsFragment?.MayLaunchUrl (this.ForgotPasswordUrl);
		}

		#endregion

		#region BaseDialogFragment.IDialogClickListener

		public void OnClickHandler<T> (int requestCode, DialogArgsObject<T> args) {
			// Do nothing
		}

		#endregion
	}
}