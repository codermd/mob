using System;
using Xamarin.Forms;
using Mxp.Core.Business;
using RestSharp.Portable;
using System.Diagnostics;
using System.Threading.Tasks;
using Mxp.Core.Services.Responses;
using Mxp.Core.Utils;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using Newtonsoft.Json.Linq;
using MimeSharp;

namespace Mxp.Core.Services
{
	public static class ServiceExtensions {
		public static String GetRootUrl (this Service.DomainApiEnum api) {
			return Service.protocol + "://" + api.ToString ().ToLowerInvariant () + "." + Service.DomainName + "/";
		}

		public static String ApiForCredential (this Service.DomainApiEnum api) {
			return (api == Service.DomainApiEnum.Staging ? api.ToString ().ToLowerInvariant () + "." : "");
		}
	}

	public abstract class Service
	{
		public static string NoConnectionError = "Network error, please check your internet connection.";

		public const string protocol = "https";
		public const string DomainName = "mobilexpense.com";

		public const int TIMEOUT_IN_SECONDS = 30;
		public const int TIMEOUT_UPLOAD_IMAGE_IN_SECONDS = 60;

		private static IServiceAppVersion serviceAppVersion = DependencyService.Get<IServiceAppVersion> ();

		public enum DomainApiEnum {
			Staging,
			Mob
		};

		private static string _appVersion;
		public static string AppVersion { 
			get {
				if (_appVersion == null) {
					string[] splitVersion = serviceAppVersion.AppVersion ().Split ('.');

					_appVersion = splitVersion [0];

					for (int i = 1; i < splitVersion.Length; i++)
						_appVersion += splitVersion [i].Length == 2 ? splitVersion [i] : ('0' + splitVersion [i]);
				}

				return _appVersion;
			}
		}

		protected Service () {

		}

		public static String BaseUrl { get; private set; }

		public static void RefreshBaseUrl () {
			BaseUrl = LoggedUser.Instance.UserAPI.GetRootUrl ();
		}

		public static String ApiUrl {
			get {
				return BaseUrl + "net/";
			}
		}

		protected void ConfigureDefaultRequest (RestRequest request) {
			request.Method = HttpMethod.Post;

			request.AddQueryParameter ("typeOs", Device.OnPlatform ("I", "A", "W"));

			request.AddQueryParameter ("appVersion", AppVersion);
		}

		protected void ConfigureLoggedRequest (RestRequest request) {
			this.ConfigureDefaultRequest (request);

			request.AddQueryParameter ("MXPSessionSharedKey", LoggedUser.Instance.Token);
		}

		protected bool PrepareDefaultRequest (RestRequest request) {
			if (!LoggedUser.Instance.IsAuthenticated) {
				Debug.WriteLine ("user is not authenticated !");
				return false;
			}

			this.ConfigureLoggedRequest (request);

			#if DEBUG
			this.PrintParameters (request);
			#endif

			return true;
		}

        public async Task<T> ExecuteAsync<T> (RestRequest request, int timeout = TIMEOUT_IN_SECONDS, CancellationToken token = default (CancellationToken)) where T : new () {
			if (!this.PrepareDefaultRequest (request))
				return default (T);

			try {
				TrackContext.AddRequest(request);
				IRestResponse<T> response = await this.GetClient (Service.ApiUrl, timeout).Execute<T> (request, token);

				this.TryValidateResponseRequest (response.Data as Response);

				return response.Data;
			} catch (ValidationError e) {
				throw e;
			} catch (OperationCanceledException e) {
				if (token != default (CancellationToken))
					throw e;
				else
					throw new ValidationError ("Error", NoConnectionError);
			} catch (Exception) {
				throw new ValidationError ("Error", NoConnectionError);
			}
		}

		public async Task ExecuteAsync (RestRequest request) {
			if (!this.PrepareDefaultRequest (request))
				return;

			RestClient client = this.GetClient (Service.ApiUrl);

			try {
				TrackContext.AddRequest(request);
				IRestResponse<dynamic> response = await client.Execute<dynamic> (request);

				if (response.Data is JObject)
					this.TryValidateResponseRequest (((JObject) response.Data).ToObject<Response> ());
			} catch (Exception e) {
				Debug.WriteLine (e.Message);
				if (e is ValidationError)
					throw e;
				else
					throw new ValidationError ("Error", NoConnectionError);
			}
		}

		protected void PrintParameters (RestRequest request) {
			string requestStr = String.Empty;
			request.Parameters.ForEach (parameter => {

				object pValue = parameter.Value;
				if(pValue is String && ((string)pValue).Length>300) {
					pValue = ((string)parameter.Value).Substring(0,300)+"...";
				}
				requestStr += parameter.Name + " - <" + pValue + ">\n";
			});
			Debug.WriteLine (requestStr);
		}

		protected RestClient GetClient (string url, int timeout = TIMEOUT_IN_SECONDS) {
			RestClient client = new RestClient (url);
			client.Timeout = TimeSpan.FromSeconds (timeout);
			client.HttpClientFactory = new ModernHttpClientFactory ();
			client.IgnoreResponseStatusCode = true;
			return client;
		}

		public async Task<T> ExecuteAsync<T> (RestRequest request, Dictionary<string, object> objectToJsonSerialize) where T : new () {
			if (!this.PrepareDefaultRequest (request))
				return default (T);

			request.AddJsonBody (objectToJsonSerialize);

			try {
				TrackContext.AddRequest(request);
				IRestResponse<T> response = await this.GetClient (Service.ApiUrl).Execute<T> (request);

				this.TryValidateResponseRequest (response.Data as Response);

				return response.Data;
			} catch (Exception e) {
				Debug.WriteLine (e.Message);
				if (e is ValidationError)
					throw e;
				else
					throw new ValidationError ("Error", NoConnectionError);
			}
		}

		public async Task<T> ExecuteBaseAsync<T> (string rootUrl, RestRequest request, CancellationToken token = default (CancellationToken)) where T : new () {
			try {
				TrackContext.AddRequest(request);
				IRestResponse<T> response = await this.GetClient (rootUrl).Execute<T> (request, token);
				return response.Data;
			} catch (OperationCanceledException e) {
				if (token != default (CancellationToken))
					throw e;
				else
					throw new ValidationError ("Error", NoConnectionError);
			} catch (Exception e) {
				Debug.WriteLine (e.Message);
				throw new ValidationError ("Error", NoConnectionError);
			}
		}

		public async Task FetchGenericAsync<T, U> (SGCollection<T> collection, string route, int timeout = TIMEOUT_IN_SECONDS) where T : Model where U : Response {
			RestRequest request = new RestRequest(route);

			List<U> responses = await this.ExecuteAsync<List<U>> (request, timeout);

			if (responses != null)
				Debug.WriteLine ("{0} responses", responses.Count);

			collection.Populate (responses);
		}

		protected void TryValidateResponseRequest (Response response) {
			if (response != null && !String.IsNullOrWhiteSpace (response.ErrorMessage))
				throw new ValidationError ("Error " + response.ErrorId,  response.ErrorMessage);
		}

		public async Task<byte []> DownloadFileAsync (string url, CancellationToken token = default (CancellationToken)) {
			Uri uri = new Uri (url);

			RestRequest request = new RestRequest (uri.LocalPath, HttpMethod.Get);
			request.AddHeader ("Accept", Mime.Lookup (url));

			TrackContext.AddRequest (request);

			IRestResponse response = await this.GetClient (Service.ApiUrl).Execute (request, token);

			return response.RawBytes;
		}

		public async Task<string> DownloadAndSaveFileAsync (string url, string filename, FileServiceBase.FileDirectory targetDirectory) {
			IFileService fileService = DependencyService.Get<IFileService> ();

			byte [] data = await this.DownloadFileAsync (url);

			return await fileService.SaveBinaryAsync (filename, targetDirectory, data);
		}
	}
}