using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Mxp.Core.Services.Responses;
using System.Collections.ObjectModel;

namespace Mxp.Core.Business
{
	public class TravelFlights : SGCollection<TravelFlight>
	{
		public TravelFlights (Travel travel) : base (travel) {

		}
	}
}