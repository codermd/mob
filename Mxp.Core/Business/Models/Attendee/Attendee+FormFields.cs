using System;

namespace Mxp.Core.Business
{
	public abstract class AttendeeFormBase : Field
	{
		public AttendeeFormBase (Attendee attendee, bool isMandatory) : base (attendee) {
			this.Permission = isMandatory ? FieldPermissionEnum.Mandatory : FieldPermissionEnum.Optional;
		}

		public override bool IsEditable {
			get {
				return true;
			}
		}
	}

	public class AttendeeFormFirstname : AttendeeFormBase
	{
		public AttendeeFormFirstname (Attendee attendee, bool isMandatory = false) : base (attendee, isMandatory) {
			this.Title = Labels.GetLoggedUserLabel (Labels.LabelEnum.FirstName);
			this.Type = FieldTypeEnum.String;
		}

		public override object Value {
			get {
				return this.GetModel<Attendee> ().Firstname;
			}
			set {
				this.GetModel<Attendee> ().Firstname = (string)value;
			}
		}
	}

	public class AttendeeFormLastname : AttendeeFormBase
	{
		public AttendeeFormLastname (Attendee attendee, bool isMandatory = false) : base (attendee, isMandatory) {
			this.Title = Labels.GetLoggedUserLabel (Labels.LabelEnum.LastName);
			this.Type = FieldTypeEnum.String;
		}

		public override object Value {
			get {
				return this.GetModel<Attendee> ().Lastname;
			}
			set {
				this.GetModel<Attendee> ().Lastname = (string)value;
			}
		}
	}

	public class AttendeeFormCompanyName : AttendeeFormBase
	{
		public AttendeeFormCompanyName (Attendee attendee, bool isMandatory = false) : base (attendee, isMandatory) {
			this.Title = Labels.GetLoggedUserLabel (Labels.LabelEnum.Company);
			this.Type = FieldTypeEnum.String;
		}

		public override object Value {
			get {
				return this.GetModel<Attendee> ().CompanyName;
			}
			set {
				this.GetModel<Attendee> ().CompanyName = (string)value;
			}
		}
	}

	public class AttendeeFormAddress : AttendeeFormBase
	{
		public AttendeeFormAddress (Attendee attendee, bool isMandatory = false) : base (attendee, isMandatory) {
			this.Title = Labels.GetLoggedUserLabel (Labels.LabelEnum.Address);
			this.Type = FieldTypeEnum.String;
		}

		public override object Value {
			get {
				return this.GetModel<Attendee> ().Address;
			}
			set {
				this.GetModel<Attendee> ().Address = (string)value;
			}
		}
	}

	public class AttendeeFormZipCode : AttendeeFormBase
	{
		public AttendeeFormZipCode (Attendee attendee, bool isMandatory = false) : base (attendee, isMandatory) {
			this.Title = Labels.GetLoggedUserLabel (Labels.LabelEnum.ZipCode);
			this.Type = FieldTypeEnum.Integer;
		}

		public override object Value {
			get {
				return this.GetModel<Attendee> ().ZipCode;
			}
			set {
				this.GetModel<Attendee> ().ZipCode = (int)value;
			}
		}
	}

	public class AttendeeFormCity : AttendeeFormBase
	{
		public AttendeeFormCity (Attendee attendee, bool isMandatory = false) : base (attendee, isMandatory) {
			this.Title = Labels.GetLoggedUserLabel (Labels.LabelEnum.City);
			this.Type = FieldTypeEnum.String;
		}

		public override object Value {
			get {
				return this.GetModel<Attendee> ().City;
			}
			set {
				this.GetModel<Attendee> ().City = (string)value;
			}
		}
	}

	public class AttendeeFormState : AttendeeFormBase
	{
		public AttendeeFormState (Attendee attendee) : base (attendee, attendee.Type.IsStateMandatory ()) {
			this.Title = Labels.GetLoggedUserLabel (Labels.LabelEnum.State);
			this.Type = FieldTypeEnum.String;
		}

		public override object Value {
			get {
				return this.GetModel<Attendee> ().State;
			}
			set {
				this.GetModel<Attendee> ().State = (string)value;
			}
		}
	}

	public class AttendeeFormSpecialty : AttendeeFormBase
	{
		public AttendeeFormSpecialty (Attendee attendee, bool isMandatory = false) : base (attendee, isMandatory) {
			this.Title = Labels.GetLoggedUserLabel (Labels.LabelEnum.Speciality);
			this.Type = FieldTypeEnum.String;
		}

		public override object Value {
			get {
				return this.GetModel<Attendee> ().Specialty;
			}
			set {
				this.GetModel<Attendee> ().Specialty = (string)value;
			}
		}
	}

	public class AttendeeFormReference : AttendeeFormBase
	{
		public AttendeeFormReference (Attendee attendee, bool isMandatory = false) : base (attendee, isMandatory) {
			this.Title = Labels.GetLoggedUserLabel (Labels.LabelEnum.Npi);
			this.Type = FieldTypeEnum.String;
		}

		public override object Value {
			get {
				return this.GetModel<Attendee> ().Reference;
			}
			set {
				this.GetModel<Attendee> ().Reference = (string)value;
			}
		}
	}


	public class AttendeeFormCountry : AttendeeFormBase
	{
		public AttendeeFormCountry (Attendee attendee) : base (attendee, true) {
			this.Title = Labels.GetLoggedUserLabel (Labels.LabelEnum.Country);
			this.Type = FieldTypeEnum.Country;
		}

		public override object Value {
			get {
				return this.GetModel<Attendee> ().Country;
			}
			set {
				this.GetModel<Attendee> ().Country = (Country)value;
			}
		}

		public override string VValue {
			get {
				return this.GetValue<Country> ()?.Name;
			}
		}
	}
}