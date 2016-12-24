
using System;
using CoreGraphics;

using Foundation;
using UIKit;
using Mxp.Core.Business;

namespace Mxp.iOS
{
	public partial class LoadingView : MXPViewController
	{
		public LoadingView () : base ("LoadingView", null)
		{
		}


		public static void showMessage(){
			MainNavigationController.Instance.showLoadingView("Loading...");
		}

		public static void showMessage(string message, string content = null){
			MainNavigationController.Instance.showLoadingView(message, content);
		}

		public static void hideMessage(){
			LoadingView.hideMessage(true);
		}

		public static void hideMessage(bool animated){
			
			MainNavigationController.Instance.hideLoadingView(animated);
		}


		private string _message;
		public string Message { 
			get {
				return this._message;
			} set {
				if (value == null) {
					this._message = Labels.GetLoggedUserLabel (Labels.LabelEnum.Loading);
				} else {
					this._message = value;
				}
				this.setTextMessage ();
			}
		}

		public void setTextMessage(){
			if(this.MessageLabel == null) {
				return;
			}
			this.MessageLabel.Text = this.Message;
		}

		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();
			
			// Release any cached data, images, etc that aren't in use.
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			this.Container.Layer.CornerRadius = 4;
			this.Container.Layer.BackgroundColor = UIColor.FromRGBA (90, 90, 90, 95).CGColor;
			this.LoadingText.TextColor = UIColor.FromRGB (255, 255, 255);
			if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad) {
				this.View.BackgroundColor = UIColor.Clear ;
			}
		}

		public override void WillMoveToParentViewController (UIViewController parent)
		{
			base.WillMoveToParentViewController (parent);
			this.Loader.StartAnimating ();
			this.setTextMessage ();
		}

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
			if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad) {
				this.Loader.StartAnimating ();
			}

		}

	}
}
