using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;

using Mxp.Core.Business;
using Mxp.Droid.Fragments;
using Mxp.Core.Services.Responses;

namespace Mxp.Droid.Adapters
{
	public class ApprovalsFragmentPagerAdapter : FragmentPagerAdapter
	{
		public static readonly int ITEMS = 2;

		public ApprovalsFragmentPagerAdapter (FragmentManager fm) : base (fm) {

		}

		public override int Count {
			get {
				return ITEMS;
			}
		}

		public override Fragment GetItem (int position) {
			switch (position) {
				case 0:
					return new ReportApprovalsListFragment ();
				case 1:
					return new TravelApprovalsListFragment ();
				default:
					return null;
			}
		}

		public override Java.Lang.ICharSequence GetPageTitleFormatted (int position) {
			switch (position) {
				case 0:
					return new Java.Lang.String (LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.Reports));
				case 1:
					return new Java.Lang.String (LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.Travels));
				default:
					return null;
			}
		}
	}
}