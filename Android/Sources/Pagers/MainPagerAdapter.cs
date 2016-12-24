using Android.Support.V4.App;
using Mxp.Droid.Fragments;
using Mxp.Core.Business;
using Java.Lang;

namespace Mxp.Droid.Adapters
{
	public class MainPagerAdapter : FragmentPagerAdapter
	{
		public MainPagerAdapter (FragmentManager fm) : base (fm) {
			
		}

		public override int Count {
			get {
				return 4;
			}
		}

		public override Fragment GetItem (int position) {
			switch (position) {
				case 0:
					return new ExpensesPagerFragment ();
				case 1:
					return new ReportsPagerFragment ();
				case 2:
					return new ApprovalsPagerFragment ();
				case 3:
					return new SettingsFragment ();
				default:
					return new Fragment ();
			}
		}

		public override ICharSequence GetPageTitleFormatted (int position) {
			switch (position) {
				case 0:
					return new String (Labels.GetLoggedUserLabel (Labels.LabelEnum.Expenses));
				case 1:
					return new String (Labels.GetLoggedUserLabel (Labels.LabelEnum.Reports));
				case 2:
					return new String (Labels.GetLoggedUserLabel (Labels.LabelEnum.Approvals));
				case 3:
					return new String (Labels.GetLoggedUserLabel (Labels.LabelEnum.Settings));
				default:
					return null;
			}
		}
	}
}