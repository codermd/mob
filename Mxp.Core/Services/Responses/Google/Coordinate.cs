using System;

namespace Mxp.Core.Services.Google
{
	public class Coordinate
	{
		public double lat { get; set; }
		public double lng { get; set; }

		public Coordinate () {

		}

		public Coordinate (double lat, double lng) {
			this.lat = lat;
			this.lng = lng;
		}
	}
}