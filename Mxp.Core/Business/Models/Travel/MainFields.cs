using System;
using Mxp.Core.Business;
using Mxp.Core.Services;

namespace Mxp.Core
{
	public class TravelFieldFirstName : Field
	{
		public TravelFieldFirstName(Travel travel) : base(travel) {
			this.Title = Labels.GetLoggedUserLabel (Labels.LabelEnum.FirstName);
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
				return this.GetModel<Travel>().EmployeeFirstname;
			}
		}
	}

	public class TravelFieldLastName : Field
	{
		public TravelFieldLastName(Travel travel) : base(travel) {
			this.Title = Labels.GetLoggedUserLabel (Labels.LabelEnum.LastName);
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
				return this.GetModel<Travel>().EmployeeLastname;
			}
		}
	}

	public class TravelFieldName : Field
	{
		public TravelFieldName(Travel travel) : base(travel) {
			this.Title = Labels.GetLoggedUserLabel (Labels.LabelEnum.Name);
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
				return this.GetModel<Travel>().Name;
			}
		}
	}

	public class TravelFieldMaxAmount : Field
	{
		public TravelFieldMaxAmount (Travel travel) : base (travel) {
			this.Title = Labels.GetLoggedUserLabel (Labels.LabelEnum.TotalAmount);
			this.Permission = FieldPermissionEnum.Optional;
			this.Type = FieldTypeEnum.Decimal;
		}

		public override bool IsEditable {
			get {
				return false;
			}
		}

		public override object Value {
			get {
				return this.GetModel<Travel>().MaxAmount;
			}
		}
	}

	public class TravelFieldFromDate : Field
	{
		public TravelFieldFromDate(Travel travel) : base(travel) {
			this.Title = Labels.GetLoggedUserLabel (Labels.LabelEnum.StartDate);
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
				return LoggedUser.Instance.Preferences.VDate (this.GetModel<Travel> ().FromDate.GetValueOrDefault ());
			}
		}
	}

	public class TravelFieldToDate : Field
	{
		public TravelFieldToDate(Travel travel) : base(travel) {
			this.Title = Labels.GetLoggedUserLabel (Labels.LabelEnum.EndDate);
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
				return LoggedUser.Instance.Preferences.VDate (this.GetModel<Travel> ().ToDate.GetValueOrDefault ());
			}
		}
	}

	public class TravelFieldStatus : Field
	{
		public TravelFieldStatus(Travel travel) : base(travel) {
			this.Title = Labels.GetLoggedUserLabel (Labels.LabelEnum.Status);
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
				return this.GetModel<Travel>().Status;
			}
		}
	}

	public class TravelFieldProject : LookupField
	{
		public TravelFieldProject(Travel travel) : base(travel) {
			this.Title = Labels.GetLoggedUserLabel (Labels.LabelEnum.ProjectID);
			this.Permission = FieldPermissionEnum.Optional;
			this.LookupKey = LookupService.ApiEnum.GetLookupProject;
			this.Type = FieldTypeEnum.String;
		}

		public override bool IsEditable {
			get {
				return false;
			}
		}

		public override object Value {
			get {
				return this.GetModel<Travel>();
			}
		}
	}

	public class TravelFieldComment : Field
	{
		public TravelFieldComment(Travel travel) : base(travel) {
			this.Title = Labels.GetLoggedUserLabel (Labels.LabelEnum.Comment);
			this.Permission = FieldPermissionEnum.Optional;
			this.Type = FieldTypeEnum.LongString;
		}

		public override bool IsEditable {
			get {
				return true;
			}
		}

		public override object Value {
			get {
				return this.GetModel<Travel> ().Comment;
			}
			set {
				this.GetModel<Travel> ().Comment = (string)value;
				base.Value = value;
			}
		}
	}
		
	public class TravelFieldOtherRequests : Field
	{
		public TravelFieldOtherRequests(Travel travel) : base(travel) {
			this.Title = Labels.GetLoggedUserLabel (Labels.LabelEnum.Additional);
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
				return this.GetModel<Travel> ().Additional;
			}
		}
	}
}