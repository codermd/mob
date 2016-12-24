using System;
using Android.Support.V4.App;
using Mxp.Droid.Fragments;
using Mxp.Core.Business;

namespace Mxp.Droid.Adapters
{
	public class TravelDetailsFragmentPagerAdapter : FragmentPagerAdapter
	{
		public static readonly int ITEMS_COUNT = 4;

		private int mTravelId;

		public TravelDetailsFragmentPagerAdapter (FragmentManager fm, int travelId) : base (fm) {
			this.mTravelId = travelId;
		}

		public override int Count {
			get {
				return ITEMS_COUNT;
			}
		}

		public override Fragment GetItem (int position) {
			return TravelDetailsListFragment.NewInstance (this.mTravelId, position);
		}

		public override Java.Lang.ICharSequence GetPageTitleFormatted (int position) {
			switch (position) {
				case 0:
					return new Java.Lang.String (Labels.GetLoggedUserLabel (Labels.LabelEnum.Details));
				case 1:
					return new Java.Lang.String (Labels.GetLoggedUserLabel (Labels.LabelEnum.Flight));
				case 2:
					return new Java.Lang.String (Labels.GetLoggedUserLabel (Labels.LabelEnum.Stay));
				case 3:
					return new Java.Lang.String (Labels.GetLoggedUserLabel (Labels.LabelEnum.CarRental));
				default:
					return null;
			}
		}
	}
}