using System;
using System.Collections.ObjectModel;
using Mxp.Business;
using System.Collections.Generic;
using Mxp.Core;

namespace Mxp.Business
{
	partial class Mileage
	{
		public override Collection<String> GetDynamicsFieldKeys() {
			return new Collection<String> {
				"Infochar1",
				"Infochar2",
				"Infochar3",
				"Infonum1",
				"Infonum2",
			};
		}

		private Collection<Field> mainFields (ExpenseItem anItem) {
			Collection<Field> result = new Collection<Field> ();

			if (CalculatedDistanceC.CanCreate (this))
				result.Add(new CalculatedDistanceC(this));

			if (BusinessDistanceC.CanCreate (this))
				result.Add(new BusinessDistanceC(this));

			if (CommutingDistanceC.CanCreate (this))
				result.Add(new CommutingDistanceC(this));

			if (PrivateDistanceC.CanCreate (this))
				result.Add(new PrivateDistanceC(this));

			if (OdometerFromC.CanCreate (this))
				result.Add(new OdometerFromC(this));

			if (OdometerToC.CanCreate (this))
				result.Add(new OdometerToC(this));

			if (DateC.CanCreate (this))
				result.Add(new DateC(this));

			if (VehicleC.CanCreate (this))
				result.Add(new VehicleC(this));

			if (VehicleCategoryC.CanCreate (this))
				result.Add(new VehicleCategoryC(this));

			return result;
		}
			
		public override Collection<Field> getStaticFields (ExpenseItem anItem) {
			Collection<Field> result = new Collection<Field>();



			if(CustomerPrefs.Instance.canShowPermission(CustomerPrefs.Instance.MILSIcomment)) {
				result.Add (new MileageExpenseItemComment (anItem));
			}

			if (CustomerPrefs.Instance.canShowPermission(CustomerPrefs.Instance.MILSIprojectId)) {
				result.Add (new MileageExpenseItemProjectId (anItem));
			}


			if (CustomerPrefs.Instance.canShowPermission(CustomerPrefs.Instance.MILSIdepartmentId)) {
				result.Add (new MileageExpenseItemDepartmentId (anItem));
			}

			if (CustomerPrefs.Instance.canShowPermission(CustomerPrefs.Instance.MILSItrqId)) {
				result.Add (new MileageExpenseItemTravelRequestID (anItem));
			}

			return result;
		}

		public override bool canShowField(string key){
			if(key.Equals("Infochar1")){
				return CustomerPrefs.Instance.canShowPermission (CustomerPrefs.Instance.MILSIinfochar1);
			} 
			if(key.Equals("Infochar2")){
				return CustomerPrefs.Instance.canShowPermission (CustomerPrefs.Instance.MILSIinfochar2);
			} 
			if(key.Equals("Infochar3")){
				return CustomerPrefs.Instance.canShowPermission (CustomerPrefs.Instance.MILSIinfochar3);
			} 
			if(key.Equals("Infonum1")){
				return CustomerPrefs.Instance.canShowPermission (CustomerPrefs.Instance.MILSIinfonum1);
			} 
			if(key.Equals("Infonum2")){
				return CustomerPrefs.Instance.canShowPermission (CustomerPrefs.Instance.MILSIinfonum2);
			} 
			if(key.Equals("MILSIcomment")){
				 return CustomerPrefs.Instance.canShowPermission (CustomerPrefs.Instance.MILSIcomment);
			} 
			if(key.Equals("MILSIprojectId")){
				 return CustomerPrefs.Instance.canShowPermission (CustomerPrefs.Instance.MILSIprojectId);
			} 
			if(key.Equals("MILSIdepartmentId")){
				 return CustomerPrefs.Instance.canShowPermission (CustomerPrefs.Instance.MILSIdepartmentId);
			} 
			if(key.Equals("MILSItrqId")){
				 return CustomerPrefs.Instance.canShowPermission (CustomerPrefs.Instance.MILSItrqId);
			} 
			return false;
		}
	}
}