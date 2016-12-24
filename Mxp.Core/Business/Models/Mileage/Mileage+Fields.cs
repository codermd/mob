using System;
using System.Collections.ObjectModel;
using Mxp.Core.Business;
using System.Collections.Generic;

namespace Mxp.Core.Business
{
	partial class Mileage
	{
		public override Collection<String> GetDynamicFieldsKeys () {
			return new Collection<String> {
				"Infochar1",
				"Infochar2",
				"Infochar3",
				"Infochar4",
				"Infochar5",
				"Infochar6",
				"Infochar7",
				"Infochar8",
				"Infonum1",
				"Infonum2",
			};
		}

		public Dictionary<string, string> FieldMapper = new Dictionary<string, string> () {
			{ "Infochar1", "MILSIinfochar1" },
			{ "Infochar2", "MILSIinfochar2" },
			{ "Infochar3", "MILSIinfochar3" },
			{ "Infochar4", "MILSIinfochar4" },
			{ "Infochar5", "MILSIinfochar5" },
			{ "Infochar6", "MILSIinfochar6" },
			{ "Infochar7", "MILSIinfochar7" },
			{ "Infochar8", "MILSIinfochar8" },
			{ "Infonum1", "MILSIinfonum1" },
			{ "Infonum2", "MILSIinfonum2" },
			{ "MILSIcomment", "MILSIcomment" },
			{ "MILSIprojectId", "MILSIprojectId" },
			{ "MILSIdepartmentId", "MILSIdepartmentId" },
			{ "MILSItrqId", "MILSItrqId" }
		};

		public override Collection<Field> GetMainFields (ExpenseItem expenseItem) {
			Collection<Field> result = new Collection<Field> ();

			if (expenseItem.CanShowPolicyTip)
				result.Add(new ExpenseItemPolicyRule (expenseItem));

			if (CalculatedDistanceC.CanCreate (this))
				result.Add(new CalculatedDistanceC(this));

			if (OdometerFromC.CanCreate (this))
				result.Add(new OdometerFromC(this));

			if (OdometerToC.CanCreate (this))
				result.Add(new OdometerToC(this));

			if (BusinessDistanceC.CanCreate (this))
				result.Add(new BusinessDistanceC(this));

			if (CommutingDistanceC.CanCreate (this))
				result.Add(new CommutingDistanceC(this));

			if (PrivateDistanceC.CanCreate (this))
				result.Add(new PrivateDistanceC(this));

			if (DateC.CanCreate (this))
				result.Add(new DateC(this));

			if (VehicleC.CanCreate (this))
				result.Add(new VehicleC(this));

			if (VehicleCategoryC.CanCreate (this))
				result.Add(new VehicleCategoryC(this));

			return result;
		}
			
		public override Collection<Field> GetStaticFields (ExpenseItem expenseItem) {
			Collection<Field> result = new Collection<Field>();

			if(Preferences.Instance.CanShowPermission(Preferences.Instance.MILSIcomment))
				result.Add (new MileageExpenseItemComment (expenseItem));

			if (Preferences.Instance.CanShowPermission(Preferences.Instance.MILSIprojectId))
				result.Add (new MileageExpenseItemProjectId (expenseItem));

			if (Preferences.Instance.CanShowPermission(Preferences.Instance.MILSIdepartmentId))
				result.Add (new MileageExpenseItemDepartmentId (expenseItem));

			if (Preferences.Instance.CanShowPermission(Preferences.Instance.MILSItrqId))
				result.Add (new MileageExpenseItemTravelRequestId (expenseItem));

			return result;
		}

		public bool CanShowField (string key) {
			return Preferences.Instance.CanShowField (this.FieldMapper[key]);
		}

		public bool IsMandatory (string key) {
			return Preferences.Instance.IsMandatory (this.FieldMapper[key]);
		}
	}
}