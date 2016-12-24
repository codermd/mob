using System;
using System.Collections.Generic;

namespace Mxp.Core.Services.Google
{
	public class Leg
	{
		public TextValue distance { get; set; }
		public TextValue duration { get; set; }
		public string end_address { get; set; }
		public Coordinate end_location { get; set; }
		public string start_address { get; set; }
		public Coordinate start_location { get; set; }
//		public List<Step> steps { get; set; }
//		public List<object> via_waypoint { get; set; }

		public Leg () {

		}
	}
}