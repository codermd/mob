using System;
using Android.Support.V7.App;
using Mxp.Core.Business;
using Mxp.Droid.Helpers;
using Mxp.Droid.Fragments;
using Android.OS;
using Android.Support.V7.Widget;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using Mxp.Droid.Adapters;
using Mxp.Droid.Utils;

namespace Mxp.Droid
{
	public class MainActivity : BaseActivity
	{
		public static readonly string EXTRA_MESSAGE = "com.sagacify.mxp.message";
		public static readonly string EXTRA_SELECTED_TAB = "com.sagacify.mxp.selectedTab";

		public static readonly string EXTRA_SELECTED_CATEGORY = "com.sagacify.mxp.selectedCategory";

		protected override void OnCreate (Bundle savedInstanceState) {
			base.OnCreate (savedInstanceState);

			this.SetContentView (Resource.Layout.Main);

			Toolbar toolbar = this.FindViewById<Toolbar> (Resource.Id.Toolbar);
			this.SetSupportActionBar (toolbar);

			toolbar.Title = Preferences.Instance.FldCustomerName;

			TabLayout tabLayout = this.FindViewById<TabLayout> (Resource.Id.TabLayout);
			ViewPager viewPager = this.FindViewById<ViewPager> (Resource.Id.ViewPager);
			viewPager.Adapter = new MainPagerAdapter (this.SupportFragmentManager);
			tabLayout.SetupWithViewPager (viewPager);

			viewPager.CurrentItem = this.Intent.GetIntExtra (EXTRA_SELECTED_TAB, 0);

			string message = this.Intent.GetStringExtra (EXTRA_MESSAGE);
			if (message != null) {
				Android.Support.V4.App.DialogFragment errorDialogFragment = BaseDialogFragment.NewInstance (this, this.GetErrorDialogRequestCode (), BaseDialogFragment.DialogTypeEnum.ErrorDialog, message);
				errorDialogFragment.Show (this.SupportFragmentManager, null);
			}
		}
	}
}