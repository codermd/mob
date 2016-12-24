using System;
using Android;
using Android.Content;
using Android.OS;
using Android.Provider;
using Android.Support.V4.App;
using Mxp.Core.Business;
using Android.Support.Design.Widget;
using Uri = Android.Net.Uri;
using Mxp.Droid.Utils;
using Android.Support.V4.Content;
using Android.Graphics;
using System.IO;

namespace Mxp.Droid.Fragments.Headless
{
	public class HeadlessCameraFragment : Fragment, HeadlessPermissionFragment.IPermissionListener {
		public static readonly string DEFAULT_CAMERA_TAG = typeof (HeadlessCameraFragment).FullName;

		private const string EXTERNAL_STORAGE_PERMISSION = Manifest.Permission.WriteExternalStorage;

		private const int REQUEST_IMAGE_GALLERY = 1;
		private const int REQUEST_IMAGE_CAPTURE = 2;

		private string mCurrentPhotoPath;
		private ICameraListener mPermissionListener;
		private HeadlessPermissionFragment mHeadlessPermissionFragment;

		public override void OnCreate (Bundle savedInstanceState) {
			base.OnCreate (savedInstanceState);

			this.RetainInstance = true;

			this.mHeadlessPermissionFragment = HeadlessFragmentHelper<HeadlessPermissionFragment>.Attach (this);
		}

		public override void OnAttach (Context context) {
			base.OnAttach (context);

			try {
				if (this.TargetFragment != null)
					this.mPermissionListener = (ICameraListener)this.TargetFragment;
				else
					this.mPermissionListener = (ICameraListener)this.Activity;
			} catch (InvalidCastException) {
				if (this.TargetFragment != null)
					throw new InvalidCastException ("Calling fragment " + this.TargetFragment.Class.SimpleName + " must implement ICameraListener interface");
				else
					throw new InvalidCastException ("Calling activity " + this.Activity.Class.SimpleName + " must implement ICameraListener interface");
			}
		}

		public override void OnDetach () {
			base.OnDetach ();

			this.mPermissionListener = null;
		}

		public interface ICameraListener {
			void OnCameraResult (string filepath);
		}

		public void PickImageFromGallery () {
			this.mHeadlessPermissionFragment.CheckPermission (REQUEST_IMAGE_GALLERY, EXTERNAL_STORAGE_PERMISSION);
		}

		public void CaptureImageWithCamera () {
			this.mHeadlessPermissionFragment.CheckPermission (REQUEST_IMAGE_CAPTURE, EXTERNAL_STORAGE_PERMISSION);
		}

		#region HeadlessPermissionFragment.IPermissionListener

		public void OnPermissionResult (int requestCode, bool granted) {
			if (!granted)
				return;

			switch (requestCode) {
				case REQUEST_IMAGE_GALLERY:
					this.OnPermissionGrantedOnGallery ();
					break;
				case REQUEST_IMAGE_CAPTURE:
					this.OnPermissionGrantedOnCamera ();
					break;
			}
		}

		public void ShouldShowPermissionRationale (int requestCode, String [] permissions) {
			if (permissions [0] == EXTERNAL_STORAGE_PERMISSION)
				Snackbar.Make (this.View, "External storage access is required for such action.", Snackbar.LengthIndefinite)
					.SetAction ("OK", v => this.mHeadlessPermissionFragment.RequestPermission (requestCode, permissions [0]))
					.Show ();
		}

		#endregion

		private void OnPermissionGrantedOnGallery () {
			if (Build.VERSION.SdkInt < BuildVersionCodes.Kitkat) {
				Intent intent = new Intent ();
				intent.SetType ("image/*");
				intent.SetAction (Intent.ActionGetContent);

				if (intent.ResolveActivity (this.Activity.PackageManager) != null)
					this.StartActivityForResult (Intent.CreateChooser (intent, Labels.GetLoggedUserLabel (Labels.LabelEnum.Select)), REQUEST_IMAGE_GALLERY);
			} else {
				Intent intent = new Intent (Intent.ActionPick, MediaStore.Images.Media.ExternalContentUri);

				if (intent.ResolveActivity (this.Activity.PackageManager) != null)
					this.StartActivityForResult (intent, REQUEST_IMAGE_GALLERY);
			}
		}

		private void OnPermissionGrantedOnCamera () {
			Intent takePictureIntent = new Intent (MediaStore.ActionImageCapture);

			if (takePictureIntent.ResolveActivity (this.Activity.PackageManager) != null) {
				Java.IO.File photoFile = null;
				try {
					photoFile = this.Context.CreateImageFile (out this.mCurrentPhotoPath);
				} catch (Java.IO.IOException) {
					DialogFragment errorDialogFragment = BaseDialogFragment.NewInstance (this.Activity, this.Activity.GetErrorDialogRequestCode (), BaseDialogFragment.DialogTypeEnum.ErrorDialog, "Oups, something went wrong when creating image...");
					errorDialogFragment.Show (this.Activity.SupportFragmentManager, null);
					return;
				}

				if (photoFile != null) {
					Uri photoURI = FileProvider.GetUriForFile (this.Context, "com.sagacify.mobilexpense.fileprovider", photoFile);
					takePictureIntent.PutExtra (MediaStore.ExtraOutput, photoURI);
					this.StartActivityForResult (takePictureIntent, REQUEST_IMAGE_CAPTURE);
				}
			}
		}

		public override void OnActivityResult (int requestCode, int resultCode, Intent data) {
			base.OnActivityResult (requestCode, resultCode, data);

			if (resultCode != (int)Android.App.Result.Ok) {
				this.mPermissionListener.OnCameraResult (null);
				return;
			}

			string path = null;

			switch (requestCode) {
				case REQUEST_IMAGE_GALLERY:
					path = data?.Data.GetPath (this.Context);
					break;
				case REQUEST_IMAGE_CAPTURE:
					path = this.mCurrentPhotoPath;
					break;
			}

			this.mPermissionListener.OnCameraResult (path);
		}
	}
}
