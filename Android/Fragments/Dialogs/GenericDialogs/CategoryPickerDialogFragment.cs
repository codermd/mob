using System;
using Android.Views;
using Android.OS;
using Android.Widget;
using Android.Util;
using Android.App;
using Mxp.Core.Business;
using Mxp.Droid.Adapters;
using Android.Text;
using Mxp.Core.Helpers;
using Android.Support.Design.Widget;
using Android.Support.V4.View;

namespace Mxp.Droid.Fragments
{
	public class CategoryPickerDialogFragment : Android.Support.V4.App.DialogFragment, CategoriesListFragment.ICategoryItemClicked
	{
		#pragma warning disable 0414
		private static readonly string TAG = typeof(CategoryPickerDialogFragment).Name;
		#pragma warning restore 0414

		private event EventHandler<EventArgsObject<Product>> mOnClickHandler;
		private bool IsSplitted;
		private Products mProducts;

		public CategoryPickerDialogFragment (Products products, EventHandler<EventArgsObject<Product>> onClickHandler, bool isSplitted = true) {
			this.mProducts = products;
			this.mOnClickHandler = onClickHandler;
			this.IsSplitted = isSplitted && Preferences.Instance.FilterTravelCategory;
		}

		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
			View view;

			if (this.IsSplitted) {
				view = inflater.Inflate (Resource.Layout.Generic_pager, container, true);

				TabLayout tabLayout = view.FindViewById<TabLayout> (Resource.Id.TabLayout);
				ViewPager viewPager = view.FindViewById<ViewPager> (Resource.Id.ViewPager);
				viewPager.Adapter = new CategoriesFragmentPagerAdapter (this.ChildFragmentManager, this.mProducts, this);
				tabLayout.SetupWithViewPager (viewPager);
			} else {
				view = inflater.Inflate (Resource.Layout.Generic_fragment, container, true);
				Android.Support.V4.App.Fragment fragment = new CategoriesListFragment (this.mProducts);
				fragment.SetTargetFragment (this, 0);

				this.ChildFragmentManager.BeginTransaction ()
					.Add (Resource.Id.FragmentContainer, fragment)
					.Commit ();
			}

			return view;
		}

		public override void OnPause () {
			this.Dismiss ();

			base.OnPause ();
		}

		public void OnItemClicked (Product product) {
			this.mOnClickHandler (this, new EventArgsObject<Product> (product));
		}
	}
}