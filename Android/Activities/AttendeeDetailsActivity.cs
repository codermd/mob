
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Mxp.Droid.Adapters;
using Mxp.Core.Business;
using Mxp.Core.Utils;

namespace Mxp.Droid
{	
	public class AttendeeDetailsActivity : BaseActivity
	{
		public static readonly string EXTRA_ATTENDEE_ID = "com.sagacify.mxp.attendee.id";
		public static readonly string EXTRA_EXPENSE_ITEM_ID = "com.sagacify.mxp.expense.id";
		public static readonly string EXTRA_EXPENSES_TYPE = "com.sagacify.mxp.expenses.type";
		public static readonly string EXTRA_REPORT_TYPE = "com.sagacify.mxp.report.type";
		public static readonly string EXTRA_REPORT_ID = "com.sagacify.mxp.report.id";

		protected override void OnCreate (Bundle savedInstanceState) {
			base.OnCreate (savedInstanceState);

			this.SetContentView (Resource.Layout.attendee_details_activity);

			Android.Support.V7.Widget.Toolbar toolbar = this.FindViewById<Android.Support.V7.Widget.Toolbar> (Resource.Id.Toolbar);
			this.SetSupportActionBar (toolbar);

			this.SupportActionBar.SetDisplayHomeAsUpEnabled (true);

			this.Title = Labels.GetLoggedUserLabel (Labels.LabelEnum.Attendees);

			ListView list = this.FindViewById<ListView> (Resource.Id.List);
			list.Adapter = new FieldsAdapter (this.SupportFragmentManager, this, this.Attendee.AllFields);
		}

		public override bool OnOptionsItemSelected (IMenuItem item) {
			switch (item.ItemId) {
				case Android.Resource.Id.Home:
					this.Finish ();
					break;
			}

			return base.OnOptionsItemSelected (item);
		}

		private Attendee _attendee;
		private Attendee Attendee {
			get {
				if (_attendee == null) {
					int attendeeId = this.Intent.GetIntExtra (EXTRA_ATTENDEE_ID, -1);
					int expenseItemId = this.Intent.GetIntExtra (EXTRA_EXPENSE_ITEM_ID, -1);
					int reportId = this.Intent.GetIntExtra (EXTRA_REPORT_ID, -1);
					int reportType = this.Intent.GetIntExtra (EXTRA_REPORT_TYPE, -1);
					Expenses.ExpensesTypeEnum expenseType = (Expenses.ExpensesTypeEnum)this.Intent.GetIntExtra (EXTRA_EXPENSES_TYPE, (int)Expenses.ExpensesTypeEnum.Business);
					Expenses expenses = expenseType == Expenses.ExpensesTypeEnum.Business
						? LoggedUser.Instance.BusinessExpenses
						: LoggedUser.Instance.PrivateExpenses;
					
					ExpenseItem item;

					if (expenseItemId != -1) {
						if (reportId != -1 && reportType != -1)
							item = LoggedUser.Instance.GetReport ((Reports.ReportTypeEnum)reportType, reportId).Expenses.SelectSingle (expense => expense.ExpenseItems.SingleOrDefault (expenseItem => expenseItem.Id == expenseItemId));
						else
							item = expenses.SelectSingle (expense => expense.ExpenseItems.SingleOrDefault (expenseItem => expenseItem.Id == expenseItemId));
					} else
						item = expenses.Single (expense => expense.IsNew).ExpenseItems [0];

					this._attendee = item.Attendees.Single (attendee => attendee.Id == attendeeId);
				}

				return this._attendee;
			}
		}
	}
}