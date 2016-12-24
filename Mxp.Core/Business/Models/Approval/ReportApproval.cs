using System;
using System.Collections.Generic;
using Mxp.Core.Services.Responses;
using System.Collections.ObjectModel;
using Mxp.Core.Utils;
using System.Threading.Tasks;
using RestSharp.Portable;
using Mxp.Core.Services;

namespace Mxp.Core.Business
{
	public partial class ReportApproval: Approval
	{
		public Report Report { get; set; }

		public ReportApproval () : base () {
			this.Report = new Report (this);
		}

		public ReportApproval (ReportApprovalResponse reportApprovalResponse) : this () {
			this.Report.Populate (reportApprovalResponse);
		}

		public void Populate (ReportApprovalResponse reportApprovalResponse) {
			this.Report.Populate (reportApprovalResponse);
		}

		public override void Serialize (RestRequest request) {
			request.AddParameter ("reportID", this.Report.Id.ToString ());
			request.AddParameter ("Comment", this.Comment);

			this.Report.Expenses.ForEach (expense => {
				expense.ExpenseItems.ForEach (expenseItem => {
					request.AddParameter ("item_" + expenseItem.Id, (expenseItem.StatusForApprovalReport == ExpenseItem.Status.Accepted).ToString ().ToLowerInvariant ());
				});
			});
		}
			
		public override void TryValidate () {
			if (String.IsNullOrWhiteSpace (this.Comment)
				&& Preferences.Instance.REPApprovalRejectionComment == PermissionEnum.Mandatory
				&& !this.IsAllExpensesAccepted) {
				throw new ValidationError ("ERROR", Labels.GetLoggedUserLabel (Labels.LabelEnum.ErrorValidation) + " : " + Labels.GetLoggedUserLabel (Labels.LabelEnum.Comment));
			}
		}

		public async Task SaveAsync () {
			this.TryValidate ();

			await ApprovalService.Instance.ApproveReportAsync (this);
			await LoggedUser.Instance.ReportApprovals.FetchAsync ();
		}
	}
}