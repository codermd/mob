using System;
using CoreGraphics;

using Foundation;
using UIKit;
using Mxp.Core.Business;
using System.Threading.Tasks;
using System.Collections.Generic;
using Mxp.Core.Utils;
using System.Linq;

namespace Mxp.iOS
{
	public class HealthCareTableViewController : SearchAttendeeTableViewController
	{
	
		public HealthCareTableViewController () : base ()
		{
		}

		public override void DidReceiveMemoryWarning ()
		{
			base.DidReceiveMemoryWarning ();
		}

		public Attendee RelatedAttendee;

		public override void ViewDidAppear (bool animated)
		{
			base.ViewDidAppear (animated);
			this.refreshTableView ();
		}

		public async void refreshTableView(){
			LoadingView.showMessage (Labels.GetLoggedUserLabel (Labels.LabelEnum.Loading) + "...");

			try {
				await privateAttendees.FetchRelatedAttendeesAsync (this.RelatedAttendee);
			} catch (Exception e) {
				MainNavigationController.Instance.showError (e);
				return;
			} finally {
				LoadingView.hideMessage ();
			}

			this.source.attendees = privateAttendees.ToList ();
			this.TableView.ReloadData ();
		}
	}
}

