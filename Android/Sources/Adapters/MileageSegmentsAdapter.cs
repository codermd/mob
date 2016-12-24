using System;

using Android.Widget;

using Mxp.Core.Business;
using Android.App;
using Android.Views;
using Mxp.Droid.Helpers;
using Mxp.Core.Services.Google;
using Mxp.Droid.Utils;
using Mxp.Core.Helpers;
using Android.Locations;
using Mxp.Core.Services;
using Android.Support.V4.App;

namespace Mxp.Droid.Adapters
{
	public class MileageSegmentsAdapter<P> : AbstractSectionAdapter<WrappedObject, P> where P : BaseSectionAdapter<WrappedObject>
	{
		#pragma warning disable 0414
		private static readonly string TAG = typeof(MileageSegmentsAdapter<P>).Name;
		#pragma warning restore 0414

		private MileageSegments mMileageSegments { get; set; }

		private Location _mCurrentLocation;
		public Location CurrentLocation {
			get {
				return this._mCurrentLocation;
			}
			set {
				this._mCurrentLocation = value;

				this.mLastLocationEvent?.Invoke (this, new EventArgsObject<Location> (this._mCurrentLocation));
			}
		}
		public event EventHandler<EventArgsObject<Location>> mLastLocationEvent;

		public MileageSegmentsAdapter (P parentAdapter, Activity activity, MileageSegments mileageSegments, string title) : base (parentAdapter, activity, title) {
			this.mMileageSegments = mileageSegments;
		}

		public override long GetItemId (int position) {
			return position;
		}

		public override WrappedObject this[int index] {
			get {
				return index == this.mMileageSegments.Count ? null : new WrappedObject (this.mMileageSegments [index]);
			}
		}

		public override int Count {
			get {
				return this.mMileageSegments.Count + (this.mMileageSegments.CanManage ? 1 : 0);
			}
		}

		public override View GetView (int position, View convertView, ViewGroup parent) {
			Java.Lang.Object viewHolder = null;

			if (convertView == null || convertView.Tag == null || (!(convertView.Tag is MileageSegmentViewHolder<P>) || !(convertView.Tag is MileageSegmentButtonsViewHolder<P>))) {
				if (this [position] == null) {
					convertView = this.Activity.LayoutInflater.Inflate (Resource.Layout.Mileage_segments_button, parent, false);
					viewHolder = new MileageSegmentButtonsViewHolder<P> (convertView, this);
				} else {
					convertView = this.Activity.LayoutInflater.Inflate (Resource.Layout.List_mileage_segments_item, parent, false);
					viewHolder = new MileageSegmentViewHolder<P> (convertView, this);
				}

				convertView.Tag = viewHolder;
			} else
				viewHolder = convertView.Tag as Java.Lang.Object;

			if (viewHolder is MileageSegmentViewHolder<P>)
				((MileageSegmentViewHolder<P>)viewHolder).BindView (position, this [position].GetInstance<MileageSegment> ());

			return convertView;
		}

		private class MileageSegmentViewHolder<D> : Java.Lang.Object where D : BaseSectionAdapter<WrappedObject> {
			private AutoCompleteTextView mAutoCompleteTextView;
			private ImageButton mDeleteButton;
			private ImageButton mCurrentLocationButton;

			private MileageSegmentsAdapter<D> mAdapter;
			private int position;

			public MileageSegmentViewHolder (View convertView, MileageSegmentsAdapter<D> adapter) {
				this.mAdapter = adapter;

				bool canManage = this.mAdapter [this.position].GetInstance<MileageSegment> ().GetCollectionParent<MileageSegments, MileageSegment> ().CanManage;

				this.mAutoCompleteTextView = convertView.FindViewById<AutoCompleteTextView> (Resource.Id.AutoCompleteTextView);

				if (canManage) {
					this.mAutoCompleteTextView.Adapter = new LocationsAdapter (this.mAdapter.Activity, GoogleService.PlaceTypeEnum.All);
					this.mAutoCompleteTextView.ItemClick += async (object sender, AdapterView.ItemClickEventArgs e) => {
						MileageSegment segment = this.mAdapter [this.position].GetInstance<MileageSegment> ();

						WrappedObject wrappedObject = e.Parent.GetItemAtPosition (e.Position).Cast<WrappedObject> ();

						if (wrappedObject.IsInstanceOf<Prediction> ())
							await segment.FetchLocationsAsync (wrappedObject.GetInstance<Prediction> ()).StartAsync (TaskConfigurator.Create ((FragmentActivity)this.mAdapter.Activity));
						else if (wrappedObject.IsInstanceOf<MileageSegment> ())
							this.mAdapter.mMileageSegments.ChangeItemAt (this.position, wrappedObject.GetInstance<MileageSegment> ());

						this.mAdapter.NotifyDataSetChanged ();
					};
				}
					
				this.mDeleteButton = convertView.FindViewById<ImageButton> (Resource.Id.DeleteButton);

				if (canManage)
					this.mDeleteButton.Click += (object sender, EventArgs e) => {
						this.mAdapter.mMileageSegments.RemoveItemAt (this.position);						
						this.mAdapter.NotifyDataSetChanged ();
					};
				else
					this.mDeleteButton.Visibility = ViewStates.Gone;

				this.mCurrentLocationButton = convertView.FindViewById<ImageButton> (Resource.Id.CurrentLocationButton);
				if (canManage)
					this.mCurrentLocationButton.Click += (object sender, EventArgs e) => {
						if (this.mAdapter.CurrentLocation != null)
							this.GetCurrentLocation (this.mAdapter.CurrentLocation);
						else
							this.mAdapter.mLastLocationEvent += (object resender, EventArgsObject<Location> re) => {
								this.GetCurrentLocation (re.Object);
							};
					};
				else
					this.mCurrentLocationButton.Visibility = ViewStates.Gone;
			}

			public void GetCurrentLocation (Location currentLocation) {
				if (currentLocation == null) {
					Toast.MakeText (this.mAdapter.Activity, "Please enable your location.", ToastLength.Short).Show ();
					return;
				}

				MileageSegment segment = this.mAdapter [this.position].GetInstance<MileageSegment> ();

				segment.SetCurrentLocation (currentLocation.Latitude, currentLocation.Longitude);
			}

			public void BindView (int position, MileageSegment mileageSegment) {
				this.position = position;

				this.mDeleteButton.Visibility = this.mAdapter.mMileageSegments.CanRemove ? ViewStates.Visible : ViewStates.Invisible;

				this.mAutoCompleteTextView.SetText (mileageSegment.LocationAliasName, false);
			}
		}

		private class MileageSegmentButtonsViewHolder<D> : Java.Lang.Object where D : BaseSectionAdapter<WrappedObject> {
			private Button newButton;
			private ImageButton returningButton;

			private MileageSegmentsAdapter<D> adapter;

			public MileageSegmentButtonsViewHolder (View convertView, MileageSegmentsAdapter<D> adapter) {
				this.adapter = adapter;

				this.newButton = convertView.FindViewById<Button> (Resource.Id.AddSegment);
				this.newButton.Text = "+ " + Labels.GetLoggedUserLabel (Labels.LabelEnum.Location);
				this.returningButton = convertView.FindViewById<ImageButton> (Resource.Id.ReturningSegment);

				this.newButton.Click += (object sender, EventArgs e) => {
					this.adapter.mMileageSegments.AddNewItem ();
//					if (!this.adapter.mMileageSegments.CanAdd)
//						this.newButton.Enabled = false;
					this.adapter.NotifyDataSetChanged ();
				};

				this.returningButton.Click += (object sender, EventArgs e) => {
					if (this.adapter.mMileageSegments.IsFirstEqualsLastSegment)
						return;

					this.adapter.mMileageSegments.AddReturningItem ();
//					if (!this.adapter.mMileageSegments.CanAdd)
//						this.newButton.Enabled = false;
					this.adapter.NotifyDataSetChanged ();
				};
			}
		}
	}
}