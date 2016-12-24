
using System;
using CoreGraphics;

using Foundation;
using UIKit;
using Mxp.Core.Business;
using Mxp.Utils;
using MXPiOS;

namespace Mxp.iOS
{
	public partial class HCPViewController : HCRelatedAttendeeViewController
	{
		public HCPViewController () : base ()
		{
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			this.attendee = new Attendee (AttendeeTypeEnum.HCP);
			this.Title = Labels.GetLoggedUserLabel (Labels.LabelEnum.Hcp);
		}


		public override void ProcessSearch ()
		{
				var vc = new HCPTableViewController(attendee);
				vc.Attendees = this.attendees;
				this.NavigationController.PushViewController(vc, true);	
		}

	}
}

