using System;
using Android.Support.CustomTabs;
using Android.App;

namespace Mxp.Droid.Helpers.CustomTabs
{
	public class CustomTabsActivityHelper : CustomTabsActivityManager
	{
		public CustomTabsActivityHelper (Activity activity) : base (activity) {
			
		}

		/**
	     * Opens the URL on a Custom Tab if possible. Otherwise fallsback to opening it on a WebView
	     *
	     * @param customTabsIntent a CustomTabsIntent to be used if Custom Tabs is available
	     * @param uri the Uri to be opened
	     * @param fallback a CustomTabFallback to be used if Custom Tabs is not available
	     */
		public void OpenCustomTab (string url, CustomTabsIntent customTabsIntent = null, CustomTabFallback fallback = null) {
			String packageName = CustomTabsHelper.GetPackageNameToUse (this.ParentActivity);

			// If we cant find a package name, it means there's no browser that supports
			// Chrome Custom Tabs installed. So, we fallback to the webview
			if (packageName == null) {
				if (fallback != null)
					fallback.OpenUri (this.ParentActivity, url);
			} else {
				if (customTabsIntent != null)
					customTabsIntent.Intent.SetPackage (packageName);
				
				this.LaunchUrl (url, customTabsIntent);
			}
		}

		/**
	     * Unbinds the Activity from the Custom Tabs Service
	     */
		public void UnbindService () {
//			this.ParentActivity.UnbindService (this.connection);
		}

		/**
	     * To be used as a fallback to open the Uri when Custom Tabs is not available
	     */
		public interface CustomTabFallback {
			/**
	         *
	         * @param activity The Activity that wants to open the Uri
	         * @param uri The uri to be opened by the fallback
	         */
			void OpenUri (Activity activity, string url);
		}
	}
}