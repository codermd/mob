using System;
using System.Collections.ObjectModel;

namespace Mxp.Core.Business
{
	public partial class TravelStay
	{
		public Collection<Field> GetMainFields(){
			return new Collection<Field> {
				new TravelStayArrivalDate (this),
				new TravelStayDepartureDate (this),
				new TravelStayNumberOfNight (this),
				new TravelStayCountry (this),
				new TravelStayClass (this),
				new TravelStayMerchant (this),
				new TravelStayPreferedHotel (this),
				new TravelStayLocation (this)
			};
		}
	}
}