using System;
using Android.Widget;
using Mxp.Core.Business;
using Android.App;

namespace Mxp.Droid.Adapters
{		
	public class CountryDialogAdapter : BaseSectionAdapter<Country>
	{
		#pragma warning disable 0414
		private static readonly string TAG = typeof(CountryDialogAdapter).Name;
		#pragma warning restore 0414

		private Countries mCountries;

		public CountryDialogAdapter (Activity activity, Countries countries) : base (activity) {
			this.mCountries = countries;
		}

		public override BaseAdapter<Country> InstantiateSection (int position) {
			switch (position) {
				case 0:
					return new CountryRecentlyUsedSectionSource (this, this.mActivity, this.mCountries);
				case 1:
					return new CountryListSectionSource (this, this.mActivity, this.mCountries);
				default:
					return null;
			}
		}

		public override int SectionCount {
			get {
				return 2;
			}
		}

		public override int FilterOnSection {
			get {
				return 1;
			}
		}
	}
}