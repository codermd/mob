using System;
using System.Collections.Generic;
using Mxp.Core.Services.Responses;
using System.Threading.Tasks;
using Mxp.Core.Services;

namespace Mxp.Core.Business
{
	public class Journeys : SGCollection<Journey>
	{
		public Journeys () {

		}

		public override async Task FetchAsync () {
			await MileageService.Instance.GetFavouriteJourneys (this);
		}

		public IEnumerable<Response> rawResponses;
		public override void Populate (IEnumerable<Response> collection) {
			this.rawResponses = collection;
			base.Populate (collection);
		}
	}
}