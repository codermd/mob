using System;
using Mxp.Core.Services.Responses;
using Mxp.Utils;

namespace Mxp.Core.Business
{
	public class ReportHistoryItem : Model
	{
		public string Comment { get; set; }
		public string Line { get; set; }
		public DateTime Date { get; set; }

		public ReportHistoryItem (ReportHistoryItemResponse reportHistoryItemResponse) {
			this.Comment = reportHistoryItemResponse.HistoryComment;
			this.Line = reportHistoryItemResponse.HistoryLine;
			this.Date = reportHistoryItemResponse.Date.ToDateTime ().Value;
		}
	}
}