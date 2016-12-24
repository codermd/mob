using System.Collections.Generic;

namespace Mxp.Core.Services.Responses
{
	public class ExpenseResponse : Response
	{
		public int fldTransactionID { get; set; }
		public string fldtransactionDate { get; set; }
		public string fldItemDateseq { get; set; }
		public int minItemID { get; set; }
		public int attachmentCount { get; set; }
		public int fldItemProductQuantity { get; set; }
		public double fldItemGrossAmountLC { get; set; }
		public double fldItemGrossAmountCC { get; set; }
		public string fldCurrencyISO { get; set; }
		public int fldCountryID { get; set; }
		public string CountryISO { get; set; }
		public int fldPaymentMethodID { get; set; }
		public string fldTransactionMerchantName { get; set; }
		public string fldTransactionMerchantCity { get; set; }
		public string fldTransactionComments { get; set; }
		public string fldTransactionInvoiceref { get; set; }
		public bool fldTransactionReceiptPresent { get; set; }
		public string fldTransactionPeriodFromDate { get; set; }
		public string fldTransactionPeriodFromTime { get; set; }
		public string fldTransactionPeriodToDate { get; set; }
		public string fldTransactionPeriodToTime { get; set; }
		public int TRAVELID { get; set; }
		public object fldTransactionTravelTicketNumber { get; set; }
		public object fldTravelAgencyCode { get; set; }
		public object fldTransactionTavelAgencyName { get; set; }
		public object fldTransactionTravelJourney { get; set; }
		public object fldTransactionTravelType { get; set; }
		public object fldTransactionTravelticketclass { get; set; }
		public int FUELID { get; set; }
		public object fldTransactionFuelType { get; set; }
		public object fldTransactionFuelOilCompany { get; set; }
		public object fldTransactionFuelUnitcost { get; set; }
		public object fldTransactionfuelUnitMeasurement { get; set; }
		public int fldTransactionFuelMileage { get; set; }
		public object fldTransactionStartMileage { get; set; }
		public string fldTransactionFuelVehicleNumber { get; set; }
		public int ALLOWANCEID { get; set; }
		public string fldJourneyPurpose { get; set; }
		public bool fldAllowanceIndicator1 { get; set; }
		public bool fldAllowanceIndicator2 { get; set; }
		public bool fldAllowanceIndicator3 { get; set; }
		public bool fldAllowanceIndicator4 { get; set; }
		public bool fldAllowanceIndicator5 { get; set; }
		public bool fldAllowanceIndicator6 { get; set; }
		public int NbrOfItems { get; set; }
		public int fldExpenseCategoryID { get; set; }
		public int fldItineraryID { get; set; }
		public bool canDelete { get; set; }
		public bool canRemove { get; set; }
		public string card { get; set; }
		public string ReceiptCode { get; set; }
		public List<ExpenseItemResponse> items { get; set; }
		public List<VehicleResponse> availableVehicles { get; set; }
		public int fldVehicleID { get; set; }
		public int fldVehicleCategoryID { get; set; }
		public int preSelect { get; set; }
		public bool IsTransactionByCard { get; set; }

		public ExpenseResponse () {}
	}
}