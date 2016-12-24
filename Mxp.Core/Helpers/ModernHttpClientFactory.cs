 using System;
using System.Net.Http;

using ModernHttpClient;
using RestSharp.Portable;
using RestSharp.Portable.HttpClientImpl;

namespace Mxp.Core
{
	public class ModernHttpClientFactory : DefaultHttpClientFactory
	{
		public ModernHttpClientFactory () {}

		protected override HttpMessageHandler CreateMessageHandler (IRestClient client, IRestRequest request) {
			NativeMessageHandler handler = new NativeMessageHandler ();

//			var proxy = GetProxy(client);
//			if (handler.SupportsProxy && proxy != null) {
//				handler.UseProxy = true;
//				handler.Proxy = proxy;
//			}

			var cookieContainer = GetCookies(client, request);
			if (cookieContainer != null) {
				handler.UseCookies = true;
				handler.CookieContainer = cookieContainer;
			}

			var credentials = GetCredentials(request);
			if (credentials != null)
				handler.Credentials = credentials;

			return handler;
		}
	}
}