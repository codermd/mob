
using System;
using CoreGraphics;

using Foundation;
using UIKit;
using Mxp.Core.Business;

namespace Mxp.iOS
{
	public class RecentlyUserTableViewController : SearchAttendeeTableViewController
	{

		public RecentlyUserTableViewController () : base ()
		{
		}

		public override void DidReceiveMemoryWarning ()
		{
			base.DidReceiveMemoryWarning ();
		}

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
			this.refreshTableView ();
		}

		public async void refreshTableView ()
		{
			LoadingView.showMessage (Labels.GetLoggedUserLabel (Labels.LabelEnum.Loading) + "...");

			try {
				this.source.attendees = await this.Attendees.FetchRecentlyUsedAttendees ();
			} catch (Exception e) {
				MainNavigationController.Instance.showError (e);
				return;
			} finally {
				LoadingView.hideMessage ();
			}

			this.TableView.ReloadData ();
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			this.Title = LoggedUser.Instance.Labels.GetLabel (Labels.LabelEnum.Recent);
		}
	}
}

