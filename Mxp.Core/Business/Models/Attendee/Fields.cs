using System;

namespace Mxp.Core.Business
{
	public class AttendeeName : Field
	{
		public AttendeeName(Attendee attendee) : base(attendee) {
			this.Title = Labels.GetLoggedUserLabel (Labels.LabelEnum.Name);
			this.Permission = FieldPermissionEnum.Optional;
			this.Type = FieldTypeEnum.String;
		}

		public override object Value {
			get {
				return this.GetModel<Attendee> ().Name;
			}
		}

		public override bool IsEditable {
			get {
				return false;
			}
		}
	}

	public class AttendeeCompanyName : Field
	{
		public AttendeeCompanyName(Attendee attendee) : base(attendee) {
			this.Title = Labels.GetLoggedUserLabel (Labels.LabelEnum.Company);
			this.Permission = FieldPermissionEnum.Optional;
			this.Type = FieldTypeEnum.String;
		}

		public override object Value {
			get {
				return this.GetModel<Attendee> ().CompanyName;
			}
		}

		public override bool IsEditable {
			get {
				return false;
			}
		}
	}

	public class AttendeeType : Field
	{
		public AttendeeType(Attendee attendee) : base(attendee) {
			this.Title = Labels.GetLoggedUserLabel (Labels.LabelEnum.Type);
			this.Permission = FieldPermissionEnum.Optional;
			this.Type = FieldTypeEnum.String;
		}

		public override object Value {
			get {
				return this.GetModel<Attendee> ().Type;
			}
		}

		public override bool IsEditable {
			get {
				return false;
			}
		}
	}
		
	public class AttendeeReference : Field
	{
		public AttendeeReference(Attendee attendee) : base(attendee) {
			this.Title = "#Reference";
			this.Permission = FieldPermissionEnum.Optional;
			this.Type = FieldTypeEnum.String;
		}

		public override object Value {
			get {
				return this.GetModel<Attendee> ().Reference;
			}
		}

		public override bool IsEditable {
			get {
				return false;
			}
		}
	}

	public class AttendeeAddress : Field
	{
		public AttendeeAddress(Attendee attendee) : base(attendee) {
			this.Title = Labels.GetLoggedUserLabel (Labels.LabelEnum.Address);
			this.Permission = FieldPermissionEnum.Optional;
			this.Type = FieldTypeEnum.String;
		}

		public override object Value {
			get {
				return this.GetModel<Attendee> ().Address;
			}
			set {
				this.GetModel<Attendee> ().Address = (string)value;
				base.Value = value;
			}
		}

		public override bool IsEditable {
			get {
				return false;
			}
		}
	}

	public class AttendeeZipCode : Field
	{
		public AttendeeZipCode(Attendee attendee) : base(attendee) {
			this.Title = Labels.GetLoggedUserLabel (Labels.LabelEnum.ZipCode);
			this.Permission = FieldPermissionEnum.Optional;
			this.Type = FieldTypeEnum.Integer;
		}

		public override object Value {
			get {
				return this.GetModel<Attendee> ().ZipCode;
			}
			set {
				this.GetModel<Attendee> ().ZipCode = (int)value;
				base.Value = value;
			}
		}

		public override bool IsEditable {
			get {
				return false;
			}
		}
	}

	public class AttendeeCity : Field
	{
		public AttendeeCity(Attendee attendee) : base(attendee) {
			this.Title = Labels.GetLoggedUserLabel (Labels.LabelEnum.City);
			this.Permission = FieldPermissionEnum.Optional;
			this.Type = FieldTypeEnum.String;
		}

		public override object Value {
			get {
				return this.GetModel<Attendee> ().City;
			}
			set {
				this.GetModel<Attendee> ().City = (string)value;
				base.Value = value;
			}
		}

		public override bool IsEditable {
			get {
				return false;
			}
		}
	}

	public class AttendeeState : Field
	{
		public AttendeeState(Attendee attendee) : base(attendee) {
			this.Title = Labels.GetLoggedUserLabel (Labels.LabelEnum.State);
			this.Permission = FieldPermissionEnum.Optional;
			this.Type = FieldTypeEnum.String;
		}

		public override object Value {
			get {
				return this.GetModel<Attendee> ().State;
			}
			set {
				this.GetModel<Attendee> ().State = (string)value;
				base.Value = value;
			}
		}

		public override bool IsEditable {
			get {
				return false;
			}
		}
	}

	public class AttendeeSpecialty : Field
	{
		public AttendeeSpecialty(Attendee attendee) : base(attendee) {
			this.Title = Labels.GetLoggedUserLabel (Labels.LabelEnum.Speciality);
			this.Permission = FieldPermissionEnum.Optional;
			this.Type = FieldTypeEnum.String;
		}

		public override object Value {
			get {
				return this.GetModel<Attendee> ().Specialty;
			}
			set {
				this.GetModel<Attendee> ().Specialty = (string)value;
				base.Value = value;
			}
		}

		public override bool IsEditable {
			get {
				return false;
			}
		}
	}
}