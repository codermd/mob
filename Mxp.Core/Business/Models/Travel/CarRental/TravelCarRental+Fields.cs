using System;
using System.Collections.ObjectModel;

namespace Mxp.Core.Business
{
	public partial class TravelCarRental
	{
		public Collection<Field> GetMainFields(){
			return new Collection<Field> {
				new CarRentalCategory (this),
				new CarRentalMerchant (this),
				new CarRentalGearBox (this),
				new CarRentalPickUpStation (this),
				new CarRentalPickUpCountry (this),
				new CarRentalPickUpDate (this),
				new CarRentalDropLocation (this),
				new CarRentalDropCountry (this),
				new CarRentalDropDate (this),
				new CarRentalDropTime (this)
			};
		}
	}
}