using System;
using System.Collections.Generic;

namespace Mxp.Core.Services.Responses
{
	public class PreferencesResponse : Response
	{
		public int FldCustomerID { get; set; }
		public int FldlanguageID { get; set; }
		public int FldcurrencyID { get; set; }
		public int FldcountryID { get; set; }
		public double FldEmployeeHomeWorkDistance { get; set; }
		public string GENDateFormat { get; set; }
		public string FldCustomerName { get; set; }
		public string GENAllowMobileApp { get; set; }
		public string ITEEnableItemAttachment { get; set; }
		public int CarsCount { get; set; }
		public string AllIndicator1 { get; set; }
		public string AllIndicator2 { get; set; }
		public string AllIndicator3 { get; set; }
		public string AllIndicator4 { get; set; }
		public string AllIndicator5 { get; set; }
		public string AllIndicator6 { get; set; }
		public string AllIndicator1Checked { get; set; }
		public string AllIndicator2Checked { get; set; }
		public string AllIndicator3Checked { get; set; }
		public string AllIndicator4Checked { get; set; }
		public string AllIndicator5Checked { get; set; }
		public string AllIndicator6Checked { get; set; }
		public object AllInterruptedSegment { get; set; }
		public string ALLSIcomment { get; set; }
		public string ALLSIprojectId { get; set; }
		public string ALLSIdepartmentId { get; set; }
		public string ALLSItrqId { get; set; }
		public string ALLSIinfochar1 { get; set; }
		public string ALLSIinfochar2 { get; set; }
		public string ALLSIinfochar3 { get; set; }
		public string ALLSIinfochar4 { get; set; }
		public string ALLSIinfochar5 { get; set; }
		public string ALLSIinfochar6 { get; set; }
		public string ALLSIinfochar7 { get; set; }
		public string ALLSIinfochar8 { get; set; }
		public string ALLSIinfonum1 { get; set; }
		public string ALLSIinfonum2 { get; set; }
		public string ALLSIinvoiceId { get; set; }
		public string ALLSIMerchantCity { get; set; }
		public string ALLSIMerchantName { get; set; }
		public string MILSIcomment { get; set; }
		public string MILSIprojectId { get; set; }
		public string MILSIdepartmentId { get; set; }
		public string MILSItrqId { get; set; }
		public string MILSIinfochar1 { get; set; }
		public string MILSIinfochar2 { get; set; }
		public string MILSIinfochar3 { get; set; }
		public string MILSIinfochar4 { get; set; }
		public string MILSIinfochar5 { get; set; }
		public string MILSIinfochar6 { get; set; }
		public string MILSIinfochar7 { get; set; }
		public string MILSIinfochar8 { get; set; }
		public string MILSIinfonum1 { get; set; }
		public string MILSIinfonum2 { get; set; }
		public string MILPassenger { get; set; }
		public string MILVehicle { get; set; }
		public string MILWithdrawCommuting { get; set; }
		public string MILMap { get; set; }
		public string MILDefaultProduct { get; set; }
		public string MILCommuting { get; set; }
		public string MILPrivate { get; set; }
		public string MILOdometer { get; set; }
		public string MilUnit { get; set; }
		public string ITEAllocationATT { get; set; }
		public string ITEAllocationDPT { get; set; }
		public string ITEAllocationPJT { get; set; }
		public string ITEAllocationTRV { get; set; }
		public string ITEAttendeeAllocation { get; set; }
		public string ITEShowReceiptTickbox { get; set; }
		public string REPAllocationDPT { get; set; }
		public string REPAllocationPJT { get; set; }
		public string REPAllocationTRV { get; set; }
		public string REPInfoChar1 { get; set; }
		public string REPInfoChar2 { get; set; }
		public string REPInfoChar3 { get; set; }
		public string REPInfoNum1 { get; set; }
		public string REPInfoNum2 { get; set; }
		public string REPApprovalRejectionComment { get; set; }
		public string REPAttachment { get; set; }
		public object TVRProject { get; set; }
		public object TVRCostCenter { get; set; }
		public int CustomerTravelRuleIndicator { get; set; }
		public int CustomerFlexiRuleIndicator { get; set; }
		public int CustomerFuelRuleIndicator { get; set; }
		public int CustomerCarMaintenanceRuleIndicator { get; set; }
		public int CustomerCarRentalRuleIndicator { get; set; }
		public int CustomerGermanAllowanceIndicator { get; set; }
		public int CustomerHotelRuleIndicator { get; set; }
		public string ITEAttendeeCompany { get; set; }
		public string HCPAllowHCO { get; set; }
		public string HCPAllowHCP { get; set; }
		public string HCPShowHideSpouse { get; set; }
		public string HCPAllowManualEntry { get; set; }
		public string EXPMobileCCMatch { get; set; }
		public string ITESecureAttachment { get; set; }
		public int MOBAllowanceActivationMobile { get; set; }
		public string ITECountryTRXCRDUser { get; set; }
		public string MOBSpendCatcher { get; set; }
		public string REPTravelRequestName { get; set; }
		public string HCPDataSource { get; set; }
		public string MOBApprovalSelectedExpenses { get; set; }
		public string MILBusinessMilesReadOnly { get; set; }
		public string EXPGooglePlaces { get; set; }
		public string ITEFilterCategory { get; set; }

		public List<DynamicFieldResponse> DynamicFields { get; set; }

		public PreferencesResponse () {}
	}
}