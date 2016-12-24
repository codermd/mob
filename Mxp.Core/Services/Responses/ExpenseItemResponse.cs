using System;
using System.Collections.Generic;

namespace Mxp.Core.Services.Responses
{
	public class ExpenseItemResponse : Response
	{
		public int itemID { get; set; }
		public int transactionid { get; set; }
		public string ItemDate { get; set; }
		public int attachmentcount { get; set; }
		public int productid { get; set; }
		public string itemfunding { get; set; }
		public string itemStatus { get; set; }
		public string itemReceiptControlstatus { get; set; }
		public string itemaccount { get; set; }
		public string itemStatementnumber { get; set; }
		public string itemComments { get; set; }
		public int itemproductQuantity { get; set; }
		public double itemGrossamountLC { get; set; }
		public double itemGrossamountCC { get; set; }
		public double itemNetamountLC { get; set; }
		public double itemNetamountCC { get; set; }
		public double itemVatLC { get; set; }
		public double itemVatAmountCC { get; set; }
		public string itemVatCode { get; set; }
		public double itemVatRate { get; set; }
		public int expensecategoryID { get; set; }
		public int expensecategorymaster { get; set; }
		public string DepartmentReference { get; set; }
		public int? projectID { get; set; }
		public string ProjectCode { get; set; }
		public int? Travelrequestid { get; set; }
		public string DepartmentName { get; set; }
		public int? DepartmentId { get; set; }
		public string ProjectName { get; set; }
		public string TravelrequestName { get; set; }
		public double itemTaxable1 { get; set; }
		public double itemTaxable2 { get; set; }
		public double itemTaxable3 { get; set; }
		public string ItemInfoChar1 { get; set; }
		public string ItemInfoChar2 { get; set; }
		public string ItemInfoChar3 { get; set; }
		public string ItemInfoChar4 { get; set; }
		public string ItemInfoChar5 { get; set; }
		public string ItemInfoChar6 { get; set; }
		public string ItemInfoChar7 { get; set; }
		public string ItemInfoChar8 { get; set; }
		public int ItemInfoNum1 { get; set; }
		public int ItemInfoNum2 { get; set; }
		public string ItemPolInformation { get; set; }
		public string ItemPolRule { get; set; }
		public string ItemPolInfoTip { get; set; }
		public string ItemPolRuleTip { get; set; }
		public int paymentMethodID { get; set; }
		public string cashAdvanceComment { get; set; }
		public int EmployeeAccountID { get; set; }
		public int itemElectronic { get; set; }
		public string transactionversionlock { get; set; }
		public List<AttendeeResponse> Attendees { get; set; }
		public int canEdit { get; set; }
		public bool canSplit { get; set; }
		public bool canUnsplit { get; set; }

		public ExpenseItemResponse () {}
	}
}