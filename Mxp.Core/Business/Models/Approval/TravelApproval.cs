using System;
using Mxp.Core.Services.Responses;
using RestSharp.Portable;
using System.Threading.Tasks;
using Mxp.Core.Services;

namespace Mxp.Core.Business
{
	public class TravelApproval : Approval
	{
		public Travel Travel { get; set; }
		public bool AcceptedStatus { get; set; }

		public string VEmployeeFullname {
			get {
				return this.Travel.VEmployeeFullname;
			}
		}

		public string VDateRange {
			get {
				return this.Travel.VDateRange;
			}
		}

		public string VDetailsBarTitle {
			get {
				return Labels.GetLoggedUserLabel (Labels.LabelEnum.Travel);
			}
		}

		public TravelApproval () : base () {
			this.Travel = new Travel (this);
			this.AcceptedStatus = false;
		}

		public TravelApproval (TravelApprovalResponse travelApprovalResponse) : this () {
			this.Populate (travelApprovalResponse);
		}

		public void Populate (TravelApprovalResponse travelApprovalResponse) {
			this.Travel.Populate (travelApprovalResponse);
		}

		public override void Serialize (RestRequest request) {
			request.AddParameter ("comment", this.Comment);
			request.AddParameter ("travelRequestID", this.Travel.Id.ToString());
		}

		public override void TryValidate () {
			// FIXME
		}

		public async Task SubmitAsync () {
			this.TryValidate ();

			if (this.AcceptedStatus)
				await ApprovalService.Instance.AcceptTravelAsync (this);
			else
				await ApprovalService.Instance.RejectTravelAsync (this);

			this.RemoveFromCollectionParent<TravelApproval> ();
		}
	}
}