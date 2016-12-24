using System;

namespace Mxp.Core.Services.Google
{
	public class PlaceDetails
	{
		public Address result { get; set; }
		public string status { get; set; }

		public PlaceDetails () {

		}

		public string Locality => this.result?.LocalityComponent?.long_name;
	}
}