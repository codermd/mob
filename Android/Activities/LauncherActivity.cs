using System;
using Android.OS;
using Mxp.Core.Business;
using Mxp.Droid.Helpers;
using Android.Content;
using Xamarin.Forms;
using Mxp.Droid.Fragments.Headless;

namespace Mxp.Droid
{
	public class LauncherActivity : BaseActivity, StatesMediator.IStateListener
	{
		private StatesMediator mStatesMediator;
		private bool mIsFromCustomTabs;
		private HeadlessCustomTabsFragment mHeadlessCustomTabsFragment;

		protected override void OnCreate (Bundle savedInstanceState) {
			base.OnCreate (savedInstanceState);

			Forms.Init (this, savedInstanceState);

			this.SetContentView (Resource.Layout.Launcher);

			this.mHeadlessCustomTabsFragment = HeadlessFragmentHelper<HeadlessCustomTabsFragment>.Attach (this);

			this.mStatesMediator = new StatesMediator (new CommandsFactory (this), this);
		}

		protected override void OnResume () {
			base.OnResume ();

			if (this.mIsFromCustomTabs) {
				Intent intent = new Intent (this, typeof(LoginActivity));
				this.StartActivity (intent);
				this.Finish ();
				return;
			}

			Android.Net.Uri uri = null;

			if (!this.Intent.Flags.HasFlag (ActivityFlags.LaunchedFromHistory)) {
				if (this.Intent.Type != null && this.Intent.Type.StartsWith ("image/"))
					uri = Android.Net.Uri.Parse (String.Concat (SchemeActionStrategy.SchemeUri, SpendCatcherAbstractCommand.HostUri));
				else
					uri = this.Intent.Data;
			}

			this.mStatesMediator.ChangeState (uri?.ToString ());
		}

		protected override void OnNewIntent (Intent intent) {
			base.OnNewIntent (intent);

			this.mIsFromCustomTabs = false;

			this.Intent = intent;
		}

		#region StatesMediator.IStateListener

		public void RedirectToLoginByEmail (string redirection) {
			this.mIsFromCustomTabs = true;
			this.mHeadlessCustomTabsFragment.OpenCustomTab (redirection);
		}

		public void RedirectToMainView () {
			Intent intent = new Intent (this, typeof(MainActivity));
			intent.SetFlags (ActivityFlags.NewTask | ActivityFlags.ClearTask);
			this.StartActivity (intent);
			this.Finish ();
		}

		public void RedirectToLoginView (ValidationError error = null) {
			Intent intent = new Intent (this, typeof(LoginActivity));

			if (error != null)
				intent.PutExtra (MainActivity.EXTRA_MESSAGE, error.Verbose);

			this.StartActivity (intent);
		}

		#endregion
	}
}