using System;

namespace Mxp.Core.Business
{
	class AFDateFrom : Field
	{
		public AFDateFrom(AllowanceSegment segment): base(segment){
			this.Title = Labels.GetLoggedUserLabel (Labels.LabelEnum.DateFrom);;
			this.Permission = FieldPermissionEnum.Mandatory;
			this.Type = FieldTypeEnum.Date;
			this.extraInfo ["Type"] = "DATE-TIME";
		}

		public static bool canCreate(AllowanceSegment segment){
			return false;
		}

		public override object Value {
			get {
				return this.GetModel<AllowanceSegment> ().DateFrom;
			}
			set {
				this.GetModel<AllowanceSegment>().DateFrom = (DateTime)value;
				base.Value = value;
			}
		}

		public override bool IsEditable {
			get {
				return this.GetModel<AllowanceSegment> ().GetModelParent<AllowanceSegment, Allowance> ().IsNew || this.GetModel<AllowanceSegment> ().GetModelParent<AllowanceSegment, Allowance> ().IsEditable;
			}
		}
	}
	
	class AFDateTo : Field
	{
		public AFDateTo(AllowanceSegment segment): base(segment){
			this.Title = Labels.GetLoggedUserLabel (Labels.LabelEnum.DateTo);
			this.Permission = FieldPermissionEnum.Mandatory;
			this.Type = FieldTypeEnum.Date;
			this.extraInfo ["Type"] = "DATE-TIME";
		}

		public static bool canCreate(AllowanceSegment segment){
			return false;
		}

		public override string VValue {
			get {
				return ((DateTime)this.Value).ToString ("D");
			}
		}

		public override object Value {
			get {
				return this.GetModel<AllowanceSegment> ().DateTo;
			}
			set {
				this.GetModel<AllowanceSegment> ().DateTo = (DateTime)value;
				base.Value = value;
			}
		}

		public override bool IsEditable {
			get {
				return this.GetModel<AllowanceSegment> ().GetModelParent<AllowanceSegment, Allowance> ().IsNew || this.GetModel<AllowanceSegment> ().GetModelParent<AllowanceSegment, Allowance> ().IsEditable;
			}
		}
	}

	class AFCountry : Field
	{
		public AFCountry(AllowanceSegment segment): base(segment){
			this.Title = Labels.GetLoggedUserLabel (Labels.LabelEnum.Country);
			this.Type = FieldTypeEnum.Country;
			this.Permission = FieldPermissionEnum.Mandatory;
		}

		public static bool canCreate(AllowanceSegment segment){
			return true;
		}

		public override string VValue {
			get {
				return this.GetValue<Country> ().Name;
			}
		}

		public override object Value {
			get {
				return this.GetModel<AllowanceSegment> ().Country;
			}
			set {
				this.GetModel<AllowanceSegment> ().Country = value as Country;
				base.Value = value;
			}
		}

		public override bool IsEditable {
			get {
				return this.GetModel<AllowanceSegment> ().GetModelParent<AllowanceSegment, Allowance> ().IsNew || this.GetModel<AllowanceSegment> ().GetModelParent<AllowanceSegment, Allowance> ().IsEditable;
			}
		}
	}

	class AFComment : Field
	{
		public AFComment(AllowanceSegment segment): base(segment){
			this.Title = Labels.GetLoggedUserLabel (Labels.LabelEnum.Comment);
			this.Type = FieldTypeEnum.LongString;
			this.Permission = Preferences.Instance.ALLSIcomment == PermissionEnum.Optional ? FieldPermissionEnum.Optional : FieldPermissionEnum.Mandatory;
		}

		public static bool canCreate(AllowanceSegment segment){
			return true;
		}

		public override object Value {
			get {
				return this.GetModel<AllowanceSegment> ().Comment;
			}
			set {
				this.GetModel<AllowanceSegment> ().Comment =(string) value;
				base.Value = value;
			}
		}

		public override bool IsEditable {
			get {
				return this.GetModel<AllowanceSegment> ().GetModelParent<AllowanceSegment, Allowance> ().IsNew || this.GetModel<AllowanceSegment> ().GetModelParent<AllowanceSegment, Allowance> ().IsEditable;
			}
		}
	}

	class AFLocation : Field
	{
		public AFLocation(AllowanceSegment segment): base(segment){
			this.Title = Labels.GetLoggedUserLabel (Labels.LabelEnum.Location);
			this.Type = FieldTypeEnum.String;
			this.Permission = Preferences.Instance.ALLSIMerchantCity == PermissionEnum.Optional ? FieldPermissionEnum.Optional : FieldPermissionEnum.Mandatory;
		}

		public static bool canCreate(AllowanceSegment segment){
			return true;
		}

		public override object Value {
			get {
				return this.GetModel<AllowanceSegment> ().Location;
			}

			set {
				this.GetModel<AllowanceSegment> ().Location = (string)value;
				base.Value = value;
			}
		}

		public override bool IsEditable {
			get {
				return this.GetModel<AllowanceSegment> ().GetModelParent<AllowanceSegment, Allowance> ().IsNew || this.GetModel<AllowanceSegment> ().GetModelParent<AllowanceSegment, Allowance> ().IsEditable;
			}
		}
	}

	class AFBreakfast : Field
	{
		public AFBreakfast(AllowanceSegment segment): base(segment){
			this.Title = Labels.GetLoggedUserLabel (Labels.LabelEnum.Breakfast);
			this.Type = FieldTypeEnum.Boolean;
		}

		public static bool canCreate(AllowanceSegment segment){
			return segment.CanShowBreakfast;
		}

		public override object Value {
			get {
				return this.GetModel<AllowanceSegment> ().Breakfast;
			}
			set {
				this.GetModel<AllowanceSegment> ().Breakfast = (bool)value;
				base.Value = value;
			}
		}

		public override bool IsEditable {
			get {
				return this.GetModel<AllowanceSegment> ().GetModelParent<AllowanceSegment, Allowance> ().IsNew || this.GetModel<AllowanceSegment> ().GetModelParent<AllowanceSegment, Allowance> ().IsEditable;
			}
		}
	}

	class AFLunch : Field
	{
		public AFLunch(AllowanceSegment segment): base(segment){
			this.Title = Labels.GetLoggedUserLabel (Labels.LabelEnum.Lunch);
			this.Type = FieldTypeEnum.Boolean;
		}

		public static bool canCreate(AllowanceSegment segment){
			return segment.CanShowLunch;
		}

		public override object Value {
			get {
				return this.GetModel<AllowanceSegment> ().Lunch;
			}
			set {
				this.GetModel<AllowanceSegment> ().Lunch = (bool)value;
				base.Value = value;
			}
		}

		public override bool IsEditable {
			get {
				return this.GetModel<AllowanceSegment> ().GetModelParent<AllowanceSegment, Allowance> ().IsNew || this.GetModel<AllowanceSegment> ().GetModelParent<AllowanceSegment, Allowance> ().IsEditable;
			}
		}
	}

	class AFDinner : Field
	{
		public AFDinner(AllowanceSegment segment): base(segment){
			this.Title = Labels.GetLoggedUserLabel (Labels.LabelEnum.Dinner);
			this.Type = FieldTypeEnum.Boolean;
		}

		public static bool canCreate(AllowanceSegment segment){
			return segment.CanShowDinner;
		}

		public override object Value {
			get {
				return this.GetModel<AllowanceSegment> ().Dinner;
			}
			set {
				this.GetModel<AllowanceSegment> ().Dinner = (bool)value;
				base.Value = value;
			}
		}

		public override bool IsEditable {
			get {
				return this.GetModel<AllowanceSegment> ().GetModelParent<AllowanceSegment, Allowance> ().IsNew || this.GetModel<AllowanceSegment> ().GetModelParent<AllowanceSegment, Allowance> ().IsEditable;
			}
		}
	}
	
	class AFLodging : Field
	{
		public AFLodging(AllowanceSegment segment): base(segment){
			this.Title = Labels.GetLoggedUserLabel (Labels.LabelEnum.Lodging);
			this.Type = FieldTypeEnum.Boolean;
		}

		public static bool canCreate(AllowanceSegment segment){
			return segment.CanShowLodging;
		}

		public override object Value {
			get {
				return this.GetModel<AllowanceSegment> ().Lodging;
			}
			set {
				this.GetModel<AllowanceSegment> ().Lodging = (bool)value;
				base.Value = value;
			}
		}

		public override bool IsEditable {
			get {
				return this.GetModel<AllowanceSegment> ().GetModelParent<AllowanceSegment, Allowance> ().IsNew || this.GetModel<AllowanceSegment> ().GetModelParent<AllowanceSegment, Allowance> ().IsEditable;
			}
		}
	}
	
	class AFInfo : Field
	{
		public AFInfo(AllowanceSegment segment): base(segment){
			this.Title = Labels.GetLoggedUserLabel (Labels.LabelEnum.Info);
			this.Type = FieldTypeEnum.Boolean;
		}

		public static bool canCreate(AllowanceSegment segment){
			return segment.CanShowInfo;
		}

		public override object Value {
			get {
				return this.GetModel<AllowanceSegment> ().Info;
			}
			set {
				this.GetModel<AllowanceSegment> ().Info = (bool)value;
				base.Value = value;
			}
		}

		public override bool IsEditable {
			get {
				return this.GetModel<AllowanceSegment> ().GetModelParent<AllowanceSegment, Allowance> ().IsNew || this.GetModel<AllowanceSegment> ().GetModelParent<AllowanceSegment, Allowance> ().IsEditable;
			}
		}
	}
	
	class AFWorkNight : Field
	{
		public AFWorkNight(AllowanceSegment segment): base(segment){
			this.Title = Labels.GetLoggedUserLabel (Labels.LabelEnum.WorkNight);
			this.Type = FieldTypeEnum.Boolean;
		}

		public static bool canCreate(AllowanceSegment segment){
			return segment.CanShowWorkNight;
		}

		public override object Value {
			get {
				return this.GetModel<AllowanceSegment> ().WorkNight;
			}
			set {
				this.GetModel<AllowanceSegment> ().WorkNight = (bool)value;
				base.Value = value;
			}
		}

		public override bool IsEditable {
			get {
				return this.GetModel<AllowanceSegment> ().GetModelParent<AllowanceSegment, Allowance> ().IsNew || this.GetModel<AllowanceSegment> ().GetModelParent<AllowanceSegment, Allowance> ().IsEditable;
			}
		}
	}
}