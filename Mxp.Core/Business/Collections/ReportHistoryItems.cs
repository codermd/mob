using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Mxp.Core.Services.Responses;
using System.Collections.ObjectModel;
using System.Linq;

namespace Mxp.Core.Business
{
	public class ReportHistoryItems : SGCollection<ReportHistoryItem>
	{
		public ReportHistoryItems (Report report) : base (report) {

		}

		public ReportHistoryItems (List<ReportHistoryItemResponse> reportHistoryItemResponses, Report report) : base (reportHistoryItemResponses, report) {

		}

		public override void Populate (IEnumerable<Response> collection) {
			List<ReportHistoryItemResponse> reportHistoryItemResponses = collection as List<ReportHistoryItemResponse>;

			reportHistoryItemResponses.RemoveAll (reportHistoryItem => String.IsNullOrWhiteSpace (reportHistoryItem.HistoryLine));

			base.Populate (reportHistoryItemResponses);
		}
	}
}