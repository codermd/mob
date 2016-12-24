using System;

using Mxp.Core.Services.Responses;
using System.Linq;

namespace Mxp.Core.Business
{
	public class Vehicle : Model, ComboField.IComboChoice
	{
		public int LinkId { get; set; }
		public string LinkEndDate { get; set; }
		public int Id { get; set; }
		public int TypeId { get; set; }
		public int CustomerId { get; set; }
		public int ModelId { get; set; }
		public int InternalId { get; set; }
		public string LicensePlateNumber { get; set; }
		public object ChassisNbr { get; set; }
		public object PurchasePrice { get; set; }
		public string DateIn { get; set; }
		public string Ownership { get; set; }
		public int LastMileage { get; set; }
		public string DateLastValidMileage { get; set; }
		public int YtdMileage { get; set; }
		public int YtdCost { get; set; }
		public object NextMaintenance { get; set; }
		public object KmtyreReplacement { get; set; }
		public string NextInspectionDate { get; set; }
		public object FirstRegistrationDate { get; set; }
		public object BookValue { get; set; }
		public object Description { get; set; }
		public double StandardConsumption { get; set; }
		public double StandardInterval { get; set; }
		public object CategoryName { get; set; }
		public double MileageRateLimit { get; set; }
		public double MileagerateRate1 { get; set; }
		public double MileagerateRate2 { get; set; }
		public double PrivateRate { get; set; }
		public double CommutingRate { get; set; }
		public double MileageRatePassenger { get; set; }
		public double StandardConsumptions { get; set; }
		public double StandardIntervals { get; set; }
		public object Firstname { get; set; }
		public object Lastname { get; set; }
		public double EmployeeHomeWorkDistance { get; set; }
		public object MileageRateName { get; set; }

		private int _categoryId;
		public int CategoryId {
			get {
				return this._categoryId;
			}
			set {
				this._categoryId = value;
				this._category = null;
			}
		}

		private VehicleCategory _category;
		public VehicleCategory Category {
			get {
				if (this._category == null)
					this._category = LoggedUser.Instance.VehicleCategories.SingleOrDefault ((VehicleCategory vehicleCategory) => vehicleCategory.Id == this.CategoryId);

				return this._category;
			}
			set {
				this._category = value;
			}
		}

		public Vehicle (VehicleResponse vehicleResponse) {
			this.LinkId = vehicleResponse.fldvehiclelinkID;
			this.LinkEndDate = vehicleResponse.fldvehiclelinkEnddate;
			this.Id = vehicleResponse.fldvehicleID;
			this.CategoryId = vehicleResponse.fldVehicleCategoryID;
			this.TypeId = vehicleResponse.fldvehicletypeID;
			this.CustomerId = vehicleResponse.fldcustomerID;
			this.ModelId = vehicleResponse.fldvehiclemodelID;
			this.InternalId = vehicleResponse.fldvehicleInternalID;
			this.LicensePlateNumber = vehicleResponse.fldvehicleLicenseplatenumber;
			this.ChassisNbr = vehicleResponse.fldvehicleChassisnbr;
			this.PurchasePrice = vehicleResponse.fldvehiclepurchaseprice;
			this.DateIn = vehicleResponse.fldvehicleDatein;
			this.Ownership = vehicleResponse.fldvehicleOwnership.ToUpper ();
			this.LastMileage = vehicleResponse.fldvehicleLastmileage;
			this.DateLastValidMileage = vehicleResponse.fldvehicleDatelastvalidmileage;
			this.YtdMileage = vehicleResponse.fldvehicleYtdmileage;
			this.YtdCost = vehicleResponse.fldvehicleYtdcost;
			this.NextMaintenance = vehicleResponse.fldvehicleNextmaintenance;
			this.KmtyreReplacement = vehicleResponse.fldvehicleKmtyrereplacement;
			this.NextInspectionDate = vehicleResponse.fldvehicleNextinspectiondate;
			this.FirstRegistrationDate = vehicleResponse.fldvehicleFirstregistrationdate;
			this.BookValue = vehicleResponse.fldvehicleBookvalue;
			this.Description = vehicleResponse.fldvehicledescription;
			this.StandardConsumption = vehicleResponse.fldVehicleStandardConsumption;
			this.StandardInterval = vehicleResponse.fldVehicleStandardInterval;
			this.CategoryName = vehicleResponse.fldVehicleCategoryName;
			this.MileageRateLimit = vehicleResponse.MileageRateLimit;
			this.MileagerateRate1 = vehicleResponse.mileagerateRate1;
			this.MileagerateRate2 = vehicleResponse.mileagerateRate2;
			this.PrivateRate = vehicleResponse.PrivateRate;
			this.CommutingRate = vehicleResponse.commutingRate;
			this.MileageRatePassenger = vehicleResponse.fldMileageRatePassenger;
			this.StandardConsumptions = vehicleResponse.fldvehiclestandardconsumption;
			this.StandardIntervals = vehicleResponse.fldvehiclestandardinterval;
			this.Firstname = vehicleResponse.Firstname;
			this.Lastname = vehicleResponse.Lastname;
			this.EmployeeHomeWorkDistance = vehicleResponse.FldemployeeHomeWorkDistance;
			this.MileageRateName = vehicleResponse.fldMileageRateName;
		}

		public string VTitle {
			get {
				return this.LicensePlateNumber;
			}
		}

		public int ComboId {
			get {
				return this.Id;
			}
		}

		public override bool Equals (object obj) {
			if (!(obj is Vehicle))
				return false;

			if (this.Id != ((Vehicle)obj).Id)
				return false;

			return true;
		}
	}
}