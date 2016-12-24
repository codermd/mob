using System;
using System.Collections.Generic;
using RestSharp.Portable;
using Mxp.Core.Business;
using Newtonsoft.Json;
using Xamarin.Forms;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Mxp.Core.Utils;
using Mxp.Core.Services;

namespace Mxp.Core
{
	public class TrackContext
	{

		static string FILENAME = "context";


		Exception _exception;
		public Exception Exception {
			set {

				Exception oldValue = this._exception;
				this._exception = value;

				if (oldValue != value) {
					this.write ();
				}

				if (value != null) {
					this.trySendException ();
				}
			}
		}

		bool isSendingException = false;
		async Task trySendException() {

			if (isSendingException) {
				return;
			}
			isSendingException = true;
			try {
				await SystemService.Instance.LogExceptionAsync (this._exception);	
			} catch(Exception) {
				this.write ();
				return;
			} finally {
				isSendingException = false;
			}

			this.Exception = null;
			this.write ();
		}

		public TrackContext ()
		{
			
		}

		public void Load() {
			var str = DependencyService.Get<ILoggedUserFileIO> ().readFile (FILENAME);
			if (!String.IsNullOrWhiteSpace(str)) {
				this.UnserializeStringFormat (str);
			}
		}

		void write() {
				DependencyService.Get<ILoggedUserFileIO> ().writeFile (FILENAME, this.SerializeStringFormat());
		}

		public static void AddView(string view) {
			LoggedUser.Instance.TrackContext.AddViews (view);
		}

		public static void AddRequest(RestRequest request) {
			LoggedUser.Instance.TrackContext.AddRequests (request);
		}

		static int LAST_REQUEST_BUFFER_SIZE = 5;

		public string[] LastRequest {
			get {
				return this._lastRequests.ToArray ();		
			}
		}

		List<string> _lastRequests = new List<string>();

		void AddRequests(RestRequest request) {
			if (this._lastRequests.Count >= LAST_REQUEST_BUFFER_SIZE) {
				_lastRequests.RemoveAt (0);
			}
			_lastRequests.Insert (_lastRequests.Count, request.Resource); 
			this.write ();
		}

		static int LAST_VIEW_BUFFER_SIZE = 5;

		public String[] LastViews { get { 
				return this._lastViews.ToArray ();
			}
		}

		List<String> _lastViews = new List<String>();

		public void AddViews(string view) {
			if (this._lastViews.Count >= LAST_VIEW_BUFFER_SIZE) {
				_lastViews.RemoveAt (0);
			}
			_lastViews.Insert (_lastViews.Count, view); 
			this.write ();
		}


		public string SerializeStringFormat () {
			return JsonConvert.SerializeObject (this.SerializeFileFormat ());
		}

		public void UnserializeStringFormat (string loggedUserStr) {

			if (loggedUserStr == null) {
				return;
			}

			Dictionary<string, object> res = JsonConvert.DeserializeObject<Dictionary<string, object>> (loggedUserStr);
			this.UnserializeFileFormat (res);
		}

		Dictionary<string, object> SerializeFileFormat () {
			
			Dictionary<string, object> result = new Dictionary<string, object> ();
			result ["_lastViews"] = this._lastViews;
			result ["_lastRequests"] = this._lastRequests;

			if (this._exception != null) {
				result ["_exception"] = new Dictionary<string, string> () {
					{ "Message", this._exception.Message }, 
					{ "StackTrace", this._exception.StackTrace }
				};
			}

			return result;

		}


		public string serializeForServerException() {
			string result = "";

			result +="Request: \n";
			this.LastRequest.ForEach ((item)=>{
				result += item+ "\n";
			});
			result += "\n\n";

			result += "LastViews: \n";
			var items = this.LastViews;
			for(int i= items.Length -1 ; i<=0; i--) {
				result += items[i]+ "\n";
			}

			return result;
		}

		void UnserializeFileFormat (Dictionary<string, object> dict) {
			
			if (dict.ContainsKey ("_lastViews")) {
				this._lastViews = (dict ["_lastViews"] as JArray).ToObject<List<string>> ();
			}

			if (dict.ContainsKey ("_lastRequests")) {
				this._lastRequests = (dict ["_lastRequests"] as JArray).ToObject<List<string>> ();
			}

			if (dict.ContainsKey ("_exception")) {
				var excAsDict = (dict ["_exception"] as JObject).ToObject<Dictionary<string, string>> ();
				this.Exception = new EmbeddedException (excAsDict);
			}
		}

		class EmbeddedException : Exception {

			string _stacktrace;
			public override string StackTrace {
				get {
					return _stacktrace;
				}
			}

			public EmbeddedException(Dictionary<string, string> dict) : base(dict["Message"]) {
				this._stacktrace = dict["StackTrace"];
			}
		}

	}
}

