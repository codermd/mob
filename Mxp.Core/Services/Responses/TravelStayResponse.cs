using System;

namespace Mxp.Core.Services.Responses
{
	public class TravelStayResponse : Response
	{
		public string travelrequestStatus { get; set; }
		public int travelStayID { get; set; }
		public string travelStayDatein { get; set; }
		public string travelStayDateout { get; set; }
		public int travelStayNumbernights { get; set; }
		public string travelStayPreferedhotel { get; set; }
		public string travelStayPurpose { get; set; }
		public string travelStayLocation { get; set; }
		public string travelStayLocationLabel { get; set; }
		public int CountryID { get; set; }
		public string CountryName { get; set; }
		public string CountryIsoName { get; set; }
		public string BookingEngine { get; set; }
		public string travelStayClass { get; set; }
		public string ToCountry { get; set; }
		public double TravelStayAmount { get; set; }
		public int TravelStayPaymentMethodId { get; set; }
		public bool TravelStayPrivate { get; set; }
		public int TravelStayBookingTypeId { get; set; }
		public int TravelStayInformTravelAgent { get; set; }
		public int Merchantid { get; set; }
		public string MerchantName { get; set; }

		public TravelStayResponse () {}
	}
}