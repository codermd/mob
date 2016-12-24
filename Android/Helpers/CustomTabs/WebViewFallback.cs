using System;
using Android.App;
using Android.Content;

namespace Mxp.Droid.Helpers.CustomTabs
{
	/**
	* A Fallback that opens a Webview when Custom Tabs is not available
	*/
	public class WebviewFallback : CustomTabsActivityHelper.CustomTabFallback
	{
		public void OpenUri (Activity activity, string url) {
			Intent intent = new Intent (activity, typeof (SAMLWebViewActivity));
			intent.PutExtra (SAMLWebViewActivity.EXTRA_URL, url);
			activity.StartActivity (intent);
		}
	}
}