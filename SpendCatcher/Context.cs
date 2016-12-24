using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.ComponentModel;
using Foundation;
using System.Threading.Tasks;
using UIKit;
using CoreGraphics;
using MobileCoreServices;
using Mxp.Core;
using System.Collections.ObjectModel;

namespace sc
{
	
	public class Context : INotifyPropertyChanged
	{
		
		public static bool USE_FAKE_DATA = false;
		public static bool SHOW_DEBUG = false;

		private static Context _instance;
		public static Context Instance {
			get {
				if (_instance == null) {
					Instance = new Context ();
				}
				return _instance;
			}
			private set {
				_instance = value;
			}
		}


		public NSUserDefaults sharedUserDefault;

		public bool isAuthenticated { 
			get {
				if (USE_FAKE_DATA) {
					return FakeData.authenticated;
				}

				return sharedUserDefault.BoolForKey (AppExtensionSharedKeys.AUTHENTICATED_KEY);
			}
		}


		Dictionary<int, string> _labels;
		public Dictionary<int, string> Labels { 
			get {
				if (this._labels == null) {
					string labelsStr = "";
					if (USE_FAKE_DATA) {
						labelsStr = FakeData.labels;
					} else {
						labelsStr = sharedUserDefault.StringForKey (AppExtensionSharedKeys.LABELS);
					}
					try {
						this._labels = JsonConvert.DeserializeObject<Dictionary<int, string>>(labelsStr);
					} catch(Exception) {
						this._labels = new Dictionary<int, string> ();
					}
				}
				return this._labels;
			}
		}

		public bool isAccessible { 
			get {

				if (USE_FAKE_DATA) {
					return true;
				}

				return sharedUserDefault.BoolForKey (AppExtensionSharedKeys.ACCESSIBILITY_KEY);
			}
		}

		public bool canCreateOrEdit { 
			get {
				return  this.isAuthenticated && this.isAccessible;
			}

		}

		public string token { 
			get {
				if (USE_FAKE_DATA) {
					return FakeData.token;
				}
				if (sharedUserDefault.StringForKey (AppExtensionSharedKeys.TOKEN_KEY) == null) {
					return null;
				}
				return sharedUserDefault.StringForKey(AppExtensionSharedKeys.TOKEN_KEY);
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;
		public virtual void NotifyPropertyChanged (string name) {
			PropertyChangedEventHandler handler = this.PropertyChanged;
			if (handler != null) {
				handler (this, new PropertyChangedEventArgs (name));
			}
		}

		private Context ()
		{
			this.sharedUserDefault = new NSUserDefaults (AppExtensionSharedKeys.GROUP_IDENTIFIER, NSUserDefaultsType.SuiteName);
			if (this.isAuthenticated) {

				try {

					string countryStr = null;
					string productStr= null;
					if(USE_FAKE_DATA) {
						countryStr = FakeData.countries;
						productStr =  FakeData.products;
					} else {
						countryStr = sharedUserDefault.StringForKey(AppExtensionSharedKeys.COUNTRIES_KEY);	
						productStr =  sharedUserDefault.StringForKey(AppExtensionSharedKeys.PRODUCTS_KEY);	
					}
					try {
						this.Countries = JsonConvert.DeserializeObject<List<Country>>(countryStr);
						this.Products = JsonConvert.DeserializeObject<List<Product>>(productStr);
					} catch (Exception) {
						
					}

				} catch(Exception e) {
				}
			}
		}

		void ResetSelectedItems() {
			//this._selectedCountry = null;
			//this._selectedProduct = null;
		}

		public void printOldLogs()
		{
			var oldLog = this.getOldLogs();
		}

		public List<Country> Countries { get; set; } = new List<Country>();
		public List<Product> Products { get; set; } = new List<Product>();

		public List<SpendCatcherItem> SpendCatchers { get; set; } = new List<SpendCatcherItem>();	

		public bool readyToUpload {
			get {
				return this.isAuthenticated;// && this.SelectedCountry != null && this.SelectedProduct != null;
			}
		}

		Collection<string> base64Images = new Collection<string>();

		public void PostData() {
			try {
				// https://developer.apple.com/library/ios/documentation/General/Conceptual/ExtensibilityPG/ExtensionScenarios.html#//apple_ref/doc/uid/TP40014214-CH21-SW1
				NSUrlSessionConfiguration config = NSUrlSessionConfiguration.CreateBackgroundSessionConfiguration ("com.mobileexpense.com.backgroundsession");
				config.SharedContainerIdentifier = AppExtensionSharedKeys.GROUP_IDENTIFIER;
				NSUrlSession session = NSUrlSession.FromConfiguration(config, new UploadDelegate(), new NSOperationQueue());

				// https://developer.apple.com/library/ios/documentation/Cocoa/Conceptual/URLLoadingSystem/Articles/UsingNSURLSession.html
				for (int i = 0; i < this.SpendCatchers.Count; i++) {
					NSUrlSessionDataTask dataTask = session.CreateUploadTask(this.SpendCatchers[i].GenerateRequest());
					dataTask.Resume();
				}
					
				this.Reset();

			}  catch(Exception e) {
				this.AppendLogs ("ER " + e.Message+ "\n" +e.StackTrace);
			}
		}

		public string Log = "";
		public void AppendLogs(string msg, bool showMessage = true) {
			this.Log += msg;
			this.NotifyPropertyChanged ("Log");

		}

		public void ResetLogs() {			
			this.sharedUserDefault.SetString ("", "Logs");
			this.sharedUserDefault.Synchronize();
		}

		public string getOldLogs() {
			var oldLog = this.sharedUserDefault.StringForKey ("Logs");
			if (oldLog == null) {
				return "";
			}
			return oldLog;
		}

		public void Reset() {
			this.base64Images.Clear ();
			this.ResetSelectedItems ();
			this.NotifyPropertyChanged ("Reset");
		}

		public class UploadDelegate : NSUrlSessionTaskDelegate
		{


			NSUserDefaults _sharedUserDefault;
			private NSUserDefaults SharedUserDefault {
				get {
					if (this._sharedUserDefault == null) {
						this._sharedUserDefault = new NSUserDefaults (AppExtensionSharedKeys.GROUP_IDENTIFIER, NSUserDefaultsType.SuiteName);			
					}
					return this._sharedUserDefault;
				}
			}

			void appendLog(string msg) {
				
				var ud = this.SharedUserDefault;
				ud.SetString (ud.StringForKey("Logs") + "\n" +msg, "Logs");
				ud.Synchronize ();
				Context.Instance.AppendLogs (msg);
			}

			public override void DidCompleteWithError (NSUrlSession session, NSUrlSessionTask task, NSError error)
			{
				if(error !=  null) {
					appendLog ("DidCompleteWithError TaskId: "+error.ToString ());	
				}

			}

			public override void DidBecomeInvalid (NSUrlSession session, NSError error)
			{
				appendLog ("DidBecomeInvalid" + (error == null ? "undefined" : error.Description));
			}

			public override void DidFinishEventsForBackgroundSession (NSUrlSession session)
			{
				appendLog ("DidFinishEventsForBackgroundSession");
			}

			public override void DidReceiveChallenge (NSUrlSession session, NSUrlAuthenticationChallenge challenge, Action<NSUrlSessionAuthChallengeDisposition, NSUrlCredential> completionHandler)
			{
				appendLog ("Receive challenge...");
			}

			public override void NeedNewBodyStream (NSUrlSession session, NSUrlSessionTask task, Action<NSInputStream> completionHandler)
			{
				appendLog ("NeedNewBodyStream");
			}

			public override void DidSendBodyData (NSUrlSession session, NSUrlSessionTask task, long bytesSent, long totalBytesSent, long totalBytesExpectedToSend)
			{
				appendLog ("DidSendBodyData bSent "+ ((float)totalBytesSent/(float)totalBytesExpectedToSend).ToString());
			}
		}
	}
}

