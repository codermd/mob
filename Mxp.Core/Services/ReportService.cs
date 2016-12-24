using System;
using System.Threading.Tasks;
using Mxp.Core.Business;
using RestSharp.Portable;
using System.Collections.Generic;
using Mxp.Core.Services.Responses;
using System.Diagnostics;
using System.Linq;
using System.Collections.ObjectModel;

namespace Mxp.Core.Services
{
	public static class ReportServiceExtensions {
		public static string GetRoute (this ReportService.ApiEnum route) {
			switch (route) {
				case ReportService.ApiEnum.GetDraftReports:
					return "APIEWF/GetDraftReports";
				case ReportService.ApiEnum.GetOpenReports:
					return "APIEWF/GetOpenReports";
				case ReportService.ApiEnum.GetClosedReports:
					return "APIEWF/GetClosedReports";
				case ReportService.ApiEnum.CreateReport:
					return "APIEWF/CreateReport";
				case ReportService.ApiEnum.CancelReport:
					return "APIEWF/CancelReport";
				case ReportService.ApiEnum.ConfirmReport:
					return "APIEWF/ConfirmReport";
				case ReportService.ApiEnum.RemoveReportExpense:
					return "APIEWF/RemoveExpense";
				case ReportService.ApiEnum.GetReportReceipts:
					return "APIReceipt/GetReportReceipts";
				case ReportService.ApiEnum.AddReportReceipt:
					return "APIReceipt/SaveReportReceipt";
				default:
					return null;
			}
		}
	}

	public class ReportService : Service
	{
		public static readonly ReportService Instance = new ReportService ();

		public enum ApiEnum {
			GetDraftReports,
			GetOpenReports,
			GetClosedReports,
			CreateReport,
			CancelReport,
			ConfirmReport,
			RemoveReportExpense,
			GetReportReceipts,
			AddReportReceipt
		}

		private ReportService () : base () {

		}

		public async Task SaveReportAsync (Report report) {
			RestRequest request = new RestRequest(ApiEnum.CreateReport.GetRoute ());
			report.Serialize (request);
			ReportResponse response = await this.ExecuteAsync<ReportResponse> (request);
			report.Populate (response);
		}

		public async Task RemoveReportExpenseAsync (Report report, Expense expense) {
			RestRequest request = new RestRequest(ApiEnum.RemoveReportExpense.GetRoute ());

			request.AddParameter ("ReportID", report.Id.ToString());
			request.AddParameter ("ItemID", expense.Id.ToString());

			ReportResponse response = await this.ExecuteAsync<ReportResponse> (request);
			report.Populate (response);
		}

		public async Task SubmitReportAsync (Report report) {
			RestRequest request = new RestRequest(ApiEnum.ConfirmReport.GetRoute ());
			request.AddParameter ("reportID", report.Id.ToString ());
			await this.ExecuteAsync (request);
		}

		public async Task DeleteReportAsync (Report report) {
			RestRequest request = new RestRequest(ApiEnum.CancelReport.GetRoute ());
			request.AddParameter ("reportID", report.Id.ToString ());
			await this.ExecuteAsync (request);
		}
	}
}