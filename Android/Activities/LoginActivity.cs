using System;
using Android.Support.V7.App;
using Android.OS;
using Mxp.Core.Business;
using Mxp.Droid.Helpers;
using Android.Support.V4.View;
using Mxp.Droid.Adapters;
using System.Collections.Generic;
using Android.Content;
using Mxp.Droid.Fragments;
using Android.Support.Design.Widget;
using System.Threading.Tasks;
using Android.Support.V4.App;
using Mxp.Droid.Utils;

namespace Mxp.Droid
{
	public class LoginActivity : BaseActivity
	{
		#pragma warning disable 0414
		private static readonly string TAG = typeof(LoginActivity).Name;
		#pragma warning restore 0414

		public static readonly string EXTRA_MESSAGE = "com.sagacify.mxp.message";

		public static readonly string EXTRA_OPEN_OBJECT_TYPE = "com.sagacify.mxp.openObject.ObjectType";
		public static readonly string EXTRA_OPEN_REFERENCE_TYPE = "com.sagacify.mxp.openObject.ReferenceType";
		public static readonly string EXTRA_OPEN_REFERENCE = "com.sagacify.mxp.openObject.Reference";

		private ICommand mCommand;

		protected override void OnCreate (Bundle savedInstanceState) {
			base.OnCreate (savedInstanceState);

			this.SetContentView (Resource.Layout.Pager_login);

			ViewPager viewPager = this.FindViewById<ViewPager> (Resource.Id.Pager);
			viewPager.Adapter = new LoginFragmentPagerAdapter (this.SupportFragmentManager);

			TabLayout tabs = FindViewById<TabLayout> (Resource.Id.TabLayout);
			tabs.SetupWithViewPager (viewPager);

			viewPager.CurrentItem = LoggedUser.Instance.CanLoginByMail ? 1 : 0;

			string message = this.Intent.GetStringExtra (EXTRA_MESSAGE);
			if (message != null) {
				Android.Support.V4.App.DialogFragment errorDialogFragment = BaseDialogFragment.NewInstance (this, this.GetErrorDialogRequestCode (), BaseDialogFragment.DialogTypeEnum.ErrorDialog, message);
				errorDialogFragment.Show (this.SupportFragmentManager, null);
			}
		}

		protected override void OnResume () {
			base.OnResume ();

			if (this.Intent.HasExtra (LoginActivity.EXTRA_OPEN_OBJECT_TYPE)) {
				int objectType = this.Intent.GetIntExtra (LoginActivity.EXTRA_OPEN_OBJECT_TYPE, 0);
				string referenceType = this.Intent.GetStringExtra (LoginActivity.EXTRA_OPEN_REFERENCE_TYPE);
				string reference = this.Intent.GetStringExtra (LoginActivity.EXTRA_OPEN_REFERENCE);
				this.mCommand = new OpenObjectCommand (this, objectType, referenceType, reference);
			} else if (this.Intent.Type != null && this.Intent.Type.StartsWith ("image/"))
				this.mCommand = new SpendCatcherCommand (this);
		}

		public async void StartNextActivityStrategy () {
			if (this.mCommand != null)
				await this.mCommand.InvokeAsync ().StartAsync (TaskConfigurator.Create (this));
			else {
				Intent intent = new Intent (this, typeof(MainActivity));
				intent.SetFlags (ActivityFlags.NewTask | ActivityFlags.ClearTask);
				this.StartActivity (intent);
				this.Finish ();
			}
		}

		protected override void OnNewIntent (Intent intent) {
			base.OnNewIntent (intent);

			this.Intent = intent;
		}

		public override void OnBackPressed () {
			ActivityCompat.FinishAffinity (this);
		}
	}
}