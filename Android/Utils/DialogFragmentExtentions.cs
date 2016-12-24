using System;
using Android.Support.V4.App;

namespace Mxp.Droid
{
	public static class DialogFragmentExtentions {
		public static void ShowAllowingStateLoss (this DialogFragment dialog, FragmentManager fragmentManager, string tag) {
			fragmentManager.BeginTransaction ()
			      .Add (dialog, tag)
			      .CommitAllowingStateLoss ();
		}
	}
}