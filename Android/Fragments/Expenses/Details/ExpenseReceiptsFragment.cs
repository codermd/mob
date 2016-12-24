using System;
using Android.OS;
using Android.Views;
using Android.Content;
using Android.Widget;
using Mxp.Droid.Adapters;
using Mxp.Core.Business;
using Android.Graphics;
using Uri = Android.Net.Uri;
using Mxp.Droid.Utils;
using Android.Support.V4.Widget;
using System.Collections.Generic;
using Mxp.Core.Utils;
using System.Threading.Tasks;
using Mxp.Droid.Fragments.Headless;
using Android;
using Android.Support.Design.Widget;
using Mxp.Core.Services;
using Android.Support.V4.App;
using Mxp.Droid.Helpers;

namespace Mxp.Droid.Fragments
{
	public class ExpenseReceiptsFragment : Fragment, AbsListView.IMultiChoiceModeListener,
		HeadlessPermissionFragment.IPermissionListener, INotifyFragmentDataSetRefreshed,
	HeadlessCameraFragment.ICameraListener, HeadlessTaskFragment.ITaskListener, BaseDialogFragment.IDialogClickListener
	{
		private const string PERMISSION_TAG = "com.sagacify.mxp.expensereceiptsfragment.permission";
		private const string UPLOADING_KEY = "com.sagacify.mxp.uploading";
		private const string EXTERNAL_STORAGE_PERMISSION = Manifest.Permission.WriteExternalStorage;

		private const int REQUEST_SAVE_FILE = 3;

		private GridView mGridView;
		private SwipeRefreshLayout mRefresher;
		private Android.Support.V7.Widget.Toolbar mToolbar;
		private IMenuItem mToolbarActionMenuItem;
		private ReceiptAdapter mReceiptAdapter;
		private DialogFragment mProgressDialog = ProgressDialogFragment.NewInstance ();
		private Receipt mSelectedReceipt;

		private bool _isUploading;
		private bool IsUploading {
			get {
				return this._isUploading;
			}
			set {
				this._isUploading = value;
				this.InvalidateToolbarMenu ();
			}
		}

		private HeadlessPermissionFragment mHeadlessPermissionFragment;
		private HeadlessCameraFragment mHeadlessCameraFragment;
		private HeadlessTaskFragment mHeadlessTaskFragment;

		protected virtual bool CanShowActions {
			get {
				return this.mExpense.CanManageReceipts;
			}
		}

		private Expense mExpense {
			get {
				return ((ExpenseDetailsActivity)this.Activity).ExpenseItem.ParentExpense;
			}
		}

		protected virtual Receipts mReceipts {
			get {
				return this.mExpense.Receipts;
			}
		}
			
		public static ExpenseReceiptsFragment NewInstance () {
			ExpenseReceiptsFragment expenseReceiptsFragment = new ExpenseReceiptsFragment ();
			return expenseReceiptsFragment;
		}

		public override void OnCreate (Bundle savedInstanceState) {
			base.OnCreate (savedInstanceState);

			this.mHeadlessPermissionFragment = HeadlessFragmentHelper<HeadlessPermissionFragment>.Attach (this, PERMISSION_TAG);
			this.mHeadlessCameraFragment = HeadlessFragmentHelper<HeadlessCameraFragment>.Attach (this);
			this.mHeadlessTaskFragment = HeadlessFragmentHelper<HeadlessTaskFragment>.Attach (this);

			this.NotifyDataSetRefreshed ();
		}

		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
			View view = inflater.Inflate (Resource.Layout.Grid_receipts, container, false);

			this.mRefresher = view.FindViewById<SwipeRefreshLayout> (Resource.Id.Refresher);
			this.mRefresher.SetColorSchemeResources(Android.Resource.Color.HoloBlueBright, Android.Resource.Color.HoloGreenLight, Android.Resource.Color.HoloOrangeLight, Android.Resource.Color.HoloRedLight);

			this.mRefresher.Refresh += (object sender, EventArgs e) => this.RefreshReceipts ();

			this.mGridView = view.FindViewById<GridView> (Resource.Id.Grid);
			this.mGridView.Adapter = this.mReceiptAdapter;
			this.mGridView.ItemClick += (object sender, AdapterView.ItemClickEventArgs e) => {
				this.mSelectedReceipt = this.mReceipts [e.Position];

				if (this.mSelectedReceipt.IsDocument)
					this.mHeadlessPermissionFragment.CheckPermission (REQUEST_SAVE_FILE, EXTERNAL_STORAGE_PERMISSION);
				else
					this.StartPhotoViewPagerActiviy (this.mSelectedReceipt);
			};

			if (this.CanShowActions) {
				this.mGridView.ChoiceMode = ChoiceMode.MultipleModal;
				this.mGridView.SetMultiChoiceModeListener (this);
			}

			this.mToolbar = view.FindViewById<Android.Support.V7.Widget.Toolbar> (Resource.Id.Toolbar);
			this.mToolbar.InflateMenu (Resource.Menu.Receipts_menu);
			this.mToolbarActionMenuItem = this.mToolbar.Menu.FindItem (Resource.Id.Action_new);
			this.mToolbar.MenuItemClick += (object sender, Android.Support.V7.Widget.Toolbar.MenuItemClickEventArgs e) => {
				switch (e.Item.ItemId) {
					case Resource.Id.Action_new:
						this.ShowNewReceiptDialog ();
						break;
				}
			};

			return view;
		}

		public override void OnViewCreated (View view, Bundle savedInstanceState) {
			base.OnViewCreated (view, savedInstanceState);

			if (savedInstanceState != null)
				this.IsUploading = savedInstanceState.GetBoolean (UPLOADING_KEY);

			if (!this.mReceipts.Loaded)
				this.mRefresher.Post (this.RefreshReceipts);

			this.mToolbar.Visibility = this.CanShowActions ? ViewStates.Visible : ViewStates.Gone;
		}

		private async void RefreshReceipts () {
			this.mRefresher.Refreshing = true;
			await this.mReceipts.FetchAsync ().StartAsync (TaskConfigurator.Create (this));
			this.mRefresher.Refreshing = false;

			this.mReceiptAdapter.NotifyDataSetChanged ();
		}

		protected virtual void StartPhotoViewPagerActiviy (Receipt receipt) {
			Intent intent = new Intent (this.Activity, typeof(PhotoViewPagerActivity));
			Report report = this.mExpense.GetModelParent<Expense, Report> ();

			if (report != null) {
				intent.PutExtra (PhotoViewPagerActivity.EXTRA_REPORT_ID, report.Id.Value);
				intent.PutExtra (PhotoViewPagerActivity.EXTRA_REPORT_TYPE, (int) report.ReportType);
			}

			if (this.mExpense.Id.HasValue)
				intent.PutExtra (PhotoViewPagerActivity.EXTRA_EXPENSE_ID, this.mExpense.Id.Value);

			if (receipt.base64 != null)
				intent.PutExtra (PhotoViewPagerActivity.EXTRA_RECEIPT_POSITION, receipt.GetCollectionParent<Receipts, Receipt> ().IndexOf (receipt));
			else
				intent.PutExtra (PhotoViewPagerActivity.EXTRA_RECEIPT_ID, receipt.AttachmentId);

			this.StartActivity(intent);
		}

		public void ShowNewReceiptDialog () {
			List<Actionable> actions = new List<Actionable> () {
				new Actionable (Labels.GetLoggedUserLabel (Labels.LabelEnum.FromLibrary), this.mHeadlessCameraFragment.PickImageFromGallery)
			};

			if (this.Activity.PackageManager.HasSystemFeature (Android.Content.PM.PackageManager.FeatureCamera))
				actions.Add (new Actionable (Labels.GetLoggedUserLabel (Labels.LabelEnum.TakeAPicture), this.mHeadlessCameraFragment.CaptureImageWithCamera));

			Android.Support.V4.App.DialogFragment actionsDialogFragment = new ActionPickerDialogFragment (actions);
			actionsDialogFragment.Show (this.ChildFragmentManager, null);
		}

		private void InvalidateToolbarMenu () {
			if (this.IsUploading) {
				this.mToolbarActionMenuItem.SetActionView (new ProgressBar (this.Activity));
				// Prevent "java.lang.IllegalArgumentException: Wrong state class, expecting View State but received class android.widget.ProgressBar$SavedState instead" to be raised.
				this.mToolbarActionMenuItem.ActionView.SaveEnabled = false;
			} else
				this.mToolbarActionMenuItem.SetActionView (null);
		}

		public override void OnSaveInstanceState (Bundle outState) {
			base.OnSaveInstanceState (outState);

			outState.PutBoolean (UPLOADING_KEY, this.IsUploading);
		}

		#region HeadlessPermissionFragment.IPermissionListener

		public void OnPermissionResult (int requestCode, bool granted) {
			if (!granted)
				return;
			
			switch (requestCode) {
				case REQUEST_SAVE_FILE:
					this.OnPermissionGrantedOnFile ();
					break;
			}
		}

		public void ShouldShowPermissionRationale (int requestCode, String[] permissions) {
			if (permissions [0] == EXTERNAL_STORAGE_PERMISSION)
				Snackbar.Make (this.View, "External storage access is required for such action.", Snackbar.LengthIndefinite)
					.SetAction ("OK", v => this.mHeadlessPermissionFragment.RequestPermission (requestCode, permissions [0]))
					.Show ();
		}

		#endregion

		#region HeadlessCameraFragment.ICameraListener

		public void OnCameraResult (string filepath) {
			this.mHeadlessTaskFragment.StartTask (async () => {
				if (filepath == null)
					return;

				if (!(new Java.IO.File (filepath)).IsImage ()) {
					DialogFragment errorDialogFragment = BaseDialogFragment.NewInstance (this.Activity, this.Activity.GetErrorDialogRequestCode (), BaseDialogFragment.DialogTypeEnum.ErrorDialog, "Sorry, image file only");
					errorDialogFragment.Show (this.Activity.SupportFragmentManager, null);
					return;
				}

				this.mHeadlessTaskFragment.OnLaunchTask ();

				string encodedImage = null;

				try {
					Bitmap bitmap = await BitmapHelper.ResizeAndRotateBitmapAsync (filepath, 1200);
					encodedImage = await BitmapHelper.EncodeToBase64 (bitmap);
				} catch (Exception) {
					DialogFragment errorDialogFragment = BaseDialogFragment.NewInstance (this.Activity, this.Activity.GetErrorDialogRequestCode (), BaseDialogFragment.DialogTypeEnum.ErrorDialog, "Sorry, something bad happened. Please try again.");
					errorDialogFragment.Show (this.Activity.SupportFragmentManager, null);
				}

				this.mHeadlessTaskFragment.OnTaskResult (new WrappedObject (encodedImage));
			});
		}

		#endregion

		#region HeadlessTaskFragment.ITaskListener

		public void OnLaunchTask () {
			this.IsUploading = true;
			this.InvalidateToolbarMenu ();
		}

		public void OnTaskResult (WrappedObject obj) {
			this.Activity.InvokeActionAsync (
				async () =>  {
					string encodedImage = obj.GetInstance<string> ();

					if (encodedImage != null)
						await this.mReceipts.AddReceipt (encodedImage);
				}, () => {
					this.IsUploading = false;
					this.InvalidateToolbarMenu ();

					this.mReceiptAdapter.NotifyDataSetChanged ();
				}, this);
		}

		#endregion

		private async void OnPermissionGrantedOnFile () {
			this.mProgressDialog.ShowAllowingStateLoss (this.ChildFragmentManager, null);

			string filepath = null;

			try {
				filepath = await ReceiptService.Instance.DownloadAndSaveFileAsync (this.mSelectedReceipt.AttachmentPath, this.mSelectedReceipt.AttachmentOriginalName, FileServiceBase.FileDirectory.Download);
			} catch (Exception) {
				this.mProgressDialog.DismissAllowingStateLoss ();

				BaseDialogFragment errorDialogFragment = BaseDialogFragment.NewInstance (this.Activity, BaseDialogFragment.DialogTypeEnum.ExceptionDialog, this.GetString (Resource.String.error_launch_pdf));
				errorDialogFragment.ShowAllowingStateLoss (this.ChildFragmentManager, null);

				return;
			}

			Java.IO.File file = new Java.IO.File (filepath);
			string mimetype = file.GetMimetype ();

			Intent intent = new Intent (Intent.ActionView);
			intent.SetDataAndType (Uri.FromFile (file), mimetype);

			if (String.IsNullOrEmpty (mimetype) || intent.ResolveActivity (this.Activity.PackageManager) == null)
				intent.SetData (Uri.Parse (filepath));

			if (intent.ResolveActivity (this.Activity.PackageManager) == null)
				intent.SetData (Uri.Parse (this.mSelectedReceipt.AttachmentPath));

			if (intent.ResolveActivity (this.Activity.PackageManager) == null) {
				this.mProgressDialog.DismissAllowingStateLoss ();

				BaseDialogFragment errorDialogFragment = BaseDialogFragment.NewInstance (this.Activity, BaseDialogFragment.DialogTypeEnum.ErrorDialog, this.GetString (Resource.String.error_no_pdf_activity));
				errorDialogFragment.ShowAllowingStateLoss (this.ChildFragmentManager, null);

				return;
			}

			this.StartActivity (Intent.CreateChooser (intent, Labels.GetLoggedUserLabel (Labels.LabelEnum.Select)));

			this.mProgressDialog.DismissAllowingStateLoss ();
		}

		#region AbsListView.IMultiChoiceModeListener

		public void OnItemCheckedStateChanged (ActionMode mode, int position, long id, bool @checked) {
			// Here you can do something when items are selected/de-selected,
			// such as update the title in the CAB
		}

		public bool OnActionItemClicked (ActionMode mode, IMenuItem item) {
			// Respond to clicks on the actions in the CAB
			switch (item.ItemId) {
				case Resource.Id.Action_delete:
					this.RemoveReceipts ();
					mode.Finish (); // Action picked, so close the CAB
					return true;
				default:
					return false;
			}
		}

		public bool OnCreateActionMode (ActionMode mode, IMenu menu) {
			// Inflate the menu for the CAB
			MenuInflater inflater = mode.MenuInflater;
			inflater.Inflate (Resource.Menu.Receipt_context, menu);
			return true;
		}

		public void OnDestroyActionMode (ActionMode mode) {
			// Here you can make any necessary updates to the activity when
			// the CAB is removed. By default, selected items are deselected/unchecked.
		}

		public bool OnPrepareActionMode (ActionMode mode, IMenu menu) {
			// Here you can perform updates to the CAB due to
			// an invalidate() request
			return false;
		}

		#endregion

		public void RemoveReceipts () {
			List<Task> tasks = new List<Task> (this.mGridView.CheckedItemPositions.Size ());

			for (int i = 0; i < this.mGridView.CheckedItemPositions.Size (); i++) {
				if (this.mGridView.CheckedItemPositions.ValueAt (i))
					tasks.Add (this.mReceipts.DeleteReceipt (this.mReceiptAdapter [this.mGridView.CheckedItemPositions.KeyAt (i)]));
			}

			TaskConfigurator.Create (this)
			                .Finally (() => {
								this.mGridView.CheckedItemPositions.Clear ();
								this.mReceiptAdapter.NotifyDataSetChanged ();
							})
			                .Start (Task.WhenAll (tasks.ToArray ()));
		}

		#region INotifyFragmentDataSetRefreshed

		public void NotifyDataSetRefreshed () {
			this.mReceiptAdapter = new ReceiptAdapter (this.Activity, this.mReceipts);
		}

		#endregion

        // used in backdoor for UI test
        public void AddImage()
        {
			this.OnCameraResult (Context.GetFileStreamPath ("UITestImage.png").AbsolutePath);
        }

		public void OnClickHandler<T> (int requestCode, DialogArgsObject<T> args) {
			
		}
	}
}