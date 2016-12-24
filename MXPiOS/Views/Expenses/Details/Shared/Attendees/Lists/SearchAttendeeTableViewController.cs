using System;
using CoreGraphics;

using Foundation;
using UIKit;
using Mxp.Core.Business;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace Mxp.iOS
{
	public class SearchAttendeeTableViewController : MXPTableViewController
	{


		public Attendees privateAttendees  = new Attendees();
		public Attendees Attendees;

		protected SearchAttendeeTableViewSource source;

		public SearchAttendeeTableViewController () : base (UITableViewStyle.Plain)
		{

		}

		public override void ViewWillAppear (bool animated)
		{
			this.Title = Labels.GetLoggedUserLabel(Labels.LabelEnum.HCPSearhResult);

			base.ViewWillAppear (animated);
			this.configureSource ();

			this.TableView.SeparatorStyle = UITableViewCellSeparatorStyle.None;

		}

		protected virtual void configureSource (){
			this.source = new SearchAttendeeTableViewSource ();
			this.source.cellSelected += Source_cellSelected;
			this.TableView.Source = this.source;
		}

		protected void Source_cellSelected (object sender, SearchAttendeeTableViewSource.SelectedEventArgs e)
		{
			this.processAttendee(e.attendee);
		}

		protected async void processAttendee(Attendee attendee) {
			LoadingView.showMessage(Labels.GetLoggedUserLabel (Labels.LabelEnum.Saving) + "...");

			try {
				await this.Attendees.AddAsync (attendee, null, true);
			} catch (Exception error) {
				MainNavigationController.Instance.showError (error);
				return;
			} finally {
				LoadingView.hideMessage (false);
			}

			if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad) {
				this.DismissViewController (true, null);
			} else {
				this.NavigationController.PopToViewController (this.NavigationController.ViewControllers.Single (vc => vc is ExpenseDetailViewController), true);
			}
		}

	}
}

