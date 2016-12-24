using System;

using UIKit;

using Foundation;
using Mxp.Core.Business;

namespace Mxp.iOS
{
	public partial class WebViewController : UIViewController
	{
		private NSUrlRequest UrlRequest;

		public bool Authenticated { get; set; }
		public NSUrlConnection UrlConnection { get; set; }

		public WebViewController (string url) : base ("WebViewController", null) {
			this.UrlRequest = new NSUrlRequest (NSUrl.FromString (url));
		}
			
		public override void ViewDidLoad () {
			base.ViewDidLoad ();

			UIBarButtonItem cancelButton = new UIBarButtonItem (Labels.GetLoggedUserLabel (Labels.LabelEnum.Cancel), UIBarButtonItemStyle.Done, (object sender, EventArgs e) => {
				this.CancelWebView ();
			});

			this.NavigationItem.SetLeftBarButtonItem (cancelButton, false);

			UIBarButtonItem openInBrowserButton = new UIBarButtonItem ("Open in browser", UIBarButtonItemStyle.Done, (object sender, EventArgs e) => {
				UIApplication.SharedApplication.OpenUrl (this.UrlRequest.Url);
				this.CancelWebView ();
			});

			this.NavigationItem.SetRightBarButtonItem (openInBrowserButton, false);

			this.webView.Delegate = new CustomUIWebViewDelegate (this);
			this.webView.LoadRequest (this.UrlRequest);
		}

		public void CancelWebView () {
			this.StopWebView (() => {
				LoadingView.hideMessage ();

				LoginViewController loginViewController = MainNavigationController.Instance.TopViewController as LoginViewController;

				if (loginViewController == null) {
					UIStoryboard storyboard =  UIStoryboard.FromName ("MainStoryBoard", NSBundle.MainBundle);
					loginViewController = storyboard.InstantiateViewController ("LoginViewController") as LoginViewController;
					UIViewController[] vcs = new UIViewController [2] {
						MainNavigationController.Instance.ViewControllers [0],
						loginViewController
					};
					MainNavigationController.Instance.SetViewControllers (vcs, true);
				}
			});
		}

		public void StopWebView (Action action = null) {
			if (this.UrlConnection != null)
				this.UrlConnection.Cancel ();
			else
				this.webView.LoadRequest (new NSUrlRequest (NSUrl.FromString ("about:blank")));

			this.webView = null;

			if (MainNavigationController.Instance.PresentedViewController != null)
				MainNavigationController.Instance.PresentedViewController.DismissViewController (true, action);
			else
				MainNavigationController.Instance.DismissViewController (true, action);
		}

		public class CustomUIWebViewDelegate : UIWebViewDelegate
		{
			private WebViewController controller;

			public CustomUIWebViewDelegate (WebViewController controller) : base () {
				this.controller = controller;
			}

			public override bool ShouldStartLoad (UIWebView webView, NSUrlRequest request, UIWebViewNavigationType navigationType) {
				if (request.Url.AbsoluteString.Equals ("about:blank"))
					return true;

				if (!this.controller.Authenticated) {
					this.controller.UrlConnection = NSUrlConnection.FromRequest (request, new CustomNSUrlConnectionDataDelegate (this.controller));
					return false;
				}

				if (request.Url.Scheme.Equals ("mobilexpense")) {
					this.controller.StopWebView (() => ((AppDelegate)UIApplication.SharedApplication.Delegate).StatesMediator.ChangeState (request.Url.ToString ()));
					return false;
				}

				return true;
			}
		}

		public class CustomNSUrlConnectionDataDelegate : NSUrlConnectionDataDelegate
		{
			private WebViewController controller;

			public CustomNSUrlConnectionDataDelegate (WebViewController controller) : base () {
				this.controller = controller;
			}

			public override void ReceivedAuthenticationChallenge (NSUrlConnection connection, NSUrlAuthenticationChallenge challenge) {
				UIAlertController alert = UIAlertController.Create ("Authentication Required", challenge.ProtectionSpace.Host, UIAlertControllerStyle.Alert);

				alert.AddAction (UIAlertAction.Create ("Log In", UIAlertActionStyle.Default, action => {
					NSUrlCredential credential = new NSUrlCredential (alert.TextFields[0].Text, alert.TextFields[1].Text, NSUrlCredentialPersistence.ForSession);

					NSUrlCredentialStorage.SharedCredentialStorage.SetDefaultCredential (credential, challenge.ProtectionSpace);

					challenge.Sender.UseCredential (credential, challenge);

					this.controller.Authenticated = true;
				}));
				alert.AddAction (UIAlertAction.Create ("Cancel", UIAlertActionStyle.Cancel, action => {
					challenge.Sender.CancelAuthenticationChallenge (challenge);
					this.controller.CancelWebView ();
				}));
				alert.AddTextField (field => field.Placeholder = "username");
				alert.AddTextField (field => {
					field.Placeholder = "password";
					field.SecureTextEntry = true;
				});

				this.controller.PresentViewController (alert, animated: true, completionHandler: null);
			}

			public override void ReceivedResponse (NSUrlConnection connection, NSUrlResponse response) {
				this.controller.UrlConnection = null;

				this.controller.Authenticated = true;

				this.controller.webView.LoadRequest (this.controller.UrlRequest);
			}

			public override bool ConnectionShouldUseCredentialStorage (NSUrlConnection connection) {
				return false;
			}
		}
	}
}