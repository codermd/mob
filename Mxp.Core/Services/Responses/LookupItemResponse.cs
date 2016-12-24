using System;

namespace Mxp.Core.Services.Responses
{
	public class LookupItemResponse : Response
	{
		public string Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public object ExtraInfo { get; set; }

		public LookupItemResponse () {}
	}
}