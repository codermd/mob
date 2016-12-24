
using System;
using CoreGraphics;

using Foundation;
using UIKit;
using Mxp.Core.Business;

namespace Mxp.iOS
{
	public partial class SpouseViewController : AttendeeFormViewController
	{
		public SpouseViewController () : base ("SpouseViewController", null)
		{
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			this.Firstname.Text = LoggedUser.Instance.Labels.GetLabel (Labels.LabelEnum.FirstName);
			this.Lastname.Text = "*"+LoggedUser.Instance.Labels.GetLabel (Labels.LabelEnum.LastName);
			this.Title = LoggedUser.Instance.Labels.GetLabel (Labels.LabelEnum.Spouse);
		}

		public override Attendee prepareAttendee() {
			return new Attendee (this.FirstnameField.Text, this.LastnameField.Text);
		}

		partial void ValueChange (NSObject sender)
		{
			this.SetEditing(true, true);
		}

		public override void clickOnCancel (object sender)
		{
			base.clickOnCancel (sender);
			this.FirstnameField.Text = "";
			this.LastnameField.Text = "";

		}

		partial void ClickOnCreate (NSObject sender)
		{
			this.processSave();
		}

	}
}

