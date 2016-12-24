using System;
using Mxp.iOS;
using Mxp.Core.Business;
using UIKit;
using System.Collections.Generic;

namespace MXPiOS
{

	public class HCPSource : SearchAttendeeTableViewSource 
	{
		protected Attendees Attendees;

		public HCPSource(Attendees attendees): base(){
			this.Attendees = attendees;
		}			

		public override nint NumberOfSections (UITableView tableView)
		{
			return 1;
		}
	}

	public class IPhoneHCPSource : HCPSource 
	{
		public IPhoneHCPSource(Attendees attendees): base(attendees) {}

		public override nint RowsInSection (UITableView tableview, nint section)
		{
			return this.Attendees.Count;
		}

		public override UITableViewCell GetCell (UITableView tableView, Foundation.NSIndexPath indexPath)
		{

			IPhoneHCPAttendeeCell cell = tableView.DequeueReusableCell (IPhoneHCPAttendeeCell.Key) as IPhoneHCPAttendeeCell;
			if (cell == null) {
				cell = IPhoneHCPAttendeeCell.Create ();
			}
			cell.Configure (this.Attendees[indexPath.Row]);

			return cell;
		}

		public override nfloat GetHeightForRow (UITableView tableView, Foundation.NSIndexPath indexPath)
		{
			return 70;
		}
		public override void RowSelected (UITableView tableView, Foundation.NSIndexPath indexPath)
		{
			this.selectAttendee (this.Attendees [indexPath.Row]);
		}

	}

	public class IPadHCPSource : HCPSource 
	{
		public IPadHCPSource(Attendees attendees): base(attendees) {}


		public override nint RowsInSection (UITableView tableview, nint section)
		{
			return this.Attendees.Count + 1;
		}

		public override UITableViewCell GetCell (UITableView tableView, Foundation.NSIndexPath indexPath)
		{
			HCPAttendeeCell cell = tableView.DequeueReusableCell(HCPAttendeeCell.Key) as HCPAttendeeCell;
			if (cell == null) {
				cell = HCPAttendeeCell.Create ();
			}

			if (indexPath.Row == 0) {
				cell.Configure (new List<string>{
					Labels.GetLoggedUserLabel(Labels.LabelEnum.FirstName),
					Labels.GetLoggedUserLabel(Labels.LabelEnum.LastName),
					Labels.GetLoggedUserLabel(Labels.LabelEnum.Npi),
					Labels.GetLoggedUserLabel(Labels.LabelEnum.Address),
					Labels.GetLoggedUserLabel(Labels.LabelEnum.City),
					Labels.GetLoggedUserLabel(Labels.LabelEnum.State),
					Labels.GetLoggedUserLabel(Labels.LabelEnum.ZipCode),
				}, true);
			} else {
				var attendee = this.Attendees [indexPath.Row - 1];
				cell.Configure (new List<string>{
					attendee.Firstname,
					attendee.Lastname,
					attendee.Reference,
					attendee.Address,
					attendee.City,
					attendee.State,
					attendee.ZipCode.HasValue ? attendee.ZipCode.ToString() : ""
				});
			}

			return cell;
		}

		public override void RowSelected (UITableView tableView, Foundation.NSIndexPath indexPath)
		{
			if (indexPath.Row == 0) {
				return;
			}
			this.selectAttendee (this.Attendees [indexPath.Row - 1]);
		}
	}


}

