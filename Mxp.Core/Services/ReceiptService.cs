using System;
using System.Linq;
using System.Text;
using System.Net.Http;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using RestSharp.Portable;
using Newtonsoft.Json;

using Mxp.Core.Business;
using Mxp.Core.Services.Responses;

namespace Mxp.Core.Services
{
	public static class ReceiptServiceExtensions {
		public static string GetRoute (this ReceiptService.ApiEnum route) {
			switch (route) {
				case ReceiptService.ApiEnum.GetExpenseReceipts:
					return "APIReceipt/GetItemReceipts";
				case ReceiptService.ApiEnum.AddExpenseReceipt:
					return "APIReceipt/SaveItemReceipt";
				case ReceiptService.ApiEnum.DeleteReceiptReceipt:
					return "APIReceipt/DeleteItemReceipt";
				case ReceiptService.ApiEnum.GetReportReceipts:
					return "APIReceipt/GetReportReceipts";
				case ReceiptService.ApiEnum.AddReportReceipt:
					return "APIReceipt/SaveReportReceipt";
				default:
					return null;
			}
		}
	}
		
	public class ReceiptService : Service
	{
		public static readonly ReceiptService Instance = new ReceiptService ();

		public enum ApiEnum {
			GetExpenseReceipts,
			AddExpenseReceipt,
			DeleteReceiptReceipt,
			GetReportReceipts,
			AddReportReceipt
		}

		private ReceiptService () : base () {

		}

		public void ConfigureAddRequest (RestRequest request, String base64) {
			request.AddParameter ("ObjectType", "trx");
			request.AddParameter ("fileType", "image/jpeg");
			request.AddParameter ("ImageName", "mobileUpload.jpg");
			request.AddParameter ("ImageData", base64);
		}

		public async Task FetchExpenseReceiptsAsync (Expense expense) {
			RestRequest request = new RestRequest (ApiEnum.GetExpenseReceipts.GetRoute ());

			request.AddParameter ("itemid", expense.Id);

			List<ReceiptResponse> receiptsResponses = await this.ExecuteAsync<List<ReceiptResponse>> (request);

			expense.Receipts.Populate (receiptsResponses);
		}

		public async Task FetchReportReceiptsAsync (Report report) {
			RestRequest request = new RestRequest (ApiEnum.GetReportReceipts.GetRoute ());

			request.AddParameter ("ReportID", report.Id);

			List<ReceiptResponse> receiptsResponses = await this.ExecuteAsync<List<ReceiptResponse>> (request);

			report.Receipts.Populate (receiptsResponses);
		}

		public async Task AddReceiptAsync (Expense expense, string base64) {
			Receipt newReceipt = new Receipt (base64);

			RestRequest request = new RestRequest (ApiEnum.AddExpenseReceipt.GetRoute ());

			request.AddParameter ("ItemID", expense.Id);

			this.ConfigureAddRequest (request, base64);

			ReceiptResponse response =  await this.ExecuteAsync<ReceiptResponse> (request, Service.TIMEOUT_UPLOAD_IMAGE_IN_SECONDS);

			newReceipt.Populate (response);
			expense.Receipts.AddItem (newReceipt);
		}

		public async Task AddReceiptAsync (Report report, string base64) {
			Receipt newReceipt = new Receipt (base64);

			RestRequest request = new RestRequest (ApiEnum.AddReportReceipt.GetRoute ());

			request.AddParameter ("ReportID", report.Id);

			this.ConfigureAddRequest (request, base64);

			ReceiptResponse response = await this.ExecuteAsync<ReceiptResponse> (request, Service.TIMEOUT_UPLOAD_IMAGE_IN_SECONDS);

			newReceipt.Populate (response);
			report.Receipts.AddItem (newReceipt);
		}

		public async Task RemoveReceiptAsync (Receipt receipt) {
			RestRequest request = new RestRequest (ApiEnum.DeleteReceiptReceipt.GetRoute ());
			request.AddParameter ("attachmentID", receipt.AttachmentId.ToString());
			await this.ExecuteAsync<object> (request);
		}
	}
}