using System;

using Foundation;
using UIKit;
using Mxp.Core.Business;
using System.Diagnostics;
using System.Threading.Tasks;
using Mxp.iOS.Helpers;

namespace Mxp.iOS
{
	public partial class LoginViewController : MXPViewController
	{
		public OpenObject OpenObject { get; set; }

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);

			this.NavigationController?.SetNavigationBarHidden (true, false);

			this.userLoginView.Layer.CornerRadius = 5;
			this.companyLoginView.Layer.CornerRadius = 5;

			this.ButtonLogin.Layer.CornerRadius = 4;
			this.ButtonLogin.Layer.BorderWidth = 1;
			this.ButtonLogin.Layer.BorderColor = UIColor.FromRGB (0, 134, 158).CGColor;

			this.CompanyButtonLogin.Layer.CornerRadius = 4;
			this.CompanyButtonLogin.Layer.BorderWidth = 1;
			this.CompanyButtonLogin.Layer.BorderColor = UIColor.FromRGB (0, 134, 158).CGColor;

			_hidenotification = UIKeyboard.Notifications.ObserveDidHide(HideCallback);
			_shownotification = UIKeyboard.Notifications.ObserveWillShow(ShowCallback);
		}

		public void ResetFields () {
			this.usernameField.Text = null;
			this.passwordField.Text = null;
			this.rememberMeSwitch.On = false;
			this.emailField.Text = null;
			this.companyRememberMeSwitch.On = false;

			this.segmentedControl.SelectedSegment = 0;
			this.userLoginView.Hidden = false;
			this.companyLoginView.Hidden = true;
		}

		NSObject _shownotification;
		NSObject _hidenotification;

		void ShowCallback (object obj,  UIKit.UIKeyboardEventArgs args)
		{
			if (!UIDevice.CurrentDevice.CheckSystemVersion (8, 0) && UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad)
				this.ScrollView.ContentInset = new UIEdgeInsets (0.0f, 0.0f, args.FrameEnd.Size.Height / 2, 0.0f);
			else
				this.ScrollView.ContentInset = new UIEdgeInsets (0.0f, 0.0f, args.FrameEnd.Size.Height, 0.0f);
		}

		void HideCallback (object obj,  UIKit.UIKeyboardEventArgs args)
		{
			this.ScrollView.ContentInset = new UIEdgeInsets (0.0f, 0.0f, 0.0f, 0.0f);
		}

		public override void ViewWillDisappear (bool animated)
		{
			base.ViewWillDisappear (animated);

			// Unregister the callbacks
			if (_shownotification != null)
				_shownotification.Dispose();
			if (_hidenotification != null)
				_hidenotification.Dispose();
		}

		public override void ViewDidLoad () {
			base.ViewDidLoad ();

			this.usernameField.ShouldReturn += (sender) => {
				this.passwordField.BecomeFirstResponder();
				return false;
			};
			this.passwordField.ShouldReturn += (sender) => {
				this.passwordField.ResignFirstResponder();
				return true;
			};
			this.emailField.ShouldReturn += (sender) => {
				this.emailField.ResignFirstResponder();
				return true;
			};

			if (!String.IsNullOrWhiteSpace (LoggedUser.Instance.Username)) {
				this.usernameField.Text = LoggedUser.Instance.VUsername;
				this.passwordField.Text = LoggedUser.Instance.Password;
				this.rememberMeSwitch.On = LoggedUser.Instance.AutoLogin;
			} else if (LoggedUser.Instance.CanLoginByMail) {
				this.emailField.Text = LoggedUser.Instance.VEmail;
				this.companyRememberMeSwitch.On = LoggedUser.Instance.AutoLogin;

				this.segmentedControl.SelectedSegment = 1;
				this.userLoginView.Hidden = true;
				this.companyLoginView.Hidden = false;
			}
		}

		public override UIStatusBarStyle PreferredStatusBarStyle ()
		{
			return UIStatusBarStyle.LightContent;
		}

		public LoginViewController (IntPtr handle) : base (handle) {
		}

		partial void clickOnLogin (UIButton sender)
		{
			this.processLogin();
		}

		partial void clickOnLoginThroughCompany (UIButton sender) {
			this.ProcessLoginThroughCompanyAsync ();
		}

		public async void ProcessLoginThroughCompanyAsync () {
			LoggedUser.Instance.Email = this.emailField.Text;

			LoggedUser.Instance.Username = null;
			LoggedUser.Instance.Password = null;

			this.emailField.ResignFirstResponder ();

			LoadingView.showMessage ("Loading...");

			string redirection;

			try {
				redirection = await LoggedUser.Instance.LoginByMailAsync ();
			} catch (Exception e) {
				LoadingView.hideMessage ();
				MainNavigationController.Instance.showError (e);
				return;
			}

			WebViewController webViewController = new WebViewController (redirection);
			UINavigationController nvc = new UINavigationController (webViewController);
			nvc.ModalPresentationStyle = UIModalPresentationStyle.FormSheet;

			if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad)
				LoadingView.hideMessage ();

			if (this.PresentedViewController != null)
				this.PresentedViewController.PresentViewController (nvc, true, null);
			else
				this.PresentViewController (nvc, true, null);
		}

		async public void processLogin ()
		{
			LoggedUser.Instance.Password = this.passwordField.Text;
			LoggedUser.Instance.Username = this.usernameField.Text;

			LoggedUser.Instance.Email = null;

			this.passwordField.ResignFirstResponder ();
			this.usernameField.ResignFirstResponder ();

			try {
				LoadingView.showMessage ("Authenticate...");
				await LoggedUser.Instance.LoginAsync();
				LoadingView.showMessage ("Loading...");
				await LoggedUser.Instance.RefreshCacheAsync();

				if (this.OpenObject != null) {
					OpenObjectCommand openObjectCommand = new OpenObjectCommand (this, this.OpenObject);
					await openObjectCommand.InvokeAsync ();
				} else {
					LoadingView.hideMessage ();
					UIViewController vc = this.Storyboard.InstantiateViewController("MainTabBar") as UIViewController;
					this.NavigationController.PushViewController(vc, true);
				}
			} catch (Exception e){
				LoadingView.hideMessage ();
				MainNavigationController.Instance.showError (e);
				return;
			}
		}
	
		partial void ChangeRememberPassword (UISwitch sender)
		{
			LoggedUser.Instance.AutoLogin = sender.On;
		}

		partial void ClickOnResetPassword (UIButton sender)
		{
			WebViewController webViewController = new WebViewController (LoggedUser.Instance.ForgotPasswordUrl);
			UINavigationController nvc = new UINavigationController (webViewController);
			nvc.ModalPresentationStyle = UIModalPresentationStyle.FormSheet;

			if (this.PresentedViewController != null)
				this.PresentedViewController.PresentViewController (nvc , true, null);
			else
				this.PresentViewController (nvc , true, null);
		}

		partial void SegmentedControlChanged (UISegmentedControl sender) {
			nint selectedSegment = sender.SelectedSegment;

			if (selectedSegment == 0) {
				this.userLoginView.Hidden = false;
				this.companyLoginView.Hidden = true;
			} else {
				this.userLoginView.Hidden = true;
				this.companyLoginView.Hidden = false;
			}
		}
	}
}
