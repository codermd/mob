using System;
using UIKit;
using Mxp.Core.Business;

namespace Mxp.iOS
{
	public class ReportsTableViewSource : UITableViewSource
	{



		// Starting off with an empty handler avoids pesky null checks
		public event EventHandler<ReportsViewController.ReportSelectedEventArgs> cellSelected = delegate {};

		private Reports reports;



		public ReportsTableViewSource (Reports reports)
		{
			this.reports = reports;
		}

		public override nint NumberOfSections (UITableView tableView)
		{
			return 1;
		}

		public override nint RowsInSection (UITableView tableview, nint section)
		{
			if (this.reports == null) {
				return 0;
			}

			if (this.reports.Count == 0) {
				return 1;
			}

			return this.reports.Count;
		}

		public override UITableViewCell GetCell (UITableView tableView, Foundation.NSIndexPath indexPath)
		{
			if (this.reports.Count == 0) {

				EmptyCell emptyCell = (EmptyCell)tableView.DequeueReusableCell ("EmptyCell");

				if (emptyCell == null) {
					emptyCell = EmptyCell.Create();
				}
				return emptyCell;
			}

			ReportCell cell = (ReportCell)tableView.DequeueReusableCell ("ReportCell");

			if (cell == null) {
				cell = ReportCell.Create();
			}

			cell.setReport (this.reports [indexPath.Row]);

			return cell;
		}

		public override nfloat GetHeightForRow (UITableView tableView, Foundation.NSIndexPath indexPath)
		{
			return 92;
		}

		public override void RowSelected (UITableView tableView, Foundation.NSIndexPath indexPath)
		{
			if (this.reports.Count == 0) {
				return;
			}

			ReportsViewController.ReportSelectedEventArgs e = new ReportsViewController.ReportSelectedEventArgs ();
			e.Report = this.reports [indexPath.Row];

			this.cellSelected(this, e);

		}

	}
}
