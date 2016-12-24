// This file has been autogenerated from a class added in the UI designer.

using System;

using Foundation;
using UIKit;
using Mxp.Core.Business;
using MXPiOS;
using System.Collections.Generic;
using CoreGraphics;

namespace Mxp.iOS
{
	public partial class HistoryTableViewController : MXPTableViewController, IReportDetailsSubController
	{

		public Report Report { get; set;}
		public HistoryTableViewController (IntPtr handle) : base (handle)
		{
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			this.TableView.Source = new HistoryTableViewSource (this.Report, this);
			this.TableView.ReloadData ();
		}
	}

	class HistoryTableViewSource : UITableViewSource {

		public Report Report;
		public UIViewController ViewController;

		public HistoryTableViewSource(Report report, UIViewController viewController){
			this.Report = report;
			this.ViewController = viewController;
		}

		public override nint NumberOfSections (UITableView tableView)
		{
			return 1;
		}

		public override nint RowsInSection (UITableView tableview, nint section)
		{
			if (this.Report == null) {
				return 0;
			}

			return this.Report.History.Count;
		}

		public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
		{
			HistoryCommentCell cell = (HistoryCommentCell)tableView.DequeueReusableCell (HistoryCommentCell.Key);

			if (cell == null) {
				cell = HistoryCommentCell.Create ();
			}
				

			cell.ReportHistoryItem = this.Report.History [indexPath.Row];

			return cell;
		}

		private Dictionary<ReportHistoryItem, nfloat> heightForString = new Dictionary<ReportHistoryItem, nfloat>();

		private HistoryCommentCell ghostCell = HistoryCommentCell.Create();

		public override nfloat GetHeightForRow (UITableView tableView, NSIndexPath indexPath)
		{
			
			ReportHistoryItem data = this.Report.History [indexPath.Row];

			if (!heightForString.ContainsKey(data)) {
				heightForString[data] = ghostCell.computeSize (data, tableView.Frame.Size.Width);
			}
			return heightForString [data];
		}
			
		public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
		{
			HistoryDetailsViewController vc = new HistoryDetailsViewController ();
			vc.History = this.Report.History [indexPath.Row];


			if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad) {
				var nvc = new UINavigationController (vc);

				vc.NavigationItem.SetLeftBarButtonItem (new UIBarButtonItem(UIBarButtonSystemItem.Cancel, (sender, args) =>{
					nvc.DismissViewControllerAsync(true);
				}), false);
				nvc.ModalPresentationStyle = UIModalPresentationStyle.FormSheet;
				this.ViewController.PresentViewControllerAsync (nvc, true);
			} else {
				this.ViewController.ParentViewController.NavigationController.PushViewController (vc, true);
			}

		}

	}
}
