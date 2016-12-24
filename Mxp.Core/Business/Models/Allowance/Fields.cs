using System;
using Mxp.Core.Services;

namespace Mxp.Core.Business
{
	public class AllowanceProjectId : LookupField
	{
		public AllowanceProjectId(Allowance allowance) : base(allowance) {
			this.Title = Labels.GetLoggedUserLabel (Labels.LabelEnum.ProjectID);
			this.Permission = Preferences.Instance.ALLSIprojectId == PermissionEnum.Optional ? FieldPermissionEnum.Optional : FieldPermissionEnum.Mandatory;
			this.LookupKey = LookupService.ApiEnum.GetLookupProject;
		}

		public override object Value {
			get {
				return this.GetModel<Allowance> ().ALLSIprojectId;
			}
			set {
				this.GetModel<Allowance> ().ALLSIprojectId = ((LookupItem)value).ComboId;
				base.Value = value;
			}
		}

		public override bool IsEditable {
			get {
				return this.GetModel<Allowance> ().IsNew || this.GetModel<Allowance> ().IsEditable;
			}
		}
	}

	public class AllowanceDepartmentId : LookupField
	{
		public AllowanceDepartmentId(Allowance allowance) : base(allowance) {
			this.Title = Labels.GetLoggedUserLabel (Labels.LabelEnum.DepartmentID);
			this.Permission = Preferences.Instance.ALLSIdepartmentId == PermissionEnum.Optional ? FieldPermissionEnum.Optional : FieldPermissionEnum.Mandatory;
			this.LookupKey = LookupService.ApiEnum.GetLookupDepartment;
		}

		public override object Value {
			get {
				return this.GetModel<Allowance> ().ALLSIdepartmentId;
			}
			set {
				this.GetModel<Allowance> ().ALLSIdepartmentId = ((LookupItem)value).ComboId;
				base.Value = value;
			}
		}

		public override bool IsEditable {
			get {
				return this.GetModel<Allowance> ().IsNew || this.GetModel<Allowance> ().IsEditable;
			}
		}
	}
		
	public class AllowanceTravelRequestID : LookupField
	{
		public AllowanceTravelRequestID(Allowance allowance) : base(allowance) {
			this.Title = Labels.GetLoggedUserLabel (Labels.LabelEnum.TravelRequestID);
			this.Permission = Preferences.Instance.ALLSItrqId == PermissionEnum.Optional ? FieldPermissionEnum.Optional : FieldPermissionEnum.Mandatory;
			this.LookupKey = LookupService.ApiEnum.GetLookupTravelRequests;
		}

		public override object Value {
			get {
				return this.GetModel<Allowance> ().ALLSItrqId;
			}
			set {
				this.GetModel<Allowance> ().ALLSItrqId = ((LookupItem)value).ComboId;
				base.Value = value;
			}
		}

		public override bool IsEditable {
			get {
				return this.GetModel<Allowance> ().IsNew || this.GetModel<Allowance> ().IsEditable;
			}
		}
	}

	public class AllowanceMerchantName : Field {
		public AllowanceMerchantName (Allowance allowance) : base (allowance) {
			this.Type = FieldTypeEnum.String;
			this.Title = Labels.GetLoggedUserLabel (Labels.LabelEnum.MerchantName);
			this.Permission = Preferences.Instance.ALLSIMerchantName == PermissionEnum.Optional ? FieldPermissionEnum.Optional : FieldPermissionEnum.Mandatory;
		}

		public override object Value {
			get {
				return this.GetModel<Allowance> ().MerchantName;
			}
			set {
				this.GetModel<Allowance> ().MerchantName = (string)value;
				base.Value = value;
			}
		}

		public override bool IsEditable {
			get {
				return this.GetModel<Allowance> ().IsNew || this.GetModel<Allowance> ().IsEditable;
			}
		}
	}

	public class AllowanceInvoiceID : Field {
		public AllowanceInvoiceID (Allowance allowance) : base (allowance) {
			this.Type = FieldTypeEnum.String;
			this.Title = Labels.GetLoggedUserLabel (Labels.LabelEnum.InvoiceID);
			this.Permission = Preferences.Instance.ALLSIinvoiceId == PermissionEnum.Optional ? FieldPermissionEnum.Optional : FieldPermissionEnum.Mandatory;
		}

		public override object Value {
			get {
				return this.GetModel<Allowance> ().InvoiceId;
			}
			set {
				this.GetModel<Allowance> ().InvoiceId = (string)value;
				base.Value = value;
			}
		}

		public override bool IsEditable {
			get {
				return this.GetModel<Allowance> ().IsNew || this.GetModel<Allowance> ().IsEditable;
			}
		}
	}

	public class AllowanceCreationDateFrom : Field
	{
		public AllowanceCreationDateFrom(Allowance allowance) : base(allowance) {
			this.Type = FieldTypeEnum.Date;
			this.Title = Labels.GetLoggedUserLabel (Labels.LabelEnum.DateFrom);
			this.Permission = FieldPermissionEnum.Mandatory;

			this.extraInfo ["Type"] = "DATE-TIME";
			this.extraInfo ["Max-Range"] = allowance.DateTo;

			allowance.PropertyChanged += HandlePropertyChangedEventHandler;
		}
		private void HandlePropertyChangedEventHandler (object sender, System.ComponentModel.PropertyChangedEventArgs e) {
			if (e.PropertyName.Equals ("DateTo")) {
				this.extraInfo ["Max-Range"] = this.GetModel<Allowance> ().DateTo;
			}
		}

		public override string VValue {
			get {
				return LoggedUser.Instance.Preferences.VDate (this.GetValue<DateTime> ()) + " " + this.GetValue<DateTime> ().ToString ("t");
			}
		}

		public override object Value {
			get {
				return this.GetModel<Allowance> ().DateFrom;
			}
			set {
				this.GetModel<Allowance> ().DateFrom = (DateTime)value;
				base.Value = value;
			}
		}

		public override bool IsEditable {
			get {
				return this.GetModel<Allowance> ().IsNew || this.GetModel<Allowance> ().IsEditable;
			}
		}
	}

	public class AllowanceCreationDateTo : Field
	{
		public AllowanceCreationDateTo(Allowance allowance) : base(allowance) {
			this.Type = FieldTypeEnum.Date;
			this.Title = Labels.GetLoggedUserLabel (Labels.LabelEnum.DateTo);
			this.Permission = FieldPermissionEnum.Mandatory;
			allowance.PropertyChanged += HandlePropertyChangedEventHandler;
			this.extraInfo ["Min-Range"] = this.GetModel<Allowance> ().DateFrom;
			this.extraInfo ["Type"] = "DATE-TIME";
		}

		void HandlePropertyChangedEventHandler (object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			if (e.PropertyName.Equals ("DateFrom"))
				this.extraInfo ["Min-Range"] = this.GetModel<Allowance> ().DateFrom;
		}

		public override string VValue {
			get {
				return LoggedUser.Instance.Preferences.VDate (this.GetValue<DateTime> ()) + " " + this.GetValue<DateTime> ().ToString ("t");
			}
		}

		public override object Value {
			get {
				return this.GetModel<Allowance> ().DateTo;
			}
			set {
				this.GetModel<Allowance> ().DateTo = (DateTime)value;
				base.Value = value;
			}
		}

		public override bool IsEditable {
			get {
				return this.GetModel<Allowance> ().IsNew || this.GetModel<Allowance> ().IsEditable;
			}
		}
	}

	public class AllowanceCreationCountry : Field
	{
		public AllowanceCreationCountry(Allowance allowance) : base(allowance) {
			this.Type = FieldTypeEnum.Country;
			this.Title = Labels.GetLoggedUserLabel (Labels.LabelEnum.Country);
			this.Permission = FieldPermissionEnum.Mandatory;
		}

		public override string VValue {
			get {
				if (this.GetValue<Country> () == null)
					return null;

				return this.GetValue<Country> ().Name;
			}
		}

		public override object Value {
			get {
				return this.GetModel<Allowance> ().Country;
			}
			set {
				this.GetModel<Allowance> ().Country = (Country)value;
				base.Value = value;
			}
		}

		public override bool IsEditable {
			get {
				return this.GetModel<Allowance> ().IsNew || this.GetModel<Allowance> ().IsEditable;
			}
		}
	}

	public class AllowanceCreationComment : Field
	{
		public AllowanceCreationComment(Allowance allowance) : base(allowance) {
			this.Type = FieldTypeEnum.LongString;
			this.Title = Labels.GetLoggedUserLabel (Labels.LabelEnum.Comment);
			this.Permission = Preferences.Instance.ALLSIcomment == PermissionEnum.Optional ? FieldPermissionEnum.Optional : FieldPermissionEnum.Mandatory;
		}

		public override string VValue {
			get {
				return this.GetValue<string> ();
			}
		}

		public override object Value {
			get {
				return this.GetModel<Allowance> ().Comment;
			}
			set {
				this.GetModel<Allowance> ().Comment = (string)value;
				base.Value = value;
			}
		}

		public override bool IsEditable {
			get {
				return this.GetModel<Allowance> ().IsNew || this.GetModel<Allowance> ().IsEditable;
			}
		}
	}

	public class AllowanceCreationLocation : Field
	{
		public AllowanceCreationLocation(Allowance allowance) : base(allowance) {
			this.Type = FieldTypeEnum.String;
			this.Title = Labels.GetLoggedUserLabel (Labels.LabelEnum.Location);
			this.Permission = Preferences.Instance.ALLSIMerchantCity == PermissionEnum.Optional ? FieldPermissionEnum.Optional : FieldPermissionEnum.Mandatory;
		}

		public override string VValue {
			get {
				return this.GetValue<string> ();
			}
		}

		public override object Value {
			get {
				return this.GetModel<Allowance> ().Location;
			}
			set {
				this.GetModel<Allowance> ().Location = (string)value;
				base.Value = value;
			}
		}

		public override bool IsEditable {
			get {
				return this.GetModel<Allowance> ().IsNew || this.GetModel<Allowance> ().IsEditable;
			}
		}
	}

	public class AllowanceCreationBreakfast : Field
	{
		public AllowanceCreationBreakfast(Allowance allowance) : base(allowance) {
			this.Type = FieldTypeEnum.Boolean;
			this.Title = Labels.GetLoggedUserLabel (Labels.LabelEnum.Breakfast);
			this.Permission = FieldPermissionEnum.Optional;
		}

		public override object Value {
			get {
				return this.GetModel<Allowance> ().Breakfast;
			}
			set {
				this.GetModel<Allowance> ().Breakfast = (bool)value;
				base.Value = value;
			}
		}

		public override bool IsEditable {
			get {
				return this.GetModel<Allowance> ().IsNew || this.GetModel<Allowance> ().IsEditable;
			}
		}
	}

	public class AllowanceCreationLunch : Field
	{
		public AllowanceCreationLunch(Allowance allowance) : base(allowance) {
			this.Type = FieldTypeEnum.Boolean;
			this.Title = Labels.GetLoggedUserLabel (Labels.LabelEnum.Lunch);
			this.Permission = FieldPermissionEnum.Optional;
		}

		public override object Value {
			get {
				return this.GetModel<Allowance> ().Lunch;
			}
			set {
				this.GetModel<Allowance> ().Lunch = (bool)value;
				base.Value = value;
			}
		}

		public override bool IsEditable {
			get {
				return this.GetModel<Allowance> ().IsNew || this.GetModel<Allowance> ().IsEditable;
			}
		}
	}

	public class AllowanceCreationDinner : Field
	{
		public AllowanceCreationDinner(Allowance allowance) : base(allowance) {
			this.Type = FieldTypeEnum.Boolean;
			this.Title = Labels.GetLoggedUserLabel (Labels.LabelEnum.Dinner);
			this.Permission = FieldPermissionEnum.Optional;
		}

		public override object Value {
			get {
				return this.GetModel<Allowance> ().Dinner;
			}
			set {
				this.GetModel<Allowance> ().Dinner = (bool)value;
				base.Value = value;
			}
		}

		public override bool IsEditable {
			get {
				return this.GetModel<Allowance> ().IsNew || this.GetModel<Allowance> ().IsEditable;
			}
		}
	}

	public class AllowanceCreationLodging : Field
	{
		public AllowanceCreationLodging(Allowance allowance) : base(allowance) {
			this.Type = FieldTypeEnum.Boolean;
			this.Title = Labels.GetLoggedUserLabel (Labels.LabelEnum.Lodging);
			this.Permission = FieldPermissionEnum.Optional;
		}

		public override object Value {
			get {
				return this.GetModel<Allowance> ().Lodging;
			}
			set {
				this.GetModel<Allowance> ().Lodging = (bool)value;
				base.Value = value;
			}
		}

		public override bool IsEditable {
			get {
				return this.GetModel<Allowance> ().IsNew || this.GetModel<Allowance> ().IsEditable;
			}
		}
	}

	public class AllowanceCreationInfo : Field
	{
		public AllowanceCreationInfo(Allowance allowance) : base(allowance) {
			this.Type = FieldTypeEnum.Boolean;
			this.Title = Labels.GetLoggedUserLabel (Labels.LabelEnum.Info);
			this.Permission = FieldPermissionEnum.Optional;
		}

		public override object Value {
			get {
				return this.GetModel<Allowance> ().Info;
			}
			set {
				this.GetModel<Allowance> ().Info = (bool)value;
				base.Value = value;
			}
		}

		public override bool IsEditable {
			get {
				return this.GetModel<Allowance> ().IsNew || this.GetModel<Allowance> ().IsEditable;
			}
		}
	}

	public class AllowanceCreationWorkNight : Field
	{
		public AllowanceCreationWorkNight(Allowance allowance) : base(allowance) {
			this.Type = FieldTypeEnum.Boolean;
			this.Title = Labels.GetLoggedUserLabel (Labels.LabelEnum.WorkNight);
			this.Permission = FieldPermissionEnum.Optional;
		}

		public override object Value {
			get {
				return this.GetModel<Allowance> ().WorkNight;
			}
			set {
				this.GetModel<Allowance> ().WorkNight = (bool)value;
				base.Value = value;
			}
		}

		public override bool IsEditable {
			get {
				return this.GetModel<Allowance> ().IsNew || this.GetModel<Allowance> ().IsEditable;
			}
		}
	}
}