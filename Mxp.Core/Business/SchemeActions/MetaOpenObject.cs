using System;
using Mxp.Utils;
using Mxp.Core.Services.Responses;

namespace Mxp.Core.Business
{
	public class MetaOpenObject
	{
		public enum LocationEnum {
			Unknown,
			PendingExpenses,
			DraftReports,
			OpenReports,
			ClosedReports,
			ApprovalReports,
			ApprovalTravelRequests
		}

		public LocationEnum Location { get; set; }
		public int Id { get; set; }
		public int FatherId { get; set; }

		public MetaOpenObject (int location, int id, int fatherId) {
			this.Location = (LocationEnum) location;
			this.Id = id;
			this.FatherId = fatherId;
		}

		public MetaOpenObject (OpenObjectResponse openObjectResponse) {
			this.Location = openObjectResponse.Location.ToEnum (LocationEnum.Unknown);
			this.Id = openObjectResponse.Id;
			this.FatherId = openObjectResponse.FatherId;
		}

		public bool HasFatherId {
			get {
				return this.FatherId != 0;
			}
		}
	}
}