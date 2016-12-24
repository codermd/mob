using System;
using System.Collections.Generic;
using System.Linq;

using Mxp.Core.Services.Responses;
using System.Threading.Tasks;

namespace Mxp.Core.Business
{
	public class LookupItems : SGCollection<LookupItem>
	{
		public LookupItems (List<LookupItemResponse> lookupItemResponses) : base (lookupItemResponses) {

		}

		public LookupItems () : base () {

		}
	}
}