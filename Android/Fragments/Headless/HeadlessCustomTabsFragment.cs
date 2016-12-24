using System;
using Android.OS;
using Android.Support.V4.App;
using Android.Content;
using Android.Support.CustomTabs;
using Mxp.Droid.Helpers.CustomTabs;
using Android.Support.V4.Content;
using Android.Graphics;

namespace Mxp.Droid.Fragments.Headless
{
	public class HeadlessCustomTabsFragment : Fragment
	{
		public static readonly string DEFAULT_CUSTOM_TABS_TAG = typeof (HeadlessCustomTabsFragment).FullName;

		private ICustomTabsServiceListener mCustomTabsServiceListener;
		private CustomTabsActivityHelper mCustomTabsActivityHelper;
		private Bitmap mCloseButtonBitmap;

		#region Lifecycle

		public override void OnAttach (Context context) {
			base.OnAttach (context);

			this.mCustomTabsActivityHelper = new CustomTabsActivityHelper (this.Activity);

			try {
				if (this.TargetFragment != null)
					this.mCustomTabsServiceListener = (ICustomTabsServiceListener) this.TargetFragment;
				else
					this.mCustomTabsServiceListener = (ICustomTabsServiceListener) this.Activity;
			} catch (InvalidCastException) {
				// Optional
			}

			if (this.mCustomTabsServiceListener != null)
				this.mCustomTabsServiceListener.OnHeadlessCustomTabsFragmentAttached ();
		}

		public override void OnCreate (Bundle savedInstanceState) {
			base.OnCreate (savedInstanceState);

			this.RetainInstance = true;

			this.mCustomTabsActivityHelper.CustomTabsServiceConnected += CustomTabsActivityManager_CustomTabsServiceConnected;
			this.mCustomTabsActivityHelper.CustomTabsServiceDisconnected += CustomTabsActivityManager_CustomTabsServiceDisconnected;

			this.mCloseButtonBitmap = BitmapFactory.DecodeResource (this.Resources, Resource.Drawable.ic_action_navigation_arrow_back);
		}

		public override void OnStart () {
			base.OnStart ();

			this.mCustomTabsActivityHelper.BindService ();
		}

		public override void OnStop () {
			this.mCustomTabsActivityHelper.UnbindService ();

			base.OnStop ();
		}

		public override void OnDestroy () {
			this.mCustomTabsActivityHelper.CustomTabsServiceConnected -= CustomTabsActivityManager_CustomTabsServiceConnected;
			this.mCustomTabsActivityHelper.CustomTabsServiceDisconnected -= CustomTabsActivityManager_CustomTabsServiceDisconnected;

			base.OnDestroy ();
		}

		public override void OnDetach () {
			this.mCustomTabsServiceListener = null;

			base.OnDetach ();
		}

		#endregion

		public void MayLaunchUrl (string url) {
			this.mCustomTabsActivityHelper?.MayLaunchUrl (url, null, null);
		}

		public void OpenCustomTab (string url) {
			CustomTabsIntent.Builder intentBuilder = new CustomTabsIntent.Builder ();

//			TypedValue typedValue = new TypedValue ();
//			this.Activity.Theme.ResolveAttribute (Resource.Attribute.colorPrimary, typedValue, true);
//			intentBuilder.SetToolbarColor (ContextCompat.GetColor (this.Activity, typedValue.Data));

			intentBuilder.SetToolbarColor (ContextCompat.GetColor (this.Activity, Resource.Color.blue));

			intentBuilder.SetShowTitle (true);

			intentBuilder.SetCloseButtonIcon (this.mCloseButtonBitmap);

			intentBuilder.SetStartAnimations (this.Activity, Resource.Animation.slide_in_right, Resource.Animation.slide_out_left);
			intentBuilder.SetExitAnimations(this.Activity, Android.Resource.Animation.SlideInLeft, Android.Resource.Animation.SlideOutRight);

			this.mCustomTabsActivityHelper.OpenCustomTab (url, intentBuilder.Build (), new WebviewFallback ());
		}

		private void CustomTabsActivityManager_CustomTabsServiceDisconnected (ComponentName name) {
			
		}

		private void CustomTabsActivityManager_CustomTabsServiceConnected (ComponentName name, CustomTabsClient client) {
			this.mCustomTabsActivityHelper.Warmup ();
			// Initialize a session asap
			CustomTabsSession session = this.mCustomTabsActivityHelper.Session;
		}

		public interface ICustomTabsServiceListener
		{
			void OnHeadlessCustomTabsFragmentAttached ();
		}
	}
}