using System;
using UIKit;
using Mxp.Core.Business;
using Foundation;
using Mxp.Core;
using ObjCRuntime;

namespace Mxp.iOS
{
	public class MXPViewController : UIViewController
	{
		public MXPViewController (string nibName, NSBundle bundle) : base(nibName, bundle) {
		}

		public MXPViewController (IntPtr ptr) : base (ptr) {
		}

		public override void ViewWillAppear (bool animated) {
			base.ViewWillAppear (animated);

			TrackContext.AddView (this.GetType ().ToString ());
		}

		public bool IsModal {
			get {
				return this.PresentingViewController?.PresentedViewController == this
					|| this.NavigationController?.PresentingViewController?.PresentedViewController == this.NavigationController
					|| this.TabBarController?.PresentingViewController is UITabBarController;
			}
		}

		public void Close () {
			if (this.IsModal)
				this.NavigationController.DismissViewController (true, null);
			else
				this.NavigationController.PopViewController (true);
		}
	}

	public class MXPTableViewController : UITableViewController
	{
		public MXPTableViewController () {
		}

		public MXPTableViewController (string nibName, NSBundle bundle) : base(nibName, bundle) {
		}

		public MXPTableViewController (UITableViewStyle withStyle) : base (withStyle) {
		}

		public MXPTableViewController (IntPtr ptr) : base (ptr) {
		}

		public override void ViewWillAppear (bool animated) {
			base.ViewWillAppear (animated);

			TrackContext.AddView (this.GetType ().ToString ());
		}

		public bool IsModal {
			get {
				return this.PresentingViewController?.PresentedViewController == this
					|| this.NavigationController?.PresentingViewController?.PresentedViewController == this.NavigationController
					|| this.TabBarController?.PresentingViewController is UITabBarController;
			}
		}

		public void Close () {
			if (this.IsModal)
				this.NavigationController.DismissViewController (true, null);
			else
				this.NavigationController.PopViewController (true);
		}
	}

	public class MXPSplitViewController : UISplitViewController
	{
		protected UIViewController listViewController;

		public MXPSplitViewController (IntPtr handle) : base (handle) {
			this.Delegate = new SplitViewDelegate ();
		}

		public class SplitViewDelegate : UISplitViewControllerDelegate
		{
			public override bool ShouldHideViewController (UISplitViewController svc, UIViewController viewController, UIInterfaceOrientation inOrientation) {
				return false;
			}
		}

		public override void ViewDidLoad () {
			base.ViewDidLoad ();

			this.listViewController = ((UINavigationController)this.ViewControllers [0]).ViewControllers[0];

			if (this.RespondsToSelector (new Selector ("setPreferredDisplayMode:")))
				this.PreferredDisplayMode = UISplitViewControllerDisplayMode.AllVisible;
		}

		public void ShouldShowDetailViewController (UIViewController vc, NSObject sender) {
			var nvc = new UINavigationController (vc);
			nvc.ToolbarHidden = true;

			if (this.RespondsToSelector (new Selector ("showDetailViewController:")))
				base.ShowDetailViewController (nvc, sender);
			else {
				this.ViewControllers = new UIViewController[] {
					this.ViewControllers [0],
					nvc
				};
			}
		}
	}
}