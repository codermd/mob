using System;

using UIKit;
using Mxp.iOS;
using Mxp.Core.Business;
using System.Threading.Tasks;
using System.Collections.Generic;
using Mxp.Core.Utils;

namespace MXPiOS
{
	public partial class HCPTableViewController : SearchAttendeeTableViewController
	{
		public Attendee RelatedAttendee;

		public HCPTableViewController (Attendee relatedAttendee) : base ()
		{
			this.RelatedAttendee = relatedAttendee;
		}

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
			if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad) {
				this.TableView.SeparatorStyle = UITableViewCellSeparatorStyle.None;
			} else {
				this.TableView.SeparatorStyle = UITableViewCellSeparatorStyle.SingleLine;
			}
		}

		public override void ViewDidAppear (bool animated)
		{
			base.ViewDidAppear (animated);
			this.fetchAttendees ();
		}

		protected override void configureSource ()
		{
			if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad) {
				this.AttendeesSource = new IPadHCPSource (this.privateAttendees);
			} else {
				this.AttendeesSource = new IPhoneHCPSource(this.privateAttendees);
			}

			this.TableView.Source = this.AttendeesSource;
			this.AttendeesSource.cellSelected += Source_cellSelected;
			this.TableView.ReloadData ();
		}


		HCPSource AttendeesSource;

		public async void fetchAttendees() {
			this.privateAttendees.Clear();

			LoadingView.showMessage(Labels.GetLoggedUserLabel(Labels.LabelEnum.Loading) + "...");

			try {
				await this.privateAttendees.FetchRelatedAttendeesAsync(this.RelatedAttendee);
			} catch (Exception e) {
				MainNavigationController.Instance.showError (e);
				return;
			} finally {
				LoadingView.hideMessage();
			}

			this.TableView.ReloadData ();
		}

		public override void DidReceiveMemoryWarning ()
		{
			base.DidReceiveMemoryWarning ();
			// Release any cached data, images, etc that aren't in use.
		}

	}
}