using Android.Support.V4.App;
using Java.Lang;
using Mxp.Core.Business;
using Mxp.Droid.Fragments;

namespace Mxp.Droid.Adapters
{
	public class ExpensesFragmentPagerAdapter : FragmentPagerAdapter
	{
		public ExpensesFragmentPagerAdapter (FragmentManager fm) : base (fm) {

		}

		public override int Count {
			get {
				return Preferences.Instance.IsSpendCatcherEnable ? 3 : 2;
			}
		}

		public override Fragment GetItem (int position) {
			switch (position) {
				case 0:
					return ExpensesListFragment.NewInstance (Expenses.ExpensesTypeEnum.Business);
				case 1:
					return ExpensesListFragment.NewInstance (Expenses.ExpensesTypeEnum.Private);
				case 2:
					return new SpendCatcherExpensesListFragment ();
				default:
					return null;
			}
		}

		public override ICharSequence GetPageTitleFormatted (int position) {
			switch (position) {
				case 0:
					return new String (Labels.GetLoggedUserLabel (Labels.LabelEnum.BusinessDistance));
				case 1:
					return new String (Labels.GetLoggedUserLabel (Labels.LabelEnum.Private));
				case 2:
					return new String (Labels.GetLoggedUserLabel (Labels.LabelEnum.SpendCatcher));
				default:
					return null;
			}
		}
	}
}