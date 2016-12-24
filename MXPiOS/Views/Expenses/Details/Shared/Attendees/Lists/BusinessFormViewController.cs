using System;
using Foundation;
using UIKit;
using Mxp.Core.Business;

namespace Mxp.iOS
{
	public partial class BusinessFormViewController : AttendeeFormViewController
	{

		public BusinessFormViewController () : base ("BusinessFormViewController", null)
		{
		}

		public override void DidReceiveMemoryWarning ()
		{
			base.DidReceiveMemoryWarning ();
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			this.Firstname.Text = LoggedUser.Instance.Labels.GetLabel (Labels.LabelEnum.FirstName);
			this.Lastname.Text = "*"+LoggedUser.Instance.Labels.GetLabel (Labels.LabelEnum.LastName);
			this.Company.Text = "*"+LoggedUser.Instance.Labels.GetLabel (Labels.LabelEnum.Company);
			this.Title = LoggedUser.Instance.Labels.GetLabel (Labels.LabelEnum.BusinessRelation);

		}

		partial void ClickOnCreate (NSObject sender)
		{
			this.processSave();
		}

		partial void companyNameChange (NSObject sender)
		{
			this.SetEditing(true, true);
		}

		partial void FirstnameChange (NSObject sender)
		{
			this.SetEditing(true, true);
		}

		partial void LastnameChange (NSObject sender)
		{
			this.SetEditing(true, true);
		}

		public override Attendee prepareAttendee() {
			return  new Attendee (this.FirstnameField.Text, this.LastnameField.Text, this.CompanyField.Text);
		}

		public override void clickOnCancel (object sender)
		{
			base.clickOnCancel (sender);

			this.CompanyField.Text = "";
			this.FirstnameField.Text = "";
			this.LastnameField.Text = "";

		}
	}
}
