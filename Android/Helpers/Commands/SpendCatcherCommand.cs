using System;
using System.Threading.Tasks;
using Mxp.Core.Business;
using Android.Content;
using Android.App;
using Android.Widget;
using Android.OS;

namespace Mxp.Droid.Helpers
{
	public class SpendCatcherCommand : SpendCatcherAbstractCommand
	{
		private Activity mActivity;

		private string mType;
		private string mAction;
		private Bundle mExtras;

		public SpendCatcherCommand (Activity activity) {
			this.mActivity = activity;

			this.mType = this.mActivity.Intent.Type;
			this.mAction = this.mActivity.Intent.Action;
			this.mExtras =  this.mActivity.Intent.Extras;
		}

		#region SAMLAbstractCommand

		protected override void RedirectToSpendCatcherSharingView () {
			Intent intent = new Intent (this.mActivity, typeof(SpendCatcherSharingActivity));

			this.ConfigureIntent (intent);

			intent.SetFlags (ActivityFlags.NewTask | ActivityFlags.ClearTask);

			this.mActivity.StartActivity (intent);
			this.mActivity.Finish ();
		}

		public override void RedirectToLoginView (ValidationError error = null) {
			Intent intent = new Intent (this.mActivity, typeof(LoginActivity));

			this.ConfigureIntent (intent);

			if (error != null)
				intent.PutExtra (LoginActivity.EXTRA_MESSAGE, error.Verbose);

			this.mActivity.StartActivity (intent);
		}

		#endregion

		private void ConfigureIntent (Intent intent) {
			intent.SetType (this.mType);
			intent.SetAction (this.mAction);
			intent.PutExtras (this.mExtras);
		}
	}
}