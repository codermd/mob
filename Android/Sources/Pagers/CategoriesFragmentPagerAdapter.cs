using Android.Support.V4.App;
using Java.Lang;
using Mxp.Core.Business;

namespace Mxp.Droid.Adapters
{
	public class CategoriesFragmentPagerAdapter : FragmentPagerAdapter
	{
		private Fragment mParentFragment;
		private Products mProducts;

		public CategoriesFragmentPagerAdapter (FragmentManager fm, Products products, Fragment fragment) : base (fm) {
			this.mProducts = products;
			this.mParentFragment = fragment;
		}

		public override int Count {
			get {
				return 2;
			}
		}

		public override Fragment GetItem (int position) {
			Fragment fragment = null;

			switch (position) {
				case 0:
					fragment = new CategoriesListFragment (this.mProducts.TravelProduct);
					break;
				case 1:
					fragment = new CategoriesListFragment (this.mProducts.NonTravelProduct);
					break;
			}

			fragment.SetTargetFragment (this.mParentFragment, 0);

			return fragment;
		}

		public override ICharSequence GetPageTitleFormatted (int position) {
			switch (position) {
				case 0:
					return new String (Labels.GetLoggedUserLabel (Labels.LabelEnum.TravelFilter));
				case 1:
					return new String (Labels.GetLoggedUserLabel (Labels.LabelEnum.NonTravelFilter));
				default:
					return null;
			}
		}
	}
}