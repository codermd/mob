using System;

namespace Mxp.Core.Services.Responses
{
	public class TravelFlightResponse : Response
	{
		public string travelrequestStatus { get; set; }
		public int travelflightID { get; set; }
		public string travelflightDate { get; set; }
		public string travelflightFrom { get; set; }
		public string travelflightTo { get; set; }
		public string travelflightType { get; set; }
		public string travelflightDeparturetime { get; set; }
		public string travelflightArrivaltime { get; set; }
		public string travelflightFlightnumber { get; set; }
		public string travelflightClass { get; set; }
		public string ToCountry { get; set; }
		public int ToCountryID { get; set; }
		public string FromCountry { get; set; }
		public int FromCountryID { get; set; }
		public string TOAirport { get; set; }
		public string FROMAirport { get; set; }
		public double TravelFlightAmount { get; set; }
		public int TravelFlightPaymentMethodId { get; set; }
		public bool TravelFlightPrivate { get; set; }
		public int TravelFlightBookingTypeId { get; set; }
		public int TravelFlightInformTravelAgent { get; set; }
		public string AirportNameFrom { get; set; }
		public string AirportNameTo { get; set; }

		public TravelFlightResponse () {}
	}
}