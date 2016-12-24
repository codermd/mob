using System;
using Android.Support.V4.App;
using System.Threading.Tasks;
using Mxp.Droid.Fragments;
using Mxp.Core.Business;

namespace Mxp.Droid.Utils
{
	public static class FragmentActivityExtentions
	{
		private const int errorDialogRequestCode = -1;

		public static int GetErrorDialogRequestCode (this FragmentActivity activity) {
			return errorDialogRequestCode;
		}

		public static async void InvokeActionAsync (this FragmentActivity activity, Func<Task> action, Action finishAction = null, Android.Support.V4.App.Fragment fragment = null) {
			Android.Support.V4.App.DialogFragment progressDialogFragment = ProgressDialogFragment.NewInstance ();
			progressDialogFragment.Show (activity.SupportFragmentManager, null);

			try {
				await action ();
			} catch (Exception error) {
				progressDialogFragment.Dismiss ();

				Android.Support.V4.App.DialogFragment errorDialogFragment = BaseDialogFragment.NewInstance (
					activity,
					activity.GetErrorDialogRequestCode (),
					BaseDialogFragment.DialogTypeEnum.ErrorDialog,
					error is ValidationError ? ((ValidationError)error).Verbose : Mxp.Core.Services.Service.NoConnectionError
				);

				if (fragment != null)
					errorDialogFragment.SetTargetFragment (fragment, activity.GetErrorDialogRequestCode ());

				errorDialogFragment.Show (activity.SupportFragmentManager, null);

				return;
			}

			if (progressDialogFragment != null)
				progressDialogFragment.DismissAllowingStateLoss ();

			if (finishAction == null)
				activity.Finish ();
			else
				finishAction ();
		}

		public static async void InvokeAsync (this FragmentActivity activity, Func<Task> action, Action finishAction = null) {
			try {
				await action ();
			} catch (Exception) {
				return;
			}

			if (finishAction == null)
				activity.Finish ();
			else
				finishAction ();
		}

		public static void ConfirmAction (this FragmentActivity activity, int requestCode, string message, string title) {
			Android.Support.V4.App.DialogFragment dialogFragment = BaseDialogFragment.NewInstance (activity, requestCode, BaseDialogFragment.DialogTypeEnum.ConfirmDialog, message, title);
			dialogFragment.Show (activity.SupportFragmentManager, null);
		}
	}
}