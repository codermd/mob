using System;
using Mxp.Core.Utils;

namespace Mxp.Core.Business
{
	public partial class Report
	{
		public String GetVFormattedStatus () {
			return Shared.GetVFormattedStatus(this.App);
		}

		public String GetVReceiptControlStatus () {
			return Shared.GetVFormattedStatus(this.Audit);
		}

		public string VAmount {
			get {

				return this.Amount + " " + this.CurrencyIso;
			}
		}
		public string VEmployeeFullname {
			get {
				return this.EmployeeFirstname + " " + this.EmployeeLastname;
			}
		}

		public string VDateRange {
			get {
				return this.FromDate.GetValueOrDefault ().ToString ("d") + " - " + this.ToDate.GetValueOrDefault ().ToString ("d");
			}
		}

		public string VDetailsBarTitle {
			get {
				return this.Ref;
			}
		}

		public string VPrivatalyFounded {
			get {
				return Math.Round (this.Pf, 2) + " " + this.CurrencyIso;
			}
		}

		public string VCompanyFounded {
			get {
				return Math.Round (this.Cf, 2) + " " + this.CurrencyIso;
			}
		}

		public string VPrepayments {
			get {
				return Math.Round (this.Retained, 2) + " " + this.CurrencyIso;
			}
		}

		public bool CanShowApprovalStatus {
			get {
				return !this.IsDraft || (this.ReportType == Reports.ReportTypeEnum.Draft && this.ApprovalStatus == ApprovalStatusEnum.Rejected);
			}
		}

		public bool CanShowReceiptStatus {
			get {
				return !this.IsDraft || (this.ReportType == Reports.ReportTypeEnum.Draft && this.ApprovalStatus == ApprovalStatusEnum.Rejected);
			}
		}
	}
}