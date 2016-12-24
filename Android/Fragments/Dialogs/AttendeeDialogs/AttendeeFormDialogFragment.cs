using System;
using Android.Views;
using Mxp.Core.Business;
using Android.OS;
using Android.App;
using Android.Widget;
using Android.Content;
using Mxp.Core.Helpers;
using Mxp.Droid.Adapters;
using Mxp.Droid.Utils;

namespace Mxp.Droid.Fragments
{
	public class AttendeeFormDialogFragment : Android.Support.V4.App.DialogFragment
	{
		#pragma warning disable 0414
		private static readonly string TAG = typeof(AttendeeFormDialogFragment).Name;
		#pragma warning restore 0414

		private event EventHandler<EventArgsObject<Attendee>> mOnClickHandler;

		private View mView;
		private Attendee mAttendee;
		private String mTitle;

		public AttendeeFormDialogFragment (String title, AttendeeTypeEnum type, EventHandler<EventArgsObject<Attendee>> onClickHandler) {
			this.mAttendee = new Attendee (type);
			this.mTitle = title;
			this.mOnClickHandler = onClickHandler;
		}

		public override void OnCreate (Bundle savedInstanceState) {
			base.OnCreate (savedInstanceState);

			this.mView = this.Activity.LayoutInflater.Inflate (Resource.Layout.form_list, null);
			ListView listView = (ListView) this.mView.FindViewById (Resource.Id.list);
			FieldsAdapter fieldsAdapter = new FieldsAdapter (this.FragmentManager, this.Activity, this.mAttendee.FormFields);
			listView.Adapter = fieldsAdapter;
			listView.ItemClick += (object sender, AdapterView.ItemClickEventArgs e) => fieldsAdapter.OnListItemClick (listView, e.View, e.Position, e.Id);
		}

		public override Dialog OnCreateDialog(Bundle savedInstanceState) {
			AlertDialog.Builder builder = new AlertDialog.Builder(this.Activity);

			builder.SetTitle (this.mTitle)
				.SetView (this.mView)
				.SetNegativeButton (Labels.GetLoggedUserLabel (Labels.LabelEnum.Cancel), (object sender, Android.Content.DialogClickEventArgs e) => { })
				.SetPositiveButton (Labels.GetLoggedUserLabel (Labels.LabelEnum.Done), (object sender, Android.Content.DialogClickEventArgs e) => { });

			return builder.Create();
		}

		public override void OnStart () {
			base.OnStart ();

			AlertDialog dialog = (AlertDialog) this.Dialog;

			dialog.GetButton ((int) DialogButtonType.Positive).Click += (object sender, EventArgs e) => {
				try {
					this.mAttendee.TryValidate ();
				} catch (Exception error) {
					Android.Support.V4.App.DialogFragment errorDialogFragment = BaseDialogFragment.NewInstance (this.Activity, BaseDialogFragment.DialogTypeEnum.ErrorDialog, error is ValidationError ? ((ValidationError)error).Verbose : Mxp.Core.Services.Service.NoConnectionError);
					errorDialogFragment.Show (this.ChildFragmentManager, null);
					return;
				}

				if (this.mAttendee.Type == AttendeeTypeEnum.HCP || this.mAttendee.Type == AttendeeTypeEnum.HCO)
					this.StartAttendeePickerDialog ();
				else
					this.mOnClickHandler (this, new EventArgsObject<Attendee> (this.mAttendee));
			};
		}

		public override void OnPause () {
			this.Dismiss ();

			base.OnPause ();
		}

		private void StartAttendeePickerDialog () {
			Attendees attendees = new Attendees ();

			this.Activity.InvokeActionAsync (async () => await attendees.FetchRelatedAttendeesAsync (this.mAttendee), () => {
				Android.Support.V4.App.DialogFragment dialogFragment = new AttendeePickerDialogFragment (attendees, (object resender, EventArgsObject<Attendee> re) => {
					((Android.Support.V4.App.DialogFragment)resender).Dismiss ();
					this.mOnClickHandler (this, re);
				});
				dialogFragment.Show (this.FragmentManager, null);
			}, this);
		}
	}
}