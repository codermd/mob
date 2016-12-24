using System;
using Mxp.Core.Services.Responses;
using System.Linq;
using Mxp.Core.Utils;
using Mxp.Core.Helpers;

namespace Mxp.Core.Business
{
	public partial class Expense : ICloneable
	{
		public ExpenseResponse RawResponse { get; set; }

		protected Expense () {
			this._date = DateTime.Now;
			this.PeriodFromDateTime = DateTime.Now;
			this.PeriodToDateTime = DateTime.Now;

			this.ExpenseItems = new ExpenseItems (this);
			this.Receipts = new Receipts (this);
		}

		public static Expense NewInstance () {
			Expense expense = new Expense ();
			expense.ExpenseItems.AddItem (new ExpenseItem ());
			return expense;
		}

		public Expense (ExpenseResponse expenseResponse) : this () {
			this.Populate (expenseResponse);
		}

		public Expense (Expense expense) : this (expense.RawResponse) {
			this.Id = null;
			this.ExpenseItems.ForEach (expenseItem => {
				expenseItem.Id = null;
				expenseItem.Attendees.Clear ();
			});
			this.NumberReceipts = 0;
		}

		public object Clone () {			
			return new Expense (this);
		}

		public virtual void Populate (ExpenseResponse expenseResponse) {
			this.RawResponse = expenseResponse;

			this.Id = expenseResponse.minItemID;
			this.TransactionId = expenseResponse.fldTransactionID;

			this.CanDelete = expenseResponse.canDelete;
			this.CanRemove = expenseResponse.canRemove;

			this.AmountLC = expenseResponse.fldItemGrossAmountLC;
			this.AmountCC = expenseResponse.fldItemGrossAmountCC;
			this.NumberReceipts = expenseResponse.attachmentCount;
			this.Comment = expenseResponse.fldTransactionComments;
			this.MerchantName = expenseResponse.fldTransactionMerchantName;
			this.MerchantCity = expenseResponse.fldTransactionMerchantCity;
			this.InvoiceId = expenseResponse.fldTransactionInvoiceref;
			this.ReceiptCode = expenseResponse.ReceiptCode;

			DateTime itemDate;
			this._date = expenseResponse.items != null && expenseResponse.items.Count > 0 && DateTime.TryParse (expenseResponse.items.First ().ItemDate, out itemDate) ? itemDate : default (DateTime);

			DateTime PeriodFromDate;
			this.PeriodFromDateTime = DateTime.TryParse (expenseResponse.fldTransactionPeriodFromDate, out PeriodFromDate) ? PeriodFromDate : default (DateTime);
			TimeSpan periodFromTime;
			if (TimeSpan.TryParse (expenseResponse.fldTransactionPeriodFromTime, out periodFromTime))
				this.PeriodFromDateTime = this.PeriodFromDateTime + periodFromTime;
			
			DateTime PeriodToDate;
			this.PeriodToDateTime = DateTime.TryParse (expenseResponse.fldTransactionPeriodToDate, out PeriodToDate) ? PeriodToDate : default (DateTime);
			TimeSpan periodToTime;
			if (TimeSpan.TryParse (expenseResponse.fldTransactionPeriodToTime, out periodToTime))
				this.PeriodFromDateTime = this.PeriodFromDateTime.Date + periodToTime;

			this.ExpenseItems.Populate (expenseResponse.items);

			this.PolicyRule = EnumUtils.GetWeakestEnum<ExpenseItem.PolicyRules> (this.ExpenseItems, "PolicyRule");
			this.ReceiptStatus = EnumUtils.GetWeakestEnum<ExpenseItem.Status> (this.ExpenseItems, "ReceiptStatus");

			//Before changing the currency!
			this.Country = LoggedUser.Instance.Countries.SingleOrDefault (country => country.Id == expenseResponse.fldCountryID);
			this.CountryId = expenseResponse.fldCountryID;

			if (!String.IsNullOrEmpty (expenseResponse.fldCurrencyISO))
				this.Currency = LoggedUser.Instance.Currencies.Single (currency => currency.Iso == expenseResponse.fldCurrencyISO);				

			this.PaymentType = expenseResponse.fldPaymentMethodID;
			this.ReceiptPresent = expenseResponse.fldTransactionReceiptPresent;
			this.FuelMileage = expenseResponse.fldTransactionFuelMileage;
			this.VehicleNumber = expenseResponse.fldTransactionFuelVehicleNumber;
			this.ReportPreselection = (ReportPreselectionEnum) expenseResponse.preSelect;
			this._isTransactionByCard = expenseResponse.IsTransactionByCard;

			this.ResetChanged ();
		}
	}
}