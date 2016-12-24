using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using Android.Support.V4.App;
using Android.OS;

namespace Mxp.Droid
{			
	public abstract class SingleFragmentActivity : FragmentActivity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			this.SetContentView (Resource.Layout.Generic_fragment);

			Fragment fragment = SupportFragmentManager.FindFragmentById (Resource.Id.FragmentContainer);

			if (fragment == null) {
				fragment = createFragment ();
				SupportFragmentManager.BeginTransaction ()
					.Add (Resource.Id.FragmentContainer, fragment)
					.Commit ();
			}
		}

		protected abstract Fragment createFragment();
	}
}