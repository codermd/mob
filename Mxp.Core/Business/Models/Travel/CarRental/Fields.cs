using System;

namespace Mxp.Core.Business
{
	public class CarRentalCategory : Field
	{
		public CarRentalCategory(TravelCarRental carRental) : base(carRental) {
			this.Title = Labels.GetLoggedUserLabel (Labels.LabelEnum.Category);
			this.Permission = FieldPermissionEnum.Optional;
			this.Type = FieldTypeEnum.String;
		}

		public override object Value {
			get {
				return this.GetModel<TravelCarRental> ().Category;
			}
		}

		public override bool IsEditable {
			get {
				return false;
			}
		}
	}

	public class CarRentalMerchant : Field
	{
		public CarRentalMerchant(TravelCarRental carRental) : base(carRental) {
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
				return this.GetModel<TravelCarRental> ().Merchant;
			}
		}
	}

	public class CarRentalGearBox : Field
	{
		public CarRentalGearBox(TravelCarRental carRental) : base(carRental) {
			this.Title = Labels.GetLoggedUserLabel (Labels.LabelEnum.Transmission);
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
				return this.GetModel<TravelCarRental> ().Transmission;
			}
		}
	}

	public class CarRentalPickUpStation : Field
	{
		public CarRentalPickUpStation(TravelCarRental carRental) : base(carRental) {
			this.Title = Labels.GetLoggedUserLabel (Labels.LabelEnum.PickupLocation);
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
				return this.GetModel<TravelCarRental> ().PickupLabel;
			}
		}
	}

	public class CarRentalPickUpCountry : Field
	{
		public CarRentalPickUpCountry(TravelCarRental carRental) : base(carRental) {
			this.Title = Labels.GetLoggedUserLabel (Labels.LabelEnum.PickupCountry);
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
				return this.GetModel<TravelCarRental> ().PickupCountry;
			}
		}
	}

	public class CarRentalPickUpDate : Field
	{
		public CarRentalPickUpDate(TravelCarRental carRental) : base(carRental) {
			this.Title = Labels.GetLoggedUserLabel (Labels.LabelEnum.PickupDate);
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
				return LoggedUser.Instance.Preferences.VDate (this.GetModel<TravelCarRental> ().PickupDate.GetValueOrDefault ());
//				return this.GetModel<TravelCarRental> ().PickupDate.GetValueOrDefault().ToString("d")
			}
		}
	}

	public class CarRentalDropLocation : Field
	{
		public CarRentalDropLocation(TravelCarRental carRental) : base(carRental) {
			this.Title = Labels.GetLoggedUserLabel (Labels.LabelEnum.DropLocation);
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
				return this.GetModel<TravelCarRental> ().DropLabel;
			}
		}
	}

	public class CarRentalDropCountry : Field
	{
		public CarRentalDropCountry(TravelCarRental carRental) : base(carRental) {
			this.Title = Labels.GetLoggedUserLabel (Labels.LabelEnum.DropCountry);
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
				return this.GetModel<TravelCarRental> ().DropCountry;
			}
		}
	}

	public class CarRentalDropDate : Field
	{
		public CarRentalDropDate(TravelCarRental carRental) : base(carRental) {
			this.Title = Labels.GetLoggedUserLabel (Labels.LabelEnum.DropDate);
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
				return LoggedUser.Instance.Preferences.VDate (this.GetModel<TravelCarRental> ().DropDate.GetValueOrDefault ());
//				return this.GetModel<TravelCarRental> ().DropDate.GetValueOrDefault().ToString("d");
			}
		}
	}

	public class CarRentalDropTime : Field
	{
		public CarRentalDropTime(TravelCarRental carRental) : base(carRental) {
			this.Title = Labels.GetLoggedUserLabel (Labels.LabelEnum.DropTime);
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
				return this.GetModel<TravelCarRental> ().DropTime;
			}
		}
	}
}