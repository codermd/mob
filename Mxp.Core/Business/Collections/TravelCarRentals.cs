using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Mxp.Core.Services.Responses;
using System.Collections.ObjectModel;

namespace Mxp.Core.Business
{
	public class TravelCarRentals : SGCollection<TravelCarRental>
	{
		public TravelCarRentals (Travel travel) : base (travel) {

		}
	}
}