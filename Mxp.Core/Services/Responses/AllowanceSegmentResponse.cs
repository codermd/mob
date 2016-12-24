using System;

namespace Mxp.Core.Services.Responses
{
	public class AllowanceSegmentResponse : Response
	{
		public int countryId { get; set; }
		public bool journeySegmentIndicator1 { get; set; }
		public bool journeySegmentIndicator2 { get; set; }
		public bool journeySegmentIndicator3 { get; set; }
		public bool journeySegmentIndicator4 { get; set; }
		public bool journeySegmentIndicator5 { get; set; }
		public bool journeySegmentIndicator6 { get; set; }
		public bool i1Display { get; set; }
		public bool i2Display { get; set; }
		public bool i3Display { get; set; }
		public bool i4Display { get; set; }
		public bool i5Display { get; set; }
		public bool i6Display { get; set; }
		public string journeySegmentPurpose { get; set; }
		public string journeySegmentLocation { get; set; }
		public string journeySegmentDateTimeFrom { get; set; }
		public int journeySegmentDateTimeFromTicks { get; set; }
		public string journeySegmentDateTimeTo { get; set; }
		public int journeySegmentDateTimeToTicks { get; set; }
		public double grossAmountCC { get; set; }
		public double netAmountCC { get; set; }
		public double vatAmountCC { get; set; }
		public double legalAmountCC { get; set; }
		public int productId { get; set; }
		public int quantity { get; set; }

		public AllowanceSegmentResponse () {}
	}
}