using System;
using System.Threading.Tasks;
using Mxp.Core.Business;
using System.Collections.Generic;
using Mxp.Core.Utils;
using Android.App;
using Android.Content;
using Android.Widget;
using Android.Support.V4.App;

namespace Mxp.Droid.Helpers
{
	public class SAMLCommand : SAMLAbstractCommand
	{
		private Activity mActivity;

		public SAMLCommand (Activity activity, Uri uri) : base (uri) {
			this.mActivity = activity;
		}

		#region SAMLAbstractCommand

		public override void Parse (Uri uri) {
			if (uri.Segments.Length != 2 || String.IsNullOrWhiteSpace (uri.Segments [1]))
				throw new ValidationError ("Error", "Wrong scheme");
			
			this.Token = uri.Segments [1];
		}

		protected override async Task RedirectAsync () {
			if (this.NextCommand != null)
				await this.NextCommand.InvokeAsync ().StartAsync (TaskConfigurator.Create ((FragmentActivity)this.mActivity));
			else {
				Intent intent = new Intent (this.mActivity, typeof(MainActivity));
				this.mActivity.StartActivity (intent);
				this.mActivity.Finish ();
			}
		}

		public override void RedirectToLoginView (ValidationError error = null) {
			Intent intent = new Intent (this.mActivity, typeof(LoginActivity));

			if (error != null)
				intent.PutExtra (MainActivity.EXTRA_MESSAGE, error.Verbose);

			this.mActivity.StartActivity (intent);
			this.mActivity.Finish ();
		}

		#endregion
	}
}