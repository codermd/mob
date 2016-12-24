using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;

using Mxp.Core.Business;
using Mxp.Droid.Fragments;

namespace Mxp.Droid.Adapters
{
	public class ReportsFragmentPagerAdapter : FragmentPagerAdapter
	{
		public static readonly int ITEMS = 3;

		public ReportsFragmentPagerAdapter (FragmentManager fm) : base (fm) {

		}

		public override int Count {
			get {
				return ITEMS;
			}
		}

		public override Fragment GetItem (int position) {
			return ReportsListFragment.NewInstance (((Reports.ReportTypeEnum) position));
		}

		public override Java.Lang.ICharSequence GetPageTitleFormatted (int position) {
			switch ((Reports.ReportTypeEnum)position) {
				case Reports.ReportTypeEnum.Draft:
					return new Java.Lang.String (Labels.GetLoggedUserLabel (Labels.LabelEnum.Draft));
				case Reports.ReportTypeEnum.Open:
					return new Java.Lang.String (Labels.GetLoggedUserLabel (Labels.LabelEnum.Open));
				case Reports.ReportTypeEnum.Closed:
					return new Java.Lang.String (Labels.GetLoggedUserLabel (Labels.LabelEnum.Closed));
				default:
					return null;
			}
		}
	}
}