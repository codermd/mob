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
using Android.Content;
using Mxp.Core.Business;
using Android.Widget;

namespace Mxp.Droid.Fragments
{
	public class ReportsPagerFragment : Fragment
	{
		private static readonly string TAG = typeof(ReportsPagerFragment).Name;

		private ReportsFragmentPagerAdapter adapterViewPager;
		private ViewPager mViewPager;

		public override void OnCreate (Bundle savedInstanceState) {
			base.OnCreate (savedInstanceState);

			Log.Debug (TAG, "OnCreate ReportsPagerFragment");

			this.HasOptionsMenu = true;

			this.adapterViewPager = new ReportsFragmentPagerAdapter(this.ChildFragmentManager);
		}

		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
			Log.Debug (TAG, "OnCreateView ReportsPagerFragment");

			View view = inflater.Inflate (Resource.Layout.Pager_tab_strip, container, false);

			this.mViewPager = view.FindViewById<ViewPager>(Resource.Id.pager);
			this.mViewPager.Adapter = this.adapterViewPager;
			this.mViewPager.CurrentItem = this.Activity.Intent.GetIntExtra (MainActivity.EXTRA_SELECTED_CATEGORY, 0);

			return view;
		}

		public override void OnCreateOptionsMenu (IMenu menu, MenuInflater inflater) {
			menu.Clear ();

			inflater.Inflate (Resource.Menu.Reports_menu, menu);

			base.OnCreateOptionsMenu (menu, inflater);
		}

		public override bool OnOptionsItemSelected (IMenuItem item) {
			switch (item.ItemId) {
				case Resource.Id.Action_new:
					this.CreateReport ();
					return true;
			}

			return base.OnOptionsItemSelected (item);
		}

		private void CreateReport () {
			ReportsListFragment fragment = (ReportsListFragment) this.ChildFragmentManager.FindFragmentByTag (
				"android:switcher:" + this.mViewPager.Id + ":"
				+ ((ReportsFragmentPagerAdapter)this.mViewPager.Adapter).GetItemId (0));
			if (fragment.IsRefreshing) {
				Toast.MakeText (this.Activity, "Please wait for the draft reports to be fetched", ToastLength.Short).Show ();
				return;
			}

			LoggedUser.Instance.DraftReports.InsertItem (0, Report.NewInstance ());

			Intent intent = new Intent (this.Activity, typeof (ReportDetailsActivity));

			this.StartActivity (intent);
		}
	}
}