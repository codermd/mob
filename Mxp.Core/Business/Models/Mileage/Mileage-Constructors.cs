using Mxp.Core.Services.Responses;
using Mxp.Core.Utils;
using System.Linq;
using System.ComponentModel;

namespace Mxp.Core.Business
{
	public partial class Mileage
	{
		private Mileage () {
			this.Vehicles = new Vehicles (this);
			this.MileageSegments = new MileageSegments (this);

			this.ComputeDistances ();

			this.ResetChanged ();
		}

		public Mileage (ExpenseResponse expenseResponse) : this () {
			this.Populate (expenseResponse);
		}
	
		public static new Mileage NewInstance () {
			Mileage mileage = new Mileage ();
			mileage.ExpenseItems.AddItem (new ExpenseItem ());
			return mileage;
		}

		public void SetJourney (Journey journey) {
			this.MileageSegments.Clear ();

			this.ItineraryId = journey.Itinerary.Id;

			this.MileageSegments.ReplaceWith (journey.Itinerary.MileageSegments);
		}

		public override void Populate (ExpenseResponse expenseResponse) {
			base.Populate (expenseResponse);

			this.ItineraryId = expenseResponse.fldItineraryID;

			this.Vehicles.Populate (expenseResponse.availableVehicles);
			this.Vehicle = this.Vehicles.Single (vehicle => vehicle.Id == expenseResponse.fldVehicleID);
			this.Vehicle.CategoryId = expenseResponse.fldVehicleCategoryID;

			this.ComputeDistances ();

			this.ResetChanged ();
		}

		public void Populate (ItineraryResponse response) {
			// Absolutely needed when creating mileage
			this.ItineraryId = response.ItineraryId;

			this.MileageSegments.Populate (response.segments);

			this.ResetChanged ();
		}

		private void ComputeDistances () {
			this.ExpenseItems.ForEach (expenseItem => {
				if (expenseItem.ProductId == 1900)
					this._businessDistance = expenseItem.Quantity;
				else if (expenseItem.ProductId == 4525 || expenseItem.ProductId == 190100)
					this._commuteDistance = expenseItem.Quantity;
				else if (expenseItem.ProductId == 4524 || expenseItem.ProductId == 190200)
					this._privateDistance = expenseItem.Quantity;
			});

			if (this.Vehicle != null)
				this.OdometerFrom = this.Vehicle.LastMileage;
		}
	}
}