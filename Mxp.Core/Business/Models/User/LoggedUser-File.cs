using System;
using System.Collections.ObjectModel;
using Mxp.Core.Business;
using System.Collections.Generic;
using Mxp.Core.Services.Responses;
using Mxp.Core.Services;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace Mxp.Core.Business
{
	public partial class LoggedUser 
	{
		public string SerializeStringFormat (bool complete = true) {
			return JsonConvert.SerializeObject (this.SerializeFileFormat (complete));
		}

		public void UnserializeStringFormat (string loggedUserStr) {
			if (loggedUserStr == null) 
				return;

			Dictionary<string, object> res = JsonConvert.DeserializeObject<Dictionary<string, object>> (loggedUserStr);
			this.UnserializeFileFormat (res);
		}

		private Dictionary<string, object> SerializeFileFormat (bool complete) {
			Dictionary<string, object> result = new Dictionary<string, object> ();

			result ["Username"] = LoggedUser.Instance.UserAPI.ApiForCredential () + this.Username;
			result ["Email"] = LoggedUser.Instance.UserAPI.ApiForCredential () + this.Email;

			result ["Token"] = this.Token;

			if (complete) {
				result ["AutoLogin"] = this.AutoLogin;

				result ["Preferences"] = JsonConvert.SerializeObject (this.Preferences.rawResponse);
				result ["Countries"] = JsonConvert.SerializeObject (this.Countries.rawResponses);
				result ["Currencies"] = JsonConvert.SerializeObject (this.Currencies.rawResponses);
				result ["Products"] = JsonConvert.SerializeObject (this.Products.rawResponses);
				result ["Labels"] = JsonConvert.SerializeObject (this.Labels.rawResponses);
				result ["VehicleCategories"] = JsonConvert.SerializeObject (this.VehicleCategories.rawResponses);
				result ["FavouriteJourneys"] = JsonConvert.SerializeObject (this.FavouriteJourneys.rawResponses);
			}

			return result;
		}

		private void UnserializeFileFormat (Dictionary<string, object> dict) {
			try {
				this.Username = dict ["Username"] as string;
				this.Email = dict ["Email"] as string;

				this.Token = dict ["Token"] as string;

				this.AutoLogin = (bool) dict ["AutoLogin"];

				this.Preferences.Populate (JsonConvert.DeserializeObject<PreferencesResponse> (dict ["Preferences"] as string));
				this.Countries.Populate (JsonConvert.DeserializeObject<IEnumerable<CountryResponse>> (dict ["Countries"] as string));
				this.Currencies.Populate (JsonConvert.DeserializeObject<IEnumerable<CurrencyResponse>> (dict["Currencies"] as string));
				this.Products.Populate (JsonConvert.DeserializeObject<IEnumerable<ProductResponse>> (dict["Products"] as string));
				this.Labels.Populate (JsonConvert.DeserializeObject<IEnumerable<LabelResponse>> (dict["Labels"] as string));
				this.VehicleCategories.Populate (JsonConvert.DeserializeObject<IEnumerable<VehicleCategoryResponse>> (dict["VehicleCategories"] as string));
				this.FavouriteJourneys.Populate (JsonConvert.DeserializeObject<IEnumerable<JourneyResponse>> (dict["FavouriteJourneys"] as string));
			} catch (Exception) {
				return;
			}
		}
	}
}