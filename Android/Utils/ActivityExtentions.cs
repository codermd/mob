using Android.App;
using Android.Views.InputMethods;

namespace Mxp.Droid
{
	public static class ActivityExtentions {
		public static void ShowSoftKeyboard (this Activity activity) {
			InputMethodManager.FromContext (activity).ToggleSoftInput (ShowFlags.Implicit, HideSoftInputFlags.None);
		}

		public static void HideSoftKeyboard (this Activity activity) {
			if (activity.CurrentFocus != null)
				InputMethodManager.FromContext (activity).HideSoftInputFromWindow (activity.CurrentFocus.WindowToken, HideSoftInputFlags.None);
		}
	}
}