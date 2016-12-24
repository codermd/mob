using System;
using System.Collections.Generic;

namespace Mxp.Core.Services.Responses
{
	public class AllowanceResponse : Response
	{
		public int journeyId { get; set; }
		public int transactionId { get; set; }
		public int itemId { get; set; }
		public int employeeId { get; set; }
		public string journeyName { get; set; }
		public string journeyPurpose { get; set; }
		public double grossAmountCC { get; set; }
		public double netAmountCC { get; set; }
		public double vatAmountCC { get; set; }
		public double legalAmountCC { get; set; }
		public string transactionComments { get; set; }
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
		public int? projectId { get; set; }
		public int? travelRequestId { get; set; }
		public int? departmentId { get; set; }
		public List<AllowanceSegmentResponse> segments { get; set; }

		public AllowanceResponse () {}
	}
}