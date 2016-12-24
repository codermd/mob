using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Mxp.Core.Services.Responses;
using System.Collections.ObjectModel;

namespace Mxp.Core.Business
{
	public class AllowanceSegments : SGCollection<AllowanceSegment>
	{
		public AllowanceSegments (Allowance allowance) : base (allowance) {

		}
	}
}