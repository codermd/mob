using System;

namespace Mxp.Core.Services.Responses
{
	public class MileageSegmentResponse : Response
	{
		public int LocationId { get; set; }
		public int ItinerarySegmentPosition { get; set; }
		public double LocationLatitude { get; set; }
		public double LocationLongitude { get; set; }
		public string LocationAliasName { get; set; }
		public int EmployeeFavouriteLocationId { get; set; }
		public int CustomerFavouriteLocationId { get; set; }

		public MileageSegmentResponse () {}
	}
}