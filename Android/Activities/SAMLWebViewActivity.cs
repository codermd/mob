using System;
using Android.Webkit;
using Android.Support.V7.App;
using Android.Content;
using Android.App;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Support.V4.App;
using Mxp.Droid.Fragments;

namespace Mxp.Droid
{
	public class SAMLWebViewActivity : BaseActivity
	{
		public static readonly string EXTRA_URL = "com.sagacify.mobilexpense.activity.WebViewActivity.EXTRA_URL";

		public event EventHandler<SchemeEventArgs> RedirectEvent;

		private WebView mWebView;
		private string mUrl;

		protected override void OnCreate (Android.OS.Bundle savedInstanceState) {
			base.OnCreate (savedInstanceState);

			this.SetContentView (Resource.Layout.activity_webview);

			this.mWebView = this.FindViewById<WebView> (Resource.Id.WebView);

			this.RedirectEvent += RedirectEventHandler;

			this.mUrl = this.Intent.GetStringExtra (EXTRA_URL);

			this.mWebView.SetWebViewClient (new CustomWebViewClient (this));

			mWebView.Settings.JavaScriptEnabled = true;
			mWebView.Settings.JavaScriptCanOpenWindowsAutomatically = true;
			mWebView.Settings.LoadWithOverviewMode = true; 
			mWebView.Settings.UseWideViewPort = true;

			this.mWebView.LoadUrl (this.mUrl);

			Toolbar toolbar = this.FindViewById<Toolbar> (Resource.Id.Toolbar);
			this.SetSupportActionBar (toolbar);

			this.SupportActionBar.SetDisplayHomeAsUpEnabled (true);

			this.Title = this.mUrl;
		}

		private void RedirectEventHandler (object sender, SchemeEventArgs e) {
			this.RedirectEvent -= RedirectEventHandler;

			this.DisposeWebView ();

			Intent intent = new Intent (Intent.ActionView, Android.Net.Uri.Parse (e.Url));
			this.StartActivity (intent);
		}

		public override bool OnCreateOptionsMenu (IMenu menu) {
			this.MenuInflater.Inflate (Resource.Menu.Auth_menu, menu);

			return base.OnCreateOptionsMenu (menu);
		}

		public override bool OnOptionsItemSelected (IMenuItem item) {
			switch (item.ItemId) {
				case Android.Resource.Id.Home:
					this.OnCancel ();
					return true;
				case Resource.Id.Action_open_in_browser:
					this.StartActivity (new Intent (Intent.ActionView, Android.Net.Uri.Parse (this.mUrl)));
					return true;
			}

			return base.OnOptionsItemSelected (item);
		}

		public override void OnBackPressed () {
			if (this.mWebView.CanGoBack ()) {
				this.mWebView.GoBack ();
				return;
			}

			this.OnCancel ();
		}

		private void DisposeWebView () {
			if (this.mWebView == null)
				return;
			
			this.mWebView.StopLoading ();
			this.mWebView.LoadUrl ("about:blank");
			this.mWebView = null;
		}

		public void OnCancel () {
			this.DisposeWebView ();

//			Intent upIntent = NavUtils.GetParentActivityIntent (this);
			Intent upIntent = new Intent (this, typeof (LoginActivity));

			if (NavUtils.ShouldUpRecreateTask (this, upIntent))
				Android.Support.V4.App.TaskStackBuilder.Create (this)
					.AddNextIntent (upIntent)
					.StartActivities ();
			else
				NavUtils.NavigateUpTo (this, upIntent);
		}

		public class CustomWebViewClient : WebViewClient
		{
			private SAMLWebViewActivity mActivity;

			public CustomWebViewClient (SAMLWebViewActivity activity) : base () {
				this.mActivity = activity;
			}

			public override bool ShouldOverrideUrlLoading (WebView view, string url) {
				if (url.StartsWith ("http:") || url.StartsWith ("https:"))
					return false;

				EventHandler<SchemeEventArgs> eventHandler = this.mActivity.RedirectEvent;
				if (eventHandler != null)
					eventHandler (this, new SchemeEventArgs (url));

				return true;
			}

			public override void OnReceivedHttpAuthRequest (WebView view, HttpAuthHandler handler, string host, string realm) {
				AuthenticationDialogFragment dialogFragment = AuthenticationDialogFragment.NewInstance (host);
				dialogFragment.OnClickHandler += (object sender, DialogArgsObject<AuthenticationDialogFragment.AuthWrapper> e) => {
					((Android.Support.V4.App.DialogFragment)sender).Dismiss ();

					switch (e.ButtonType) {
						case DialogButtonType.Negative:
							this.mActivity.OnCancel ();
							break;
						case DialogButtonType.Positive:
							handler.Proceed (e.Object.Username, e.Object.Password);
							break;
					}
				};

				dialogFragment.Show (this.mActivity.SupportFragmentManager, null);
			}
		}

		public class SchemeEventArgs : EventArgs
		{
			public string Url { get; }

			public SchemeEventArgs (string url) {
				this.Url = url;
			}
		}
	}
}