using System;
using Android.Support.V4.App;
using Android.OS;
using Android.Content;
using Mxp.Droid.Utils;
using Android.Content.PM;
using Android.Support.V4.Content;
using System.Collections.Generic;

namespace Mxp.Droid.Fragments.Headless
{
	public class HeadlessPermissionFragment : Fragment
	{
		public static readonly string DEFAULT_PERMISSION_TAG = typeof (HeadlessPermissionFragment).FullName;

		private IPermissionListener mPermissionListener;

		public override void OnCreate (Bundle savedInstanceState) {
			base.OnCreate (savedInstanceState);

			this.RetainInstance = true;
		}

		public override void OnAttach (Context context) {
			base.OnAttach (context);

			try {
				if (this.TargetFragment != null)
					this.mPermissionListener = (IPermissionListener) this.TargetFragment;
				else
					this.mPermissionListener = (IPermissionListener) this.Activity;
			} catch (InvalidCastException) {
				if (this.TargetFragment != null)
					throw new InvalidCastException ("Calling fragment " + this.TargetFragment.Class.SimpleName + " must implement IPermissionListener interface");
				else
					throw new InvalidCastException ("Calling activity " + this.Activity.Class.SimpleName + " must implement IPermissionListener interface");
			}
		}

		public override void OnDetach () {
			base.OnDetach ();

			this.mPermissionListener = null;
		}

		public void CheckPermission (int requestCode, string permission) {
			// Checks whether we have the permission we need
			if (ContextCompat.CheckSelfPermission (this.Context, permission) == Permission.Granted)
				this.mPermissionListener.OnPermissionResult (requestCode, true);
			// Check whether the application has previously requested the specified permission and been denied
			else if (ActivityCompat.ShouldShowRequestPermissionRationale (this.Activity, permission))
				this.mPermissionListener.ShouldShowPermissionRationale (requestCode, new String[] { permission });
			else
				this.RequestPermission (requestCode, permission);
		}

		public void CheckPermissions (int requestCode, String[] permissions) {
			List<String> permissionsRestricted = new List<String> ();
			foreach (string permission in permissions)
				if (ContextCompat.CheckSelfPermission (this.Context, permission) != Permission.Granted)
					permissionsRestricted.Add (permission);

			if (permissionsRestricted.Count > 0) {
				List<String> permissionsDenied = new List<String> ();
				foreach (string permission in permissionsRestricted)
					if (ActivityCompat.ShouldShowRequestPermissionRationale (this.Activity, permission)) {
						permissionsDenied.Add (permission);
						permissionsRestricted.Remove (permission);
					}
				if (permissionsDenied.Count > 0)
					this.mPermissionListener.ShouldShowPermissionRationale (requestCode, permissionsDenied.ToArray ());
				if (permissionsRestricted.Count > 0)
					this.RequestPermissions (requestCode, permissionsRestricted.ToArray ());
			} else
				this.mPermissionListener.OnPermissionResult (requestCode, true);
		}

		public void RequestPermission (int requestCode, string permission) {
			this.RequestPermissions (new String[] { permission }, requestCode);
		}

		private void RequestPermissions (int requestCode, String[] permissions) {
			this.RequestPermissions (permissions, requestCode);
		}

		public override void OnRequestPermissionsResult (int requestCode, String[] permissions, Permission[] grantResults) {
			this.mPermissionListener.OnPermissionResult (requestCode, PermissionUtils.VerifyPermissions (grantResults));
		}

		public interface IPermissionListener {
			void OnPermissionResult (int requestCode, bool granted);
			void ShouldShowPermissionRationale (int requestCode, String[] permissions);
		}
	}
}