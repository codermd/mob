using System;

using Android.Widget;

using Mxp.Core.Business;
using System.Collections.Generic;
using Android.App;
using Android.Views;
using com.refractored.components.stickylistheaders;
using Android.Util;
using Android.Content;
using Mxp.Droid.Helpers;
using Android.Support.V4.App;
using Mxp.Droid.Utils;
using Android.Graphics;
using Android.Support.V4.Content;

namespace Mxp.Droid.Adapters
{
	public class ExpenseAdapter : BaseAdapter<Expense>, IStickyListHeadersAdapter
	{
		private static readonly string TAG = typeof(ExpenseAdapter).Name;

		private const int SINGLE_EXPENSE = 0;
		private const int MULTIPLE_EXPENSE = 1;

		public Expenses Expenses { get; set; }
		private FragmentActivity mActivity;
		private Report mReport;

		private bool IsSelectable {
			get {
				return this.mReport != null && this.Expenses.ParentModel == null;
			}
		}

		private bool IsRemovable {
			get {
				return this.mReport != null
					&& !this.mReport.IsFromApproval
					&& this.Expenses.ParentModel is Report;
			}
		}

		private bool IsValidable {
			get {
				return this.mReport != null && this.mReport.CanApproveExpense;
			}
		}

		public ExpenseAdapter (FragmentActivity activity, Expenses expenses, Report report = null) : base () {
			this.mActivity = activity;
			this.Expenses = expenses;
			this.mReport = report;
		}

		public override long GetItemId (int position) {
			return position;
		}

		public override Expense this[int index] {
			get {
				return this.Expenses [index];
			}
		}

		public override int Count {
			get {
				return this.Expenses.Count;
			}
		}

		public override int GetItemViewType (int position) {
			return this.Expenses [position].IsSplit ? MULTIPLE_EXPENSE : SINGLE_EXPENSE;
		}

		public override int ViewTypeCount {
			get {
				return 2;
			}
		}

		public override View GetView (int position, View convertView, ViewGroup parent) {
			ViewHolder<Expense> viewHolder = null;

			if (convertView == null || convertView.Tag == null
				|| ((convertView.Tag is SplittedExpenseViewHolder) && ((SplittedExpenseViewHolder) convertView.Tag).length != this.Expenses[position].ExpenseItems.Count)) {
				switch (this.GetItemViewType (position)) {
					case SINGLE_EXPENSE:
						if (this.IsSelectable)
							convertView = this.mActivity.LayoutInflater.Inflate (Resource.Layout.List_expenses_item_selectable, parent, false);
						else if (this.IsRemovable)
							convertView = this.mActivity.LayoutInflater.Inflate (Resource.Layout.List_expenses_item_removable, parent, false);
						else if (this.IsValidable)
							convertView = this.mActivity.LayoutInflater.Inflate (Resource.Layout.List_expenses_item_validable, parent, false);
						else
							convertView = this.mActivity.LayoutInflater.Inflate (Resource.Layout.List_expenses_item, parent, false);

						viewHolder = new ExpenseViewHolder (this, convertView);
						break;
					case MULTIPLE_EXPENSE:
						if (this.IsSelectable)
							convertView = this.mActivity.LayoutInflater.Inflate (Resource.Layout.List_expenses_items_group_selectable, parent, false);
						else if (this.IsRemovable)
							convertView = this.mActivity.LayoutInflater.Inflate (Resource.Layout.List_expenses_items_group_removable, parent, false);
						else
							convertView = this.mActivity.LayoutInflater.Inflate (Resource.Layout.List_expenses_items_group, parent, false);

						viewHolder = new SplittedExpenseViewHolder (this, convertView, this.Expenses[position].ExpenseItems.Count);
						break;
				}

				convertView.Tag = viewHolder;
			} else
				viewHolder = convertView.Tag as ViewHolder<Expense>;

			viewHolder.BindView (this.Expenses[position]);

			return convertView;
		}

		public View GetHeaderView (int position, View convertView, ViewGroup parent) {
			HeaderViewHolder headerViewHolder;

			if (convertView == null) {
				convertView = this.mActivity.LayoutInflater.Inflate (Resource.Layout.List_expenses_header, parent, false);
				headerViewHolder = new HeaderViewHolder (convertView);
				convertView.Tag = headerViewHolder;
			} else {
				headerViewHolder = convertView.Tag as HeaderViewHolder;
			}

			headerViewHolder.BindView (this.Expenses [position]);

			return convertView;
		}

		public long GetHeaderId (int position) {
			return (long) this [position].OrderKey;
		}

		private class ExpenseViewHolder : ViewHolder<Expense> {
			private TextView title;
			private TextView amountLC;
			private TextView amountCC;
			private TextView date;

			private ImageView receiptIcon;
			private ImageView attendeeIcon;
			private ImageView creditCardIcon;
			private ImageView flagIcon;
			private ImageView policyRuleIcon;
			private ImageView statusIcon;

			private CheckBox checkbox;
			private ImageButton removeButton;

			private ExpenseAdapter adapter;
			private Expense currentExpense;

			public ExpenseViewHolder (ExpenseAdapter adapter, View convertView) {
				this.adapter = adapter;
				this.title = convertView.FindViewById<TextView> (Resource.Id.Title);
				this.amountLC = convertView.FindViewById<TextView> (Resource.Id.AmountLC);
				this.amountCC = convertView.FindViewById<TextView> (Resource.Id.AmountCC);
				this.date = convertView.FindViewById<TextView> (Resource.Id.Date);

				this.receiptIcon = convertView.FindViewById<ImageView> (Resource.Id.ReceiptIcon);
				this.attendeeIcon = convertView.FindViewById<ImageView> (Resource.Id.AttendeeIcon);
				this.creditCardIcon = convertView.FindViewById<ImageView> (Resource.Id.CreditCardIcon);
				this.flagIcon = convertView.FindViewById<ImageView> (Resource.Id.FlagIcon);
				this.policyRuleIcon = convertView.FindViewById<ImageView> (Resource.Id.PolicyIcon);
				this.statusIcon = convertView.FindViewById<ImageView> (Resource.Id.StatusIcon);

				this.checkbox = convertView.FindViewById<CheckBox> (Resource.Id.Checkbox);
				this.removeButton = convertView.FindViewById<ImageButton> (Resource.Id.DeleteButton);

				if (this.checkbox != null) {
					if (this.adapter.mReport.IsFromApproval)
						this.checkbox.Click +=  (object sender, EventArgs e) => this.currentExpense.ExpenseItems [0].Toggle ();
					else {
						this.checkbox.Click +=  (object sender, EventArgs e) => {
							if (this.checkbox.Checked)
								this.adapter.mReport.Expenses.Add (currentExpense);
							else
								this.adapter.mReport.Expenses.Remove (currentExpense);
						};
					}
				} else if (this.removeButton != null) {
					this.removeButton.Click += (object sender, EventArgs e) => {
						this.adapter.mActivity.InvokeActionAsync (
							() => this.adapter.mReport.Expenses.RemoveReportExpenseAsync (this.currentExpense),
							() => this.adapter.NotifyDataSetChanged ()
						);
					};
				}
			}

			public override void BindView (Expense expense) {
				this.currentExpense = expense;

				this.title.Text = expense.VTitle;
				this.title.SetTextColor (expense.IsHighlighted ? Color.Red : ContextCompat.GetColor (this.adapter.mActivity, Resource.Color.text_color_black));

				this.amountLC.Text = expense.VAmountLC;
				this.amountCC.Visibility = expense.AreCurrenciesDifferent ? ViewStates.Visible : ViewStates.Gone;

				if (expense.AreCurrenciesDifferent)
					this.amountCC.Text = expense.VAmountCC;
				
				this.date.Text = expense.VDate;

				if (expense.Country != null) {
					try {
						int resource = (int)typeof(Resource.Drawable).GetField(expense.Country.VResourceName).GetValue(null);
						this.flagIcon.SetImageResource (resource);
					} catch (Exception e) {
						Log.Error (TAG, "Failure to get drawable id.", e);
						this.flagIcon.SetImageResource (Resource.Drawable.NoFlag);
					}
				} else 
					this.flagIcon.SetImageResource (Resource.Drawable.NoFlag);

				this.attendeeIcon.Visibility = expense.HasAttendees ? ViewStates.Visible : ViewStates.Gone;
				this.receiptIcon.Visibility = expense.HasReceipts ? ViewStates.Visible : ViewStates.Gone;

				ViewHolderHelper.SetCreditCardImage (expense, this.creditCardIcon);

				switch (expense.PolicyRule) {
					case ExpenseItem.PolicyRules.Green:
						this.policyRuleIcon.SetImageResource (Resource.Drawable.expense_is_comliant);
						break;
					case ExpenseItem.PolicyRules.Orange:
						this.policyRuleIcon.SetImageResource (Resource.Drawable.expense_not_compliant_policy);
						break;
					case ExpenseItem.PolicyRules.Red:
						this.policyRuleIcon.SetImageResource (Resource.Drawable.Expense_not_compliant);
						break;
				}

				if (this.checkbox != null) {
					if (this.adapter.mReport.IsFromApproval)
						this.checkbox.Checked = expense.ExpenseItems [0].StatusForApprovalReport == ExpenseItem.Status.Accepted;
					else {
						this.checkbox.Checked = this.adapter.mReport.Expenses.Contains (this.currentExpense);
						this.checkbox.Enabled = this.currentExpense.IsDeselectable;
					}
				}

				if (this.removeButton != null)
					this.removeButton.Visibility = this.currentExpense.CanRemove ? ViewStates.Visible : ViewStates.Gone;

				this.ConfigureStatusIcon (expense);
			}

			private void ConfigureStatusIcon (Expense expense) {
				this.statusIcon.Visibility = expense.CanShowExpenseReportStatus ? ViewStates.Visible : ViewStates.Gone;

				if (!expense.CanShowExpenseReportStatus)
					return;

				switch (expense.MainStatus) {
					case ExpenseItem.Status.Accepted:
						this.statusIcon.SetImageResource (Resource.Drawable.report_approved);
						break;
					case ExpenseItem.Status.Rejected:
						this.statusIcon.SetImageResource (Resource.Drawable.report_refused);
						break;
					case ExpenseItem.Status.Other:
						this.statusIcon.SetImageResource (Resource.Drawable.ic_report_is_pending_schedule);
						break;
				}
			}
		}

		private class SplittedExpenseViewHolder : ViewHolder<Expense> {
			private TextView mainTitle;
			private TextView totalAmountLC;
			private TextView totalAmountCC;
			private TextView date;

			private ImageView receiptIcon;
			private ImageView attendeeIcon;
			private ImageView creditCardIcon;
			private ImageView flagIcon;
			private ImageView policyRuleIcon;
			private ImageView statusIcon;

			private CheckBox checkbox;
			private ImageButton removeButton;

			private List<ExpenseItemViewHolder> expenseItemViewHolders;
			private LinearLayout itemsLayout;

			private ExpenseAdapter adapter;
			public int length;

			private Expense mCurrentExpense;

			public SplittedExpenseViewHolder (ExpenseAdapter adapter, View convertView, int length) {
				this.adapter = adapter;
				this.length = length;

				this.expenseItemViewHolders = new List<ExpenseItemViewHolder> (this.length);

				this.mainTitle = convertView.FindViewById<TextView> (Resource.Id.Main_title);
				this.totalAmountLC = convertView.FindViewById<TextView> (Resource.Id.AmountLC);
				this.totalAmountCC = convertView.FindViewById<TextView> (Resource.Id.AmountCC);
				this.date = convertView.FindViewById<TextView> (Resource.Id.Date);

				this.receiptIcon = convertView.FindViewById<ImageView> (Resource.Id.ReceiptIcon);
				this.attendeeIcon = convertView.FindViewById<ImageView> (Resource.Id.AttendeeIcon);
				this.creditCardIcon = convertView.FindViewById<ImageView> (Resource.Id.CreditCardIcon);
				this.flagIcon = convertView.FindViewById<ImageView> (Resource.Id.FlagIcon);
				this.policyRuleIcon = convertView.FindViewById<ImageView> (Resource.Id.PolicyIcon);
				this.statusIcon = convertView.FindViewById<ImageView> (Resource.Id.StatusIcon);

				this.checkbox = convertView.FindViewById<CheckBox> (Resource.Id.Checkbox);
				this.removeButton = convertView.FindViewById<ImageButton> (Resource.Id.DeleteButton);

				if (this.checkbox != null) {
					this.checkbox.Click +=  (object sender, EventArgs e) => {
						if (this.checkbox.Checked)
							this.adapter.mReport.Expenses.Add (this.mCurrentExpense);
						else
							this.adapter.mReport.Expenses.Remove (this.mCurrentExpense);
					};
				} else if (this.removeButton != null) {
					this.removeButton.Click += (object sender, EventArgs e) => {
						this.adapter.mActivity.InvokeActionAsync (
							() => this.adapter.mReport.Expenses.RemoveReportExpenseAsync (this.mCurrentExpense),
							() => this.adapter.NotifyDataSetChanged ()
						);
					};
				}
					
				this.itemsLayout = convertView.FindViewById<LinearLayout> (Resource.Id.Items);

				if (this.itemsLayout == null) {
					View includedView = convertView.FindViewById (Resource.Id.Included);
					this.itemsLayout = includedView.FindViewById<LinearLayout> (Resource.Id.Items);
				}

				for (int i = 0; i < this.length; i++) {
					View view;

					if (this.adapter.IsValidable)
						view = this.adapter.mActivity.LayoutInflater.Inflate (Resource.Layout.List_expenses_item_group_validable, null);
					else
						view = this.adapter.mActivity.LayoutInflater.Inflate(Resource.Layout.List_expenses_item_group, null);

					this.expenseItemViewHolders.Add (new ExpenseItemViewHolder (this.adapter.mActivity, view));
					this.itemsLayout.AddView (view, i);
				}
			}

			public override void BindView (Expense expense) {
				this.mCurrentExpense = expense;

				this.itemsLayout.Visibility = ViewStates.Gone;

				this.mainTitle.Text = expense.VTitle;

				this.totalAmountLC.Text = expense.VAmountLC;
				this.totalAmountCC.Visibility = expense.AreCurrenciesDifferent ? ViewStates.Visible : ViewStates.Gone;

				if (expense.AreCurrenciesDifferent)
					this.totalAmountCC.Text = expense.VAmountCC;

				this.date.Text = expense.VDate;

				if (expense.Country != null) {
					try {
						int resource = (int)typeof(Resource.Drawable).GetField(expense.Country.VResourceName).GetValue(null);
						this.flagIcon.SetImageResource (resource);
					} catch (Exception e) {
						Log.Error (TAG, "Failure to get drawable id.", e);
						this.flagIcon.SetImageResource (Resource.Drawable.NoFlag);
					}
				} else
					this.flagIcon.SetImageResource (Resource.Drawable.NoFlag);

				this.attendeeIcon.Visibility = expense.HasAttendees ? ViewStates.Visible : ViewStates.Gone;
				this.receiptIcon.Visibility = expense.HasReceipts ? ViewStates.Visible : ViewStates.Gone;

				ViewHolderHelper.SetCreditCardImage (expense, this.creditCardIcon);

				switch (expense.PolicyRule) {
					case ExpenseItem.PolicyRules.Green:
						this.policyRuleIcon.SetImageResource (Resource.Drawable.expense_is_comliant);
						break;
					case ExpenseItem.PolicyRules.Orange:
						this.policyRuleIcon.SetImageResource (Resource.Drawable.expense_not_compliant_policy);
						break;
					case ExpenseItem.PolicyRules.Red:
						this.policyRuleIcon.SetImageResource (Resource.Drawable.Expense_not_compliant);
						break;
				}

				for (int i = 0; i < this.length; i++)
					this.expenseItemViewHolders [i].BindView (expense.ExpenseItems [i]);

				if (this.checkbox != null)
					this.checkbox.Checked = this.adapter.mReport.Expenses.Contains (expense);

				if (this.removeButton != null)
					this.removeButton.Visibility = expense.CanRemove ? ViewStates.Visible : ViewStates.Gone;

				this.ConfigureStatusIcon (expense);
			}

			private void ConfigureStatusIcon (Expense expense) {
				this.statusIcon.Visibility = expense.CanShowExpenseReportStatus ? ViewStates.Visible : ViewStates.Gone;

				if (!expense.CanShowExpenseReportStatus)
					return;

				switch (expense.MainStatus) {
					case ExpenseItem.Status.Accepted:
						this.statusIcon.SetImageResource (Resource.Drawable.report_approved);
						break;
					case ExpenseItem.Status.Rejected:
						this.statusIcon.SetImageResource (Resource.Drawable.report_refused);
						break;
					case ExpenseItem.Status.Other:
						this.statusIcon.SetImageResource (Resource.Drawable.ic_report_is_pending_schedule);
						break;
				}
			}

			private class ExpenseItemViewHolder : ViewHolder<ExpenseItem> {
				private ImageView policyRuleIcon;
				private ImageView statusIcon;
				private TextView Title;
				private TextView AmountLC;
				private TextView AmountCC;

				private CheckBox checkbox;

				private ExpenseItem mCurrentExpenseItem;
				private Activity mActivity;

				public ExpenseItemViewHolder (Activity activity, View convertView) {
					this.mActivity = activity;

					this.policyRuleIcon = convertView.FindViewById<ImageView> (Resource.Id.PolicyIcon);
					this.statusIcon = convertView.FindViewById<ImageView> (Resource.Id.StatusIcon);
					this.Title = convertView.FindViewById<TextView> (Resource.Id.Title);
					this.AmountLC = convertView.FindViewById<TextView> (Resource.Id.AmountLC);
					this.AmountCC = convertView.FindViewById<TextView> (Resource.Id.AmountCC);

					convertView.Click += (object sender, EventArgs e) => {
						Intent intent = new Intent (activity, typeof(ExpenseDetailsActivity));
						intent.PutExtra (ExpenseDetailsActivity.EXTRA_EXPENSE_ITEM_ID, this.mCurrentExpenseItem.Id.Value);
						if (this.mCurrentExpenseItem.ParentExpense.IsFromReport) {
							intent.PutExtra (ExpenseDetailsActivity.EXTRA_REPORT_TYPE, (int)this.mCurrentExpenseItem.ParentExpense.Report.ReportType);
							intent.PutExtra (ExpenseDetailsActivity.EXTRA_REPORT_ID, this.mCurrentExpenseItem.ParentExpense.Report.Id.Value);
						}
						activity.StartActivity (intent);
					};

					this.checkbox = convertView.FindViewById<CheckBox> (Resource.Id.Checkbox);

					if (this.checkbox != null)
						this.checkbox.Click +=  (object sender, EventArgs e) => this.mCurrentExpenseItem.Toggle ();
				}

				public override void BindView (ExpenseItem expenseItem) {
					this.mCurrentExpenseItem = expenseItem;

					this.Title.Text = expenseItem.VCategoryName;
					this.Title.SetTextColor (expenseItem.IsHighlighted ? Color.Red : ContextCompat.GetColor (this.mActivity, Resource.Color.text_color_black));

					this.AmountLC.Text = expenseItem.VAmountLC;

					this.AmountCC.Visibility = expenseItem.AreCurrenciesDifferent ? ViewStates.Visible : ViewStates.Gone;

					if (expenseItem.AreCurrenciesDifferent)
						this.AmountCC.Text = expenseItem.VAmountCC;

					switch (expenseItem.PolicyRule) {
						case ExpenseItem.PolicyRules.Green:
							this.policyRuleIcon.SetImageResource (Resource.Drawable.expense_is_comliant);
							break;
						case ExpenseItem.PolicyRules.Orange:
							this.policyRuleIcon.SetImageResource (Resource.Drawable.expense_not_compliant_policy);
							break;
						case ExpenseItem.PolicyRules.Red:
							this.policyRuleIcon.SetImageResource (Resource.Drawable.Expense_not_compliant);
							break;
					}

					if (this.checkbox != null)
						this.checkbox.Checked = expenseItem.StatusForApprovalReport == ExpenseItem.Status.Accepted;

					this.ConfigureStatusIcon (expenseItem);
				}

				private void ConfigureStatusIcon (ExpenseItem expenseItem) {
					this.statusIcon.Visibility = expenseItem.ParentExpense.CanShowExpenseReportStatus ? ViewStates.Visible : ViewStates.Gone;

					if (!expenseItem.ParentExpense.CanShowExpenseReportStatus)
						return;

					switch (expenseItem.MainStatus) {
						case ExpenseItem.Status.Accepted:
							this.statusIcon.SetImageResource (Resource.Drawable.report_approved);
							break;
						case ExpenseItem.Status.Rejected:
							this.statusIcon.SetImageResource (Resource.Drawable.report_refused);
							break;
						case ExpenseItem.Status.Other:
							this.statusIcon.SetImageResource (Resource.Drawable.ic_report_is_pending_schedule);
							break;
					}
				}
			}
		}

		public static class ViewHolderHelper
		{
			public static void SetCreditCardImage (Expense expense, ImageView creditCardIcon) {
				if (expense.IsTempTransaction) {
					creditCardIcon.SetImageResource (Resource.Drawable.ic_temp_transaction);
					creditCardIcon.Visibility = ViewStates.Visible;
				} else if (expense.IsPaidByCreditCard) {
					creditCardIcon.SetImageResource (Resource.Drawable.ic_action_ic_expense_card_icon);
					creditCardIcon.Visibility = ViewStates.Visible;
				} else
					creditCardIcon.Visibility = ViewStates.Gone;
			}
		}

		private class HeaderViewHolder : ViewHolder<Expense> {
			private TextView Text { get; set; }

			public HeaderViewHolder (View convertView) {
				this.Text = convertView.FindViewById<TextView> (Resource.Id.Text);
			}

			public override void BindView (Expense expense) {
				this.Text.Text = expense.VDateHeader;
			}
		}
	}
}