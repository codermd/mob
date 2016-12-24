using System;

using Foundation;
using UIKit;
using Mxp.Core.Business;
using CoreGraphics;
using System.ComponentModel;

namespace Mxp.iOS
{
	public partial class ReportsViewController : MXPViewController
	{
		private UIRefreshControl refreshControl;
		private ReportsTableViewSource source;

		private Reports selectedReports;
		private Report draftSelectedReport;
		private Report openSelectedReport;
		private Report closedSelectedReport;

		public class ReportSelectedEventArgs : EventArgs
		{
			public Report Report { get; set; }
		}

		public event EventHandler<ReportSelectedEventArgs> cellSelected;

		public ReportsViewController (IntPtr handle) : base (handle) {
			
		}
			
		public override void ViewDidLoad () {
			base.ViewDidLoad ();

			this.SegmentedFilter.RemoveAllSegments ();
			this.SegmentedFilter.InsertSegment (LoggedUser.Instance.Labels.GetLabel (Labels.LabelEnum.Draft), 0, false);
			this.SegmentedFilter.InsertSegment (LoggedUser.Instance.Labels.GetLabel (Labels.LabelEnum.Open), 1, false);
			this.SegmentedFilter.InsertSegment (LoggedUser.Instance.Labels.GetLabel (Labels.LabelEnum.Closed), 2, false);
			this.ConfigureRefresh ();
			this.SegmentedFilter.SelectedSegment = 0;

			this.ShowReportForIndex ((int)this.SegmentedFilter.SelectedSegment);

			this.Title = Labels.GetLoggedUserLabel (Labels.LabelEnum.Reports);

			this.EdgesForExtendedLayout = UIRectEdge.None;

			this.SegmentedFilter.SelectedSegment = (nint) ((MainTabBarControllerView)this.TabBarController)?.SelectedCategory;
			this.SegmentedFilter.SendActionForControlEvents (UIControlEvent.ValueChanged);

			ExpenseItem expenseItem = ((MainTabBarControllerView)this.TabBarController)?.ExpenseItem;
			if (expenseItem != null)
				this.SetSelectedReport (expenseItem.ParentExpense.Report);
		}

		public override void ViewWillAppear (bool animated) {
			base.ViewWillAppear (animated);

			this.ReloadData ();
		}

		private void ReloadData () {
			this.TableView.ReloadData ();

			this.ShowReport ((this.PresentedReportExists ? this.SelectedReport : null), false);
			this.HighlightSelectedReport ();
		}

		public void ConfigureRefresh () {
			this.refreshControl = new UIRefreshControl ();
			this.TableView.AddSubview (refreshControl);
			this.refreshControl.ValueChanged += HandleRefreshControlChanged;
		}
			
		public void SetReports (Reports reports) {
			if (this.selectedReports != null)
				this.selectedReports.PropertyChanged -= HandlePropertyChanged;

			if (this.source != null)
				this.source.cellSelected -= HandleCellSelected;

			this.selectedReports = reports;

			if (this.selectedReports != null) {
				if (!this.selectedReports.Loaded)
					this.RefreshReports ();

				this.source = new ReportsTableViewSource (this.selectedReports);
				this.source.cellSelected += HandleCellSelected;

				this.TableView.Source = this.source;

				this.selectedReports.PropertyChanged += HandlePropertyChanged;
				this.ReloadData ();
			}
		}

		private void HandleCellSelected (object sender, ReportSelectedEventArgs e) {
			this.ShowReport (e.Report);
		}

		public void HandleRefreshControlChanged (object sender, EventArgs args) {
			this.RefreshReports ();
		}

		private void HandlePropertyChanged (object sender, PropertyChangedEventArgs e) {
			this.ReloadData ();
		}

		private bool PresentedReportExists {
			get {
				return this.SelectedReport != null && this.selectedReports.Contains (this.SelectedReport);
			}
		}

		public async void RefreshReports () {
			this.refreshControl.BeginRefreshing ();

			try {
				await this.selectedReports.FetchAsync ();
			} catch (Exception e) {
				MainNavigationController.Instance.showError (e);
				return;
			} finally {
				this.refreshControl.EndRefreshing ();
			}
		}

		private void HighlightSelectedReport () {
			if (this.SelectedReport == null || !this.PresentedReportExists)
				return;

			NSIndexPath indexPath = NSIndexPath.FromRowSection (this.selectedReports.IndexOf (this.SelectedReport), 0);
			this.TableView.SelectRow (indexPath, false, UITableViewScrollPosition.Middle);
		}

		partial void newSelectedFilter (UISegmentedControl sender) {
			this.ShowReportForIndex((int)sender.SelectedSegment);
		}

		public void ShowReportForIndex (int index) {
			switch(this.SegmentedFilter.SelectedSegment){
				case 0:
					this.SetReports (LoggedUser.Instance.DraftReports);
					break;
				case 1:
					this.SetReports (LoggedUser.Instance.OpenReports);
					break;
				case 2:
					this.SetReports (LoggedUser.Instance.ClosedReports);
					break;
			}
		}

		partial void ClickOnAdd (UIBarButtonItem sender) {
			this.ShowReport(Report.NewInstance ());
		}

		public void ShowReport (Report report, bool animated = true) {
			this.SetSelectedReport (report);

			if (this.cellSelected != null) {
				ReportSelectedEventArgs e = new ReportSelectedEventArgs ();
				e.Report = report;
				this.cellSelected(this, e);
				return;
			}

			if (report == null)
				return;

			this.SetSelectedReport (null);

			UIStoryboard storyBoard = UIStoryboard.FromName ("ReportDetails", null);	
			ReportDetailsViewController reportDetailsViewController = storyBoard.InstantiateInitialViewController () as ReportDetailsViewController;
			reportDetailsViewController.Report = report;
			this.NavigationController.PushViewController (reportDetailsViewController, animated);
		}

		private Report SelectedReport {
			get {
				switch (this.selectedReports.ReportType) {
					case Reports.ReportTypeEnum.Draft:
						return this.draftSelectedReport;
					case Reports.ReportTypeEnum.Open:
						return this.openSelectedReport;
					case Reports.ReportTypeEnum.Closed:
						return this.closedSelectedReport;
					default:
						return null;
				}
			}
		}

		private void SetSelectedReport (Report report) {
			switch (this.selectedReports.ReportType) {
				case Reports.ReportTypeEnum.Draft:
					this.draftSelectedReport = report;
					break;
				case Reports.ReportTypeEnum.Open:
					this.openSelectedReport = report;
					break;
				case Reports.ReportTypeEnum.Closed:
					this.closedSelectedReport = report;
					break;
			}
		}
	}
}