using System;
using System.Linq;
using System.Threading.Tasks;

using Mxp.Core.Services;
using Mxp.Core.Services.Responses;

namespace Mxp.Core.Business
{
	public class Reports : SGCollection<Report> 
	{
		public enum ReportTypeEnum {
			Draft,
			Open,
			Closed,
			Approval
		}

		public ReportTypeEnum ReportType { get; }

		public Reports (ReportTypeEnum type) : base () {
			this.ReportType = type;
		}

		public override async Task FetchAsync () {
			ReportService.ApiEnum route = default (ReportService.ApiEnum);
			switch (this.ReportType) {
			case ReportTypeEnum.Draft:
				route = ReportService.ApiEnum.GetDraftReports;
				break;
			case ReportTypeEnum.Open:
				route = ReportService.ApiEnum.GetOpenReports;
				break;
			case ReportTypeEnum.Closed:
				route = ReportService.ApiEnum.GetClosedReports;
				break;
			}

			await ReportService.Instance.FetchGenericAsync<Report, ReportResponse> (this, route.GetRoute ());
			this.ReplaceWith (this.OrderByDescending (report => report.FromDate).ToList());
		}
	}
}