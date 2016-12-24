using System;
using Android.Support.V4.App;

namespace Mxp.Droid.Adapters
{
	public class LoginFragmentPagerAdapter : FragmentPagerAdapter
	{
		public LoginFragmentPagerAdapter (FragmentManager fm) : base (fm) {
			
		}

		public override int Count {
			get {
				return 2;
			}
		}

		public override Fragment GetItem (int position) {
			switch (position) {
				case 0:
					return DefaultLoginFragment.NewInstance ();
				case 1:
					return MailLoginFragment.NewInstance ();
				default:
					return null;
			}
		}

		public override Java.Lang.ICharSequence GetPageTitleFormatted (int position) {
			switch (position) {
				case 0:
					return new Java.Lang.String ("User Login");
				case 1:
					return new Java.Lang.String ("Company Login");
				default:
					return null;
			}
		}
	}
}