using System;
using System.Collections.Generic;

namespace Mxp.Core.Services.Responses
{
	public class ReportResponse : Response
	{
		public string reportdate { get; set; }
		public string ApprovalStatus { get; set; }
		public string receiptStatus { get; set; }
		public double grossamountcc { get; set; }
		public int ReportID { get; set; }
		public ReportHeaderResponse ReportHeader { get; set; }
		public List<ReportHistoryItemResponse> ReportHistory { get; set; }
		public List<ExpenseResponse> Transactions { get; set; }

		public ReportResponse () {}
	}
}