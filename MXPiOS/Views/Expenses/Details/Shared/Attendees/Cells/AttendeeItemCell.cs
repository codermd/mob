using System;
using Foundation;
using UIKit;
using Mxp.Core.Business;
using Mxp.iOS;
using System.Diagnostics;

namespace MXPiOS
{
	public partial class AttendeeItemCell : UITableViewCell
	{
		public static readonly NSString Key = new NSString ("AttendeeItemCell");
		public static readonly UINib Nib;

		public event EventHandler ShowAttendee;

		private Attendee attendee;

		static AttendeeItemCell () {
			Nib = UINib.FromName ("AttendeeItemCell", NSBundle.MainBundle);
		}

		public AttendeeItemCell (IntPtr handle) : base (handle) {
			
		}

		public static AttendeeItemCell Create () {
			AttendeeItemCell attendeeItemCell = (AttendeeItemCell)Nib.Instantiate (null, null) [0];


			if (!Preferences.Instance.IsGTPEnabled) {
				//WARNING dangerous if not managed by NSLayoutConstraint... (FIX)
				//What about if some cells are IsGTPEnable and other not? (in the future)
				attendeeItemCell.ShownButton.RemoveFromSuperview ();
			}

			return attendeeItemCell;
		}

		public void configure (Attendee attendee) {
			this.attendee = attendee;

			this.ShownButton.Selected = attendee.IsShown;

			this.TitleLabel.Text = attendee.VName;
			this.PriceLabel.Text = attendee.VAmount;
			this.TypeLabel.Text = attendee.Type.ToString ();
		}

		bool cellClickable = true;

		public override void WillTransitionToState (UITableViewCellState mask)
		{
			base.WillTransitionToState (mask);
			Debug.WriteLine ("Will transite to " + mask.ToString ());
			this.cellClickable = false;
		}

		public override void DidTransitionToState (UITableViewCellState mask)
		{
			base.DidTransitionToState (mask);
			Debug.WriteLine ("did transite to " + mask.ToString ());
			if (mask == UITableViewCellState.ShowingDeleteConfirmationMask) {
				this.cellClickable = false;
			}
			if (mask == UITableViewCellState.DefaultMask) {
				this.cellClickable = true;
			}
		}

		async partial void ClickOnAttendee (UIButton sender) {
			sender.Selected = !sender.Selected;

			try {
				await this.attendee.ChangeIsShownAsync ();
			} catch (Exception e) {
				sender.Selected = !sender.Selected;
				MainNavigationController.Instance.showError (e);
			}
		}

		partial void ClickOnCell (NSObject sender) {
			if(this.cellClickable) {
				this.ShowAttendee(this.attendee, EventArgs.Empty);	
			}

		}
	}
}