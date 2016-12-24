using Mxp.Core.Services.Responses;

namespace Mxp.Core
{
	public class CheckTokenResponse : Response {
		public bool SharedKeyValid { get; set; }

		public CheckTokenResponse () {
			
		}
	}
}
