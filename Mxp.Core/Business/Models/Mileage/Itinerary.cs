using System;
using System.Collections.Generic;
using Mxp.Core.Services.Responses;

namespace Mxp.Core.Business
{
	public class Itinerary : Model
	{
		public int Id { get; set; }
		public double Distance { get; set; }
		public string Departure { get; set; }
		public string Arrival { get; set; }
		public MileageSegments MileageSegments { get; set; }
		public int LinkId { get; set; }
		public string Name { get; set; }
		public double TotalQuantity { get; set; }
		public double BusinessQuantity { get; set; }
		public double CommutingQuantity { get; set; }
		public double PrivateQuantity { get; set; }
		public object LinkType { get; set; }

		public Itinerary () {
			this.MileageSegments = new MileageSegments (this);
		}

		public void Populate (ItineraryResponse itineraryResponse) {
			this.Id = itineraryResponse.ItineraryId;
			this.Distance = itineraryResponse.ItineraryDistance;
			this.Departure = itineraryResponse.departure;
			this.Arrival = itineraryResponse.arrival;
			this.LinkId = itineraryResponse.LinkID;
			this.Name = itineraryResponse.ItineraryName;
			this.TotalQuantity = itineraryResponse.ItineraryTotalQuantity;
			this.BusinessQuantity = itineraryResponse.ItineraryBusinessQuantity;
			this.CommutingQuantity = itineraryResponse.ItineraryCommutingQuantity;
			this.PrivateQuantity = itineraryResponse.ItineraryPrivateQuantity;
			this.LinkType = itineraryResponse.ItineraryLinkType;

			this.MileageSegments.Populate (itineraryResponse.segments);
		}
	}
}