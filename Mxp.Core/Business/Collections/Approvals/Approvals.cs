using System;
using System.Threading.Tasks;
using Mxp.Core.Services;
using Mxp.Core.Utils;
using System.Collections.Generic;
using Mxp.Core.Services.Responses;
using System.Linq;

namespace Mxp.Core.Business
{
	public abstract class Approvals<T> : SGCollection<T> where T : Approval
	{
		public Approvals () {

		}
	}
}