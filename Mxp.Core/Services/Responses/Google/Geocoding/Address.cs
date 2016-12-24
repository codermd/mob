using System;
using System.Collections.Generic;

namespace Mxp.Core.Services.Google
{
	public class Address
	{
		public List<AddressComponent> address_components { get; set; }
		public string formatted_address { get; set; }
		public LocationGeometry geometry { get; set; }

		public Address () {

		}

		public AddressComponent LocalityComponent => this.address_components?.Find (addressComponent => addressComponent.IsLocality)
		                                                 ?? this.address_components?.Find (addressComponent => addressComponent.IsState);
	}
}