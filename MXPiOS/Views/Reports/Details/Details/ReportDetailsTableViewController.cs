// This file has been autogenerated from a class added in the UI designer.

using System;

using Foundation;
using Mxp.Core.Business;
using UIKit;

namespace Mxp.iOS
{
	public partial class ReportDetailsTableViewController : MXPTableViewController, IReportDetailsSubController
	{
		public ReportDetailsTableViewController (IntPtr handle) : base (handle)
		{
		}

		public Report Report { get; set;}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			this.TableView.Source = new ReportDetailsTableViewSource (this.Report, this);
			this.TableView.ReloadData();

		}

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
			this.TableView.Source = new ReportDetailsTableViewSource (this.Report, this);
			this.TableView.ReloadData();
		}

	}

	public class ReportDetailsTableViewSource : UITableViewSource
	{
	
		private SectionFieldsSource FirstSectionController;
		private SectionFieldsSource SecondSectionController;

		public ReportDetailsTableViewSource (Report aReport, UIViewController ViewController)
		{
			this.FirstSectionController = new SectionFieldsSource (aReport.GetMainFields(), ViewController.ParentViewController, Labels.GetLoggedUserLabel (Labels.LabelEnum.General));
			this.SecondSectionController = new SectionFieldsSource(aReport.GetAllFields(), ViewController.ParentViewController, Labels.GetLoggedUserLabel(Labels.LabelEnum.Details));
		}

		public override nint NumberOfSections (UITableView tableView)
		{
			return 2;
		}

		public override nint RowsInSection (UITableView tableview, nint section)
		{
			if (section == 0) {
				return this.FirstSectionController.RowsInSection (tableview);
			}
			if (section == 1) {
				return this.SecondSectionController.RowsInSection (tableview);
			}
			return 0;
		}

		public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
		{
			if (indexPath.Section == 0) {
				return this.FirstSectionController.GetCell (tableView, indexPath.Row);
			}
			if (indexPath.Section == 1) {
				return this.SecondSectionController.GetCell (tableView, indexPath.Row);
			}
			return null;

		}

		public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
		{
			if (indexPath.Section == 0) {
				this.FirstSectionController.RowSelected (tableView, indexPath.Row, tableView.CellAt(indexPath));
			}
			if (indexPath.Section == 1) {
				this.SecondSectionController.RowSelected (tableView, indexPath.Row, tableView.CellAt(indexPath));
			}
		}

		public override string TitleForHeader (UITableView tableView, nint section)
		{
			if (section == 0) {
				return null;
			}

			if (section == 1) {
				return this.SecondSectionController.Title;
			}
			return null;
		}

		public override UITableViewCellAccessory AccessoryForRow (UITableView tableView, NSIndexPath indexPath)
		{
			if (indexPath.Section == 0) {
				return this.FirstSectionController.AccessoryForRow (tableView, indexPath.Row);
			}
			if (indexPath.Section == 1) {
				return this.SecondSectionController.AccessoryForRow (tableView, indexPath.Row);
			}

			return UITableViewCellAccessory.None;
		}


	}


}
