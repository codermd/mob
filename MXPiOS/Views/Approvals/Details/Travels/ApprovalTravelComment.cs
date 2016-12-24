
using System;
using CoreGraphics;

using Foundation;
using UIKit;
using Mxp.Core.Business;
using System.Threading.Tasks;
using Mxp.Core;

namespace Mxp.iOS
{
	public partial class ApprovalTravelComment : MXPViewController
	{
		public ApprovalTravelComment () : base ("ApprovalTravelComment", null)
		{
		}

		public bool icCanceled;

		public TravelApproval approval;

		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();
			// Release any cached data, images, etc that aren't in use.
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			this.CancelButton.SetTitle (Labels.GetLoggedUserLabel (Labels.LabelEnum.Cancel), UIControlState.Normal);

			DoneToolBar tb = new DoneToolBar();
			tb.Frame = new CGRect(0,0, 320, 44);
			tb.ClickOnButton += (sender, e) => {
				this.TextView.ResignFirstResponder();
			};
			TextView.InputAccessoryView = tb;

//			this.EdgesForExtendedLayout = UIRectEdge.None;
			this.icCanceled = true;


			this.TextView.Layer.BorderColor = UIColor.LightGray.CGColor;
			this.TextView.Layer.BorderWidth = 0.5f;

		}

		public override void ViewWillAppear (bool animated)
		{

			base.ViewWillAppear (animated);
			this.Title = this.approval.Travel.Name;
			this.refreshView ();

//			this.NavigationController.SetNavigationBarHidden(true, true);

			_hidenotification = UIKeyboard.Notifications.ObserveDidHide(HideCallback);
			_shownotification = UIKeyboard.Notifications.ObserveWillShow(ShowCallback);

		}


		NSObject _shownotification;
		NSObject _hidenotification;

		void ShowCallback (object obj,  UIKit.UIKeyboardEventArgs args)
		{
			this.ScrollView.ContentInset = new UIEdgeInsets (0.0f, 0.0f, args.FrameEnd.Size.Height, 0.0f);
		}

		void HideCallback (object obj,  UIKit.UIKeyboardEventArgs args)
		{
			this.ScrollView.ContentInset = new UIEdgeInsets (0.0f, 0.0f, 0.0f, 0.0f);
		}

		public override void ViewWillDisappear (bool animated)
		{
			base.ViewWillDisappear (animated);

			if (_shownotification != null)
				_shownotification.Dispose();
			if (_hidenotification != null)
				_hidenotification.Dispose();

		}
		public override void ViewDidAppear (bool animated)
		{
			base.ViewDidAppear (animated);
			this.TextView.BecomeFirstResponder ();
		}
		public void refreshView(){
			if (this.approval.AcceptedStatus) {
				this.ProcessButton.SetTitle (Labels.GetLoggedUserLabel (Labels.LabelEnum.Accept), UIControlState.Normal);
			} else {
				this.ProcessButton.SetTitle (Labels.GetLoggedUserLabel (Labels.LabelEnum.Reject), UIControlState.Normal);
			}
		}

		partial void ClickOnCancel (NSObject sender)
		{
			this.approval.Comment = null;
			this.icCanceled = true;
			this.DismissViewController(true, null);
		}

		partial void ClickOnProcess (NSObject sender)
		{
			this.icCanceled = false;
			this.saveProcess();
		}

		public async void saveProcess(){
			this.approval.Comment = TextView.Text;

			LoadingView.showMessage(Labels.GetLoggedUserLabel (Labels.LabelEnum.Submitting) + "...");

			try {
				await this.approval.SubmitAsync ();
			} catch (Exception error) {
				MainNavigationController.Instance.showError (error);
				return;
			} finally {
				LoadingView.hideMessage ();
			}

			this.DismissViewController(true, null);
		}
	}
}

