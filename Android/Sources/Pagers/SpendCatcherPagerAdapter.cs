using System;
using Android.Support.V4.View;
using Android.Views;
using Android.Support.V4.App;
using Android.Support.Design.Widget;
using Android.Widget;
using Mxp.Droid.Fragments;
using Mxp.Core.Business;

namespace Mxp.Droid.Adapters
{
	public class SpendCatcherPagerAdapter : FragmentPagerAdapter
	{
		public SpendCatcherExpenses SpendCatcherExpenses;

		public SpendCatcherPagerAdapter (FragmentManager fm, SpendCatcherExpenses spendCatcherExpenses) : base (fm) {
			this.SpendCatcherExpenses = spendCatcherExpenses;
		}

		public override int Count {
			get {
				return this.SpendCatcherExpenses.Count;
			}
		}

		public override Fragment GetItem (int position) {
			return SharingSpendCatcherFragment.NewInstance (position);
		}
	}
}