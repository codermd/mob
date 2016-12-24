using System;
using Android.Support.V4.App;

namespace Mxp.Droid.Fragments.Headless
{
	public static class HeadlessFragmentHelper<T> where T : Fragment {
		public static T Attach (Fragment fragment, string tag = null) {
			tag = tag ?? typeof (T).FullName;

			T headlessFragment = (T)fragment.FragmentManager.FindFragmentByTag (tag);

			if (headlessFragment == null) {
				headlessFragment = (T)Activator.CreateInstance (typeof (T));
				headlessFragment.SetTargetFragment (fragment, 0);
				fragment.FragmentManager.BeginTransaction ().Add (headlessFragment, tag).Commit ();
			}

			return headlessFragment;
		}

		public static T Attach (FragmentActivity activity, string tag = null) {
			tag = tag ?? typeof (T).FullName;

			T headlessFragment = (T)activity.SupportFragmentManager.FindFragmentByTag (tag);

			if (headlessFragment == null) {
				headlessFragment = (T)Activator.CreateInstance (typeof (T));
				activity.SupportFragmentManager.BeginTransaction ().Add (headlessFragment, tag).Commit ();
			}

			return headlessFragment;
		}
	}
}
