using System;
using System.Threading.Tasks;
using RestSharp.Portable;
using Mxp.Core.Services.Responses;
using Mxp.Core.Business;
using System.Collections.Generic;

namespace Mxp.Core.Services
{
	public static class MileageServiceExtensions {
		public static string GetRoute (this MileageService.ApiEnum route) {
			switch (route) {
				case MileageService.ApiEnum.GetItinerary:
					return "APIMileage/getItinerary";
				case MileageService.ApiEnum.AddItinerary:
					return "APIMileage/AddItinerary";
				case MileageService.ApiEnum.AddMileage:
					return "APIMileage/AddMileage";

				case MileageService.ApiEnum.GetFavouriteItineraries:
					return "APIMileage/GetFavouriteItinerariesFull";
				case MileageService.ApiEnum.GetFavouriteSegments:
					return "APIMileage/GetFavouriteSegments";

				case MileageService.ApiEnum.EditMileage:
					return "APIMileage/EditMileage";
				case MileageService.ApiEnum.DeleteMileage:
					return "APIMileage/DeleteMileage";

				case MileageService.ApiEnum.GetVehicles:
					return "APIMileage/getVehicles";
				case MileageService.ApiEnum.GetVehicleCategories:
					return "APIMileage/getVehicleCategories";
				default:
					return null;
			}
		}
	}

	public class MileageService : Service
	{
		public static readonly MileageService Instance = new MileageService ();

		public enum ApiEnum {

			GetItinerary,
			AddItinerary,

			GetFavouriteItineraries,
			GetFavouriteSegments,

			AddMileage,
			EditMileage,
			DeleteMileage,

			GetVehicles,
			GetVehicleCategories,
		}

		private MileageService () : base () {

		}

		public async Task FetchMileageSegmentsAsync (MileageSegments mileageSegments) {
			RestRequest request = new RestRequest(ApiEnum.GetItinerary.GetRoute ());

			request.AddParameter("itineraryID", mileageSegments.GetParentModel<Mileage> ().ItineraryId.ToString());

			ItineraryResponse itineraryResponse = await this.ExecuteAsync<ItineraryResponse> (request);

			mileageSegments.GetParentModel<Mileage> ().Populate (itineraryResponse);
		}

		

		public async Task AddItineraryAsync (Mileage mileage) {
			RestRequest request = new RestRequest (ApiEnum.AddItinerary.GetRoute ());

			mileage.SeralizeItinerary (request);

			ItineraryResponse response = await this.ExecuteAsync<ItineraryResponse> (request);

			mileage.Populate (response);
		}

		public async Task SaveMileageAsync (Mileage mileage) {
			RestRequest request = new RestRequest (mileage.IsNew ? ApiEnum.AddMileage.GetRoute () : ApiEnum.EditMileage.GetRoute ());

			mileage.EditCreateSerialize (request);

			await this.ExecuteAsync (request);
		}

		public async Task GetFavouriteJourneys (Journeys journeys) {
			RestRequest request = new RestRequest (ApiEnum.GetFavouriteItineraries.GetRoute ());

			List<JourneyResponse> journeysResponse = await this.ExecuteAsync<List<JourneyResponse>> (request);

			journeys.Populate (journeysResponse);
		}

		public async Task GetFavouriteLocations (MileageSegments mileageSegments, string search) {
			RestRequest request = new RestRequest (ApiEnum.GetFavouriteSegments.GetRoute ());

			request.AddParameter ("search", search);

			List<MileageSegmentResponse> mileageSegmentsResponse = await this.ExecuteAsync<List<MileageSegmentResponse>> (request);
		
			mileageSegments.Populate (mileageSegmentsResponse);
		}

		public async Task FetchVehicles (Vehicles vehicles, DateTime date) {
			RestRequest request = new RestRequest(ApiEnum.GetVehicles.GetRoute ());

			request.AddParameter("date", date.ToString (@"dd\/MM\/yyyy"));

			List<VehicleResponse> vehiclesResponse = await this.ExecuteAsync<List<VehicleResponse>> (request);

			vehicles.Populate (vehiclesResponse);
		}
	}
}