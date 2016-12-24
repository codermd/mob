using System;
using Mxp.Droid.Helpers;
using Mxp.Core.Business;
using Android.App;
using Android.Views;
using Android.Widget;
using Mxp.Droid.Fragments;
using Mxp.Core.Helpers;
using Android.Util;
using Mxp.Droid.Utils;
using Android.Support.V4.App;

namespace Mxp.Droid.Adapters
{
	public class AllowanceSegmentsAdapter<P> : AbstractSectionAdapter<WrappedObject, P> where P : BaseSectionAdapter<WrappedObject>
	{
		private static readonly string TAG = typeof(AllowanceSegmentsAdapter<P>).Name;

		public AllowanceSegments mAllowanceSegments { get; set; }

		public AllowanceSegmentsAdapter (P parentAdapter, Activity activity, AllowanceSegments allowanceSegments, string title) : base (parentAdapter, activity, title) {
			this.mAllowanceSegments = allowanceSegments;
		}

		public override long GetItemId (int position) {
			return position;
		}

		public override WrappedObject this[int index] {
			get {
				return new WrappedObject (this.mAllowanceSegments [index]);
			}
		}

		public override int Count {
			get {
				return this.mAllowanceSegments.Count;
			}
		}

		public override View GetView (int position, View convertView, ViewGroup parent) {
			AllowanceSegmentViewHolder viewHolder = null;

			if (convertView == null || convertView.Tag == null || !(convertView.Tag is AllowanceSegmentViewHolder)) {
				convertView = this.Activity.LayoutInflater.Inflate (Resource.Layout.List_allowance_segments_item, parent, false);
				viewHolder = new AllowanceSegmentViewHolder (convertView);
				convertView.Tag = viewHolder;
			} else {
				viewHolder = convertView.Tag as AllowanceSegmentViewHolder;
			}

			viewHolder.BindView (this [position].GetInstance<AllowanceSegment> ());

			return convertView;
		}

		private class AllowanceSegmentViewHolder : ViewHolder<AllowanceSegment> {
			private TextView CommentTextView;
			private TextView AmountTextView;
			private TextView DateFromTextView;
 			private TextView DateToTextView;

			private CheckBox breakfastIcon;
			private CheckBox lunchIcon;
			private CheckBox dinnerIcon;
			private CheckBox lodgingIcon;
			private CheckBox infoIcon;
			private CheckBox worknightIcon;

			private ImageView flagIcon;

			public AllowanceSegmentViewHolder (View convertView) {
				this.CommentTextView = convertView.FindViewById<TextView> (Resource.Id.Comment);
				this.AmountTextView = convertView.FindViewById<TextView> (Resource.Id.Amount);
				this.DateFromTextView = convertView.FindViewById<TextView> (Resource.Id.DateFrom);
				this.DateToTextView = convertView.FindViewById<TextView> (Resource.Id.DateTo);

				this.breakfastIcon = convertView.FindViewById<CheckBox> (Resource.Id.BreakfastIcon);
				this.lunchIcon = convertView.FindViewById<CheckBox> (Resource.Id.LunchIcon);
				this.dinnerIcon = convertView.FindViewById<CheckBox> (Resource.Id.DinnerIcon);
				this.lodgingIcon = convertView.FindViewById<CheckBox> (Resource.Id.LodgingIcon);
				this.infoIcon = convertView.FindViewById<CheckBox> (Resource.Id.InfoIcon);
				this.worknightIcon = convertView.FindViewById<CheckBox> (Resource.Id.WorknightIcon);

				this.flagIcon = convertView.FindViewById<ImageView> (Resource.Id.FlagIcon);
			}

			public override void BindView (AllowanceSegment allowanceSegment) {
				this.CommentTextView.Text = allowanceSegment.Comment;
				this.AmountTextView.Text = allowanceSegment.VAmount;
				this.DateFromTextView.Text = allowanceSegment.VDateFrom;
				this.DateToTextView.Text = allowanceSegment.VDateTo;

				this.ConfigureIcons (allowanceSegment);
				this.ConfigureFlag (allowanceSegment);
			}

			private void ConfigureIcons (AllowanceSegment allowanceSegment) {
				this.breakfastIcon.Visibility = allowanceSegment.CanShowBreakfast ? ViewStates.Visible : ViewStates.Gone;
				this.breakfastIcon.Checked = allowanceSegment.CanShowBreakfast && allowanceSegment.Breakfast;

				this.lunchIcon.Visibility = allowanceSegment.CanShowLunch ? ViewStates.Visible : ViewStates.Gone;
				this.lunchIcon.Checked = allowanceSegment.CanShowLunch && allowanceSegment.Lunch;

				this.dinnerIcon.Visibility = allowanceSegment.CanShowDinner ? ViewStates.Visible : ViewStates.Gone;
				this.dinnerIcon.Checked = allowanceSegment.CanShowDinner && allowanceSegment.Dinner;

				this.lodgingIcon.Visibility = allowanceSegment.CanShowLodging ? ViewStates.Visible : ViewStates.Gone;
				this.lodgingIcon.Checked = allowanceSegment.CanShowLodging && allowanceSegment.Lodging;

				this.infoIcon.Visibility = allowanceSegment.CanShowInfo ? ViewStates.Visible : ViewStates.Gone;
				this.infoIcon.Checked = allowanceSegment.CanShowInfo && allowanceSegment.Info;

				this.worknightIcon.Visibility = allowanceSegment.CanShowWorkNight ? ViewStates.Visible : ViewStates.Gone;
				this.worknightIcon.Checked = allowanceSegment.CanShowWorkNight && allowanceSegment.WorkNight;
			}

			private void ConfigureFlag (AllowanceSegment allowanceSegment) {
				if (allowanceSegment.Country != null) {
					try {
						int resource = (int)typeof(Resource.Drawable).GetField(allowanceSegment.Country.VResourceName).GetValue(null);
						this.flagIcon.SetImageResource (resource);
					} catch (Exception e) {
						Log.Info (TAG, "Failure to get drawable id.", e);
						this.flagIcon.SetImageResource (Resource.Drawable.NoFlag);
					}
				} else
					this.flagIcon.SetImageResource (Resource.Drawable.NoFlag);
			}
		}

		public override void OnListItemClick (ListView listView, View view, int position, long id) {
			Android.Support.V4.App.DialogFragment dialogFragment = new SegmentDetailsDialogFragment (this [position].GetInstance<AllowanceSegment> (), (object sender, EventArgsObject<AllowanceSegment> e) => {
				if (!this [position].GetInstance<AllowanceSegment> ().IsChanged)
					return;

				this.RefreshAsync ();
			});
			dialogFragment.Show (((IChildFragmentManager)this.ParentAdapter).GetChildFragmentManager (), null);
		}

		public void RefreshAsync () {
			((FragmentActivity)this.Activity).InvokeActionAsync (this.mAllowanceSegments.GetParentModel<Allowance> ().RecalculateAsync,
				() => {
					this.NotifyDataSetChanged ();
				});
		}
	}
}