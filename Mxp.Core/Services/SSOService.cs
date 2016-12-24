using System;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Diagnostics;
using System.Threading.Tasks;

using Newtonsoft.Json;
using ModernHttpClient;
using Newtonsoft.Json.Linq;

using Mxp.Core.Services.Responses;
using RestSharp.Portable;
using Mxp.Core.Business;
using System.Threading;
using Xamarin.Forms;
using System.Net;
using System.Collections.Generic;
using Mxp.Core.Utils;

namespace Mxp.Core.Services
{
	public static class SSOServiceExtensions {
		public static string GetRoute (this SSOService.ApiEnum route) {
			switch (route) {
				case SSOService.ApiEnum.CheckToken:
					return "APILogin/CheckSharedKey";
				case SSOService.ApiEnum.ValidateLogin:
					return "APILogin/Validate";
				case SSOService.ApiEnum.LoginByMail:
					return "APIlogin/LoginByMail";
				default:
					return null;
			}
		}
	}

	public class SSOService : Service
	{
		public static readonly SSOService Instance = new SSOService ();

		public enum ApiEnum {
			CheckToken,
			ValidateLogin,
			LoginByMail
		}

		private SSOService () : base () {

		}

		public async Task CheckTokenAsync () {
			RestRequest request = new RestRequest (ApiEnum.CheckToken.GetRoute (), HttpMethod.Post);

			request.AddParameter ("MXPSessionSharedKey", LoggedUser.Instance.Token);

			try {
				IRestResponse<CheckTokenResponse> response = await this.GetClient (Service.ApiUrl).Execute<CheckTokenResponse> (request);

				this.TryValidateResponseRequest (response.Data);
			} catch (Exception e) {
				if (e is ValidationError)
					throw e;
				else
					throw new ValidationError ("Error", Service.NoConnectionError);
			}
		}

		public async Task LoginAsync () {
			RestRequest request = new RestRequest (ApiEnum.ValidateLogin.GetRoute ());

			this.ConfigureDefaultRequest (request);

			request.AddParameter ("txtusername", LoggedUser.Instance.Username);
			request.AddParameter ("txtpassword", LoggedUser.Instance.Password);

			try {
				IRestResponse<LoginResponse> response = await this.GetClient(Service.ApiUrl).Execute<LoginResponse> (request);

				this.TryValidateResponseRequest ((Response) response.Data);

				LoggedUser.Instance.Token = response.Data.SessionSharedKey;
			} catch (Exception e) {
				if (e is ValidationError)
					throw e;
				else
					throw new ValidationError ("Error", Service.NoConnectionError);
			}
		}

		public async Task<string> GetRedirectionURLLoginByMailAsync () {
			RestRequest request = new RestRequest (ApiEnum.LoginByMail.GetRoute ());

			this.ConfigureDefaultRequest (request);

			request.AddParameter ("deviceOs", Device.OnPlatform ("I", "A", "W"));

			request.AddParameter ("EmployeeEmail", LoggedUser.Instance.Email);

			try {
				IRestResponse<LoginByMailResponse> response = await this.GetClient(Service.ApiUrl).Execute<LoginByMailResponse> (request);

				this.TryValidateResponseRequest ((Response) response.Data);

				return response.Data.redirection;
			} catch (Exception e) {
				if (e is ValidationError)
					throw e;
				else
					throw new ValidationError ("Error", Service.NoConnectionError);
			}
		}
	}
}