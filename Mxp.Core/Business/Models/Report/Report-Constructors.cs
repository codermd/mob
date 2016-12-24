using System;
using System.Collections.Generic;
using RestSharp.Portable;
using Mxp.Core.Services;
using System.Threading.Tasks;
using Mxp.Core.Services.Responses;
using System.Linq;

namespace Mxp.Core.Business
{
	public partial class Report
	{
		public Report () : base () {
			this.Receipts = new Receipts (this);
			this.Expenses = new ReportExpenses (this);
			this.History = new ReportHistoryItems (this);

			this.BindReceipts ();
		}

		public static Report NewInstance () {
			Report report = new Report ();
			report.Expenses = new ReportExpenses (LoggedUser.Instance.BusinessExpenses.Where (expense => expense.ReportPreselection != Expense.ReportPreselectionEnum.None), report);
			return report;
		}

		public Report (ReportResponse reportResponse) : this () {
			this.Populate (reportResponse);
		}

		public Report (ReportApproval parent) : this () {
			this.Approval = parent;
		}
	}
}