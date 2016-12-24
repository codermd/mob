
using System;
using CoreGraphics;

using Foundation;
using UIKit;
using Mxp.Core.Business;
using System.Collections.ObjectModel;
using Mxp.Core.Utils;
using System.Collections.Generic;

namespace Mxp.iOS
{
	public partial class TravelViewController : MXPViewController
	{

		public Travel Travel;

		private UIImage _actionImage;
		private UIImage ActionImage {
			get {
				if (this._actionImage == null)
					this._actionImage = UIImage.FromFile ("navbar_overflow.png");

				return this._actionImage;
			}
		}

		public TravelViewController () : base ("TravelViewController", null)
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

			this.Title = Labels.GetLoggedUserLabel (Labels.LabelEnum.Travel);

			this.EdgesForExtendedLayout = UIRectEdge.None;
			this.AutomaticallyAdjustsScrollViewInsets = false;
		}

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);

			this.SegmentView.RemoveAllSegments ();

			this.SegmentView.InsertSegment (Labels.GetLoggedUserLabel (Labels.LabelEnum.Details), 0, false);
			this.SegmentView.InsertSegment (Labels.GetLoggedUserLabel (Labels.LabelEnum.Flight), 1, false);
			this.SegmentView.InsertSegment (Labels.GetLoggedUserLabel (Labels.LabelEnum.Stay), 2, false);
			this.SegmentView.InsertSegment (Labels.GetLoggedUserLabel (Labels.LabelEnum.CarRental), 3, false);

			this.SegmentView.SelectedSegment = 0;
			this.TableView.Source = new SectionedFieldsSource (this.Travel.GetMainFields (), this);

			this.TableView.ReloadData ();


			this.NavigationItem.SetRightBarButtonItem (new UIBarButtonItem (this.ActionImage, UIBarButtonItemStyle.Plain, (sender, args) => {
				this.showAction ();
			}), true);

		}

		public void showAction(){

			List<Actionable> actions = new List<Actionable> ();
			actions.Add (new Actionable (LoggedUser.Instance.Labels.GetLabel (Labels.LabelEnum.Accept), () => {
				this.Travel.Approval.AcceptedStatus = true;
				this.showApprovalSubmitView ();
			}));

			actions.Add (new Actionable (LoggedUser.Instance.Labels.GetLabel (Labels.LabelEnum.Reject), () => {
				this.Travel.Approval.AcceptedStatus = false;
				this.showApprovalSubmitView ();
			}));
		

			Actionables actionables = new Actionables (Labels.GetLoggedUserLabel (Labels.LabelEnum.Actions), actions);
			new ActionnablesWrapper (actionables, this, this.NavigationItem.RightBarButtonItem).show();



//			if (UIDevice.CurrentDevice.CheckSystemVersion (7, 0)) {
//				UIActionSheet actionSheet = new UIActionSheet ("Actions");
//
//				actionSheet.AddButton (LoggedUser.Instance.Labels.GetLabel (Labels.LabelId.Accept));
//				actionSheet.AddButton (LoggedUser.Instance.Labels.GetLabel (Labels.LabelId.Reject));
//				actionSheet.AddButton (LoggedUser.Instance.Labels.GetLabel (Labels.LabelId.Cancel));
//
//				actionSheet.Clicked += (sen, e) => {
//					if (LoggedUser.Instance.Labels.GetLabel (Labels.LabelId.Cancel).Equals (actionSheet.ButtonTitle (e.ButtonIndex))) {
//						return;
//					}
//
//					if (LoggedUser.Instance.Labels.GetLabel (Labels.LabelId.Accept).Equals (actionSheet.ButtonTitle (e.ButtonIndex))) {
//						this.Travel.Approval.AcceptedStatus = true;
//						this.showApprovalSubmitView ();
//					}
//
//					if (LoggedUser.Instance.Labels.GetLabel (Labels.LabelId.Reject).Equals (actionSheet.ButtonTitle (e.ButtonIndex))) {
//						this.Travel.Approval.AcceptedStatus = false;
//						this.showApprovalSubmitView ();
//					}
//				};
//				actionSheet.ShowInView (this.View.Window);
//
//			} else {
//
//				UIAlertController alert = UIAlertController.Create ("Actions", "", UIAlertControllerStyle.ActionSheet);
//
//				alert.AddAction (UIAlertAction.Create (LoggedUser.Instance.Labels.GetLabel (Labels.LabelId.Accept), UIAlertActionStyle.Default, (sender) => {
//					this.Travel.Approval.AcceptedStatus = true;
//					this.showApprovalSubmitView ();
//				}));
//				
//				alert.AddAction (UIAlertAction.Create (LoggedUser.Instance.Labels.GetLabel (Labels.LabelId.Reject), UIAlertActionStyle.Destructive, (sender) => {
//					this.Travel.Approval.AcceptedStatus = false;
//					this.showApprovalSubmitView ();
//				}));
//
//				alert.AddAction (UIAlertAction.Create (LoggedUser.Instance.Labels.GetLabel (Labels.LabelId.Cancel), UIAlertActionStyle.Cancel, null));
//				this.HidesBottomBarWhenPushed = true;
//
//				this.ParentViewController.PresentViewController (alert, true, null);
//			}
		}

		public void showApprovalSubmitView() {
			ApprovalTravelComment vc = new ApprovalTravelComment();
			vc.approval = this.Travel.Approval;
			vc.ModalPresentationStyle = UIModalPresentationStyle.FormSheet;
			this.PresentViewController (vc, true, () => this.NavigationController.PopViewController (true));
		}

		partial void ClickOnSegment (NSObject sender)
		{
			switch(this.SegmentView.SelectedSegment) {
			case 0: 
				this.TableView.Source = new SectionedFieldsSource (this.Travel.GetMainFields (), this);
				break;
			case 1: 
				this.TableView.Source = new SectionedFieldsSource (this.Travel.GetFlightsFields (), this);
				break;
			case 2: 
				this.TableView.Source = new SectionedFieldsSource (this.Travel.GetStayFields (), this);
				break;
			case 3: 
				this.TableView.Source = new SectionedFieldsSource (this.Travel.GetCarRentalsFields (), this);
				break;

			}
			this.TableView.ReloadData();

		}
	}
}

