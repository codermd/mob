using System;
using System.Collections.Generic;

namespace Mxp.Core.Services.Responses
{
	public class ReportHeaderResponse : Response
	{
		public string ReportPolResult { get; set; }
		public string ReportPolTip { get; set; }
		public string ReportSupervisorRemark { get; set; }
		public string ReportUserRemark { get; set; }
		public string fldreportName { get; set; }
		public string fldreportRef { get; set; }
		public string flddepartmentname { get; set; }
		public string FldEmployeeReference { get; set; }
		public string FldEmployeeFirstname { get; set; }
		public string FldEmployeeLastname { get; set; }
		public string flddepartmentreference { get; set; }
		public string fldprojectname { get; set; }
		public string fldprojectcode { get; set; }
		public string fldtravelrequestname { get; set; }
		public int? fldtravelrequestid { get; set; }
		public string fldReportInfoChar1 { get; set; }
		public string fldReportInfoChar2 { get; set; }
		public string fldReportInfoChar3 { get; set; }
		public int fldReportInfoNum1 { get; set; }
		public int fldReportInfoNum2 { get; set; }
		public int Nbtrx { get; set; }
		public double Total { get; set; }
		public double Retained { get; set; }
		public string Dmin { get; set; }
		public string Dmax { get; set; }
		public double CF { get; set; }
		public double PF { get; set; }
		public string SUPFirstName { get; set; }
		public string SUPLastName { get; set; }
		public int fldEmployeeID { get; set; }
		public string App { get; set; }
		public string Audit { get; set; }
		public string fldcurrencyISO { get; set; }

		public ReportHeaderResponse () {}
	}
}