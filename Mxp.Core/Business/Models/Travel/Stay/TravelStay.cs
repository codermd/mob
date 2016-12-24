using System;
using Mxp.Core.Services.Responses;
using Mxp.Utils;

namespace Mxp.Core.Business
{
	public partial class TravelStay : Model
	{
		public int NumberNights { get; set; }
		public string PreferredHotel { get; set; }
		public string LocationLabel { get; set; }
		public string CountryName { get; set; }
		public string Class { get; set; }
		public string MerchantName { get; set; }
		public DateTime? DateIn { get; set; }
		public DateTime? DateOut { get; set; }

		public TravelStay (TravelStayResponse travelStayResponse) {
			this.NumberNights = travelStayResponse.travelStayNumbernights;
			this.PreferredHotel = travelStayResponse.travelStayPreferedhotel;
			this.LocationLabel = travelStayResponse.travelStayLocationLabel;
			this.CountryName = travelStayResponse.CountryName;
			this.Class = travelStayResponse.travelStayClass;
			this.MerchantName = travelStayResponse.MerchantName;

			this.DateIn = travelStayResponse.travelStayDatein.ToDateTime();
			this.DateOut = travelStayResponse.travelStayDateout.ToDateTime();
		}
	}
}