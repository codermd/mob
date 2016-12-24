using System;
using System.Collections.Generic;

namespace Mxp.Core.Services.Responses
{
	public class ItineraryResponse : Response
	{
		public int ItineraryId { get; set; }
		public double ItineraryDistance { get; set; }
		public string departure { get; set; }
		public string arrival { get; set; }
		public List<MileageSegmentResponse> segments { get; set; }
		public int LinkID { get; set; }
		public string ItineraryName { get; set; }
		public double ItineraryTotalQuantity { get; set; }
		public double ItineraryBusinessQuantity { get; set; }
		public double ItineraryCommutingQuantity { get; set; }
		public double ItineraryPrivateQuantity { get; set; }
		public object ItineraryLinkType { get; set; }

		public ItineraryResponse () {}
	}
}