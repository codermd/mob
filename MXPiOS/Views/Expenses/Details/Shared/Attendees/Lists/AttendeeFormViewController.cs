using System;
using Foundation;
using UIKit;
using Mxp.Core.Business;

namespace Mxp.iOS
{
	public class AttendeeFormViewController : MXPViewController
	{
		public AttendeeFormViewController (String nibName, NSBundle bundle) : base(nibName, bundle)
		{
		}

		public Attendees Attendees;

		public override void SetEditing (bool editing, bool animated)
		{
			if (editing) {
				this.NavigationItem.SetHidesBackButton (true, animated);
				UIBarButtonItem saveButton = new UIBarButtonItem(LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.Save), UIBarButtonItemStyle.Done, (action, args)=>clickOnSave(action));

				UIBarButtonItem CancelButton = new UIBarButtonItem(LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.Cancel), UIBarButtonItemStyle.Done, (action, args)=>clickOnCancel(action));

				this.NavigationItem.SetRightBarButtonItem (saveButton, animated);

				this.NavigationItem.SetLeftBarButtonItem (CancelButton, animated);
			} else {
				if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad) {
					this.NavigationItem.SetLeftBarButtonItem (new UIBarButtonItem (Labels.GetLoggedUserLabel (Labels.LabelEnum.Close), UIBarButtonItemStyle.Done, (sender, e) => {
						this.DismissViewController (true, null);
					}), false);
				} else {
					this.NavigationItem.SetLeftBarButtonItems (new UIBarButtonItem [0], animated);
					this.NavigationItem.SetHidesBackButton (false, animated);
				}
				
				this.NavigationItem.SetRightBarButtonItems (new UIBarButtonItem[0], animated);
			}
		}

		public virtual void clickOnSave(object sender) {
			this.processSave ();
		}

		public async void processSave() {
			LoadingView.showMessage (Labels.GetLoggedUserLabel (Labels.LabelEnum.Add));

			Attendee attendee = this.prepareAttendee ();

			try {
				await this.Attendees.AddAsync (attendee);
			} catch(Exception error) {
				MainNavigationController.Instance.showError (error);
				return;
			} finally {
				LoadingView.hideMessage ();
			}

			if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad) {
				this.DismissViewController (true, null);
			} else {
				this.NavigationController.PopViewController (true);
			}
		}

		public override void ViewDidLoad () {
			base.ViewDidLoad ();

			this.EdgesForExtendedLayout = UIRectEdge.None;
		}

		public virtual Attendee prepareAttendee() {
			return null;
		}

		public virtual void clickOnCancel(object sender){
			this.SetEditing (false, true);
		}
			
	}
}

