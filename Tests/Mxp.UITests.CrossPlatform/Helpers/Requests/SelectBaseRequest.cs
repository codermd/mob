using System;
namespace Mxp.UITests.CrossPlatform
{
	public class SelectBaseRequest
	{
		public string Label { get; set; }
		public string Value { get; set; }
		public string SearchControlClass { get; set; }
		public string SearchControlId { get; set; }
		public string WaitBackControlClass { get; set; }
		public string WaitBackSearchControlId { get; set; }
	}
}

