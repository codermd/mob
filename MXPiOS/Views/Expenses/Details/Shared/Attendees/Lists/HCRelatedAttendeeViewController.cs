using UIKit;
using Mxp.Core.Business;
using Foundation;
using System;

namespace  Mxp.iOS
{
	public partial class HCRelatedAttendeeViewController : MXPViewController
	{

		NSObject _shownotification;
		NSObject _hidenotification;

		public HCRelatedAttendeeViewController () : base ("HCRelatedAttendeeViewController", null)
		{
		}
		public Attendees attendees = new Attendees();
		protected Attendee attendee;


		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			this.EdgesForExtendedLayout = UIRectEdge.None;

			this.SearchButton.Layer.CornerRadius = 4;
			this.SearchButton.Layer.MasksToBounds = true;
			this.SearchButton.SetTitle (LoggedUser.Instance.Labels.GetLabel (Labels.LabelEnum.Search), UIControlState.Normal);
		}


		public override void ViewWillDisappear(bool animated)
		{
			base.ViewWillDisappear(animated);
			// Unregister the callbacks
			this._shownotification?.Dispose();
			this._hidenotification?.Dispose();
		}

		void ShowCallback(object obj, UIKeyboardEventArgs args)
		{
			this.TableView.ContentInset = new UIEdgeInsets(0.0f, 0.0f, args.FrameEnd.Size.Height, 0.0f);
		}

		void HideCallback(object obj, UIKeyboardEventArgs args)
		{
			this.TableView.ContentInset = new UIEdgeInsets(0.0f, 0.0f, 0.0f, 0.0f);
		}

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);

			var source = new TableSectionsSource ();
			this.TableView.Source = source;
			source.Sections.Add (new SectionFieldsSource (this.attendee.FormFields, this, null));
			this.TableView.ReloadData();

			this._hidenotification = UIKeyboard.Notifications.ObserveDidHide(this.HideCallback);
			this._shownotification = UIKeyboard.Notifications.ObserveWillShow(this.ShowCallback);
		}

		partial void ClickOnSearch (NSObject sender)
		{
			try {
				this.attendee.TryValidate ();
			} catch (Exception e) {
				MainNavigationController.Instance.showError (e);
				return;
			}

			this.ProcessSearch();	
		}

		public virtual void ProcessSearch(){}

	}
}


