using System;
using Foundation;
using System.Collections.Generic;
using Mxp.Core.Business;
using Newtonsoft.Json;
using Mxp.Core.Utils;
using Mxp.Core;
using Mxp.Core.Services;
using System.Diagnostics;

namespace mxp
{
	public class SharedDataController
	{
		public SharedDataController ()
		{
		}



		public void Remove(){
			
			this.SharedDefault.RemoveObject (AppExtensionSharedKeys.COUNTRIES_KEY);
			this.SharedDefault.RemoveObject (AppExtensionSharedKeys.PRODUCTS_KEY);
			this.SharedDefault.RemoveObject (AppExtensionSharedKeys.TOKEN_KEY);
			this.SharedDefault.RemoveObject (AppExtensionSharedKeys.ACCESSIBILITY_KEY);
			this.SharedDefault.RemoveObject (AppExtensionSharedKeys.AUTHENTICATED_KEY);
			this.SharedDefault.RemoveObject (AppExtensionSharedKeys.POST_URL_KEY);
			this.SharedDefault.RemoveObject (AppExtensionSharedKeys.VERSION_KEY);

			this.saveAsAuthenticated (false);
			this.SharedDefault.Synchronize ();
		}

		public void Save() {


			this.saveAsAuthenticated (true);

			this.SaveExtensionAccessibility ();

			if (!Preferences.Instance.IsSpendCatcherEnable) {
				return;
			}

			this.SaveCountries ();
			this.SaveProducts ();
			this.SaveToken ();
			this.SaveVersion ();
			this.SaveAPIUrl ();
			this.SaveLabels ();
			this.SharedDefault.Synchronize ();
		}
			
		NSUserDefaults _sharedDefault;
		NSUserDefaults SharedDefault {
			get {
				if (this._sharedDefault == null) {
					this._sharedDefault = new NSUserDefaults (AppExtensionSharedKeys.GROUP_IDENTIFIER, NSUserDefaultsType.SuiteName);
				}
				return this._sharedDefault;
			}
		}

		private void SaveCountries (){
			List<Dictionary<string, string>> countriesStr = new List<Dictionary<string, string>>();
			LoggedUser.Instance.Countries.ForEach(country => {
				countriesStr.Add(new Dictionary<string, string>(){
					{"Name", country.VName},
					{"Id", country.Id.ToString()},
					{"Recent", country.Recent.ToString()},
				});
			});
			var cStr = JsonConvert.SerializeObject (countriesStr);
			this.SharedDefault.SetString (cStr, AppExtensionSharedKeys.COUNTRIES_KEY);
		}

		private void SaveProducts() {
			List<Dictionary<string, string>> products = new List<Dictionary<string, string>>();

			LoggedUser.Instance.Products.ExpenseProducts.ForEach(product => {
				products.Add(new Dictionary<string, string>(){
					{"Id", product.Id.ToString()},
					{"Name", product.ExpenseCategory.Name},
				});
			});

			var pStr = JsonConvert.SerializeObject (products);
			this.SharedDefault.SetString (pStr, AppExtensionSharedKeys.PRODUCTS_KEY);
		}


		public void SaveAPIUrl() {
			Uri uri = new Uri (new Uri (Service.ApiUrl), SpendCatcherService.ApiEnum.Send.GetRoute ());
			this.SharedDefault.SetString (uri.ToString (), AppExtensionSharedKeys.POST_URL_KEY);
			Debug.WriteLine ("API-->");
			Debug.WriteLine (uri.ToString ());
		}

		Dictionary<int, string> labelsToSave(List<int> labelsId) {
			Dictionary<int, string> labels = new Dictionary<int, string> ();
			labelsId.ForEach (id => labels.Add (id, Labels.Instance.GetLabel (id)));
			return labels;
		}

		public void SaveLabels() {
			List<int> labelsId = new List<int> ();
			labelsId.Add (1090);
			labelsId.Add (4607);
			labelsId.Add (4612);
			labelsId.Add (4613);
			labelsId.Add (4614);
			labelsId.Add (4615);
			labelsId.Add (4616);
			labelsId.Add (3678);
			labelsId.Add (39);
			labelsId.Add (42);
			labelsId.Add (2933);
			labelsId.Add (386);
			labelsId.Add (377);

			var pStr = JsonConvert.SerializeObject (labelsToSave (labelsId));
			this.SharedDefault.SetString (pStr, AppExtensionSharedKeys.LABELS);
		}


		public void SaveToken() {
			this.SharedDefault.SetString (LoggedUser.Instance.Token, AppExtensionSharedKeys.TOKEN_KEY);
		}

		public void SaveVersion() {
			this.SharedDefault.SetString (Service.AppVersion, AppExtensionSharedKeys.VERSION_KEY);
		}

		public void SaveExtensionAccessibility() {
			this.SharedDefault.SetBool (Preferences.Instance.IsSpendCatcherEnable, AppExtensionSharedKeys.ACCESSIBILITY_KEY);
		}
			
		public void saveAsAuthenticated(bool authenticated) {
			this.SharedDefault.SetBool (authenticated, AppExtensionSharedKeys.AUTHENTICATED_KEY);
		}

	}
}

