using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;
using System.Globalization;
using System.Diagnostics;
using Mxp.Core.Business;
using Xamarin.Forms;
using System.Runtime.InteropServices;
using ObjCRuntime;
using Xamarin;
using Mxp.Core.Services;
using Mxp.iOS.Helpers;
using mxp;
using System.ComponentModel;

namespace Mxp.iOS
{
	[Register ("AppDelegate")]
	public partial class AppDelegate : UIApplicationDelegate
	{
		public override UIWindow Window { get; set;	}
		public StatesMediator StatesMediator { get; set; }

		private NSUrl url;
		private bool IsLaunched = false;

		public override bool FinishedLaunching (UIApplication application, NSDictionary launchOptions) {
			Forms.Init ();

			#if ENABLE_TEST_CLOUD
				Xamarin.Calabash.Start();
			#endif
									
			UIStoryboard board = null;

			if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad)
				board = UIStoryboard.FromName ("IPadMainStoryBoard", NSBundle.MainBundle);
			else
				board = UIStoryboard.FromName ("MainStoryBoard", NSBundle.MainBundle);

			UIViewController vc = board.InstantiateInitialViewController() as UIViewController;

			this.Window = new UIWindow (UIScreen.MainScreen.Bounds);
			this.Window.RootViewController = vc;
			this.Window.MakeKeyAndVisible ();

			this.StatesMediator = new StatesMediator (new CommandsFactory (MainNavigationController.Instance), MainNavigationController.Instance);

			return true;
		}
	
		public override void OnActivated (UIApplication application) {
			if (!this.IsLaunched || this.url != null) {
				MainNavigationController.Instance.View.BackgroundColor = UIColor.White;

				this.StatesMediator.ChangeState (this.url?.ToString ());

				LoggedUser.Instance.PropertyChanged += LoggedUser_Instance_PropertyChanged;

				this.url = null;
			}

			this.IsLaunched = true;
		}

		private void LoggedUser_Instance_PropertyChanged (object sender, PropertyChangedEventArgs e) {
			if (e.PropertyName.Equals ("logout")) {
				new SharedDataController ().Remove ();
				((LoggedUser) sender).PropertyChanged -= LoggedUser_Instance_PropertyChanged;
				LoggedUser.Instance.PropertyChanged += LoggedUser_Instance_PropertyChanged;
			}

			if (e.PropertyName.Equals ("RefreshCacheAsync"))
				new SharedDataController ().Save ();
		}

		public override bool OpenUrl (UIApplication application, NSUrl url, string sourceApplication, NSObject annotation) {
			if (!(MainNavigationController.Instance.TopViewController is LoginViewController) && !SchemeActionStrategy.IsHome (url.ToString ()))
				MainNavigationController.Instance.PopToRootViewController (false);

			this.url = url;
			
			return true;
		}

        [Export("addImageToDevice:")]
        public NSString AddImageToDevice(NSString value)
        {
            var image = UIImage.FromBundle("icon/1152-1920");

            image.SaveToPhotosAlbum(null);

            return value;
        }
	}
}