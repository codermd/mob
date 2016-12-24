using System;

namespace Mxp.Core.Business
{
	public class TravelStayArrivalDate : Field
	{
		public TravelStayArrivalDate(TravelStay travelStay) : base(travelStay) {
			this.Title = Labels.GetLoggedUserLabel (Labels.LabelEnum.ArrivalDate);
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
				return this.GetModel<TravelStay> ().DateIn.GetValueOrDefault ().ToString ("d");
			}
		}
	}

	public class TravelStayDepartureDate : Field
	{
		public TravelStayDepartureDate(TravelStay travelStay) : base(travelStay) {
			this.Title = Labels.GetLoggedUserLabel (Labels.LabelEnum.DepartureDate);
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
				return this.GetModel<TravelStay> ().DateOut.GetValueOrDefault ().ToString ("d");
			}
		}
	}
		
	public class TravelStayNumberOfNight : Field
	{
		public TravelStayNumberOfNight(TravelStay travelStay) : base(travelStay) {
			this.Title = Labels.GetLoggedUserLabel (Labels.LabelEnum.NumberNights);
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
				return this.GetModel<TravelStay> ().NumberNights;
			}
		}
	}
		
	public class TravelStayCountry : Field
	{
		public TravelStayCountry(TravelStay travelStay) : base(travelStay) {
			this.Title = Labels.GetLoggedUserLabel (Labels.LabelEnum.Country);
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
				return this.GetModel<TravelStay> ().CountryName;
			}
		}
	}

	public class TravelStayClass : Field
	{
		public TravelStayClass(TravelStay travelStay) : base(travelStay) {
			this.Title = Labels.GetLoggedUserLabel (Labels.LabelEnum.Class);
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
				return this.GetModel<TravelStay> ().Class;
			}
		}
	}
		
	public class TravelStayMerchant : Field
	{
		public TravelStayMerchant(TravelStay travelStay) : base(travelStay) {
			this.Title = Labels.GetLoggedUserLabel (Labels.LabelEnum.Merchant);
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
				return this.GetModel<TravelStay> ().MerchantName;
			}
		}
	}

	public class TravelStayPreferedHotel : Field
	{
		public TravelStayPreferedHotel(TravelStay travelStay) : base(travelStay) {
			this.Title = Labels.GetLoggedUserLabel (Labels.LabelEnum.PreferredHotel);
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
				return this.GetModel<TravelStay> ().PreferredHotel;
			}
		}
	}

	public class TravelStayLocation : Field
	{
		public TravelStayLocation(TravelStay travelStay) : base(travelStay) {
			this.Title = Labels.GetLoggedUserLabel (Labels.LabelEnum.Location);
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
				return this.GetModel<TravelStay> ().LocationLabel;
			}
		}
	}
}