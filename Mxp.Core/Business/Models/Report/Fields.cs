using System;
using Mxp.Core.Business;
using Mxp.Core.Services;
using Mxp.Core.Extensions;
using Mxp.Utils;

namespace Mxp.Core
{
	public class ReportFieldEmployee : Field
	{
		public ReportFieldEmployee(Report report): base(report){
			this.Title = LoggedUser.Instance.Labels.GetLabel (Labels.LabelEnum.Employee);
			this.Permission = FieldPermissionEnum.Optional;
			this.Type = FieldTypeEnum.String;
		}

		public override string VValue {
			get {
				return this.GetModel<Report> ().VEmployeeFullname;
			}
		}

		public override bool IsEditable {
			get {
				return this.GetModel<Report> ().IsNew || this.GetModel<Report> ().IsEditable;
			}
		}
	}

	public class ReportFieldAllocationPJT : LookupField
	{
		public ReportFieldAllocationPJT(Report report): base(report){
			this.Title = LoggedUser.Instance.Labels.GetLabel (Labels.LabelEnum.ProjectID);
			this.LookupKey = LookupService.ApiEnum.GetLookupProject;
		}
			
		public override string VValue {
			get {
				if (!this.GetModel<Report> ().IsNew) {
					return this.GetModel<Report> ().ProjectName;
				}
				return base.VValue;
			}
		}

		public override object Value {
			get {
				return this.GetModel<Report>().ProjectId;
			}
			set {
				this.GetModel<Report> ().ProjectId = ((LookupItem)value).Id;
				base.Value = value;
			}
		}

		public override bool IsEditable {
			get {
				return this.GetModel<Report> ().IsNew || this.GetModel<Report> ().IsEditable;
			}
		}
	}

	public class ReportFieldAllocationDPT : LookupField
	{
		public ReportFieldAllocationDPT(Report report): base(report){
			this.Title = LoggedUser.Instance.Labels.GetLabel (Labels.LabelEnum.DepartmentID);
			this.LookupKey = LookupService.ApiEnum.GetLookupDepartment;
		}

		public override string VValue {
			get {
				if (!this.GetModel<Report> ().IsNew)
					return this.GetModel<Report> ().DepartmentName;

				return base.VValue;
			}
		}

		public override object Value {
			get {
				return this.GetModel<Report>().DepartmentId;
			}
			set {
				this.GetModel<Report> ().DepartmentId = ((LookupItem)value).Id;
				base.Value = value;
			}
		}

		public override bool IsEditable {
			get {
				return this.GetModel<Report> ().IsNew || this.GetModel<Report> ().IsEditable;
			}
		}
	}

	public class ReportFieldAllocationTRV : LookupField
	{
		public ReportFieldAllocationTRV (Report report): base (report) {
			this.Title = LoggedUser.Instance.Labels.GetLabel (Labels.LabelEnum.TravelRequestID);
			this.LookupKey = LookupService.ApiEnum.GetLookupTravelRequests;
		}

		public override string VValue {
			get {
				if (!this.GetModel<Report> ().IsNew)
					return this.GetModel<Report> ().TravelRequestName;

				return base.VValue;
			}
		}

		public override object Value {
			get {
				return this.GetModel<Report>().TravelRequestId;
			}
			set {
				LookupItem lookup = (LookupItem)value;
				this.GetModel<Report> ().TravelRequestId = lookup.ComboId;
				this.GetModel<Report> ().TravelRequestName = !lookup.Id.IsInt () ? lookup.Name : null;

				base.Value = lookup;
			}
		}

		public override bool IsEditable {
			get {
				return this.GetModel<Report> ().IsNew || this.GetModel<Report> ().IsEditable;
			}
		}
	}
}