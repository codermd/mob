using System;
using System.Collections.Generic;

namespace Mxp.Core.Services.Google
{
	public class Addresses
	{
		public List<Address> results { get; set; }
		public string status { get; set; }

		public Addresses () {
		}
	}
}