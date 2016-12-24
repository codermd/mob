using System;
using System.Collections.Generic;

namespace Mxp.Core.Services.Responses
{
	public class TravelResponse : Response
	{
		public int travelrequestID { get; set; }
		public string employeeFirstname { get; set; }
		public string employeeLastname { get; set; }
		public string travelrequestName { get; set; }
		public double travelrequestMaxamount { get; set; }
		public string travelrequestComments { get; set; }
		public string travelrequestSubmittedon { get; set; }
		public string travelRequestStartDate { get; set; }
		public string travelRequestEndDate { get; set; }
		public string travelrequestAdditional { get; set; }
		public string TravelRequestType { get; set; }
		public object TravelRequestPNRID { get; set; }
		public string TravelRequestPolResult { get; set; }
		public string TravelRequestPolTip { get; set; }
		public string travelrequeststatus { get; set; }
		public int merchantID { get; set; }
		public int DepartmentID { get; set; }
		public int ProjectID { get; set; }
		public string TravelRequestparchar1ov { get; set; }
		public string TravelRequestparchar2ov { get; set; }
		public string TravelRequestparchar3ov { get; set; }
		public string TravelRequestparchar4ov { get; set; }
		public string TravelRequestparchar5ov { get; set; }
		public string TravelRequestparchar6ov { get; set; }
		public string TravelRequestparchar7ov { get; set; }
		public string TravelRequestparchar8ov { get; set; }
		public string TravelRequestparchar9ov { get; set; }
		public string TravelRequestparchar10ov { get; set; }
		public string TravelRequestparint1ov { get; set; }
		public string TravelRequestparint2ov { get; set; }
		public string TravelRequestparint3ov { get; set; }
		public string TravelRequestparint4ov { get; set; }
		public string TravelRequestparint5ov { get; set; }
		public string TravelRequestparint6ov { get; set; }
		public string TravelRequestparint7ov { get; set; }
		public string TravelRequestparint8ov { get; set; }
		public string TravelRequestparint9ov { get; set; }
		public string TravelRequestparint10ov { get; set; }
		public double TravelRequestparamount1ov { get; set; }
		public double TravelRequestparamount2ov { get; set; }
		public double TravelRequestparamount3ov { get; set; }
		public double TravelRequestparamount4ov { get; set; }
		public double TravelRequestparamount5ov { get; set; }
		public double TravelRequestparamount6ov { get; set; }
		public double TravelRequestparamount7ov { get; set; }
		public double TravelRequestparamount8ov { get; set; }
		public double TravelRequestparamount9ov { get; set; }
		public double TravelRequestparamount10ov { get; set; }
		public int TravelRequestparind1ov { get; set; }
		public int TravelRequestparind2ov { get; set; }
		public int TravelRequestparind3ov { get; set; }
		public int TravelRequestparind4ov { get; set; }
		public int TravelRequestparind5ov { get; set; }
		public int TravelRequestparind6ov { get; set; }
		public int TravelRequestparind7ov { get; set; }
		public int TravelRequestparind8ov { get; set; }
		public int TravelRequestparind9ov { get; set; }
		public int TravelRequestparind10ov { get; set; }
		public List<TravelFlightResponse> Flights { get; set; }
		public List<TravelStayResponse> Stays { get; set; }
		public List<TravelCarRentalResponse> CarRentals { get; set; }

		public TravelResponse () {}
	}
}