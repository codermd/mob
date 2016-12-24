using System;
using Android.App;
using Android.OS;
using Mxp.Core.Business;
using Android.Widget;
using Android.Views;
using Mxp.Droid.Adapters;
using Mxp.Core.Helpers;

namespace Mxp.Droid.Fragments
{
	public class AttendeePickerDialogFragment : Android.Support.V4.App.DialogFragment
	{
		private event EventHandler<EventArgsObject<Attendee>> mOnClickHandler;

		private View mView;
		private Attendees mAttendees;

		public AttendeePickerDialogFragment (Attendees attendess, EventHandler<EventArgsObject<Attendee>> onClickHandler) {
			this.mAttendees = attendess;
			this.mOnClickHandler = onClickHandler;
		}

		public override void OnCreate (Bundle savedInstanceState) {
			base.OnCreate (savedInstanceState);

			this.mView = this.Activity.LayoutInflater.Inflate (Resource.Layout.List_attendees, null);
			ListView listView = this.mView.FindViewById<ListView> (Resource.Id.List);

			AttendeeAdapter attendeeAdapter = new AttendeeAdapter (this.Activity, this.mAttendees, true, true);
			listView.Adapter = attendeeAdapter;
			listView.ItemClick += (object sender, AdapterView.ItemClickEventArgs e) => {
				this.mOnClickHandler (this, new EventArgsObject<Attendee> (this.mAttendees[e.Position]));
			};
		}

		public override Dialog OnCreateDialog(Bundle savedInstanceState) {
			AlertDialog.Builder builder = new AlertDialog.Builder(this.Activity);

			builder.SetTitle (Labels.GetLoggedUserLabel (Labels.LabelEnum.ChooseAttendee))
				.SetView (this.mView);

			return builder.Create();
		}

		public override void OnPause () {
			this.Dismiss ();

			base.OnPause ();
		}
	}
}