using System;

using Mxp.Core.Services.Responses;

namespace Mxp.Core.Business
{
	public class VehicleCategory : Model, ComboField.IComboChoice
	{
		public string MileageRateName { get; set;}
		public int Id { get; set;}

		public VehicleCategory (VehicleCategoryResponse vehicleCategoryResponse) {
			this.MileageRateName = vehicleCategoryResponse.MileageRateName;
			this.Id = vehicleCategoryResponse.VehicleCategoryID;
		}

		public string VTitle {
			get {
				return this.MileageRateName ?? String.Empty;
			}
		}

		public int ComboId {
			get {
				return this.Id;
			}
		}
	}
}