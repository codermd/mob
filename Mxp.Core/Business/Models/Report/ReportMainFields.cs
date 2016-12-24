using System;
using Mxp.Core.Business;

namespace Mxp.Core.Business
{
	public class ReportMainFieldName : Field
	{
		public ReportMainFieldName(Report report) : base(report) {
			this.Title = Labels.GetLoggedUserLabel (Labels.LabelEnum.Name);
			this.Permission = FieldPermissionEnum.Mandatory;
			this.Type = FieldTypeEnum.String;
		}

		public override bool IsEditable {
			get {
				if (this.GetModel<Report> ().IsNew
					&& Preferences.Instance.CanShowPermission (Preferences.Instance.REPAllocationTRV)
					&& !String.IsNullOrEmpty (this.GetModel<Report> ().TravelRequestName)
					&& Preferences.Instance.TravelRequestPermission == Preferences.TravelRequestPermissionEnum.Forced)
						return false;

				return this.GetModel<Report> ().IsNew;
			}
		}

		public override object Value {
			get {
				return this.GetModel<Report>().Name;
			}
			set {
				this.GetModel<Report> ().Name = (string)value;
				base.Value = value;
			}
		}
	}

	public class ReportMainFieldDateRange : Field
	{
		public ReportMainFieldDateRange(Report report) : base(report) {
			this.Title = Labels.GetLoggedUserLabel (Labels.LabelEnum.Date);
			this.Permission = FieldPermissionEnum.Optional;
			this.Type = FieldTypeEnum.String;
		}

		public override bool IsEditable {
			get {
				return false;
			}
		}

		public override object Value {
			get {
				return this.GetModel<Report>().VDateRange;
			}
			set {
				base.Value = value;
			}
		}
	}

	public class ReportMainFieldApprovalStatus : Field
	{
		public ReportMainFieldApprovalStatus(Report report) : base(report) {
			this.Title = Labels.GetLoggedUserLabel (Labels.LabelEnum.ApprovalStatus);
			this.Permission = FieldPermissionEnum.Optional;
			this.Type = FieldTypeEnum.String;
		}

		public override bool IsEditable {
			get {
				return false;
			}
		}

		public override object Value {
			get {
				return this.GetModel<Report>().GetVFormattedStatus();
			}
			set {
				base.Value = value;
			}
		}
	}
		
	public class ReportMainFieldReceiptStatus : Field
	{
		public ReportMainFieldReceiptStatus(Report report) : base(report) {
			this.Title = Labels.GetLoggedUserLabel (Labels.LabelEnum.ControlStatus);
			this.Permission = FieldPermissionEnum.Optional;
			this.Type = FieldTypeEnum.String;
		}

		public override bool IsEditable {
			get {
				return false;
			}
		}

		public override object Value {
			get {
				return this.GetModel<Report>().GetVReceiptControlStatus();
			}
			set {
				base.Value = value;
			}
		}
	}

	public class ReportMainFieldPrivatelyFunded : Field
	{
		public ReportMainFieldPrivatelyFunded(Report report) : base(report) {
			this.Title = Labels.GetLoggedUserLabel (Labels.LabelEnum.PrivatelyFunded);
			this.Permission = FieldPermissionEnum.Optional;
			this.Type = FieldTypeEnum.String;
		}

		public override string VValue {
			get {
				return this.GetModel<Report> ().VPrivatalyFounded;
			}
		}

		public override bool IsEditable {
			get {
				return false;
			}
		}
	}

	public class ReportMainFieldCompanyFunded : Field
	{
		public ReportMainFieldCompanyFunded(Report report) : base(report) {
			this.Title = Labels.GetLoggedUserLabel (Labels.LabelEnum.CompanyFounded);
			this.Permission = FieldPermissionEnum.Optional;
			this.Type = FieldTypeEnum.String;
		}

		public override string VValue {
			get {
				return this.GetModel<Report> ().VCompanyFounded;
			}
		}

		public override bool IsEditable {
			get {
				return false;
			}
		}
	}

	public class ReportMainFieldPrepaymeqts : Field
	{
		public ReportMainFieldPrepaymeqts(Report report) : base(report) {
			this.Title = Labels.GetLoggedUserLabel (Labels.LabelEnum.Prepayments);
			this.Permission = FieldPermissionEnum.Optional;
			this.Type = FieldTypeEnum.String;
		}

		public override string VValue {
			get {
				return this.GetModel<Report> ().VPrepayments;
			}
		}

		public override bool IsEditable {
			get {
				return false;
			}
		}
	}

	public class ReportMainFieldAmount : Field
	{
		public ReportMainFieldAmount(Report report) : base(report) {
			this.Title = Labels.GetLoggedUserLabel (Labels.LabelEnum.Amount);
			this.Permission = FieldPermissionEnum.Optional;
			this.Type = FieldTypeEnum.String;
		}

		public override bool IsEditable {
			get {
				return false;
			}
		}

		public override object Value {
			get {
				return this.GetModel<Report>().VAmount;
			}
			set {
				base.Value = value;
			}
		}
	}
}