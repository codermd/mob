using System;

namespace Mxp.Core.Services.Responses
{
	public class ProductResponse : Response
	{
		public int ProductID { get; set; }
		public int ExpenseCategoryID { get; set; }
		public int ExpenseCategoryCountryVATLinkID { get; set; }
		public string ExpenseCategoryName { get; set; }
		public int Receipt { get; set; }
		public int List { get; set; }
		public string PaymentMethod { get; set; }
		public string fldcategorypolicySIwc { get; set; }
		public int SImerchantName { get; set; }
		public int SImerchantCity { get; set; }
		public int SIinvoiceId { get; set; }
		public int SIprojectId { get; set; }
		public int SIdepartmentId { get; set; }
		public int SIcomments { get; set; }
		public int SItrqid { get; set; }
		public int SIattendees { get; set; }
		public int SIinfoChar1 { get; set; }
		public int SIinfoChar2 { get; set; }
		public int SIinfoChar3 { get; set; }
		public int SIinfoChar4 { get; set; }
		public int SIinfoChar5 { get; set; }
		public int SIinfoChar6 { get; set; }
		public int SIinfoChar7 { get; set; }
		public int SIinfoChar8 { get; set; }
		public int SIinfoNum1 { get; set; }
		public int SIinfoNum2 { get; set; }
		public int ExtraLine { get; set; }
		public int MerchantLine { get; set; }
		public int AllocationLine { get; set; }
		public int CommentLine { get; set; }
		public int Allowanceline { get; set; }
		public int HotelLine { get; set; }
		public int TravelLine { get; set; }
		public int CarRentalLine { get; set; }
		public int FuelLine { get; set; }
		public int SIvehicleOdometer { get; set; }
		public int SIvehicleNumber { get; set; }
		public int IsCalculated { get; set; }
		public int Mandatory { get; set; }
		public bool NonTravelProduct { get; set; }

		public ProductResponse () {}
	}
}