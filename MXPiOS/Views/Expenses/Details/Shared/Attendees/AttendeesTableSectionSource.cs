using System;
using CoreGraphics;

using Foundation;
using UIKit;
using ObjCRuntime;
using System.Collections.ObjectModel;
using Mxp.Core.Business;
using System.Threading.Tasks;
using Mxp.Core.Utils;
using MXPiOS;

namespace Mxp.iOS
{
	public class AttendeesTableSectionSource : SectionSource
	{

		public override string Title {
			get {
				return Labels.GetLoggedUserLabel (Labels.LabelEnum.Attendees);
			}
		}

		public override bool CanEditRow (UITableView tableView,int row) {
			if (this.expenseItem.Attendees.Count == row) {
				return false;
			}
			return true;
		}

		private ExpenseItem expenseItem;
		private Expense Expense {
			get {
				return this.expenseItem.ParentExpense;
			}
		}

		public UIViewController ParentViewController;

		public AttendeesTableSectionSource (ExpenseItem expenseItem, UIViewController parentViewController)
		{
			this.expenseItem = expenseItem;
			this.ParentViewController = parentViewController;
		}


		public override int RowsInSection (UITableView tableview)
		{

			int res =  this.expenseItem.Attendees.Count;
			if (this.expenseItem.CanManageAttendees) {
				res++;
			}
			return res;
		}

		public override UITableViewCell GetCell (UITableView tableView, int Row)
		{
			if (this.expenseItem.CanManageAttendees && Row == this.expenseItem.Attendees.Count) {

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
	
			cell.configure (this.expenseItem.Attendees [Row]);
			cell.SelectionStyle = UITableViewCellSelectionStyle.None;

			cell.ShowAttendee += (object sender, EventArgs e) => {

				var vc = new AttendeeDetailsViewController(sender as Attendee);

				if(UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad) {
					var nvc = new UINavigationController(vc);
					nvc.ModalPresentationStyle = UIModalPresentationStyle.FormSheet;
					this.ParentViewController.PresentViewControllerAsync(nvc, true);
				} else {
					this.ParentViewController.NavigationController.PushViewController(vc, true);	
				}

			};

			return cell;
		}

		public override UITableViewCellEditingStyle EditingStyleForRow (UITableView tableView, int Row)
		{
			if (this.expenseItem.CanManageAttendees) {
				if (Row == this.expenseItem.Attendees.Count) {
					return UITableViewCellEditingStyle.None;
				}
				return UITableViewCellEditingStyle.Delete;
			} else {
				return UITableViewCellEditingStyle.None;
			}
		}

		public override void CommitEditingStyle (UITableView tableView, UITableViewCellEditingStyle editingStyle, int Row)
		{
			if (Row != this.expenseItem.Attendees.Count) {
				this.processRemoveAttendee (this.expenseItem.Attendees [Row], tableView);
			}
		}

		private bool IsRemovingItem = false;
		public async Task processRemoveAttendee(Attendee attendee, UITableView tableView){
			if (this.IsRemovingItem) {
				return;
			}

			this.IsRemovingItem = true;

			int index = this.expenseItem.Attendees.IndexOf (attendee);

			LoadingView.showMessage (Labels.GetLoggedUserLabel (Labels.LabelEnum.Deleting) + "...");

			try {
				await this.expenseItem.Attendees.RemoveAsync(attendee);
			} catch (Exception e) {
				MainNavigationController.Instance.showError (e);
				return;
			} finally {
				LoadingView.hideMessage ();
				this.IsRemovingItem = false;
			}
		}

		public override float GetHeightForRow (UITableView tableView, int row)
		{
			if (row != this.expenseItem.Attendees.Count) {
				return 48;
			} else {
				return 44;
			}
		}

		public override void RowSelected (UITableView tableView,  int row, UITableViewCell cell) 
		{
			if (!this.expenseItem.CanManageAttendees) {
				return;
			}
			if (row != this.RowsInSection (tableView) - 1) {
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
			), this.ParentViewController, cell).show ();
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

				this.ParentViewController.PresentViewController (nvc, true, null);

			} else {
				this.ParentViewController.NavigationController.PushViewController (vc, true);
			}
		}
	}
}


