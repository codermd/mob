using System;
using Mxp.Core.Business;
using Mxp.Core.Services.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mxp.Core.Services;

namespace Mxp.Core.Business
{
	public class Vehicles : SGCollection<Vehicle>
	{
		public Vehicles (Mileage mileage) : base (mileage) {

		}

		public override async Task FetchAsync () {
			await MileageService.Instance.FetchVehicles (this, this.GetParentModel<Mileage> ().Date.Value);
		}

		public IEnumerable<Response> rawResponses;
		public override void Populate (IEnumerable<Response> collection) {
			this.rawResponses = collection;
			base.Populate (collection);
		}
	}
}