using System;
using System.Linq;
using System.Threading.Tasks;

using Mxp.Core.Services;
using Mxp.Core.Services.Responses;

namespace Mxp.Core.Business
{
	public class TravelApprovals : Approvals<TravelApproval>
	{
		public TravelApprovals () {

		}

		public async override Task FetchAsync () {
			await ApprovalService.Instance.FetchGenericAsync<TravelApproval, TravelApprovalResponse>(this, ApprovalService.ApiEnum.GetApprovalTravels.GetRoute ());
			this.ReplaceWith (this.OrderByDescending (approval => ((TravelApproval) approval).Travel.FromDate).ToList ());
		}
	}
}