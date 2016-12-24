using System;
using Android.OS;
using Android.Support.V7.Widget;
using Mxp.Core.Business;
using Android.Support.V4.View;
using Mxp.Droid.Adapters;
using DK.Ostebaronen.Droid.ViewPagerIndicator;
using Android.Graphics;
using Mxp.Droid.Fragments;
using Mxp.Droid.Utils;
using System.Threading.Tasks;
using System.Collections;
using Android.Views;
using Android.Support.V4.App;
using Android.Content;
using Mxp.Droid.Helpers;

namespace Mxp.Droid
{		
	public class SpendCatcherSharingActivity : BaseActivity
	{
		public const int REQUEST_CODE = 1;

		public static SpendCatcherExpenses sSpendCatcherExpenses { get; set; } = new SpendCatcherExpenses ();

		private ViewPager mViewPager;
		private IMenu mMenu;

		protected override void OnCreate (Bundle savedInstanceState) {
			base.OnCreate (savedInstanceState);

			this.SetContentView (Resource.Layout.spendcatcher_sharing_activity);

			Toolbar toolbar = this.FindViewById<Toolbar> (Resource.Id.Toolbar);
			this.SetSupportActionBar (toolbar);

			this.Title = Labels.GetLoggedUserLabel (Labels.LabelEnum.SpendCatcher);

			this.SupportActionBar.SetDisplayHomeAsUpEnabled (true);
			this.SupportActionBar.SetHomeAsUpIndicator (Resource.Drawable.ic_action_clear);

			this.mViewPager = this.FindViewById<ViewPager> (Resource.Id.pager);
			this.mViewPager.PageSelected += (object sender, ViewPager.PageSelectedEventArgs e) => {
				if (e.Position == this.mViewPager.Adapter.Count - 1)
					this.mMenu.FindItem (Resource.Id.action_next).SetIcon (Resource.Drawable.ic_action_done);
				else if (e.Position == this.mViewPager.Adapter.Count - 2)
					this.mMenu.FindItem (Resource.Id.action_next).SetIcon (Resource.Drawable.ic_action_navigation_arrow_forward);
			};

			if (Preferences.Instance.IsSpendCatcherEnable) {
				if (sSpendCatcherExpenses.Count == 0)
					this.HandleImages ();
				else
					this.Refresh ();
			} else {
				DialogFragment errorDialogFragment = BaseDialogFragment.NewInstance (this, 0, BaseDialogFragment.DialogTypeEnum.MessageDialog, Labels.GetLoggedUserLabel(Labels.LabelEnum.SpendCatcherDisabledMessage));
				errorDialogFragment.Show (this.SupportFragmentManager, null);
			}
		}

		public override bool OnCreateOptionsMenu (IMenu menu) {
			this.mMenu = menu;

			this.MenuInflater.Inflate (Resource.Menu.spendcatcher_menu, menu);

			if (this.mViewPager?.CurrentItem == this.mViewPager?.Adapter?.Count - 1)
				this.mMenu.FindItem (Resource.Id.action_next).SetIcon (Resource.Drawable.ic_action_done);

			return base.OnCreateOptionsMenu (menu);
		}

		public override bool OnOptionsItemSelected (IMenuItem item) {
			switch (item.ItemId) {
				case Android.Resource.Id.Home:
					this.Finish ();
					return true;
				case Resource.Id.action_next:
					if (this.mViewPager.CurrentItem == this.mViewPager.Adapter?.Count - 1)
						this.SendSpendCatcherExpenses ();
					else
						this.mViewPager.SetCurrentItem (this.mViewPager.CurrentItem + 1, true);
					
					return true;
			}

			return base.OnOptionsItemSelected (item);
		}

		private void SendSpendCatcherExpenses () {
			TaskConfigurator.Create (this)
			                .SetWithProgress (true)
							.Finally (() => {
								DialogFragment errorDialogFragment = BaseDialogFragment.NewInstance (REQUEST_CODE, null, Labels.GetLoggedUserLabel (Labels.LabelEnum.SpendCatcherConfirmationMessage), Labels.GetLoggedUserLabel (Labels.LabelEnum.Cancel), null, Labels.GetLoggedUserLabel (Labels.LabelEnum.SpendCatcherOpenApp));
								errorDialogFragment.Show (this.SupportFragmentManager, null);
							}).Start (Task.Run (async () => {
								await sSpendCatcherExpenses.SendAsync ();
								await LoggedUser.Instance.SpendCatcherExpenses.FetchAsync ();
							}));
		}

		private async void HandleImages () {
			if (!this.Intent.Type.StartsWith ("image/"))
				return;

			if (Intent.ActionSend.Equals (this.Intent.Action) && this.Intent.Type != null)
				await this.HandleSendImage ();
			else if (Intent.ActionSendMultiple.Equals (this.Intent.Action) && this.Intent.Type != null)
				await this.HandleSendMultipleImages ();

			this.Refresh ();
		}

		private async Task HandleSendImage () {
			Android.Net.Uri imageUri = (Android.Net.Uri) this.Intent.GetParcelableExtra (Intent.ExtraStream);
			if (imageUri != null) {
				try {
					await this.HandleImage (imageUri);
				} catch (Exception) {
					DialogFragment errorDialogFragment = BaseDialogFragment.NewInstance (this, this.GetErrorDialogRequestCode (), BaseDialogFragment.DialogTypeEnum.ErrorDialog, "Sorry, something bad happened. Please try again.");
					errorDialogFragment.Show (this.SupportFragmentManager, null);
				}
			}
		}

		private async Task HandleSendMultipleImages () {
			IList imageUris = this.Intent.GetParcelableArrayListExtra (Intent.ExtraStream);
			if (imageUris != null) {
				foreach (Android.Net.Uri imageUri in imageUris) {
					try {
						await this.HandleImage (imageUri);
					} catch (Exception) {
						DialogFragment errorDialogFragment = BaseDialogFragment.NewInstance (this, this.GetErrorDialogRequestCode (), BaseDialogFragment.DialogTypeEnum.ErrorDialog, "Sorry, something bad happened. Please try again.");
						errorDialogFragment.Show (this.SupportFragmentManager, null);
					}
				}
			}
		}

		private async Task HandleImage (Android.Net.Uri imageUri) {
			Bitmap bitmap = await BitmapHelper.ResizeAndRotateBitmapAsync (imageUri, this.BaseContext, 1200);
			string encodedImage = await BitmapHelper.EncodeToBase64 (bitmap);
			sSpendCatcherExpenses.AddItem (new SpendCatcherExpense (encodedImage));
			bitmap = null;
		}

		private void Refresh () {
			this.mViewPager.Adapter = new SpendCatcherPagerAdapter (this.SupportFragmentManager, sSpendCatcherExpenses);

			if (this.mViewPager.CurrentItem == this.mViewPager.Adapter?.Count - 1)
				this.mMenu?.FindItem (Resource.Id.action_next).SetIcon (Resource.Drawable.ic_action_done);

			CirclePageIndicator indicator = FindViewById<CirclePageIndicator> (Resource.Id.indicator);
			indicator.SetViewPager (this.mViewPager);
		}

		#region IDialogClickListener

		public override void OnClickHandler<T> (int requestCode, DialogArgsObject<T> args) {
			if (requestCode != REQUEST_CODE)
				return;
			
			if (args.ButtonType == DialogButtonType.Positive) {
				Intent intent = new Intent (this, typeof(MainActivity));

				intent.PutExtra (MainActivity.EXTRA_SELECTED_TAB, 0);
				intent.PutExtra (MainActivity.EXTRA_SELECTED_CATEGORY, 2);

				intent.SetFlags (ActivityFlags.NewTask | ActivityFlags.ClearTask);

				this.StartActivity (intent);
			}

			this.Finish ();
		}

		#endregion

		public override void Finish () {
			sSpendCatcherExpenses.Clear ();

			base.Finish ();
		}
	}
}