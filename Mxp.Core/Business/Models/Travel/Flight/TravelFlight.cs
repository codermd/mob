using System;
using Mxp.Core.Services.Responses;
using Mxp.Utils;

namespace Mxp.Core.Business
{
	public partial class TravelFlight : Model
	{
		public string Type { get; set; }
		public string DepartureTime { get; set; }
		public string ArrivalTime { get; set; }
		public string Class { get; set; }
		public string FromCountry { get; set; }
		public string ToCountry { get; set; }
		public string FromAirport { get; set; }
		public string ToAirport { get; set; }
		public DateTime? Date { get; set; }

		public TravelFlight (TravelFlightResponse travelFlightResponse) {
			this.Type = travelFlightResponse.travelflightType;
			this.DepartureTime = travelFlightResponse.travelflightDeparturetime;
			this.ArrivalTime = travelFlightResponse.travelflightArrivaltime;
			this.Class = travelFlightResponse.travelflightClass;
			this.ToCountry = travelFlightResponse.ToCountry;
			this.FromCountry = travelFlightResponse.FromCountry;
			this.ToAirport = travelFlightResponse.TOAirport;
			this.FromAirport = travelFlightResponse.FROMAirport;

			this.Date = travelFlightResponse.travelflightDate.ToDateTime();
		}
	}
}