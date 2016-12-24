using System;
using Android.Support.V7.App;
using Android.OS;
using Mxp.Core.Business;
using Android.Support.V4.View;
using Mxp.Droid.Helpers;
using Android.Views;
using Mxp.Droid.Adapters;
using Mxp.Droid.Fragments;
using Android.Support.V4.App;
using System.Linq;
using Android.Content;
using Mxp.Droid.Utils;
using Android.Support.Design.Widget;
using Android.Support.V7.Widget;

namespace Mxp.Droid
{		
	public class TravelDetailsActivity : BaseActivity
	{
		#pragma warning disable 0414
		private static readonly string TAG = typeof(TravelDetailsActivity).Name;
		#pragma warning restore 0414

		public static readonly string EXTRA_TRAVEL_ID = "com.sagacify.mxp.travel.id";

		private const int errorDialogRequestCode = 0;

		private TravelApproval mTravelApproval;

		protected override void OnCreate (Bundle savedInstanceState) {
			int travelId = this.Intent.GetIntExtra (EXTRA_TRAVEL_ID, -1);

			if (travelId != -1)
				this.mTravelApproval = LoggedUser.Instance.TravelApprovals.Single (approval => approval.Travel.Id == travelId);
			
			base.OnCreate (savedInstanceState);

			this.SetContentView (Resource.Layout.Pager_generic);

			Toolbar toolbar = this.FindViewById<Toolbar> (Resource.Id.Toolbar);
			this.SetSupportActionBar (toolbar);

			this.SupportActionBar.SetDisplayHomeAsUpEnabled (true);

			this.Title = this.mTravelApproval.VDetailsBarTitle;

			TabLayout tabLayout = this.FindViewById<TabLayout> (Resource.Id.TabLayout);
			ViewPager viewPager = this.FindViewById<ViewPager> (Resource.Id.ViewPager);
			viewPager.Adapter = new TravelDetailsFragmentPagerAdapter (this.SupportFragmentManager, travelId);
			tabLayout.SetupWithViewPager (viewPager);
		}

		public override bool OnCreateOptionsMenu (IMenu menu) {
			this.MenuInflater.Inflate (Resource.Menu.Approval_travel_menu, menu);

			return base.OnCreateOptionsMenu (menu);
		}

		public override bool OnOptionsItemSelected (IMenuItem item) {
			switch (item.ItemId) {
				case Android.Resource.Id.Home:
					this.Finish ();
					return true;
				case Resource.Id.Action_submit:
					this.SubmitApprovalTravel ();
					return true;
			}

			return base.OnOptionsItemSelected (item);
		}

		private void SubmitApprovalTravel () {
			DialogFragment dialogFragment = new StringDialogFragment (this.mTravelApproval.Comment, Resource.Layout.Dialog_string_multilines, Resource.Id.EditText, (object sender, DialogArgsObject<string> e) => {
				if (e.ButtonType == DialogButtonType.Neutral)
					return;

				this.mTravelApproval.Comment = e.Object;
				this.mTravelApproval.AcceptedStatus = e.ButtonType == DialogButtonType.Positive;

				this.InvokeActionAsync (this.mTravelApproval.SubmitAsync);
			}) {
				mTitle = Labels.GetLoggedUserLabel (Labels.LabelEnum.Select),
				mNegativeTextButton = Labels.GetLoggedUserLabel (Labels.LabelEnum.Reject),
				mNeutralTextButton = Labels.GetLoggedUserLabel (Labels.LabelEnum.Cancel),
				mPositiveTextButton = Labels.GetLoggedUserLabel (Labels.LabelEnum.Accept)
			};
			dialogFragment.Show (this.SupportFragmentManager, null);
		}
	}
}