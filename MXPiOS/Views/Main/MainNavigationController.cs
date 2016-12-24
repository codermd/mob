using System;
using Mxp.Core.Business;
using UIKit;
using Mxp.Core;
using System.ComponentModel;
using Mxp.Core.Services;

namespace Mxp.iOS
{
	partial class MainNavigationController : UINavigationController, StatesMediator.IStateListener
	{
		public static MainNavigationController Instance {
			get {
				return UIApplication.SharedApplication.Windows [0].RootViewController as MainNavigationController;
			}
		}

		private LoadingView loadingView;

		public MainNavigationController (IntPtr handle) : base (handle) {
			
		}

		public override void ViewWillAppear (bool animated) {
			base.ViewWillAppear (animated);

			LoggedUser.Instance.AlertMessageChanged += HandleAlertMessageChangedEvent;
			LoggedUser.Instance.PropertyChanged += HandleLoggedUserPropertyChangedEvent;
		}

		public override void ViewWillDisappear (bool animated) {
			LoggedUser.Instance.AlertMessageChanged -= HandleAlertMessageChangedEvent;
			LoggedUser.Instance.PropertyChanged -= HandleLoggedUserPropertyChangedEvent;

			base.ViewWillDisappear (animated);
		}

		private void HandleLoggedUserPropertyChangedEvent (object sender, PropertyChangedEventArgs e) {
			if (e.PropertyName.Equals ("logout")) {
				((LoggedUser)sender).PropertyChanged -= HandleLoggedUserPropertyChangedEvent;
				LoggedUser.Instance.PropertyChanged += HandleLoggedUserPropertyChangedEvent;

				((LoggedUser)sender).AlertMessageChanged -= HandleAlertMessageChangedEvent;
				LoggedUser.Instance.AlertMessageChanged += HandleAlertMessageChangedEvent;
			}
		}

		public void HandleAlertMessageChangedEvent (object sender, AlertMessage.AlertMessageEventArgs e) {
			LoadingView.hideMessage ();

			switch (e.AlertMessage.MessageType) {
				case AlertMessage.MessageTypeEnum.StartLoading:
					LoadingView.showMessage (e.AlertMessage.Title, e.AlertMessage.Message);
					break;
				case AlertMessage.MessageTypeEnum.Error:
					this.showError(e.AlertMessage.Message, e.AlertMessage.Title);
					break;
			}
		}

		private UIAlertView alertView;
		public void showLoadingView(string message, string content = null) {
			if (!UIDevice.CurrentDevice.CheckSystemVersion (8, 0) && UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad) {
				if (this.alertView != null) {
					this.alertView.DismissWithClickedButtonIndex (0, true);
					this.alertView = null;
				}
				this.alertView = new UIAlertView (message, content, null, null, null);
				alertView.Show ();
			} else {
				if (this.loadingView == null) {
					this.loadingView = new LoadingView ();
					this.loadingView.View.Frame = this.View.Bounds;
				} else {
					if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad) {
						this.loadingView.DismissViewController (false, () => {
							this.loadingView = null;
							this.showLoadingView (message);
						});
						return;
					}
				}

				this.loadingView.Message = message;

				if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad) {
					if (this.alertView != null)
					{
						this.alertView.DismissWithClickedButtonIndex(0, true);
						this.alertView = null;
					}
					this.alertView = new UIAlertView(message, null, null, null, null);
					alertView.Show();
				} else {
					if (this.PresentedViewController != null) {
						this.loadingView.ModalPresentationStyle = UIModalPresentationStyle.Custom;
						this.loadingView.ModalTransitionStyle = UIModalTransitionStyle.CrossDissolve;
						this.PresentedViewController.PresentViewController (loadingView, false, null);
					} else {
						this.AddChildViewController (loadingView);
						this.View.AddSubview (loadingView.View);
						this.loadingView.DidMoveToParentViewController (this);
					}
				}
			}
		}

		public void showLoadingView () {
			this.showLoadingView ("Loading...");
		}

		public void hideLoadingView (bool animated) {
			if (!UIDevice.CurrentDevice.CheckSystemVersion (8, 0) && UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad) {
				if (this.alertView != null) {
					this.alertView.DismissWithClickedButtonIndex (0, true);
					this.alertView = null;
				}
			} else {
				if (this.loadingView == null)
					return;

				if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad) {
					this.alertView.DismissWithClickedButtonIndex(0, true);
					this.alertView = null;
				} else {
					this.loadingView.Message = null;

					if (this.PresentedViewController != null) {
						loadingView.DismissViewController (false, null);
					} else {
						this.loadingView.WillMoveToParentViewController (null);
						this.loadingView.View.RemoveFromSuperview ();
						this.loadingView.RemoveFromParentViewController ();
					}
				}

				this.loadingView = null;
			}
		}

		public void showError(string errorMessage, string title = "Error")
		{
			UIAlertView alert = new UIAlertView()
			{
				Title = title ?? "Error",
				Message = errorMessage
			};
			alert.AddButton("OK");
			alert.Show();
		}

		public void showError (ValidationError error) {
			this.showError(error.Verbose);
		}

		public void showError (Exception error) {
			if (error is ValidationError) {
				this.showError (error as ValidationError);
				return;
			}

			#if DEBUG
				this.showError(error.StackTrace, error.Message);
			#else
				this.showError (Service.NoConnectionError);
			#endif
		}

		#region StatesMediator.IStateListener

		public void RedirectToLoginByEmail (string redirection) {
			WebViewController webViewController = new WebViewController (redirection);
			UINavigationController nvc = new UINavigationController (webViewController);
			nvc.ModalPresentationStyle = UIModalPresentationStyle.FormSheet;

			if (this.PresentedViewController != null)
				this.PresentedViewController.PresentViewController (nvc , true, null);
			else
				this.PresentViewController (nvc , true, null);
		}

		public void RedirectToMainView () {
			if (!(MainNavigationController.Instance.TopViewController is MainTabBarControllerView))
				this.PushViewController (this.Storyboard.InstantiateViewController ("MainTabBar"), true);
		}

		public void RedirectToLoginView (ValidationError error = null) {
			if (!(MainNavigationController.Instance.TopViewController is LoginViewController))
				this.PushViewController (this.Storyboard.InstantiateViewController ("LoginViewController"), true);

			if (error != null)
				MainNavigationController.Instance.showError (error);
		}

		#endregion

		public override UIViewController TopViewController {
			get {
				if (base.TopViewController is LoadingView)
					return this.ViewControllers [this.ViewControllers.Length - 2];
				else
					return base.TopViewController;
			}
		}
	}
}