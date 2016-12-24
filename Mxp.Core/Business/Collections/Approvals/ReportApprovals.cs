using System;
using System.Linq;
using System.Threading.Tasks;

using Mxp.Core.Services;
using Mxp.Core.Services.Responses;

namespace Mxp.Core.Business
{
	public class ReportApprovals : Approvals<ReportApproval>
	{
		public ReportApprovals () {

		}

		public async override Task FetchAsync () {
			await ApprovalService.Instance.FetchGenericAsync<ReportApproval, ReportApprovalResponse>(this, ApprovalService.ApiEnum.GetApprovalReports.GetRoute ());
			this.ReplaceWith (this.OrderByDescending (approval => ((ReportApproval) approval).Report.FromDate).ToList());
		}
	}
}