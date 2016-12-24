using System;
using System.Collections.ObjectModel;

namespace Mxp.Core.Business
{
	public partial class TravelFlight
	{
		public Collection<Field> GetMainFields(){
			return new Collection<Field> {
				new TravelFLightType (this),
				new TravelFLightClass (this),
				new TravelFLightDate (this),
				new TravelFLightDepartureTime (this),
				new TravelFLightDepartureCountry (this),
				new TravelFLightDepartureAirport (this),
				new TravelFLightArrivalTime (this),
				new TravelFLightArrivalCountry (this),
				new TravelFLightArrivalAirport (this)
			};
		}
	}
}