using System;
using Android.Support.V7.App;
using Android.OS;
using Android.App;
using Mxp.Droid.Fragments;
using Android.Content;
using Mxp.Droid.Fragments.Headless;
using Android.Content.PM;

namespace Mxp.Droid
{	
	public abstract class BaseActivity : AppCompatActivity, BaseDialogFragment.IDialogClickListener
	{
		protected MxpApplication mMxpApplication;

		public const int mExceptionDialogRequestCode = -2;
		public event EventHandler onClickHandler;

		protected override void OnCreate (Bundle savedInstanceState) {
			base.OnCreate (savedInstanceState);

			this.mMxpApplication = (MxpApplication)this.ApplicationContext;
		}

		protected override void OnResume () {
			base.OnResume ();

			this.mMxpApplication.mCurrentActivity = this;
		}

		protected override void OnPause () {
			this.ClearReferences();

			base.OnPause();
		}

		protected override void OnDestroy () {        
			this.ClearReferences ();

			base.OnDestroy();
		}

		private void ClearReferences () {
			Activity currActivity = this.mMxpApplication.mCurrentActivity;

			if (currActivity != null && currActivity.Equals (this))
				this.mMxpApplication.mCurrentActivity = null;
		}

		#region IDialogClickListener

		public virtual void OnClickHandler<T> (int requestCode, DialogArgsObject<T> args) {
			if (this.onClickHandler != null && requestCode == mExceptionDialogRequestCode)
				this.onClickHandler (this, EventArgs.Empty);
		}

		#endregion
	}
}