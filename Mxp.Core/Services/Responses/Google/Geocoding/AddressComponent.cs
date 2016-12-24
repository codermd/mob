using System.Collections.Generic;

namespace Mxp.Core
{
	public class AddressComponent {
		public string long_name { get; set; }
		public string short_name { get; set; }
		public List<string> types { get; set; }

		public AddressComponent () {}

		public bool IsLocality => this.types.Contains ("locality");

		public bool IsState => this.types.Contains ("administrative_area_level_1");
	}
}