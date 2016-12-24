using System;
using Mxp.Core.Utils;
using System.Linq;

namespace Mxp.Core.Business
{
	public partial class ReportApproval
	{
		public string VEmployeeFullname {
			get {
				return this.Report.VEmployeeFullname;
			}
		}

		public string VDateRange {
			get {
				return this.Report.VDateRange;
			}
		}

		public string VAmount {
			get {
				return this.Report.VAmount;
			}
		}

		public int VNumberOfAccepted {
			get {
				int accepted = 0;

				this.Report.Expenses.ForEach (expense => {
					if (expense.IsSplit)
						accepted += expense.ExpenseItems.Count (expenseItem => expenseItem.StatusForApprovalReport == ExpenseItem.Status.Accepted);
					else if (expense.ExpenseItems [0].StatusForApprovalReport == ExpenseItem.Status.Accepted)
						accepted++;
				});

				return accepted;
			}
		}

		public int VNumberOfRejected {
			get {
				int rejected = 0;

				this.Report.Expenses.ForEach (expense => {
					if (expense.IsSplit)
						rejected += expense.ExpenseItems.Count (expenseItem => expenseItem.StatusForApprovalReport == ExpenseItem.Status.Rejected);
					else if (expense.ExpenseItems [0].StatusForApprovalReport == ExpenseItem.Status.Rejected)
						rejected++;
				});

				return rejected;
			}
		}

		public bool IsAllExpensesAccepted {
			get {
				bool accepted = true;

				this.Report.Expenses.ForEach (expense => {
					if (expense.IsSplit)
						accepted = expense.ExpenseItems.All (expenseItem => expenseItem.StatusForApprovalReport == ExpenseItem.Status.Accepted);
					else
						accepted = expense.ExpenseItems [0].StatusForApprovalReport == ExpenseItem.Status.Accepted;
				});

				return accepted;
			}
		}
	}
}