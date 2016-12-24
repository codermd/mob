using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mxp.Core.Business;
using Mxp.Core.Helpers;
using Mxp.Core.Services.Google;
using Mxp.Core.Utils;
using Org.BouncyCastle.Crypto.Digests;
using RestSharp.Portable;
using System.Threading;

namespace Mxp.Core.Services
{
	public static class GoogleServiceExtensions
	{
		public static string GetRoute (this GoogleService.ApiEnum route) {
			switch (route) {
				case GoogleService.ApiEnum.GooglePlacesAutocomplete:
					return "place/autocomplete/json";
				case GoogleService.ApiEnum.GooglePlacesDetails:
					return "place/details/json";
				case GoogleService.ApiEnum.GoogleDirections:
					return "directions/json";
				case GoogleService.ApiEnum.GoogleGeocoding:
					return "geocode/json";
				default:
					return null;
			}
		}
	}

	public class GoogleService : Service
	{
		public static readonly GoogleService Instance = new GoogleService ();

		public const string GoogleAPIBaseUrl = "https://maps.googleapis.com/maps/api/";
		public const string GooglePremiumKey = "Xgh9uiRElkYVZTaFQesR1dw5Bo0=";
		public const string GoogleServerKey = "AIzaSyC2n3K674FMdOfCZSMOSbAxj2GdHIpZzIA";
		public const string ClientId = "gme-mobilexpensenv";

		public enum ApiEnum {
			GooglePlacesAutocomplete,
			GooglePlacesDetails,
			GoogleDirections,
			GoogleGeocoding
		}

		public enum PlaceTypeEnum {
			All,
			Merchant
		}

		public async Task<Directions> FetchDirectionsAsync (MileageSegments segments, CancellationToken token) {
			RestRequest request = new RestRequest (ApiEnum.GoogleDirections.GetRoute ());

			request.AddQueryParameter ("origin", segments [0].Coordinate);
			request.AddQueryParameter ("destination", segments [segments.Count - 1].Coordinate);

			if (segments.Count > 2) {
				string[] waypoints = new String[segments.Count - 2];

				for (int i = 1; i < segments.Count - 1; i++)
					waypoints[i - 1] = segments [i].Coordinate;

				request.AddQueryParameter ("waypoints", String.Join ("|", waypoints));
			}

			request.AddQueryParameter ("units", Preferences.Instance.MilUnit.ToString ().ToLower ());

			this.ConfigureGoogleRequest (request);

			return await this.ExecuteBaseAsync<Directions> (GoogleAPIBaseUrl, request, token);
		}

		private async Task<Predictions> FetchAllPlacesAsync (RestRequest request, Country country = null) {
			if (country != null)
				request.AddQueryParameter ("components", "country:" + country.IsoName);
			
			return await this.ExecuteBaseAsync<Predictions> (GoogleAPIBaseUrl, request);
		}

		private async Task<Predictions> FetchMerchantPlacesAsync (RestRequest request, string search, Country country = null) {
			request.AddQueryParameter ("types", "establishment");

			if (country != null)
				request.AddQueryParameter ("components", "country:" + country.IsoName);

			Predictions predictions = await this.ExecuteBaseAsync<Predictions> (GoogleAPIBaseUrl, request);

			Prediction prediction = new Prediction { description = search };
			if (!predictions.Contains  (prediction))
				predictions.AddDefault (prediction);

			return predictions;
		}

		public async Task<Predictions> FetchPlacesLocationsAsync (string search, PlaceTypeEnum requestType = PlaceTypeEnum.All, Country country = null) {
			RestRequest request = new RestRequest (ApiEnum.GooglePlacesAutocomplete.GetRoute ());

			request.AddQueryParameter ("input", search);
			request.AddQueryParameter ("key", GoogleServerKey);

			if (country != null)
				country = await country.GetMasterCountryAsync ();

			switch (requestType) {
				case PlaceTypeEnum.All:
					return await this.FetchAllPlacesAsync (request, country);
				case PlaceTypeEnum.Merchant:
					return await this.FetchMerchantPlacesAsync (request, search, country);
			}

			return null;
		}

		public async Task<bool> FetchGeocodingLocationAsync (MileageSegment segment) {
			RestRequest request = new RestRequest (ApiEnum.GoogleGeocoding.GetRoute ());

			request.AddQueryParameter ("address", segment.LocationAliasName);

			this.ConfigureGoogleRequest (request);

			Addresses addresses = await this.ExecuteBaseAsync<Addresses> (GoogleAPIBaseUrl, request);

			if (addresses == null)
				return false;

			segment.LocationLatitude = addresses.results [0].geometry.location.lat;
			segment.LocationLongitude = addresses.results [0].geometry.location.lng;

			return true;
		}

		public async Task FetchPlaceLocationAsync (MileageSegment segment, string placeId) {
			PlaceDetails place = await this.FetchPlaceLocationAsync (placeId);

			segment.LocationLatitude = place.result.geometry.location.lat;
			segment.LocationLongitude = place.result.geometry.location.lng;
		}

		public async Task<PlaceDetails> FetchPlaceLocationAsync (string placeId) {
			RestRequest request = new RestRequest (ApiEnum.GooglePlacesDetails.GetRoute ());

			request.AddQueryParameter ("placeid", placeId);
			request.AddQueryParameter ("key", GoogleServerKey);

			 return await this.ExecuteBaseAsync<PlaceDetails> (GoogleAPIBaseUrl, request);
		}

		public async Task GetLocationNameAsync (MileageSegment segment) {
			if (!segment.IsLocationValid)
				return;

			RestRequest request = new RestRequest (ApiEnum.GoogleGeocoding.GetRoute ());

			request.AddQueryParameter ("latlng", segment.Coordinate);

			this.ConfigureGoogleRequest (request);

			Addresses addresses = await this.ExecuteBaseAsync<Addresses> (GoogleAPIBaseUrl, request);

			if (addresses.results.Count == 0)
				throw new ValidationError ("Error", Labels.GetLoggedUserLabel (Labels.LabelEnum.FetchPlaceError));

			segment.LocationAliasName = addresses.results [0].formatted_address;
		}

		// https://developers.google.com/maps/premium/previous-licenses/webservices/auth
		// https://m4b-url-signer.appspot.com/
		private void ConfigureGoogleRequest (RestRequest request) {
			request.AddQueryParameter ("client", ClientId);

			// converting key to bytes will throw an exception, need to replace '-' and '_' characters first.
			string usablePrivateKey = GooglePremiumKey.Replace ("-", "+").Replace ("_", "/");
			byte [] privateKeyBytes = Convert.FromBase64String (usablePrivateKey);

			UriBuilder uri = new UriBuilder (GoogleAPIBaseUrl);
			uri.Path += request.Resource;

			string[] array = request.Parameters.Select (parameter => String.Format ("{0}={1}", parameter.Name, Uri.EscapeDataString ((string)parameter.Value))).ToArray ();
			uri.Query = String.Join ("&", array);

			// Exemple URL Portion to Sign: /maps/api/#api_name#/json?#param#=#value#&client=#clientID#
			byte [] encodedPathAndQueryBytes = new UTF8Encoding ().GetBytes (uri.Path + uri.Query);

			// compute the hash
		    byte[] hash = CryptoHelper.GetMacAlgorithmHash (new Sha1Digest (), privateKeyBytes, encodedPathAndQueryBytes);

			// convert the bytes to string and make url-safe by replacing '+' and '/' characters
			string signature = Convert.ToBase64String (hash).Replace ("+", "-").Replace ("/", "_");

			// Add the signature to the existing URI.
			request.AddQueryParameter ("signature", signature);
		}
	}
}
