using System;
using RestSharp.Portable;
using Mxp.Core.Utils;

namespace Mxp.Core.Business
{
	public partial class Expense
	{
		public virtual void EditCreateSerialize (RestRequest request, ExpenseItem expenseItem) {
			request.AddParameter("itemId", expenseItem.Id);
			request.AddParameter("ItemProductQuantity", expenseItem.Quantity);
			request.AddParameter("ItemGrossAmountLC", expenseItem.AmountLC);

			request.AddParameter("CountryID", this.Country.Id);
			request.AddParameter("productID", expenseItem.Product.Id);
			request.AddParameter("CurrencyID", this.Currency.Id);

			request.AddParameter ("TransactionDate", this.SerizalizeDate (this.Date.Value));// this.Date.Value.ToString("dd/MM/yyyy"));
			request.AddParameter("transactionComments", this.Comment);

			if (expenseItem.IsTransactionByCard || expenseItem.IsTransactionBySpendCatcher || expenseItem.Product.CanShowPermission (expenseItem.Product.MerchantName)) {
				if (String.IsNullOrEmpty (this.GooglePlacesId))
					request.AddParameter ("merchantName", this.MerchantName);
				else {
					request.AddParameter ("merchantNameSearch", this.MerchantName);
					request.AddParameter ("GooglePlaceId", this.GooglePlacesId);
				}
			}

			if (expenseItem.IsTransactionByCard || expenseItem.IsTransactionBySpendCatcher || expenseItem.Product.CanShowPermission (expenseItem.Product.MerchantCity))
				request.AddParameter("merchantCity", this.MerchantCity);

			if (expenseItem.Product.CanShowPermission(expenseItem.Product.InvoiceId) && this.InvoiceId != null)
				request.AddParameter("invoiceID", this.InvoiceId);

			if (expenseItem.Product.CanShowPermission (expenseItem.Product.ProjectId) && expenseItem.ProjectId.HasValue)
				request.AddParameter("projectId", expenseItem.ProjectId);

			if (expenseItem.Product.CanShowPermission (expenseItem.Product.DepartmentId) && expenseItem.DepartmentId.HasValue)
				request.AddParameter("departmentId", expenseItem.DepartmentId);

			if (expenseItem.Product.CanShowPermission (expenseItem.Product.TravelRequestId) && expenseItem.TravelRequestId.HasValue)
				request.AddParameter("travelRequestId", expenseItem.TravelRequestId);

			if (expenseItem.Product.CanShowPermission (expenseItem.Product.Allowanceline) || expenseItem.Product.CanShowPermission (expenseItem.Product.HotelLine)) {
				request.AddParameter ("fldTransactionPeriodFromDate", this.SerizalizeDate (this.PeriodFromDateTime));
				request.AddParameter ("fldTransactionPeriodToDate", this.SerizalizeDate (this.PeriodToDateTime));
			}

			if (expenseItem.Product.CanShowPermission (expenseItem.Product.Allowanceline)) {
				request.AddParameter ("fldTransactionPeriodFromTime", this.PeriodFromDateTime.ToString ("HH:mm"));
				request.AddParameter ("fldTransactionPeriodToTime", this.PeriodToDateTime.ToString ("HH:mm"));
			}

			if (expenseItem.Product.CanShowPermission (expenseItem.Product.VehicleOdometer))
				request.AddParameter("fldTransactionFuelMileage", this.FuelMileage);

			if (expenseItem.Product.CanShowPermission (expenseItem.Product.VehicleNumber))
				request.AddParameter("fldTransactionFuelVehicleNumber", this.VehicleNumber);

			if (expenseItem.Product.CanShowPermission (expenseItem.Product.Infochar1))
				request.AddParameter ("ItemInfoChar1", this.GetDynamicField("Infochar1").SerializeValue(expenseItem));

			if (expenseItem.Product.CanShowPermission (expenseItem.Product.Infochar2))
				request.AddParameter ("ItemInfoChar2", this.GetDynamicField("Infochar2").SerializeValue(expenseItem));

			if (expenseItem.Product.CanShowPermission (expenseItem.Product.Infochar3))
				request.AddParameter ("ItemInfoChar3", this.GetDynamicField("Infochar3").SerializeValue(expenseItem));

			if (expenseItem.Product.CanShowPermission (expenseItem.Product.Infochar4))
				request.AddParameter ("ItemInfoChar4", this.GetDynamicField("Infochar4").SerializeValue(expenseItem));

			if (expenseItem.Product.CanShowPermission (expenseItem.Product.Infochar5))
				request.AddParameter ("ItemInfoChar5", this.GetDynamicField("Infochar5").SerializeValue(expenseItem));

			if (expenseItem.Product.CanShowPermission (expenseItem.Product.Infochar6))
				request.AddParameter ("ItemInfoChar6", this.GetDynamicField("Infochar6").SerializeValue(expenseItem));

			if (expenseItem.Product.CanShowPermission (expenseItem.Product.Infochar7))
				request.AddParameter ("ItemInfoChar7", this.GetDynamicField("Infochar7").SerializeValue(expenseItem));

			if (expenseItem.Product.CanShowPermission (expenseItem.Product.Infochar8))
				request.AddParameter ("ItemInfoChar8", this.GetDynamicField("Infochar8").SerializeValue(expenseItem));

			if (expenseItem.Product.CanShowPermission (expenseItem.Product.Infonum1))
				request.AddParameter ("ItemInfoNum1", this.GetDynamicField("Infonum1").SerializeValue(expenseItem));

			if (expenseItem.Product.CanShowPermission (expenseItem.Product.Infonum2))
				request.AddParameter ("ItemInfoNum2", this.GetDynamicField("Infonum2").SerializeValue(expenseItem));
			
			if (Preferences.Instance.ITEShowReceiptTickbox)
				request.AddParameter ("ReceiptPresent", expenseItem.ParentExpense.ReceiptPresent);

			request.AddParameter ("CCMatch", expenseItem.CCMatch ? 1 : 0);

			request.AddParameter ("itemAccount", expenseItem.AccountType.GetString ());
		}
	}
}