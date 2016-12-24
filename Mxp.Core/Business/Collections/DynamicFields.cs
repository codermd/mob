using System;
using System.Threading.Tasks;
using Mxp.Core.Services.Responses;
using System.Collections.ObjectModel;
using System.Linq;
using System.Collections.Generic;

namespace Mxp.Core.Business
{
	public class DynamicFields : SGCollection<DynamicFieldHolder>
	{
		public DynamicFields (List<DynamicFieldResponse> dynamicFieldResponses) : base (dynamicFieldResponses) {
		}

		public DynamicFieldHolder GetFieldWithKey (String key, DynamicFieldHolder.LocationEnum location) {
			return this.SingleOrDefault (dynamicField =>
				dynamicField.LinkName.Equals (key) && dynamicField.LocationName.Equals(location));
		}
	}
}