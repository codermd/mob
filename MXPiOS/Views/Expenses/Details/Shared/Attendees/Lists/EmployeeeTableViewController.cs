using System;
using UIKit;
using CoreGraphics;
using System.Threading.Tasks;
using Mxp.Core.Services;
using Mxp.Core.Business;
using System.Collections.Generic;

namespace Mxp.iOS
{
	public class EmployeeeTableViewController : SearchAttendeeTableViewController
	{

		private UISearchBar searchBar;

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
			this.searchBar = new UISearchBar ();
			this.searchBar.Frame = new CGRect (0, 0, 320, 44);
			this.searchBar.SearchButtonClicked += (object sender, EventArgs e) => {
				this.refreshTableView();
			};

			this.searchBar.TextChanged += (object sender, UISearchBarTextChangedEventArgs e) => {
				this.refreshTableView();
			};

			this.TableView.TableHeaderView = searchBar;
			this.searchBar.BecomeFirstResponder ();

		}

		public async void refreshTableView(){
			LoadingView.showMessage (Labels.GetLoggedUserLabel (Labels.LabelEnum.Waiting) + "...");

			try {
				this.source.attendees = await LookupService.Instance.FetchEmployee (this.searchBar.Text);
			} catch (Exception) { }

			LoadingView.hideMessage ();
			this.TableView.ReloadData ();
		}
	}
}

