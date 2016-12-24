using System;
using System.Collections.Generic;
using Mxp.Core.Utils;
using System.Linq;

namespace Mxp.Core.Business
{
	public partial class Expense
	{
		public bool IsNew {
			get {
				return !this.Id.HasValue;
			}
		}

		public bool HasReceipts {
			get {
				return this.NumberReceipts != 0;
			}
		}

		public virtual bool CanShowReceipts {
			get {
				return LoggedUser.Instance.Preferences.ITEEnableItemAttachment == VisibilityEnum.Show;
			}
		}

		public bool CanManageReceipts {
			get {
				return this.Receipts.CanManage;
			}
		}

		internal virtual bool CanShowAttendees {
			get {
				return true;
			}
		}
			
		public bool HasAttendees {
			get {
				return this.ExpenseItems.Count > 1 ? false : this.NumberOfAttendees > 0;
			}
		}

		public int NumberOfAttendees {
			get {
				return this.ExpenseItems[0].Attendees.Count;
			}
		}

		public bool IsFromReport {
			get {
				return this.GetModelParent<Expense> () is Report;
			}
		}

		public Report Report {
			get {
				return this.IsFromReport ? this.GetModelParent<Expense, Report> () : null;
			}
		}

		public virtual bool IsEditable {
			get {
				return !(this.IsFromReport && (this.Report.IsClosed || this.Report.IsFromApproval));
			}
		}

		public int OrderKey {
			get { 
				return this.Date.Value.Year * 1000 + this.Date.Value.Month;
			}
		}

		public static Actionables ListShowAddExpenses (Action actionExpense, Action actionMileage, Action actionAllowance) {
			List<Actionable> actions = new List<Actionable> () {
				new Actionable (Labels.GetLoggedUserLabel (Labels.LabelEnum.Expense), actionExpense)
			};

			if (Mileage.CanCreateMileage)
				actions.Add (new Actionable (Labels.GetLoggedUserLabel(Labels.LabelEnum.Mileage), actionMileage));

			if (Allowance.CanCreateAllowance)
				actions.Add (new Actionable (Labels.GetLoggedUserLabel(Labels.LabelEnum.Allowance), actionAllowance));

			return new Actionables (Labels.GetLoggedUserLabel (Labels.LabelEnum.ChooseExpense), actions);
		}

		public override bool IsChanged {
			get {
				return this.ExpenseItems.Any (segment => segment.IsChanged) || base.IsChanged;
			}
		}

		public bool CanShowExpenseReportStatus {
			get {
				return this.Report != null
					&& this.Report.ApprovalStatus != Report.ApprovalStatusEnum.Waiting
					&& this.Report.ApprovalStatus != Report.ApprovalStatusEnum.Unknown;
			}
		}

		public bool IsOwnedToCompany => this.ExpenseItems.Any (expenseItem => expenseItem.IsOwnedToCompany);

		#region ICountriesFor

		public virtual Countries Countries {
			get {
				return LoggedUser.Instance.Countries.ForExpense;
			}
		}

		#endregion

		public bool IsHighlighted => this.IsOwnedToCompany && !this.IsSplit;

		public bool IsDeselectable => this.ReportPreselection != ReportPreselectionEnum.Mandatory;

		public virtual bool CanCopy => this.IsNew || (this.GetCollectionParent<Expenses, Expense> ().ExpensesType == Expenses.ExpensesTypeEnum.Business && !this.IsSplit);
	}
}