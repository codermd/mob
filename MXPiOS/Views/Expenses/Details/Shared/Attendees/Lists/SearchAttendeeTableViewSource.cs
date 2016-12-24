using System;
using CoreGraphics;

using Foundation;
using UIKit;
using Mxp.Core.Business;
using System.Collections.Generic;

namespace Mxp.iOS
{
	public class SearchAttendeeTableViewSource : UITableViewSource
	{

		//Event management
		public class SelectedEventArgs : EventArgs
		{
			public Attendee attendee { get; set;}
		}

		public event EventHandler<SelectedEventArgs> cellSelected = delegate {};

		public List<Attendee> attendees;

		public override nint NumberOfSections (UITableView tableView)
		{
			return 1;
		}

		public bool isEmpty(){
			return this.attendees == null || this.attendees.Count == 0;
		}

		public override nint RowsInSection (UITableView tableview, nint section)
		{
			if (this.isEmpty()) {
				return 1;
			}
			return this.attendees.Count;
		}

		public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
		{
			if (this.isEmpty ()) {
				var cell = tableView.DequeueReusableCell (EmptyCell.Key) as EmptyCell;
				if (cell == null)
					cell = EmptyCell.Create ();
				cell.SelectionStyle = UITableViewCellSelectionStyle.None;
				return cell;
			} else {
				DefaultCell cell = (DefaultCell)tableView.DequeueReusableCell (DefaultCell.Key);

				if ( cell == null) {
					cell = DefaultCell.Create();
				}

				cell.TextLabel.Text = this.attendees [indexPath.Row].VName;
				return cell;
			}
		}

		public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
		{
			if (this.isEmpty()) {
				return;
			}
			this.selectAttendee (this.attendees [indexPath.Row]);
		}

		protected void selectAttendee(Attendee a) {
			SelectedEventArgs e = new SelectedEventArgs ();
			e.attendee = a;
			this.cellSelected (this, e);
		}
	}
}

