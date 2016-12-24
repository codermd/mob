using System;
using Mxp.Core.Utils;

namespace Mxp.Core.Business
{
	public partial class Expense : Model, ICountriesFor
	{
		public enum ReportPreselectionEnum {
			None,
			Optional,
			Mandatory
		}

		public int? Id { get; set; }
		public int TransactionId { get; set; }

		// Classic suppresion
		public bool CanDelete { get; set; }

		// Suppresion from Report expenses collection
		private bool _canRemove;
		public bool CanRemove {
			get {
				return this._canRemove
					       && this.GetCollectionParent<Expense> ()?.Count > 1
					       && this.ReportPreselection != ReportPreselectionEnum.Mandatory;
			}
			set {
				this._canRemove = value;
			}
		}

		public virtual bool IsSplit {
			get {
				return this.ExpenseItems?.Count > 1;
			}
		}

		public double AmountLC { get; set; }
		public double AmountCC { get; set; }
		public string Comment { get; set; }

		public string CategoryName {
			get {
				return this.Product.ExpenseCategory.Name;
			}
		}

		// FIXME Getter only
		public int NumberReceipts { get; set; }

		public string MerchantName { get; set; }
		public string GooglePlacesId { get; set; }
		public string MerchantCity { get; set; }
		public string InvoiceId { get; set; }
		// TODO Enumerise
		public int PaymentType;

		public string ReceiptCode { get; set; }
		public Receipts Receipts { get; set; }

		private DateTime? _date;
		public virtual DateTime? Date {
			get {
				return this._date;
			}
			set {
				this._date = value;
			}
		}

		public DateTime PeriodFromDateTime { get; set ; }
		public DateTime PeriodToDateTime { get; set ; }

		public ExpenseItem.PolicyRules PolicyRule { get; set; }

		public ExpenseItem.Status MainStatus { 
			get {
				return EnumUtils.GetWeakestEnum<ExpenseItem.Status> (this.ExpenseItems, "MainStatus");
			}
		}

		public ExpenseItem.Status ReceiptStatus { get; set; }

		public bool ReceiptPresent { get; set; } = true;

		public ExpenseItems ExpenseItems { get; set; }

		public Currency Currency { get; set; } = LoggedUser.Instance.Currency;
		public Product Product => this.ExpenseItems [0].Product;
		public int CountryId { get; set; }
		public Country Country { get; set; }

		public int? FuelMileage { get; set; }
		public string VehicleNumber { get; set; }

		private bool _isTransactionByCard;
		public bool IsTransactionByCard {
			get {
				return !this.IsNew ? this._isTransactionByCard : false;
			}
		}

		public ReportPreselectionEnum ReportPreselection { get; set; }

		//  iOS only
		public bool IsOpen { get; set; }
		
		public override bool Equals (object obj) {
			if (!(obj is Expense)) {
				return false;
			}

			return this.Id == ((Expense)obj).Id;
		}

		public override void ResetChanged () {
			this.ExpenseItems.ForEach (item => item.ResetChanged ());

			base.ResetChanged ();
		}
	}
}