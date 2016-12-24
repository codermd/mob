using System;

namespace Mxp.Core.Services.Responses
{
	public class TravelCarRentalResponse : Response
	{
		public int TravelCarRentalID { get; set; }
		public string CarRentalPickUp { get; set; }
		public string CarRentalPickUpLabel { get; set; }
		public string TravelCarRentalPickUpCountryName { get; set; }
		public string TravelCarRentalPickupCountryIsoName { get; set; }
		public string TravelCarRentalPickupDate { get; set; }
		public string TravelCarRentalPickupTime { get; set; }
		public string CarRentalDrop { get; set; }
		public string CarRentalDropLabel { get; set; }
		public string TravelCarRentalDropCountryName { get; set; }
		public string TravelCarRentalDropCountryIsoName { get; set; }
		public string TravelCarRentalDropDate { get; set; }
		public string TravelCarRentalDropTime { get; set; }
		public string TravelCarRentalCategory { get; set; }
		public string TravelCarRentalMerchant { get; set; }
		public string TravelCarRentalTransmission { get; set; }
		public int Merchantid { get; set; }
		public int TravelCarRentalPaymentMethodId { get; set; }
		public double TravelCarRentalAmount { get; set; }
		public int TravelCarRentalPool { get; set; }
		public string TravelCarRentalPurpose { get; set; }
		public int TravelCarRentalBookingTypeId { get; set; }
		public bool TravelCarRentalInformTravelAgent { get; set; }
		public int TravelCarRentalPickUpCountryId { get; set; }
		public int TravelCarRentalDropCountryId { get; set; }
		public string MerchantName { get; set; }

		public TravelCarRentalResponse () {}
	}
}