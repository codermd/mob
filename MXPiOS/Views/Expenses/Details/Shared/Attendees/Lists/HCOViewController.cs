
using System;
using CoreGraphics;

using Foundation;
using UIKit;
using Mxp.Core.Business;
using Mxp.Utils;
using MXPiOS;

namespace Mxp.iOS
{
	public partial class HCOViewController : HCRelatedAttendeeViewController
	{
		public HCOViewController () : base ()
		{
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			this.attendee = new Attendee (AttendeeTypeEnum.HCO);
			this.Title = Labels.GetLoggedUserLabel (Labels.LabelEnum.Hco);
		}


		public override void ProcessSearch ()
		{
			HealthCareTableViewController vc = new HealthCareTableViewController();
			vc.RelatedAttendee = attendee;
			vc.Attendees = attendees;
			this.NavigationController.PushViewController(vc, true);
		}

	}
}
