using Mxp.Core.Services.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mxp.Core.Services;

namespace Mxp.Core.Business
{
	public class VehicleCategories : SGCollection<VehicleCategory>
	{
		public VehicleCategories () : base () {

		}

		public override async Task FetchAsync () {
			await MileageService.Instance.FetchGenericAsync<VehicleCategory, VehicleCategoryResponse> (this, MileageService.ApiEnum.GetVehicleCategories.GetRoute ());
		}

		public IEnumerable<Response> rawResponses;
		public override void Populate (IEnumerable<Response> collection) {
			this.rawResponses = collection;
			base.Populate (collection);
		}
	}
}