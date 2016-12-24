using System;
using Mxp.Utils;

namespace Mxp.Core.Services.Responses
{
	public class OpenObjectResponse : Response
	{
		public string Location { get; set; }
		public int Id { get; set; }
		public int FatherId { get; set; }
		public string Login { get; set; }

		public OpenObjectResponse () {}
	}
}