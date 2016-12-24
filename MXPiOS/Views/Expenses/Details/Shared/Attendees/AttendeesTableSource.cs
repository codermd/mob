using System;
using UIKit;
using Mxp.Core;
using Mxp.Core.Business;
using SDWebImage;
using Foundation;
using CoreGraphics;
using System.Drawing;
using MXPiOS;

namespace Mxp.iOS
{
	public class AttendeesTableSource : UITableViewSource
	{
		public event EventHandler spendCatcherSelected = delegate {};

		private UILabel title = new PaddingLabel ();

		private ExpenseItem expenseItem;
		private Expense Expense {
			get {
				return this.expenseItem.ParentExpense;
			}
		}
		private UIViewController parentViewController;

		public AttendeesTableSource (ExpenseItem expenseItem, UIViewController parentViewController) {
			this.expenseItem = expenseItem;
			this.parentViewController = parentViewController;

			title.Lines = 0;
			title.LineBreakMode = UILineBreakMode.WordWrap;
			title.SizeToFit ();
			title.TextColor = UIColor.LightGray;
			title.Font = UIFont.FromName ("Avenir", 15);
			title.BackgroundColor = UIColor.White;
		}

		public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath) {
			if (this.expenseItem.CanManageAttendees && indexPath.Row == this.expenseItem.Attendees.Count) {

				AddAttendeeButtonCell addButton =  (AddAttendeeButtonCell)tableView.DequeueReusableCell ("AddAttendeeButtonCell");
				if (addButton == null) {
					addButton = AddAttendeeButtonCell.Create ();
				}
				addButton.SelectionStyle = UITableViewCellSelectionStyle.None;

				return addButton;
			}

			AttendeeItemCell cell =  tableView.DequeueReusableCell (AttendeeItemCell.Key) as AttendeeItemCell;
			if (cell == null) {
				cell = AttendeeItemCell.Create ();
			}	

			cell.configure (this.expenseItem.Attendees [indexPath.Row]);
			cell.SelectionStyle = UITableViewCellSelectionStyle.None;

			cell.ShowAttendee += (object sender, EventArgs e) => {

				var vc = new AttendeeDetailsViewController(sender as Attendee);

				if(UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad) {
					var nvc = new UINavigationController(vc);
					vc.NavigationItem.SetLeftBarButtonItem (new UIBarButtonItem (Labels.GetLoggedUserLabel (Labels.LabelEnum.Close), UIBarButtonItemStyle.Plain, (resender, args) => {
						nvc.DismissViewControllerAsync (true);
					}), false);
					nvc.ModalPresentationStyle = UIModalPresentationStyle.FormSheet;
					this.parentViewController.PresentViewControllerAsync(nvc, true);
				} else {
					this.parentViewController.NavigationController.PushViewController(vc, true);	
				}

			};

			return cell;
		}

		public override UIView GetViewForHeader (UITableView tableView, nint section) {
			if (Preferences.Instance.IsGTPEnabled) {
				title.Text = Labels.GetLoggedUserLabel (Labels.LabelEnum.GTPHeaderMessage);
				return title;
			} else
				return null;
		}

		public override nfloat GetHeightForHeader (UITableView tableView, nint section) {
			if (Preferences.Instance.IsGTPEnabled) {
				CGSize maxHeight = new CGSize (tableView.Frame.Width, float.MaxValue);
				NSString nsstr = new NSString (Labels.GetLoggedUserLabel (Labels.LabelEnum.SpendCatcherHeaderMessage));
				CGSize size = nsstr.StringSize (title.Font, maxHeight, title.LineBreakMode);

				return size.Height;
			} else
				return (nfloat)0;
		}

		public override nint RowsInSection (UITableView tableview, nint section) {
			int count = this.expenseItem.Attendees.Count;
			return this.expenseItem.CanManageAttendees ? count + 1 : count;
		}

		public override UITableViewCellEditingStyle EditingStyleForRow (UITableView tableView, NSIndexPath indexPath) {
			if (this.expenseItem.CanManageAttendees) {
				if (indexPath.Row == this.expenseItem.Attendees.Count) {
					return UITableViewCellEditingStyle.None;
				}
				return UITableViewCellEditingStyle.Delete;
			} else {
				return UITableViewCellEditingStyle.None;
			}
		}

		public override void CommitEditingStyle (UITableView tableView, UITableViewCellEditingStyle editingStyle, NSIndexPath indexPath) {
			if (indexPath.Row != this.expenseItem.Attendees.Count) {
				this.processRemoveAttendee (this.expenseItem.Attendees [indexPath.Row], tableView);
			}
		}

		private bool IsRemovingItem = false;
		public async void processRemoveAttendee(Attendee attendee, UITableView tableView){
			if (this.IsRemovingItem)
				return;

			this.IsRemovingItem = true;

			LoadingView.showMessage (Labels.GetLoggedUserLabel (Labels.LabelEnum.Deleting) + "...");

			try {
				await this.expenseItem.Attendees.RemoveAsync(attendee);
			} catch(Exception e) {
				MainNavigationController.Instance.showError (e);
				return;
			} finally {
				LoadingView.hideMessage ();
				this.IsRemovingItem = false;
			}
		}

		public override nfloat GetHeightForRow (UITableView tableView, NSIndexPath indexPath) {
			if (indexPath.Row != this.expenseItem.Attendees.Count) {
				return 48;
			} else {
				return 44;
			}
		}

		public override void RowSelected (UITableView tableView, NSIndexPath indexPath) {
			if (!this.expenseItem.CanManageAttendees) {
				return;
			}
			if (indexPath.Row != this.RowsInSection (tableView, indexPath.Section) - 1) {
				return;
			}

			new ActionnablesWrapper (Attendee.ListShowAttendees (
				actionRecent: () => {
					this.showRecentUser (null);
				},
				actionBusinessRelation: () => {
					this.showBusinessRelation (null);
				},
				actionEmployee: () => {
					this.showEmployee (null);
				},
				actionSpouse: () => {
					this.showSpouse (null);
				},
				actionHCP: () => {
					this.showHCP (null);
				},
				actionHCO: () => {
					this.showHCO (null);
				},
				actionHCU: () => {
					this.showHCU (null);
				}
			), this.parentViewController, tableView.CellAt (indexPath)).show ();
		}

		public void showRecentUser(object sender){
			RecentlyUserTableViewController ruvc = new RecentlyUserTableViewController ();
			ruvc.Attendees = this.expenseItem.Attendees;
			this.showNextVC (ruvc);
		}
		public void showEmployee(object sender){
			EmployeeeTableViewController ruvc = new EmployeeeTableViewController ();
			ruvc.Attendees = this.expenseItem.Attendees;
			this.showNextVC (ruvc);
		}

		public void showBusinessRelation(object sender){
			BusinessFormViewController bcv = new BusinessFormViewController ();
			bcv.Attendees = this.expenseItem.Attendees;
			this.showNextVC (bcv);
		}

		public void showSpouse(object sender){
			SpouseViewController bcv = new SpouseViewController ();
			bcv.Attendees = this.expenseItem.Attendees;
			this.showNextVC (bcv);
		}

		public void showHCU(object sender){
			HCUViewController bcv = new HCUViewController ();
			bcv.Attendees = this.expenseItem.Attendees;
			this.showNextVC (bcv);
		}

		public void showHCP(object sender){
			HCPViewController bcv = new HCPViewController ();
			bcv.attendees = this.expenseItem.Attendees;
			bcv.ModalPresentationStyle = UIModalPresentationStyle.PageSheet;
			this.showNextVC (bcv);
		}

		public void showHCO(object sender){
			HCOViewController bcv = new HCOViewController ();
			bcv.attendees = this.expenseItem.Attendees;
			this.showNextVC (bcv);
		}

		public void showNextVC(UIViewController vc) {

			if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad) {

				UINavigationController nvc = new UINavigationController (vc);
				nvc.ModalPresentationStyle = UIModalPresentationStyle.FormSheet;

				vc.NavigationItem.SetLeftBarButtonItem(new UIBarButtonItem(Labels.GetLoggedUserLabel (Labels.LabelEnum.Close), UIBarButtonItemStyle.Done,  (sender, e)=>{
					nvc.DismissViewController(true, null);
				}), false);

				this.parentViewController.PresentViewController (nvc, true, null);

			} else {
				this.parentViewController.NavigationController.PushViewController (vc, true);
			}
		}

		public override bool CanEditRow (UITableView tableView, NSIndexPath indexPath) {
			if (this.expenseItem.Attendees.Count == indexPath.Row) {
				return false;
			}
			return true;
		}

		class PaddingLabel : UILabel
		{
			public PaddingLabel () : base() {
				
			}

			public override void DrawText (CGRect rect) {
				UIEdgeInsets Insets = new UIEdgeInsets (0, 5, 0, -5);
				RectangleF padded = new RectangleF ((float)(rect.X + Insets.Left), (float)rect.Y, (float)(rect.Width + Insets.Left + Insets.Right), (float)rect.Height);

				base.DrawText (padded);
			}
		}
	}
}