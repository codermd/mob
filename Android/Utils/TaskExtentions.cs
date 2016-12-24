using System;
using System.Threading.Tasks;
using Mxp.Core.Business;
using Android.Support.V4.App;
using Mxp.Droid.Fragments;
using Mxp.Droid.Utils;
using Mxp.Droid.Helpers;

namespace Mxp.Droid {
	public static class TaskExtentions {
		public static void ShowErrorDialog (string error, TaskConfigurator configurator) {
			DialogFragment errorDialogFragment = BaseDialogFragment.NewInstance (
				configurator.TargetActivity,
				configurator.TargetActivity.GetErrorDialogRequestCode (),
				BaseDialogFragment.DialogTypeEnum.ErrorDialog,
				error
			);

			if (configurator.TargetFragment != null)
				errorDialogFragment.SetTargetFragment (configurator.TargetFragment, configurator.TargetActivity.GetErrorDialogRequestCode ());

			errorDialogFragment.Show (configurator.TargetActivity.SupportFragmentManager, null);
		}

		public static async Task StartAsync (this Task task, TaskConfigurator configurator) {
			configurator.Validate ();

			DialogFragment progressDialogFragment = null;

			if (configurator.WithProgress) {
				progressDialogFragment = ProgressDialogFragment.NewInstance ();
				progressDialogFragment.Show (configurator.TargetActivity.SupportFragmentManager, null);
			}

			try {
				await task;
			} catch (ValidationError e) {
				progressDialogFragment?.Dismiss ();

				if (configurator.CanShowErrorDialog)
					ShowErrorDialog (e.Verbose, configurator);

				configurator.CatchCallback?.Invoke (e.Verbose);

				return;
			} catch (Exception) {
				progressDialogFragment?.Dismiss ();

				if (configurator.CanShowErrorDialog)
					ShowErrorDialog (Mxp.Core.Services.Service.NoConnectionError, configurator);

				configurator.CatchCallback?.Invoke (Mxp.Core.Services.Service.NoConnectionError);

				return;
			}

			configurator.FinallyCallback?.Invoke ();

			progressDialogFragment?.Dismiss ();

			if (configurator.CanFinishActivity)
				configurator.TargetActivity?.Finish ();
		}

		public static async Task<T> StartAsync<T> (this Task<T> task, TaskConfigurator configurator) {
			configurator.Validate ();

			DialogFragment progressDialogFragment = null;

			if (configurator.WithProgress) {
				progressDialogFragment = ProgressDialogFragment.NewInstance ();
				progressDialogFragment.Show (configurator.TargetActivity.SupportFragmentManager, null);
			}

			object value = default (T);

			try {
				value = await task;
			} catch (ValidationError e) {
				progressDialogFragment?.Dismiss ();

				if (configurator.CanShowErrorDialog)
					ShowErrorDialog (e.Verbose, configurator);

				configurator.CatchCallback?.Invoke (e.Verbose);

				return default (T);
			} catch (Exception) {
				progressDialogFragment?.Dismiss ();

				if (configurator.CanShowErrorDialog)
					ShowErrorDialog (Mxp.Core.Services.Service.NoConnectionError, configurator);
				
				configurator.CatchCallback?.Invoke (Mxp.Core.Services.Service.NoConnectionError);

				return default (T);
			}

			configurator.TypedFinallyCallback?.Invoke (value);

			progressDialogFragment?.Dismiss ();

			if (configurator.CanFinishActivity)
				configurator.TargetActivity?.Finish ();

			return (T)value;
		}
	}
}
