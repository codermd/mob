using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Mxp.Core.Business
{
	public class TravelStays : SGCollection<TravelStay>
	{
		public TravelStays(Travel travel) : base (travel) {

		}
	}
}