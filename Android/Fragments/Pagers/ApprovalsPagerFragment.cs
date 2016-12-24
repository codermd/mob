using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.Support.V4.View;
using Android.Support.V4.App;
using Android.OS;
using Android.Views;
using Android.Util;
using Mxp.Droid.Adapters;

namespace Mxp.Droid.Fragments
{
	public class ApprovalsPagerFragment : Fragment
	{
		private static readonly string TAG = typeof(ApprovalsPagerFragment).Name;

		private ApprovalsFragmentPagerAdapter mAdapterViewPager;

		public override void OnCreate (Bundle savedInstanceState) {
			base.OnCreate (savedInstanceState);

			Log.Debug (TAG, "OnCreate ApprovalsPagerFragment");

			this.HasOptionsMenu = true;

			this.mAdapterViewPager = new ApprovalsFragmentPagerAdapter(this.ChildFragmentManager);
		}

		public override void OnPrepareOptionsMenu (IMenu menu) {
			base.OnPrepareOptionsMenu (menu);

			menu.Clear ();
		}

		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
			Log.Debug (TAG, "OnCreateView ApprovalsPagerFragment");

			View view = inflater.Inflate (Resource.Layout.Pager_tab_strip, container, false);

			ViewPager viewPager = view.FindViewById<ViewPager>(Resource.Id.pager);
			viewPager.Adapter = this.mAdapterViewPager;
			viewPager.OffscreenPageLimit = ApprovalsFragmentPagerAdapter.ITEMS - 1;

			return view;
		}
	}
}