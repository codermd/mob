using System;
using Android.Support.V4.App;
using Mxp.Droid.Fragments;
using Mxp.Core.Business;
using Android.Support.V4.View;
using Android.Support.V7.App;
using Mxp.Droid.Helpers;
using Android.Content;
using Android.Support.Design.Widget;

namespace Mxp.Droid.Adapters
{
	public class ExpenseDetailsFragmentPagerAdapter : FragmentPagerAdapter
	{
		private ExpenseItem mExpenseItem;
		private Expense mExpense {
			get {
				return this.mExpenseItem.ParentExpense;
			}
		}

		private TabLayout mTabLayout;

		public ExpenseDetailsFragmentPagerAdapter (TabLayout tabLayout, ExpenseItem expenseItem, FragmentManager fm) : base (fm) {
			this.mExpenseItem = expenseItem;
			this.mTabLayout = tabLayout;
		}

		public override int Count {
			get {
				return 1
					+ (this.mExpense.CanShowReceipts ? 1 : 0)
					+ (this.mExpenseItem.CanShowAttendees ? 1 : 0)
					+ (this.mExpense is Mileage ? 1 : 0);
			}
		}

		public override Fragment GetItem (int position) {
			switch (position) {
				case 0:
					if (this.mExpense is Mileage)
						return MileageDetailsListFragment.NewInstance ();
					else if (this.mExpense is Allowance)
						return AllowanceDetailsListFragment.NewInstance ();
					else
						return ExpenseDetailsListFragment.NewInstance ();
				case 1:
					if (this.mExpense.CanShowReceipts) {
						return ExpenseReceiptsFragment.NewInstance ();
					} else
						if (this.mExpenseItem.CanShowAttendees)
							return ExpenseAttendeesListFragment.NewInstance ();
						else 
							return MileageMapFragment.NewInstance ();
				case 2:
					if (this.mExpenseItem.CanShowAttendees)
						return ExpenseAttendeesListFragment.NewInstance ();
					else 
						return MileageMapFragment.NewInstance ();
				case 3:
					return MileageMapFragment.NewInstance ();
				default:
					return null;
			}
		}

		public override int GetItemPosition (Java.Lang.Object @object) {
			if (@object is INotifyFragmentDataSetRefreshed)
				((INotifyFragmentDataSetRefreshed) @object).NotifyDataSetRefreshed ();
			
			return base.GetItemPosition (@object);
		}

		public override long GetItemId (int position) {
			return position;
		}

		public override void NotifyDataSetChanged () {
			base.NotifyDataSetChanged ();

			this.SynchronizeActionBar ();
		}

		public void SynchronizeActionBar () {
			this.mTabLayout.RemoveAllTabs ();

			this.mTabLayout.AddTab (this.mTabLayout.NewTab ()
				.SetText (Labels.GetLoggedUserLabel (Labels.LabelEnum.Details)));
			if (this.mExpense.CanShowReceipts)
				this.mTabLayout.AddTab (this.mTabLayout.NewTab ()
					.SetText (Labels.GetLoggedUserLabel (Labels.LabelEnum.Receipts)));
			if (this.mExpenseItem.CanShowAttendees)
				this.mTabLayout.AddTab (this.mTabLayout.NewTab ()
					.SetText (Labels.GetLoggedUserLabel (Labels.LabelEnum.Attendees)));
			if (this.mExpense is Mileage)
				this.mTabLayout.AddTab (this.mTabLayout.NewTab ()
					.SetText (Labels.GetLoggedUserLabel (Labels.LabelEnum.ShowMap)));
		}
	}
}