using System;
using System.Threading.Tasks;
using Mxp.Core.Business;
using RestSharp.Portable;
using System.Collections.Generic;
using Mxp.Core.Services.Responses;
using System.Diagnostics;
using System.Linq;
using System.Collections.ObjectModel;
using Mxp.Core.Utils;

namespace Mxp.Core.Services
{
	public static class ApprovalServiceExtensions {
		public static string GetRoute (this ApprovalService.ApiEnum route) {
			switch (route) {
				case ApprovalService.ApiEnum.GetApprovalReports:
					return "APIEWF/GetApprovalReports";
				case ApprovalService.ApiEnum.GetApprovalTravels:
					return "APITR/GetTRApprovalList";
				case ApprovalService.ApiEnum.ApproveReport:
					return "APIEWF/ApproveReport";
				case ApprovalService.ApiEnum.AcceptTravel:
					return "APITR/AcceptTR";
				case ApprovalService.ApiEnum.RejectTravel:
					return "APITR/RejectTR";
				default:
					return null;
			}
		}
	}

	public class ApprovalService : Service
	{
		public static readonly ApprovalService Instance = new ApprovalService ();

		public enum ApiEnum {
			GetApprovalReports,
			GetApprovalTravels,
			ApproveReport,
			AcceptTravel,
			RejectTravel
		}

		private ApprovalService () : base () {

		}

		public async Task AcceptTravelAsync (TravelApproval approval) {
			RestRequest request = new RestRequest (ApiEnum.AcceptTravel.GetRoute ());
			approval.Serialize (request);

			await this.ExecuteAsync (request);
		}

		public async Task RejectTravelAsync (TravelApproval approval) {
			RestRequest request = new RestRequest (ApiEnum.RejectTravel.GetRoute ());
			approval.Serialize (request);

			await this.ExecuteAsync (request);
		}
			
		public async Task ApproveReportAsync (ReportApproval approval) {
			RestRequest request = new RestRequest (ApiEnum.ApproveReport.GetRoute ());
			approval.Serialize (request);

			await this.ExecuteAsync (request);
		}
	}
}