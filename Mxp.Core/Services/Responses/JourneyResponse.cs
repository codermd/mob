using System;
using System.Collections.Generic;

namespace Mxp.Core.Services.Responses
{
	public class JourneyResponse : Response
	{
		public int ItineraryId { get; set; }
		public int LinkID { get; set; }
		public string ItineraryName { get; set; }
		public double ItineraryTotalQuantity { get; set; }
		public double ItineraryBusinessQuantity { get; set; }
		public double ItineraryCommutingQuantity { get; set; }
		public double ItineraryPrivateQuantity { get; set; }
		public string ItineraryLinkType { get; set; }
		public ItineraryResponse Itinerary { get; set; }

		public JourneyResponse () {}
	}
}