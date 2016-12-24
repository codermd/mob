using System;

namespace Mxp.Core.Services.Responses
{
	public class ReportHistoryItemResponse : Response
	{
		public string Date { get; set; }
		public string HistoryLine { get; set; }
		public string HistoryComment { get; set; }

		public ReportHistoryItemResponse ()	{}
	}
}