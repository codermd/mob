using System;
using System.Diagnostics;

namespace Mxp.Core.Services.Responses
{
	public class AttendeeResponse : Response
	{
		public int AttendeeID { get; set; }
		public int ItemID { get; set; }
		public string AttendeeName { get; set; }
		public string AttendeeType { get; set; }
		public string AttendeeLastupdateby { get; set; }
		public string AttendeeLastupdateOn { get; set; }
		public bool? AttendeeCompany { get; set; }
		public int? EmployeeId { get; set; }
		public string AttendeeCompanyName { get; set; }
		public string AttendeeVersionLock { get; set; }
		public string AttendeeFirstName { get; set; }
		public string AttendeeLastName { get; set; }
		public double? AttendeeAmountLC { get; set; }
		public string AttendeeInfoChar1 { get; set; }
		public string AttendeeInfoChar2 { get; set; }
		public string AttendeeInfoChar3 { get; set; }
		public int? AttendeeInfoNum1 { get; set; }
		public int? AttendeeInfoNum2 { get; set; }
		public string AttendeeReference { get; set; }
		public string AttendeeAddress { get; set; }
		public string AttendeeZipCode { get; set; }
		public string AttendeeCity { get; set; }
		public string AttendeeState { get; set; }
		public string AttendeeSpecialty { get; set; }
		public string AttendeeTitle { get; set; }
		public string AttendeeExternalreference { get; set; }
		public string AttendeeExternalSource { get; set; }
		public double? AttendeeQuantity { get; set; }
		public bool NoShow { get; set; }

		public AttendeeResponse () {}
	}
}