using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Mxp.Core.Utils;

namespace Mxp.Core.Business
{
	public partial class Expense
	{
		public virtual Collection<String> GetDynamicFieldsKeys () {
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
				"Infonum2"
			};
		}

		public bool CanShowCreditCard {
			get {
				return (Preferences.Instance.ExpMobileCCMatch.Equals ("M")
				|| Preferences.Instance.ExpMobileCCMatch.Equals ("MP"))
				&& this.IsNew;
			}
		}
	
		public virtual Collection<Field> GetMainFields (ExpenseItem expenseItem) {
			Collection<Field> result = new Collection<Field>();

			result.Add (new ExpenseItemDate (expenseItem));
			result.Add (new ExpenseItemCountry (expenseItem));
			result.Add (new ExpenseItemCategory (expenseItem));
			result.Add (new ExpenseItemAmount (expenseItem));

			if (Preferences.Instance.ITEShowReceiptTickbox)
				result.Add (new ExpenseItemReceiptTickBox (expenseItem));

			return result;
		}

		public virtual Collection<Field> GetAllFields (ExpenseItem expenseItem) {
			Collection<Field> result = new Collection<Field>();

			this.GetStaticFields (expenseItem).ForEach (field => result.Add (field));

			this.GetDynamicField (expenseItem).ForEach (field => result.Add (field));

			return result;
		}

		public virtual Collection<Field> GetStaticFields (ExpenseItem expenseItem) {
			Collection<Field> result = new Collection<Field>();

			if (this.CanShowCreditCard)
				result.Add (new ExpenseItemCreditCard (expenseItem));

			if (expenseItem.Product.CanShowPermission (expenseItem.Product.Comment))
				result.Add (new ExpenseItemComment (expenseItem));
			
			if (expenseItem.IsTransactionByCard || expenseItem.IsTransactionBySpendCatcher || expenseItem.Product.CanShowPermission(expenseItem.Product.MerchantName))
				result.Add (new ExpenseItemMerchantName (expenseItem));

			if (expenseItem.IsTransactionByCard || expenseItem.IsTransactionBySpendCatcher || expenseItem.Product.CanShowPermission(expenseItem.Product.MerchantCity))
				result.Add (new ExpenseItemMerchantCity (expenseItem));

			if (expenseItem.Product.CanShowPermission(expenseItem.Product.InvoiceId))
				result.Add (new ExpenseItemInvoiceId (expenseItem));

			if (expenseItem.Product.CanShowPermission(expenseItem.Product.ProjectId))
				result.Add (new ExpenseItemProjectId (expenseItem));

			if (expenseItem.Product.CanShowPermission(expenseItem.Product.DepartmentId))
				result.Add (new ExpenseItemDepartmentId (expenseItem));

			if (expenseItem.Product.CanShowPermission(expenseItem.Product.TravelRequestId))
				result.Add(new ExpenseItemTravelRequestId(expenseItem));

			if (expenseItem.Product.CanShowPermission(expenseItem.Product.HotelLine)) {
				result.Add (new ExpenseItemHotelLineFrom (expenseItem));
				result.Add (new ExpenseItemHotelLineTo (expenseItem));
			}

			if (expenseItem.Product.CanShowPermission(expenseItem.Product.Allowanceline)) {
				result.Add (new ExpenseItemAllowancelineFrom (expenseItem));
				result.Add (new ExpenseItemAllowancelineTo (expenseItem));
			}

			if (expenseItem.Product.CanShowPermission(expenseItem.Product.VehicleNumber))
				result.Add(new ExpenseItemVehicleNumber(expenseItem));

			if (expenseItem.Product.CanShowPermission (expenseItem.Product.VehicleOdometer))
				result.Add (new ExpenseItemFuelMileage (expenseItem));

			return result;
		}

		public Collection<Field> GetDynamicField (ExpenseItem expenseItem) {
			return expenseItem.DynamicFields;
		}
			
		public DynamicFieldHolder GetDynamicField (string key) {
			return Preferences.Instance.DynamicFields.GetFieldWithKey (key, DynamicFieldHolder.LocationEnum.Item);
		}

		public virtual PermissionEnum GetPermissionForKey(String fieldName) {
			return this.Product.GetPropertyValue<PermissionEnum> (fieldName);
		}
	}
}