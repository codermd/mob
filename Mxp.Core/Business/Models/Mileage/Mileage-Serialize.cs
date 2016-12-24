using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using Mxp.Core.Services.Responses;
using RestSharp.Portable;
using Mxp.Core.Utils;

namespace Mxp.Core.Business
{
	public partial class Mileage
	{
		public void EditCreateSerialize (RestRequest request) {
			if (!this.IsNew)
				request.AddParameter ("transactionId", this.TransactionId.ToString ());

			request.AddParameter ("ItineraryID", this.ItineraryId.ToString ());
			request.AddParameter ("QtyB", this.BusinessDistance.ToString ());
			request.AddParameter ("QtyC", this.CommuteDistance.ToString ());
			request.AddParameter ("QtyP", this.PrivateDistance.ToString ());

			if (this.VehicleCategory != null)
				request.AddParameter ("VehicleCategoryID", this.VehicleCategory.Id.ToString ());

			request.AddParameter ("odometerTo", this.OdometerTo.ToString());
			request.AddParameter ("transactionDate", this.SerizalizeDate(this.Date.Value));
			request.AddParameter ("transactioncomments", this.Comment);

			if (this.Vehicle != null)
				request.AddParameter ("vehicleID", this.Vehicle.Id.ToString ());

			request.AddParameter("fldTransactionFuelMileage", this.FuelMileage.ToString());

			if(Preferences.Instance.CanShowPermission(Preferences.Instance.MILSIcomment))
				request.AddParameter ("comment", this.ExpenseItems[0].GetModelParent<ExpenseItem, Expense> ().Comment);

			if (Preferences.Instance.CanShowPermission(Preferences.Instance.MILSIprojectId))
				request.AddParameter ("projectId", this.ExpenseItems[0].ProjectId);

			if (Preferences.Instance.CanShowPermission(Preferences.Instance.MILSIdepartmentId))
				request.AddParameter ("departmentId", this.ExpenseItems[0].DepartmentId);

			if (Preferences.Instance.CanShowPermission(Preferences.Instance.MILSItrqId))
				request.AddParameter ("travelRequestId", this.ExpenseItems[0].TravelRequestId);

			if (Preferences.Instance.CanShowPermission (Preferences.Instance.MILSIinfochar1))
				request.AddParameter ("ItemInfochar1", this.GetDynamicField ("Infochar1").SerializeValue (this.ExpenseItems [0]));

			if (Preferences.Instance.CanShowPermission (Preferences.Instance.MILSIinfochar2))
				request.AddParameter ("ItemInfochar2", this.GetDynamicField ("Infochar2").SerializeValue (this.ExpenseItems [0]));

			if (Preferences.Instance.CanShowPermission (Preferences.Instance.MILSIinfochar3))
				request.AddParameter ("ItemInfochar3", this.GetDynamicField ("Infochar3").SerializeValue (this.ExpenseItems [0]));

			if (Preferences.Instance.CanShowPermission (Preferences.Instance.MILSIinfochar4))
				request.AddParameter ("ItemInfochar4", this.GetDynamicField ("Infochar4").SerializeValue (this.ExpenseItems [0]));

			if (Preferences.Instance.CanShowPermission (Preferences.Instance.MILSIinfochar5))
				request.AddParameter ("ItemInfochar5", this.GetDynamicField ("Infochar5").SerializeValue (this.ExpenseItems [0]));

			if (Preferences.Instance.CanShowPermission (Preferences.Instance.MILSIinfochar6))
				request.AddParameter ("ItemInfochar6", this.GetDynamicField ("Infochar6").SerializeValue (this.ExpenseItems [0]));

			if (Preferences.Instance.CanShowPermission (Preferences.Instance.MILSIinfochar7))
				request.AddParameter ("ItemInfochar7", this.GetDynamicField ("Infochar7").SerializeValue (this.ExpenseItems [0]));

			if (Preferences.Instance.CanShowPermission (Preferences.Instance.MILSIinfochar8))
				request.AddParameter ("ItemInfochar8", this.GetDynamicField ("Infochar8").SerializeValue (this.ExpenseItems [0]));
			
			if (Preferences.Instance.CanShowPermission (Preferences.Instance.MILSIinfonum1))
				request.AddParameter ("ItemInfoNum1", this.GetDynamicField ("Infonum1").SerializeValue (this.ExpenseItems [0]));

			if (Preferences.Instance.CanShowPermission (Preferences.Instance.MILSIinfonum2))
				request.AddParameter ("ItemInfoNum2", this.GetDynamicField ("Infonum2").SerializeValue (this.ExpenseItems [0]));
		}

		public void SeralizeItinerary (RestRequest request) {
			this.MileageSegments.ForEach ((segment, index) => {
				request.AddParameter ("LocationLatitude" + index, segment.LocationLatitude.ToString ());
				request.AddParameter ("LocationLongitude" + index, segment.LocationLongitude.ToString ());
				request.AddParameter ("LocationName" + index, segment.LocationAliasName);
			});

			request.AddParameter ("ItineraryDistance",this.CalculatedDistance.ToString ());
			request.AddParameter ("nbrLocations", this.MileageSegments.Count.ToString ());
		}
	}
}