using Android.OS;
using Mxp.Core.Business;
using Mxp.Droid.Adapters;
using Android.Widget;
using Android.Views;
using Android.Content;
using System.Collections.Generic;
using Mxp.Core.Services;
using Mxp.Core.Helpers;
using Mxp.Core.Utils;
using System.Threading.Tasks;
using Mxp.Droid.Utils;
using Mxp.Droid.Helpers;
using System;

namespace Mxp.Droid.Fragments
{
	public class ExpenseAttendeesListFragment : Android.Support.V4.App.ListFragment, AbsListView.IMultiChoiceModeListener, INotifyFragmentDataSetRefreshed, BaseDialogFragment.IDialogClickListener
	{
		#pragma warning disable 0414
		private static readonly string TAG = typeof(ExpenseAttendeesListFragment).Name;
		#pragma warning restore 0414

		private Android.Support.V7.Widget.Toolbar mToolbar;
		private IMenuItem mToolbarActionMenuItem;
		private AttendeeAdapter mAttendeeAdapter;

		private ExpenseItem mExpenseItem {
			get {
				return ((ExpenseDetailsActivity) this.Activity).ExpenseItem;
			}
		}

		protected bool CanShowActions {
			get {
				return this.mExpenseItem.CanManageAttendees;
			}

		}

		public static ExpenseAttendeesListFragment NewInstance () {
			ExpenseAttendeesListFragment expenseAttendeesListFragment = new ExpenseAttendeesListFragment ();

			return expenseAttendeesListFragment;
		}

		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
			View view = inflater.Inflate (Resource.Layout.List_expense_attendees, container, false);

			this.mToolbar = view.FindViewById<Android.Support.V7.Widget.Toolbar> (Resource.Id.Toolbar);
			this.mToolbar.InflateMenu (Resource.Menu.Attendees_menu);
			this.mToolbarActionMenuItem = this.mToolbar.Menu.FindItem (Resource.Id.Action_new);
			this.mToolbar.MenuItemClick += (object sender, Android.Support.V7.Widget.Toolbar.MenuItemClickEventArgs e) => {
				switch (e.Item.ItemId) {
					case Resource.Id.Action_new:
						this.ShowNewAttendeeDialog ();
						break;
				}
			};

			return view;
		}

		public override void OnViewCreated (View view, Bundle savedInstanceState) {
			base.OnViewCreated (view, savedInstanceState);

			this.mToolbar.Visibility = this.CanShowActions ? ViewStates.Visible : ViewStates.Gone;

			this.ListView.ChoiceMode = ChoiceMode.MultipleModal;
			this.ListView.SetMultiChoiceModeListener (this);

			if (Preferences.Instance.IsGTPEnabled) {
				TextView textView = (TextView) LayoutInflater.From (this.Context).Inflate (Resource.Layout.List_header_item, null);
				textView.Text = Labels.GetLoggedUserLabel (Labels.LabelEnum.GTPHeaderMessage);
				this.ListView.AddHeaderView (textView);
			}

			this.NotifyDataSetRefreshed ();
		}

		public void ShowNewAttendeeDialog () {
			Actionables actionables = Attendee.ListShowAttendees (
				actionRecent: () => {
					Attendees attendees = new Attendees ();

					this.Activity.InvokeActionAsync (attendees.FetchRecentlyUsedAttendeesAsync, () => {
						Android.Support.V4.App.DialogFragment dialogFragment = new AttendeePickerDialogFragment (attendees, (object sender, EventArgsObject<Attendee> e) => this.AddAttendee (sender, e, true));
						dialogFragment.Show (this.ChildFragmentManager, null);
					}, this);
				},
				actionBusinessRelation: () => {
					Android.Support.V4.App.DialogFragment dialogFragment = new AttendeeFormDialogFragment (Labels.GetLoggedUserLabel (Labels.LabelEnum.BusinessRelation), AttendeeTypeEnum.Business, this.AddAttendee);
					dialogFragment.Show (this.ChildFragmentManager, null);
				},
				actionEmployee: () => {
					Android.Support.V4.App.DialogFragment dialogFragment = new LookupPickerDialogFragment (LookupService.ApiEnum.GetLookUpEmployee, (object sender, EventArgsObject<LookupItem> e) => {
						this.AddAttendee (sender, new EventArgsObject<Attendee> (new Attendee (e.Object.Id, e.Object.Name)), true);
					});
					dialogFragment.Show (this.ChildFragmentManager, null);
				},
				actionSpouse: () => {
					Android.Support.V4.App.DialogFragment dialogFragment = new AttendeeFormDialogFragment (Labels.GetLoggedUserLabel (Labels.LabelEnum.Spouse), AttendeeTypeEnum.Spouse, this.AddAttendee);
					dialogFragment.Show (this.ChildFragmentManager, null);
				},
				actionHCP: () => {
					Android.Support.V4.App.DialogFragment dialogFragment = new AttendeeFormDialogFragment (Labels.GetLoggedUserLabel (Labels.LabelEnum.Hcp), AttendeeTypeEnum.HCP, (object sender, EventArgsObject<Attendee> e) => this.AddAttendee (sender, e, true));
					dialogFragment.Show (this.ChildFragmentManager, null);
				},
				actionHCO: () => {
					Android.Support.V4.App.DialogFragment dialogFragment = new AttendeeFormDialogFragment (Labels.GetLoggedUserLabel (Labels.LabelEnum.Hco), AttendeeTypeEnum.HCO, (object sender, EventArgsObject<Attendee> e) => this.AddAttendee (sender, e, true));
					dialogFragment.Show (this.ChildFragmentManager, null);
				},
				actionHCU: () => {
					Android.Support.V4.App.DialogFragment dialogFragment = new AttendeeFormDialogFragment (Labels.GetLoggedUserLabel (Labels.LabelEnum.Hcu), AttendeeTypeEnum.UCP, (object sender, EventArgsObject<Attendee> e) => this.AddAttendee (sender, e, true));
					dialogFragment.Show (this.ChildFragmentManager, null);
				}
			);

			Android.Support.V4.App.DialogFragment actionsDialogFragment = new ActionPickerDialogFragment (actionables.Actions);
			actionsDialogFragment.Show (this.ChildFragmentManager, null);
		}

		private void InvalidateToolbarMenu () {
			this.mToolbarActionMenuItem.SetActionView ((this.mToolbarActionMenuItem.ActionView == null ? new ProgressBar (this.Activity) : null));
		}

		private void AddAttendee (object sender, EventArgsObject<Attendee> e, bool force = false) {
			((Android.Support.V4.App.DialogFragment)sender).Dismiss ();

			this.InvalidateToolbarMenu ();

			TaskConfigurator.Create (this).Finally (() => {
				this.mAttendeeAdapter.NotifyDataSetChanged ();
				this.InvalidateToolbarMenu ();
			}).Start (this.mExpenseItem.Attendees.AddAsync (e.Object, this.mExpenseItem, force));
		}

		private void AddAttendee (object sender, EventArgsObject<Attendee> e) {
			this.AddAttendee (sender, e, false);
		}

		public override void OnListItemClick (ListView listView, View view, int position, long id) {
			if (position < this.ListView.HeaderViewsCount)
				return;

			position -= this.ListView.HeaderViewsCount;

			Intent intent = new Intent (this.Activity, typeof (AttendeeDetailsActivity));

			intent.PutExtra (AttendeeDetailsActivity.EXTRA_ATTENDEE_ID, this.mExpenseItem.Attendees [position].Id);

			if (this.mExpenseItem.Id.HasValue)
				intent.PutExtra (AttendeeDetailsActivity.EXTRA_EXPENSE_ITEM_ID, this.mExpenseItem.Id.Value);

			intent.PutExtra (AttendeeDetailsActivity.EXTRA_EXPENSES_TYPE, (int)this.mExpenseItem.ParentExpensesCollection.ExpensesType);

			if (this.mExpenseItem.ParentExpense.IsFromReport) {
				intent.PutExtra (AttendeeDetailsActivity.EXTRA_REPORT_TYPE, (int)this.mExpenseItem.ParentExpense.Report.ReportType);
				intent.PutExtra (AttendeeDetailsActivity.EXTRA_REPORT_ID, this.mExpenseItem.ParentExpense.Report.Id.Value);
			}

			this.Activity.StartActivity (intent);
		}

		#region AbsListView.IMultiChoiceModeListener

		public void OnItemCheckedStateChanged (ActionMode mode, int position, long id, bool @checked) {
			// Here you can do something when items are selected/de-selected,
			// such as update the title in the CAB
		}

		public bool OnActionItemClicked (ActionMode mode, IMenuItem item) {
			// Respond to clicks on the actions in the CAB
			switch (item.ItemId) {
				case Resource.Id.Action_delete:
					this.RemoveAttendees ();
					mode.Finish (); // Action picked, so close the CAB
					return true;
				default:
					return false;
			}
		}

		public bool OnCreateActionMode (ActionMode mode, IMenu menu) {
			// Inflate the menu for the CAB
			MenuInflater inflater = mode.MenuInflater;
			inflater.Inflate (Resource.Menu.Attendee_context, menu);
			return true;
		}

		public void OnDestroyActionMode (ActionMode mode) {
			// Here you can make any necessary updates to the activity when
			// the CAB is removed. By default, selected items are deselected/unchecked.
		}

		public bool OnPrepareActionMode (ActionMode mode, IMenu menu) {
			// Here you can perform updates to the CAB due to
			// an invalidate() request
			return false;
		}

		#endregion

		public void RemoveAttendees () {
			this.InvalidateToolbarMenu ();
			List<Task> tasks = new List<Task> (this.ListView.CheckedItemPositions.Size ());

			for (int i = 0; i < this.ListView.CheckedItemPositions.Size (); i++) {
				if (this.ListView.CheckedItemPositions.ValueAt (i))
					tasks.Add (this.mExpenseItem.Attendees.RemoveAsync (this.mAttendeeAdapter [this.ListView.CheckedItemPositions.KeyAt (i) - this.ListView.HeaderViewsCount]));
			}

			TaskConfigurator.Create (this)
			                .Finally (() => {
								this.ListView.CheckedItemPositions.Clear ();
								this.mAttendeeAdapter.NotifyDataSetChanged ();
								this.InvalidateToolbarMenu ();
				            })
			                .Start (Task.WhenAll (tasks.ToArray ()));
		}

		#region INotifyFragmentDataSetRefreshed

		public void NotifyDataSetRefreshed () {
			this.mAttendeeAdapter = new AttendeeAdapter (this.Activity, this.mExpenseItem.Attendees);
			this.ListAdapter = this.mAttendeeAdapter;
		}

		public void OnClickHandler<T> (int requestCode, DialogArgsObject<T> args) {
			
		}

		#endregion
	}
}