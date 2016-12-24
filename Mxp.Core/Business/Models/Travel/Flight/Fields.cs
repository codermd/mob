using System;

namespace Mxp.Core.Business
{
	public class TravelFLightType : Field
	{
		public TravelFLightType(TravelFlight travelFlight) : base(travelFlight) {
			this.Title = Labels.GetLoggedUserLabel (Labels.LabelEnum.Type);
			this.Permission = FieldPermissionEnum.Optional;
			this.Type = FieldTypeEnum.String;
		}

		public override object Value {
			get {
				return this.GetModel<TravelFlight> ().Type;
			}
		}

		public override bool IsEditable {
			get {
				return false;
			}
		}
	}

	public class TravelFLightClass : Field
	{
		public TravelFLightClass(TravelFlight travelFlight) : base(travelFlight) {
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
				return this.GetModel<TravelFlight> ().Class;
			}
		}
	}

	public class TravelFLightDate : Field
	{
		public TravelFLightDate(TravelFlight travelFlight) : base(travelFlight) {
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
				return LoggedUser.Instance.Preferences.VDate (this.GetModel<TravelFlight> ().Date.GetValueOrDefault ());
//				return this.GetModel<TravelFlight> ().Date.GetValueOrDefault().ToString("d");
			}
		}
	}
		
	public class TravelFLightDepartureTime : Field
	{
		public TravelFLightDepartureTime(TravelFlight travelFlight) : base(travelFlight) {
			this.Title = Labels.GetLoggedUserLabel (Labels.LabelEnum.DepartureTime);
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
				return this.GetModel<TravelFlight> ().DepartureTime;
			}
		}
	}

	public class TravelFLightDepartureCountry : Field
	{
		public TravelFLightDepartureCountry(TravelFlight travelFlight) : base(travelFlight) {
			this.Title = Labels.GetLoggedUserLabel (Labels.LabelEnum.DepartureCountry);
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
				return this.GetModel<TravelFlight> ().FromCountry;
			}
		}
	}
		
	public class TravelFLightDepartureAirport : Field
	{
		public TravelFLightDepartureAirport(TravelFlight travelFlight) : base(travelFlight) {
			this.Title = Labels.GetLoggedUserLabel (Labels.LabelEnum.DepartureAirport);
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
				return this.GetModel<TravelFlight> ().FromAirport;
			}
		}
	}

	public class TravelFLightArrivalTime : Field
	{
		public TravelFLightArrivalTime(TravelFlight travelFlight) : base(travelFlight) {
			this.Title = Labels.GetLoggedUserLabel (Labels.LabelEnum.ArrivalTime);
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
				return this.GetModel<TravelFlight> ().ArrivalTime;
			}
		}
	}

	public class TravelFLightArrivalCountry : Field
	{
		public TravelFLightArrivalCountry(TravelFlight travelFlight) : base(travelFlight) {
			this.Title = Labels.GetLoggedUserLabel (Labels.LabelEnum.ArrivalCountry);
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
				return this.GetModel<TravelFlight> ().ToCountry;
			}
		}
	}

	public class TravelFLightArrivalAirport : Field
	{
		public TravelFLightArrivalAirport(TravelFlight travelFlight) : base(travelFlight) {
			this.Title = Labels.GetLoggedUserLabel (Labels.LabelEnum.ArrivalAirport);
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
				return this.GetModel<TravelFlight> ().ToAirport;
			}
		}
	}
}