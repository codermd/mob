
using System;
using CoreGraphics;

using Foundation;
using UIKit;
using Mxp.Core.Business;
using Mxp.Utils;

namespace Mxp.iOS
{
	public partial class HCUViewController : AttendeeFormViewController
	{
		public HCUViewController () : base ("HCUViewController", null)
		{
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			Labels labels = LoggedUser.Instance.Labels;

			this.Firstname.Text = "*" + labels.GetLabel (Labels.LabelEnum.FirstName);
			this.Lastname.Text = "*" + labels.GetLabel (Labels.LabelEnum.LastName);
			this.Company.Text = "*" + labels.GetLabel (Labels.LabelEnum.Company);
			this.Address.Text = labels.GetLabel (Labels.LabelEnum.Address);
			this.Zip.Text = labels.GetLabel (Labels.LabelEnum.ZipCode);
			this.City.Text = "*"+labels.GetLabel (Labels.LabelEnum.City);
			this.State.Text = labels.GetLabel (Labels.LabelEnum.State);
			this.Speciality.Text = labels.GetLabel (Labels.LabelEnum.Speciality);
			this.NPI.Text = labels.GetLabel (Labels.LabelEnum.Npi);

			DoneToolBar tb = new DoneToolBar (Labels.GetLoggedUserLabel (Labels.LabelEnum.Next));
			tb.Frame = new CGRect (0, 0, 320, 44);
			this.ZipField.InputAccessoryView = tb;
			tb.ClickOnButton += (object sender, EventArgs e) => {
				this.CityField.BecomeFirstResponder();
			};

			this.NPIField.ShouldReturn += (sender) => {
				this.NPIField.ResignFirstResponder();
				return true;
			};

			for (int i = 0; i < this.inputs.Length; i++) {
				UITextField field = this.inputs [i];
				var currentI = i;
				field.ShouldReturn += (textField) => {
					field.ResignFirstResponder();
					if(currentI+1 < this.inputs.Length){
						this.inputs[currentI+1].BecomeFirstResponder();
					}
					return true;
				};
			}

			this.Title = LoggedUser.Instance.Labels.GetLabel (Labels.LabelEnum.Hcu);
		}

		public override void ViewDidAppear (bool animated)
		{
			base.ViewDidAppear (animated);
			this.ScrollView.ContentSize = this.MainContainer.Bounds.Size;
			_hidenotification = UIKeyboard.Notifications.ObserveDidHide(HideCallback);
			_shownotification = UIKeyboard.Notifications.ObserveWillShow(ShowCallback);


		}


		public override Attendee prepareAttendee() {
			return new Attendee (this.FirstnameField.Text, this.LastnameField.Text, this.CompanyField.Text, this.AddressField.Text, this.ZipField.Text.ToInt(), this.CityField.Text, this.StateField.Text, this.SpecialityField.Text, this.NPIField.Text);
		}


		public override void clickOnCancel (object sender)
		{
			base.clickOnCancel (sender);

			this.FirstnameField.Text = "";
			this.LastnameField.Text = "";
			this.CompanyField.Text = "";
			this.AddressField.Text = "";
			this.ZipField.Text = "";
			this.CityField.Text = "";
			this.StateField.Text = "";
			this.SpecialityField.Text = "";
			this.NPIField.Text = "";

			if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad) {
				this.DismissViewController (true, null);
			}

		}

		partial void textFieldChange (NSObject sender)
		{
			this.SetEditing(true, true);
		}


		NSObject _shownotification;
		NSObject _hidenotification;

		void ShowCallback (object obj,  UIKit.UIKeyboardEventArgs args)
		{
			this.ScrollView.ContentInset = new UIEdgeInsets (0.0f, 0.0f, args.FrameEnd.Size.Height, 0.0f);
		}

		void HideCallback (object obj,  UIKit.UIKeyboardEventArgs args)
		{
			this.ScrollView.ContentInset = new UIEdgeInsets (0.0f, 0.0f, 0.0f, 0.0f);
		}

		public override void ViewWillDisappear (bool animated)
		{
			base.ViewWillDisappear (animated);

			// Unregister the callbacks
			if (_shownotification != null)
				_shownotification.Dispose();
			if (_hidenotification != null)
				_hidenotification.Dispose();

		}

		partial void ClickOnCreate (NSObject sender)
		{
			this.processSave();
		}
	}
}

