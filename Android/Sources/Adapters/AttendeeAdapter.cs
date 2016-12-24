using System;
using Android.Widget;
using Android.Views;
using Mxp.Droid.Helpers;
using Android.App;
using Mxp.Core.Business;
using Android.Support.Design.Widget;
using System.Threading.Tasks;
using Android.Graphics;

namespace Mxp.Droid.Adapters
{
	public class AttendeeAdapter : BaseAdapter<Attendee>
	{
		#pragma warning disable 0414
		private static readonly string TAG = typeof(AttendeeAdapter).Name;
		#pragma warning restore 0414

		private Attendees mAttendees;
		private Activity mActivity;
		private bool mIsGTPEnabled;
		private bool mIsInSearch;

		public AttendeeAdapter (Activity activity, Attendees attendees, bool forceDisableGTP = false, bool isInSearch = false) : base () {
			this.mActivity = activity;
			this.mAttendees = attendees;
			this.mIsGTPEnabled = !forceDisableGTP && Preferences.Instance.IsGTPEnabled;
			this.mIsInSearch = isInSearch;
		}

		public override long GetItemId (int position) {
			return position;
		}

		public override Attendee this[int index] {
			get {
				return this.mAttendees [index];
			}
		}

		public override int Count {
			get {
				return this.mAttendees.Count;
			}
		}

		public override View GetView (int position, View convertView, ViewGroup parent) {
			AttendeeViewHolder viewHolder = null;

			if (convertView == null || convertView.Tag == null) {
				convertView = this.mActivity.LayoutInflater.Inflate (Resource.Layout.List_attendees_item, parent, false);
				viewHolder = new AttendeeViewHolder (convertView, this);
				convertView.Tag = viewHolder;
			} else {
				viewHolder = convertView.Tag as AttendeeViewHolder;
			}

			viewHolder.BindView (this [position]);

			return convertView;
		}

		private class AttendeeViewHolder : ViewHolder<Attendee> {
			private AttendeeAdapter mAttendeeAdapter;
			private Attendee attendee;

			private CheckBox checkBox;
			private TextView name;
			private TextView company;
			private TextView location;
			private TextView amount;
			private ImageView icon;

			public AttendeeViewHolder (View convertView, AttendeeAdapter attendeeAdapter) {
				this.mAttendeeAdapter = attendeeAdapter;

				this.checkBox = convertView.FindViewById<CheckBox> (Resource.Id.CheckBox);

				if (!this.mAttendeeAdapter.mIsGTPEnabled) {
					((ViewGroup)this.checkBox.Parent).RemoveView (this.checkBox);
					this.checkBox = null;
				}

				this.name = convertView.FindViewById<TextView> (Resource.Id.Name);
				this.company = convertView.FindViewById<TextView> (Resource.Id.Company);
				this.location = convertView.FindViewById<TextView> (Resource.Id.Location);
				this.amount = convertView.FindViewById<TextView> (Resource.Id.Amount);
				this.icon = convertView.FindViewById<ImageView> (Resource.Id.Icon);
			}

			public override void BindView (Attendee attendee) {
				if (this.checkBox != null) {
					this.checkBox.CheckedChange -= CheckedChangeHandler;
					this.attendee = attendee;
					this.checkBox.Checked = attendee.IsShown;
					this.checkBox.CheckedChange += CheckedChangeHandler;
				}

				this.name.Text = attendee.VName;

				if (this.mAttendeeAdapter.mIsInSearch)
					this.SetSearchMode (attendee);
				else {
					this.company.Text = attendee.CompanyName;
					this.location.Text = attendee.VLocation;
					this.amount.Text = attendee.VAmount;

					this.SetIcon (attendee);
				}
			}

			private void SetSearchMode (Attendee attendee) {
				this.icon.Visibility = ViewStates.Gone;

				switch (attendee.Type) {
					case AttendeeTypeEnum.HCP:
						this.company.Text = attendee.Reference;
						this.location.Text = attendee.VHCPSearchLocation;
						this.company.SetTypeface (this.company.Typeface, TypefaceStyle.Italic);
						this.location.SetTypeface (this.location.Typeface, TypefaceStyle.Italic);
						break;
					default:
						break;
				}
			}

			private void CheckedChangeHandler (object sender, CompoundButton.CheckedChangeEventArgs e) {
				TaskConfigurator.Create ()
				                .SetCanShowErrorDialog (false)
				                .Catch (error => {
									this.checkBox.CheckedChange -= CheckedChangeHandler;
									this.checkBox.Checked = !e.IsChecked;
									this.checkBox.CheckedChange += CheckedChangeHandler;

									Toast.MakeText (this.mAttendeeAdapter.mActivity, error, ToastLength.Long).Show ();
								})
				                .Start (this.attendee.ChangeIsShownAsync ());
			}

			private void SetIcon (Attendee attendee) {
				switch (attendee.Type) {
					case AttendeeTypeEnum.Business:
						this.icon.SetImageResource (Resource.Drawable.ic_business_attendee);
						this.icon.Visibility = ViewStates.Visible;
						break;
					case AttendeeTypeEnum.Employee:
						this.icon.SetImageResource (Resource.Drawable.ic_employee_attendee);
						this.icon.Visibility = ViewStates.Visible;
						break;
					case AttendeeTypeEnum.Spouse:
						this.icon.SetImageResource (Resource.Drawable.ic_personal_attendee);
						this.icon.Visibility = ViewStates.Visible;
						break;
					case AttendeeTypeEnum.HCO:
					case AttendeeTypeEnum.HCP:
					case AttendeeTypeEnum.UCP:
						this.icon.SetImageResource (Resource.Drawable.ic_medical_attendee);
						this.icon.Visibility = ViewStates.Visible;
						break;
					default:
						this.icon.Visibility = ViewStates.Gone;
						break;
				}
			}
		}
	}
}