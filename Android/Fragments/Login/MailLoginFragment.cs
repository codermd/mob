using System;
using Android.Support.V4.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using Mxp.Droid.Fragments;
using Mxp.Core.Business;
using System.Threading.Tasks;
using Mxp.Droid.Utils;
using Mxp.Droid.Fragments.Headless;
using Mxp.Droid.Helpers;

namespace Mxp.Droid
{		
	public class MailLoginFragment : Fragment, BaseDialogFragment.IDialogClickListener
	{
		#pragma warning disable 0414
		private static readonly string TAG = typeof(MailLoginFragment).Name;
		#pragma warning restore 0414

		private Button mLoginButton;
		private HeadlessCustomTabsFragment mHeadlessCustomTabsFragment;

		public static MailLoginFragment NewInstance () {
			return new MailLoginFragment ();
		}

		public override void OnCreate (Bundle savedInstanceState) {
			base.OnCreate (savedInstanceState);

			this.mHeadlessCustomTabsFragment = HeadlessFragmentHelper<HeadlessCustomTabsFragment>.Attach (this);
		}

		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
			View view = inflater.Inflate(Resource.Layout.Login_company, container, false);

			EditText mailEditText = view.FindViewById<EditText> (Resource.Id.Email);
			mailEditText.Text = LoggedUser.Instance.VEmail;

			this.mLoginButton = view.FindViewById<Button> (Resource.Id.Login);

			mailEditText.SetImeActionLabel ("Login", Android.Views.InputMethods.ImeAction.Send);
			mailEditText.EditorAction += (object sender, TextView.EditorActionEventArgs e) => {
				if (e.ActionId == Android.Views.InputMethods.ImeAction.Send) {
					this.mLoginButton.PerformClick ();
					this.Activity.HideSoftKeyboard ();
					e.Handled = true;
				} else
					e.Handled = false;
			};
			CheckBox remember = view.FindViewById<CheckBox> (Resource.Id.RememberMe);
			remember.Checked = LoggedUser.Instance.AutoLogin;

			remember.CheckedChange += (object sender, CompoundButton.CheckedChangeEventArgs e) => {
				LoggedUser.Instance.AutoLogin = e.IsChecked;
			};

			this.mLoginButton.Click += (object sender, EventArgs e) => {
				this.mLoginButton.Enabled = false;

				this.LoginAsync (mailEditText.Text);
			};

			return view;
		}

		public void OnClickHandler<T> (int requestCode, DialogArgsObject<T> args) {
			// Do nothing
		}

		private void LoginAsync (string email) {
			LoggedUser.Instance.Email = email;

			LoggedUser.Instance.Username = null;
			LoggedUser.Instance.Password = null;

			TaskConfigurator.Create (this)
							.SetWithProgress (true)
			                .Finally (() => this.mLoginButton.Enabled = true)
			                .Start (Task.Run (async () => {
								string redirection = await LoggedUser.Instance.LoginByMailAsync ();
								this.mHeadlessCustomTabsFragment.OpenCustomTab (redirection);
							}));
		}
	}
}