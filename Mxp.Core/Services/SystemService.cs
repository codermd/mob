using System;
using System.Threading.Tasks;
using Mxp.Core.Business;
using RestSharp.Portable;
using Mxp.Core.Services.Responses;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Mxp.Core.Services
{
	public static class SystemServiceExtensions {
		public static string GetRoute (this SystemService.ApiEnum route) {
			switch (route) {
				case SystemService.ApiEnum.GetUserLabels:
					return "APISystem/getUserLabels";
				case SystemService.ApiEnum.GetVATrates:
					return "APISystem/getVATrates";
				case SystemService.ApiEnum.GetCountries:
					return "APISystem/getCountries";
				case SystemService.ApiEnum.GetCurrencies:
					return "APISystem/getCurrenciesList";
				case SystemService.ApiEnum.GetProducts:
					return "APISystem/getProducts";
				case SystemService.ApiEnum.GetPreferences:
					return "APISystem/getCustomerPrefs";
				case SystemService.ApiEnum.GetOpenObject:
					return "APISystem/openObject";
				case SystemService.ApiEnum.LogError:
					return "APISystem/logError";
				case SystemService.ApiEnum.GetCountry:
					return "APISystem/getCountry";
				default:
					return null;
			}
		}
	}

	public class SystemService : Service
	{
		public static readonly SystemService Instance = new SystemService ();

		public enum ApiEnum {
			GetUserLabels,
			GetVATrates,
			GetCountries,
			GetCurrencies,
			GetProducts,
			GetPreferences,
			GetOpenObject,
			LogError,
			GetCountry
		}

		private SystemService () : base () {

		}

		public async Task<PreferencesResponse> FetchPreferencesAsync () {
			RestRequest request = new RestRequest(ApiEnum.GetPreferences.GetRoute ());
			return await this.ExecuteAsync<PreferencesResponse> (request);
		}

		//TODO Replace FetchGenericAsync
		public async Task FetchProductsAsync (Products products) {
			RestRequest request = new RestRequest(ApiEnum.GetProducts.GetRoute ());

			List<ProductResponse> productsResponses = await this.ExecuteAsync<List<ProductResponse>> (request);

			Debug.WriteLine ("{0} productsResponses", productsResponses.Count);

			products.Populate (productsResponses);
		}

		public async Task FetchProductsAsync (Products products, Product product) {
			RestRequest request = new RestRequest (ApiEnum.GetProducts.GetRoute ());

			request.AddParameter ("fromCategory", product.ExpenseCategory.Id.ToString ());

			List<ProductResponse> productsResponses = await this.ExecuteAsync<List<ProductResponse>> (request);

			Debug.WriteLine ("{0} productsResponses", productsResponses.Count);

			products.Populate (productsResponses);
		}
			
		//TODO Replace FetchGenericAsync
		public async Task FetchCountriesAsync(Countries countries) {
			RestRequest request = new RestRequest(ApiEnum.GetCountries.GetRoute ());

			List<CountryResponse> countryResponses = await this.ExecuteAsync<List<CountryResponse>> (request);

			Debug.WriteLine ("{0} countryResponses", countryResponses.Count);

			countries.Populate (countryResponses);
		}

		//TODO Replace FetchGenericAsync
		public async Task FetchCurrenciesAsync(Currencies currencies) {
			RestRequest request = new RestRequest(ApiEnum.GetCurrencies.GetRoute ());

			List<CurrencyResponse> currencyResponses = await this.ExecuteAsync<List<CurrencyResponse>> (request);

			Debug.WriteLine ("{0} currencyResponses", currencyResponses.Count);

			currencies.Populate (currencyResponses);

			//TODO Replace with CompareTo
			currencies.ReplaceWith (currencies.OrderBy (currency => currency.Name).ToList ());
		}

		//TODO Replace FetchGenericAsync
		public async Task FetchLabelsAsync(Labels labels) {
			RestRequest request = new RestRequest(ApiEnum.GetUserLabels.GetRoute ());

			List<LabelResponse> labelResponses = await this.ExecuteAsync<List<LabelResponse>> (request);

			Debug.WriteLine ("{0} labelResponses", labelResponses.Count);

			labels.Populate (labelResponses);
		}

		public async Task<MetaOpenObject> FetchOpenObjectAsync (OpenObject openObject) {
			RestRequest request = new RestRequest (ApiEnum.GetOpenObject.GetRoute ());

			openObject.Serialize (request);

			return new MetaOpenObject (await this.ExecuteAsync<OpenObjectResponse> (request));
		}

		public async Task LogExceptionAsync (Exception exception) {
			RestRequest request = new RestRequest(ApiEnum.LogError.GetRoute ());

			request.AddParameter ("pageDetails", LoggedUser.Instance.TrackContext.serializeForServerException ());
			request.AddParameter ("errorDescription", exception.Message);
			request.AddParameter ("errorContext", exception.StackTrace);	

			await this.ExecuteAsync (request);
		}

		public async Task<Country> FetchCountryAsync (int id, bool parent = false) {
			RestRequest request = new RestRequest (ApiEnum.GetCountry.GetRoute ());

			request.AddParameter ("countryID", id);
			request.AddParameter ("parent", parent);

			return new Country (await this.ExecuteAsync<CountryResponse> (request));
		}
	}
}