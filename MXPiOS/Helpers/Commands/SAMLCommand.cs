using System;
using System.Threading.Tasks;
using Mxp.Core.Business;
using System.Collections.Generic;
using Mxp.Core.Utils;
using UIKit;
using System.Linq;

namespace Mxp.iOS.Helpers
{
	public class SAMLCommand : SAMLAbstractCommand
	{
		private UIViewController viewController;

		public SAMLCommand (UIViewController viewController, Uri uri) : base (uri) {
			this.viewController = viewController;
		}

		#region SAMLAbstractCommand

		public override void Parse (Uri uri) {
			Dictionary<String, String> parameters = HttpUtility.ParseQueryString (System.Net.WebUtility.UrlDecode (uri.Query));

			if (!parameters.ContainsKey ("MXPSessionSharedKey"))
				throw new ValidationError ("Error", "Wrong scheme");

			this.Token = parameters ["MXPSessionSharedKey"];
		}

		protected override async Task RedirectAsync () {
			if (this.NextCommand != null)
				await this.NextCommand.InvokeAsync ();
			else {
				LoadingView.hideMessage ();
				MainNavigationController.Instance.PushViewController (this.viewController.Storyboard.InstantiateViewController ("MainTabBar"), true);
			}
		}

		public override void RedirectToLoginView (ValidationError error = null) {
			if (this.NextCommand != null)
				this.NextCommand.RedirectToLoginView (error);
			else {
				LoginViewController loginViewController = MainNavigationController.Instance.TopViewController as LoginViewController;

				if (loginViewController == null) {
					loginViewController = this.viewController.Storyboard.InstantiateViewController ("LoginViewController") as LoginViewController;
					MainNavigationController.Instance.PushViewController (loginViewController, true);
				}
				
				if (error != null)
					MainNavigationController.Instance.showError (error);
			}
		}

		#endregion
	}
}