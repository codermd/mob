using System;
using Mxp.Core.Services.Responses;
using System.Linq;
using System.Collections.ObjectModel;
using Mxp.Utils;
using Mxp.Core.Helpers;

namespace Mxp.Core.Business
{
	public partial class Report : Model
	{
		public enum PolicyRulesEnum {
			Red,
			Orange,
			Green
		}

		public enum ApprovalStatusEnum {
			Unknown,
			Rejected,
			Waiting,
			Accepted
		}

		public enum ReceiptStatusEnum {
			Unknown,
			Red,
			Orange,
			Green,
			Black
		}

		public static PolicyRulesEnum GetPolicyRule(string policy) {
			switch (policy.ToLower ()) {
				case "r":
				case "x":
					return PolicyRulesEnum.Red;
				case "o":
					return PolicyRulesEnum.Orange;
				case "g":
				default:
					return PolicyRulesEnum.Green;
			}
		}

		public static ApprovalStatusEnum GetApprovalStatus(string status) {
			if (string.IsNullOrWhiteSpace(status))
				return ApprovalStatusEnum.Unknown;

			switch (status.ToUpper ()) {
				case "R":
					return ApprovalStatusEnum.Rejected;
				case "C":
					return ApprovalStatusEnum.Accepted;
				case "O":
				default:
					return ApprovalStatusEnum.Waiting;
			}
		}

		public static ReceiptStatusEnum GetReceiptStatus(string status) {
			if (string.IsNullOrWhiteSpace(status))
				return ReceiptStatusEnum.Unknown;

			switch (status.ToUpper ()) {
				case "C":
					return ReceiptStatusEnum.Green;
				case "R":
					return ReceiptStatusEnum.Red;
				case "V":
					return ReceiptStatusEnum.Black;
				default:
					return ReceiptStatusEnum.Orange;
			}
		}

		public Reports.ReportTypeEnum ReportType {
			get {
				if (this.IsNew)
					return Reports.ReportTypeEnum.Draft;
				
				if (this.IsFromApproval)
					return Reports.ReportTypeEnum.Approval;
				
				return this.GetCollectionParent<Reports, Report> ().ReportType;
			}
		}

		public int? Id { get; set; }
		public string Comment { get; set; }

		public ApprovalStatusEnum ApprovalStatus { get; set; }
		public ReceiptStatusEnum ReceiptStatus { get; set; }

		public PolicyRulesEnum PolicyRule { get; set; }

		private WeakReferenceObject<ReportApproval> _approval;
		public ReportApproval Approval {
			get {
				return this._approval != null ? this._approval.Value : null;
			}
			private set {
				this._approval = new WeakReferenceObject<ReportApproval> (value);
			}
		}

		// FIXME Getter only
		private int _numberReceipts;
		public int NumberReceipts { 
			get {
				// FIXME WTF
				if (this.Receipts.Count != this._numberReceipts) {
					return this.Receipts.Count;
				}
				return this._numberReceipts;
			} set {
				this._numberReceipts = value;
			}
		}
		public Receipts Receipts { get; set; }

		public DateTime? Date { get; set; }

		public ReportExpenses Expenses { get; set; }
		public ReportHistoryItems History { get; set; }

		private string _name;
		public string Name { 
			get {
				return this._name;
			} 
			set {
				this._name = value;
				this.NotifyPropertyChanged ("Name");
			}
		}

		public double Amount { get; set; }
		public string EmployeeFirstname { get; set; }
		public string EmployeeLastname { get; set; }
		public double Pf { get; set; }
		public double Cf { get; set; }
		public double Retained { get; set; }
		public string DepartmentName { get; set; }
		public string ProjectName { get; set; }

		public string App { get; set; }
		public string Audit { get; set; }
		public string ProjectId { get; set; }
		public string DepartmentId { get; set; }

		private string _travelRequestName;
		public string TravelRequestName { 
			get {
				return this._travelRequestName;
			} 
			set {
				this._travelRequestName = value;

				if (this.IsNew
					&& (LoggedUser.Instance.Preferences.TravelRequestPermission == Preferences.TravelRequestPermissionEnum.Forced
						|| LoggedUser. Instance.Preferences.TravelRequestPermission == Preferences.TravelRequestPermissionEnum.Enabled))
						this.Name = this.TravelRequestName;
			}
		}
		public int? TravelRequestId { get; set; }

		public object Infochar1 { get; set; }
		public object Infochar2 { get; set; }
		public object Infochar3 { get; set; }
		public int Infonum1 { get; set; }
		public int Infonum2 { get; set; }

		public string PolicyRuleTip { get; set; }
		public DateTime? FromDate { get; set; }
		public DateTime? ToDate { get; set; }

		public string Ref { get; set; }
		public string CurrencyIso { get; set; }

		public void Populate (ReportResponse reportResponse) {
			this.Id = reportResponse.ReportID;
			this.ApprovalStatus = GetApprovalStatus(reportResponse.ApprovalStatus);
			this.ReceiptStatus = GetReceiptStatus(reportResponse.receiptStatus);

			// FIXME -> constructor
			this.NumberReceipts = 0;

			this.Date = reportResponse.reportdate.ToDateTime ();
			this.Expenses.Populate (reportResponse.Transactions.Reverse<ExpenseResponse> ());
			this.History.Populate (reportResponse.ReportHistory);
			this.Comment = this.History.Count > 0 ? this.History [0].Comment : null;
			this.Name = reportResponse.ReportHeader.fldreportName;
			this.Amount = reportResponse.ReportHeader.Total;
			this.EmployeeFirstname = reportResponse.ReportHeader.FldEmployeeFirstname;
			this.EmployeeLastname = reportResponse.ReportHeader.FldEmployeeLastname;
			this.Pf = reportResponse.ReportHeader.PF;
			this.Cf = reportResponse.ReportHeader.CF;
			this.Retained = reportResponse.ReportHeader.Retained;
			this.DepartmentName = reportResponse.ReportHeader.flddepartmentname;
			this.ProjectName = reportResponse.ReportHeader.fldprojectname;

			this.TravelRequestName = reportResponse.ReportHeader.fldtravelrequestname;
			this.TravelRequestId = reportResponse.ReportHeader.fldtravelrequestid;

			this.App = reportResponse.ReportHeader.App;
			this.Audit = reportResponse.ReportHeader.Audit;
			this.Ref = reportResponse.ReportHeader.fldreportRef;
			this.CurrencyIso = reportResponse.ReportHeader.fldcurrencyISO;

			this.Infochar1 = reportResponse.ReportHeader.fldReportInfoChar1;
			this.Infochar2 = reportResponse.ReportHeader.fldReportInfoChar2;
			this.Infochar3 = reportResponse.ReportHeader.fldReportInfoChar3;
			this.Infonum1 = reportResponse.ReportHeader.fldReportInfoNum1;
			this.Infonum2 = reportResponse.ReportHeader.fldReportInfoNum2;

			this.PolicyRule = GetPolicyRule(reportResponse.ReportHeader.ReportPolResult);
			this.PolicyRuleTip = reportResponse.ReportHeader.ReportPolTip;

			this.FromDate = reportResponse.ReportHeader.Dmin.ToDateTime ();
			this.ToDate = reportResponse.ReportHeader.Dmax.ToDateTime ();

			this.ResetChanged ();
		}

		public override bool Equals (object obj) {
			if (!(obj is Report))
				return false;

			return this.Id == ((Report)obj).Id;
		}
	}
}