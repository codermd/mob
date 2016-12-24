using System;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Support.V4.App;
using Mxp.Core.Business;
using Mxp.Droid.Utils;
using Mxp.Droid.Helpers;

namespace Mxp.Droid.Fragments.Headless
{
	public class HeadlessTaskFragment : Fragment {
		public static readonly string DEFAULT_SERVICE_TAG = typeof (HeadlessTaskFragment).FullName;

		private ITaskListener mTaskListener;

		public override void OnCreate (Bundle savedInstanceState) {
			base.OnCreate (savedInstanceState);

			this.RetainInstance = true;
		}

		public override void OnAttach (Context context) {
			base.OnAttach (context);

			try {
				if (this.TargetFragment != null)
					this.mTaskListener = (ITaskListener)this.TargetFragment;
				else
					this.mTaskListener = (ITaskListener)this.Activity;
			} catch (InvalidCastException) {
				if (this.TargetFragment != null)
					throw new InvalidCastException ("Calling fragment " + this.TargetFragment.Class.SimpleName + " must implement ICameraListener interface");
				else
					throw new InvalidCastException ("Calling activity " + this.Activity.Class.SimpleName + " must implement ICameraListener interface");
			}
		}

		public override void OnDetach () {
			base.OnDetach ();

			this.mTaskListener = null;
		}

		public interface ITaskListener {
			void OnTaskResult (WrappedObject obj);
			void OnLaunchTask ();
		}

		public void StartTask (Action action) {
			action ();
		}

		public void OnLaunchTask () {
			this.mTaskListener.OnLaunchTask ();
		}

		public void OnTaskResult (WrappedObject obj) {
			this.mTaskListener.OnTaskResult (obj);
		}
	}
}
