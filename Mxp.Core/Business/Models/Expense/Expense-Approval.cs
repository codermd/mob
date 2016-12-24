using System;
using System.Collections.Generic;
using RestSharp.Portable;

namespace Mxp.Core.Business
{
	public partial class Expense
	{
		public bool IsFromApproval {
			get {
				return this.Report != null ? this.Report.IsFromApproval : false;
			}
		}

		public ReportApproval Approval {
			get {
				return this.IsFromApproval ? this.Report.Approval : null;
			}
		}
	}
}