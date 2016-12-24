using System;

namespace Mxp.Core.Services.Responses
{
	public class VehicleResponse : Response
	{
		public int fldvehiclelinkID { get; set; }
		public string fldvehiclelinkEnddate { get; set; }
		public int fldvehicleID { get; set; }
		public int fldVehicleCategoryID { get; set; }
		public int fldvehicletypeID { get; set; }
		public int fldcustomerID { get; set; }
		public int fldvehiclemodelID { get; set; }
		public int fldvehicleInternalID { get; set; }
		public string fldvehicleLicenseplatenumber { get; set; }
		public object fldvehicleChassisnbr { get; set; }
		public object fldvehiclepurchaseprice { get; set; }
		public string fldvehicleDatein { get; set; }
		public string fldvehicleOwnership { get; set; }
		public int fldvehicleLastmileage { get; set; }
		public string fldvehicleDatelastvalidmileage { get; set; }
		public int fldvehicleYtdmileage { get; set; }
		public int fldvehicleYtdcost { get; set; }
		public object fldvehicleNextmaintenance { get; set; }
		public object fldvehicleKmtyrereplacement { get; set; }
		public string fldvehicleNextinspectiondate { get; set; }
		public object fldvehicleFirstregistrationdate { get; set; }
		public object fldvehicleBookvalue { get; set; }
		public object fldvehicledescription { get; set; }
		public double fldVehicleStandardConsumption { get; set; }
		public double fldVehicleStandardInterval { get; set; }
		public object fldVehicleCategoryName { get; set; }
		public double MileageRateLimit { get; set; }
		public double mileagerateRate1 { get; set; }
		public double mileagerateRate2 { get; set; }
		public double PrivateRate { get; set; }
		public double commutingRate { get; set; }
		public double fldMileageRatePassenger { get; set; }
		public double fldvehiclestandardconsumption { get; set; }
		public double fldvehiclestandardinterval { get; set; }
		public object Firstname { get; set; }
		public object Lastname { get; set; }
		public double FldemployeeHomeWorkDistance { get; set; }
		public object fldMileageRateName { get; set; }

		public VehicleResponse () {}
	}
}