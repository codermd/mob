using System;

using Android.Support.V4.App;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Content;
using Android.Widget;
using Android.Util;
using Mxp.Core.Business;
using Mxp.Core.Helpers;
using Mxp.Droid.Adapters;

namespace Mxp.Droid.Fragments
{
	public class SegmentDetailsDialogFragment : Android.Support.V4.App.DialogFragment
	{
		#pragma warning disable 0414
		private static readonly string TAG = typeof(SegmentDetailsDialogFragment).Name;
		#pragma warning restore 0414

		private readonly AllowanceSegment mAllowanceSegment;
		private View mView;
		private event EventHandler<EventArgsObject<AllowanceSegment>> mOnClickHandler;

		public SegmentDetailsDialogFragment (AllowanceSegment allowanceSegment, EventHandler<EventArgsObject<AllowanceSegment>> onClickHandler) {
			this.mAllowanceSegment = allowanceSegment;
			this.mOnClickHandler = onClickHandler;
		}

		public override void OnCreate (Bundle savedInstanceState) {
			base.OnCreate (savedInstanceState);

			this.mView = this.Activity.LayoutInflater.Inflate (Resource.Layout.Allowance_segment_details, null);
			ListView listView = this.mView.FindViewById<ListView> (Resource.Id.List);
			FieldsAdapter fieldsAdapter = new FieldsAdapter (this.ChildFragmentManager, this.Activity, this.mAllowanceSegment.GetAllowanceFields ());
			listView.Adapter = fieldsAdapter;
			listView.ItemClick += (object sender, AdapterView.ItemClickEventArgs e) => {
				fieldsAdapter.OnListItemClick (listView, e.View, e.Position, e.Id);
			};
		}

		public override Dialog OnCreateDialog (Bundle savedInstanceState) {
			return new AlertDialog.Builder(this.Activity)
				.SetTitle (Labels.GetLoggedUserLabel (Labels.LabelEnum.SegmentDetails))
				.SetView (this.mView)
				.SetPositiveButton (Labels.GetLoggedUserLabel (Labels.LabelEnum.Done), (object sender, DialogClickEventArgs e) => {
					this.mOnClickHandler (this, new EventArgsObject<AllowanceSegment> (this.mAllowanceSegment));
				})
				.Create ();
		}

		public override void OnPause () {
			this.Dismiss ();

			base.OnPause ();
		}
	}
}