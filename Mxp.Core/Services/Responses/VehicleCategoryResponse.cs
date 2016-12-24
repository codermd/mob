using System;

namespace Mxp.Core.Services.Responses
{
	public class VehicleCategoryResponse : Response
	{
		public int VehicleCategoryID { get; set; }
		public string MileageRateName { get; set; }

		public VehicleCategoryResponse () {}
	}
}