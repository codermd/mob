using System;
using Mxp.Core.Services.Responses;
using Mxp.Utils;

namespace Mxp.Core.Business
{
	public partial class TravelCarRental : Model
	{
		public int Id { get; set; }
		public string PickupLabel { get; set; }
		public string PickupCountry { get; set; }
		public DateTime? PickupDate { get; set; }
		public string PickupTime { get; set; }
		public string DropLabel { get; set; }
		public string DropCountry { get; set; }
		public DateTime? DropDate { get; set; }
		public string DropTime { get; set; }
		public string Category { get; set; }
		public string Merchant { get; set; }
		public string Transmission { get; set; }

		public TravelCarRental (TravelCarRentalResponse travelCarRentalResponse) {
			this.Id = travelCarRentalResponse.TravelCarRentalID;

			this.PickupLabel = travelCarRentalResponse.CarRentalPickUpLabel;
			this.PickupCountry = travelCarRentalResponse.TravelCarRentalPickUpCountryName;
			this.PickupTime = travelCarRentalResponse.TravelCarRentalPickupTime;
			this.PickupDate = travelCarRentalResponse.TravelCarRentalPickupDate.ToDateTime();

			this.DropLabel = travelCarRentalResponse.CarRentalDropLabel;
			this.DropCountry = travelCarRentalResponse.TravelCarRentalDropCountryName;
			this.DropTime = travelCarRentalResponse.TravelCarRentalDropTime;
			this.DropDate = travelCarRentalResponse.TravelCarRentalDropDate.ToDateTime ();

			this.Category = travelCarRentalResponse.TravelCarRentalCategory;
			this.Merchant = travelCarRentalResponse.TravelCarRentalMerchant;
			this.Transmission = travelCarRentalResponse.TravelCarRentalTransmission;
		}
	}
}