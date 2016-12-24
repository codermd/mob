using System;
using System.Text;

namespace Mxp.Core.Services.Responses
{
	public class LoginResponse : Response
	{
		public string SessionSharedKey { get; set; }

		public LoginResponse () {}
	}
}