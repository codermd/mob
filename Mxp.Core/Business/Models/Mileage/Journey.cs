using System;
using Mxp.Core.Services.Responses;

namespace Mxp.Core.Business
{
	public class Journey : Model
	{
		public int Id { get; set; }
		public int LinkId { get; set; }
		public string Name { get; set; }
		public double TotalQuantity { get; set; }
		public double BusinessQuantity { get; set; }
		public double CommutingQuantity { get; set; }
		public double PrivateQuantity { get; set; }
		public string LinkType { get; set; }
		public Itinerary Itinerary { get; set; }

		private Journey () {
			this.Itinerary = new Itinerary ();
		}

		public Journey (JourneyResponse journeyResponse) : this () {
			this.Id = journeyResponse.ItineraryId;
			this.LinkId = journeyResponse.LinkID;
			this.Name = journeyResponse.ItineraryName;
			this.TotalQuantity = journeyResponse.ItineraryTotalQuantity;
			this.BusinessQuantity = journeyResponse.ItineraryBusinessQuantity;
			this.CommutingQuantity = journeyResponse.ItineraryCommutingQuantity;
			this.PrivateQuantity = journeyResponse.ItineraryPrivateQuantity;
			this.LinkType = journeyResponse.ItineraryLinkType;

			this.Itinerary.Populate (journeyResponse.Itinerary);
		}
	}
}