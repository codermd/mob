using System;
using Android.Support.V4.View;
using Android.Support.V4.App;
using Android.OS;
using Android.Views;
using Android.Util;
using Mxp.Droid.Adapters;
using Android.Content;
using Mxp.Core.Business;
using Android.Widget;
using Mxp.Core.Utils;
using Mxp.Core.Helpers;
using Mxp.Droid.Utils;

namespace Mxp.Droid.Fragments
{
	public class ExpensesPagerFragment : Fragment, BaseDialogFragment.IDialogClickListener
	{
		private ExpensesFragmentPagerAdapter adapterViewPager;
		private ViewPager mViewPager;

		public override void OnCreate (Bundle savedInstanceState) {
			base.OnCreate (savedInstanceState);

			this.HasOptionsMenu = true;

			this.adapterViewPager = new ExpensesFragmentPagerAdapter (this.ChildFragmentManager);
		}

		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
			View view = inflater.Inflate (Resource.Layout.Pager_tab_strip, container, false);

			this.mViewPager = view.FindViewById<ViewPager>(Resource.Id.pager);
			this.mViewPager.Adapter = this.adapterViewPager;
			this.mViewPager.CurrentItem = this.Activity.Intent.GetIntExtra (MainActivity.EXTRA_SELECTED_CATEGORY, 0);

			return view;
		}

		private bool IsRefreshing {
			get {
				ExpensesListFragment fragment = (ExpensesListFragment) this.ChildFragmentManager.FindFragmentByTag (
					"android:switcher:" + this.mViewPager.Id + ":"
					+ ((ExpensesFragmentPagerAdapter)this.mViewPager.Adapter).GetItemId (0));
				return fragment.IsRefreshing;
			}
		}

		public override void OnCreateOptionsMenu (IMenu menu, MenuInflater inflater) {
			menu.Clear ();

			inflater.Inflate (Resource.Menu.Expenses_menu, menu);

			base.OnCreateOptionsMenu (menu, inflater);
		}

		public override bool OnOptionsItemSelected (IMenuItem item) {
			switch (item.ItemId) {
				case Resource.Id.Action_new:
					this.ChooseExpense ();
					return true;
			}

			return base.OnOptionsItemSelected (item);
		}

		private void ChooseExpense () {
			if (this.IsRefreshing) {
				Toast.MakeText (this.Activity, "Please wait for the expenses to be fetched", ToastLength.Short).Show ();
				return;
			}

			Actionables actionables = Expense.ListShowAddExpenses (this.AddExpense, this.AddMileage, this.AddAllowance);

			DialogFragment actionsDialogFragment = new ActionPickerDialogFragment (actionables.Actions);
			actionsDialogFragment.Show (this.ChildFragmentManager, null);
		}

		private void AddExpense () {
			Expense expense = Expense.NewInstance ();

			DialogFragment categoryDialog = new CategoryPickerDialogFragment (LoggedUser.Instance.Products.ExpenseProducts, (object sender, EventArgsObject<Product> e) => {
				expense.ExpenseItems [0].Product = e.Object;

				DialogFragment countryDialog = new CountryPickerDialogFragment (null, expense.Countries, (object resender, EventArgsObject<Country> re) => {
					expense.ExpenseItems [0].Country = re.Object;

					DialogFragment amountDialog = new AmountPickerDialogFragment (expense.ExpenseItems [0], null, (object reresender, EventArgsObject<ExpenseItem> rere) => {
						LoggedUser.Instance.BusinessExpenses.AddItem (expense);

						Intent intent = new Intent (this.Activity, typeof (ExpenseDetailsActivity));
						this.StartActivity (intent);

						((DialogFragment)sender).Dismiss ();
						((DialogFragment)resender).Dismiss ();
						((DialogFragment)reresender).Dismiss ();
					});
					amountDialog.Show (this.ChildFragmentManager, null);
				});
				countryDialog.Show (this.ChildFragmentManager, null);
			});
			categoryDialog.Show (this.ChildFragmentManager, null);
		}

		private void AddMileage () {
			Mileage mileage = Mileage.NewInstance ();
			mileage.MileageSegments.AddNewItem ();
			mileage.MileageSegments.AddNewItem ();

			LoggedUser.Instance.BusinessExpenses.AddItem (mileage);

			Action startIntent = () => {
				Intent intent = new Intent (this.Activity, typeof (ExpenseDetailsActivity));
				this.StartActivity (intent);
			};

			if (mileage.CanShowJourneysList) {
				Actionables actionables = mileage.ListJourneys (startIntent);

				DialogFragment actionsDialogFragment = new ActionPickerDialogFragment (actionables.Actions);
				actionsDialogFragment.Show (this.ChildFragmentManager, null);
			} else
				startIntent ();
		}

		private void AddAllowance () {
			Allowance allowance = Allowance.NewInstance ();

			DialogFragment newAllowanceDialog = new NewAllowanceDialogFragment (allowance, (object sender, EventArgsObject<Allowance> e) => {
				this.Activity.InvokeActionAsync (() => allowance.CreateAsync (), () => {
					LoggedUser.Instance.BusinessExpenses.AddItem (allowance);

					Intent intent = new Intent (this.Activity, typeof (ExpenseDetailsActivity));
					this.StartActivity (intent);

					((DialogFragment)sender).Dismiss ();
				}, this);
			});
			newAllowanceDialog.Show (this.ChildFragmentManager, null);
		}

		public void OnClickHandler<T> (int requestCode, DialogArgsObject<T> args) {
			
		}
	}
}