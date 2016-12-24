using System;

namespace Mxp.Core.Services.Responses
{
	public class Response
	{
		public int ErrorType { get; set; }
		public string ErrorMessage { get; set; }
		public string ErrorField { get; set; }
		public int ErrorId { get; set; }
		public object Context { get; set; }

		public Response () {}
	}
}