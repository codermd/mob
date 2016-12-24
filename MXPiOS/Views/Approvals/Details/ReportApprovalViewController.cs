
using System;
using CoreGraphics;

using Foundation;
using UIKit;
using Mxp.Core.Business;

namespace Mxp.iOS
{
	public partial class ReportApprovalViewController : MXPViewController
	{


		public event EventHandler HasApprove = delegate {};
		public event EventHandler HasCancel = delegate {};

		public ReportApproval approval;

		public ReportApprovalViewController () : base ("ReportApprovalViewController", null)
		{
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

		}

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);

//			this.EdgesForExtendedLayout = UIRectEdge.None;
//			this.NavigationController.SetNavigationBarHidden (true, false);
			this.TitleLabel.Text = Labels.GetLoggedUserLabel (Labels.LabelEnum.ApprovalReports);
			this.AcceptedLabel.Text = Labels.GetLoggedUserLabel (Labels.LabelEnum.Accepted);
			this.AcceptedNumbr.TextColor = UIColor.FromRGB (2,193,118);
			this.RejectedLabel.Text = Labels.GetLoggedUserLabel (Labels.LabelEnum.Rejected);
			this.RejectedNumber.TextColor = UIColor.FromRGB (243,81,85);
			this.CommentLabel.Text = Labels.GetLoggedUserLabel (Labels.LabelEnum.Comment);

			if (Preferences.Instance.REPApprovalRejectionComment == PermissionEnum.Mandatory) {
				this.CommentLabel.Text = Labels.GetLoggedUserLabel (Labels.LabelEnum.Comment) + " (" + Labels.GetLoggedUserLabel (Labels.LabelEnum.Mandatory) +")";
			}

			this.AcceptButton.SetTitle (Labels.GetLoggedUserLabel (Labels.LabelEnum.Approve), UIControlState.Normal);
			this.AcceptButtonBackground.Layer.CornerRadius = 4;
			this.CancelButton.SetTitle (Labels.GetLoggedUserLabel (Labels.LabelEnum.Cancel), UIControlState.Normal);

			this.CommentTextView.Text = this.approval.Comment;
			this.CommentTextView.Layer.BorderWidth = 1;
			this.CommentTextView.Layer.BorderColor = UIColor.FromRGB (200, 200, 200).CGColor;
			this.CommentTextView.Layer.CornerRadius = 4;

			DoneToolBar tb = new DoneToolBar ();
			tb.Frame = new CGRect (0, 0, 320, 44);
			tb.ClickOnButton += (sender, e) => {
				CommentTextView.ResignFirstResponder();
			};
			this.CommentTextView.InputAccessoryView = tb;

			this.AcceptedNumbr.Text = this.approval.VNumberOfAccepted.ToString ();
			this.RejectedNumber.Text = this.approval.VNumberOfRejected.ToString ();

			_hidenotification = UIKeyboard.Notifications.ObserveDidHide(HideCallback);
			_shownotification = UIKeyboard.Notifications.ObserveWillShow(ShowCallback);

		}

		NSObject _shownotification;
		NSObject _hidenotification;
		void ShowCallback (object obj,  UIKit.UIKeyboardEventArgs args)
		{
			this.ScrollView.SetContentOffset (new CGPoint(0.0f,this.CommentLabel.Frame.Y) ,  true);
			this.ScrollView.ContentInset = new UIEdgeInsets (0.0f, 0.0f, args.FrameEnd.Size.Height, 0.0f);
		}

		void HideCallback (object obj,  UIKit.UIKeyboardEventArgs args)
		{
			this.ScrollView.ContentInset = new UIEdgeInsets (0.0f, 0.0f, 0.0f, 0.0f);
		}

		public override void ViewWillDisappear (bool animated)
		{
			base.ViewWillDisappear (animated);


			if(this.TabBarController != null) 
				this.TabBarController.HidesBottomBarWhenPushed = false;

			if (_shownotification != null)
				_shownotification.Dispose();
			if (_hidenotification != null)
				_hidenotification.Dispose();
		}

		partial void ClickOnAccept (NSObject sender)
		{
			this.processAccepting();
		}

		partial void ClickOnCancel (NSObject sender)
		{
			this.approval.Comment = null;
		
			this.HasCancel (null, null);
		}

		public void processAccepting(){
			this.approval.Comment = this.CommentTextView.Text;

			this.HasApprove (null, null);
		}
	}
}
