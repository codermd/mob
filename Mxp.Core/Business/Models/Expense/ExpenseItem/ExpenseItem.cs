using System;
using Mxp.Core.Services.Responses;
using System.Linq;

namespace Mxp.Core.Business
{
	public partial class ExpenseItem : Model, ICountriesFor
	{
		public override void NotifyPropertyChanged (string name) {
			base.NotifyPropertyChanged (name);

			if (this.ParentExpense != null)
				this.ParentExpense.NotifyPropertyChanged (name);
		}

		public enum ScopeTypeEnum {
			Unknown,
			Company,
			Private
		}

		public enum PolicyRules {
		 	Green,
			Orange,
			Red
		}

		public enum Status {
			Rejected,
			Other,
			Accepted
		}

		public int? Id { get; set; }

		private Product _product { get; set; }
		public Product Product {
			get {
				return this._product;
			}
			set {
				this._product = value;
				this.ClearCachedFields ();
				this.AddModifiedObject ("Product");
				this.NotifyPropertyChanged ("Product");
			}
		}

		// TODO Change int to Enum or Bool
		public int CanEdit { get; set; }

		public int PaymentMethodId;

		private int _quantity;
		public int Quantity {
			get {
				// FIXME Uh ?
				if (this._quantity == 0) {
					this._quantity = 1;
				}
				return this._quantity;
			}
			set {
				this._quantity = value;
				this.AddModifiedObject ("Quantity");
				this.NotifyPropertyChanged ("Quantity");
			}
		}

		//For credit card
		public bool CCMatch { get; set; }

		private double mAmountLc;
		public double AmountLC {
			get { return this.mAmountLc; }
			set {
				this.mAmountLc = value;
				this.AddModifiedObject ("AmountLC");
				this.NotifyPropertyChanged ("AmountLC");
			}
		}

		public double AmountCC { get; set; }
		public int NumberReceipts { get; set; }
		public PolicyRules PolicyRule { get; set; }
		public string PolicyRuleTip { get; set; }
		public string VatCode { get; set; }
		public double VatRate { get; set; }
		public Status ReceiptStatus { get; set; }
		public string StatementNumber { get; set; }
		private bool CanSplit { get; set; }
		private bool CanUnsplit { get; set; }
		public int? ProjectId { get; set; }
		public int? ProductId { get; set; }
		public int? DepartmentId { get; set; }
		public int? TravelRequestId { get; set; }

		public object Infochar1 { get; set; }
		public object Infochar2 { get; set; }
		public object Infochar3 { get; set; }
		public object Infochar4 { get; set; }
		public object Infochar5 { get; set; }
		public object Infochar6 { get; set; }
		public object Infochar7 { get; set; }
		public object Infochar8 { get; set; }
		public int? Infonum1 { get; set; }
		public int? Infonum2 { get; set; }

		public ScopeTypeEnum AccountType { get; set; }
		public ScopeTypeEnum FundingType { get; set; }

		public Attendees Attendees { get; set; }

		private Status _mainStatus;
		public Status MainStatus {
			get {
				return this._mainStatus;
			}
			set {
				this._mainStatus = value;
				// TODO Useless ?
				this.NotifyPropertyChanged ("MainStatus");
			}
		}

		public Country Country {
			get {
				Model parent = this.GetModelParent<ExpenseItem> ();

				if (parent is Expense)
					return ((Expense)parent).Country;
				else if (parent is ExpenseItem)
					return ((ExpenseItem)parent).Country;

				return null;
			}
			set {
				Model parent = this.GetModelParent<ExpenseItem> ();

				if (parent is Expense)
					((Expense)parent).Country = value;
				else if (parent is ExpenseItem)
					((ExpenseItem)parent).Country = value;

				if (!this.IsTransactionByCard
					|| (this.GetModelParent<ExpenseItem> () is Expense && this.GetModelParent<ExpenseItem, Expense> ().IsNew)
					|| this.GetModelParent<ExpenseItem> () is ExpenseItem)
					this.Currency = value.Currency;

				this.AddModifiedObject ("Country");
				this.NotifyPropertyChanged ("Country");
			}
		}

		public Currency Currency {
			get {
				Model parent = this.GetModelParent<ExpenseItem> ();

				if (parent is Expense)
					return ((Expense)parent).Currency;
				else if (parent is ExpenseItem)
					return ((ExpenseItem)parent).Currency;

				return null;
			}
			set {
				Model parent = this.GetModelParent<ExpenseItem> ();

				if (parent is Expense)
					((Expense)parent).Currency = value;
				else if (parent is ExpenseItem)
					((ExpenseItem)parent).Currency = value;

				this.AddModifiedObject ("Currency");
				this.NotifyPropertyChanged ("Currency");
			}
		}

		public void Populate (ExpenseItemResponse expenseItemResponse) {
			this.Id = expenseItemResponse.itemID;

			this.Product = LoggedUser.Instance.Products.Single (product => product.ExpenseCategory.Id == expenseItemResponse.expensecategoryID);
			this.CanEdit = expenseItemResponse.canEdit;
			this.Quantity = expenseItemResponse.itemproductQuantity;
			this.AmountCC = expenseItemResponse.itemGrossamountCC;
			this.AmountLC = expenseItemResponse.itemGrossamountLC;
			this.NumberReceipts = expenseItemResponse.attachmentcount;
			this.PolicyRule = GetPolicyRule (expenseItemResponse.ItemPolRule);
			this.PolicyRuleTip = expenseItemResponse.ItemPolRuleTip.Trim ();
			this.VatCode = expenseItemResponse.itemVatCode;
			this.VatRate = expenseItemResponse.itemVatRate;

			this.AccountType = this.GetScope (expenseItemResponse.itemaccount);
			this.FundingType = this.GetScope (expenseItemResponse.itemfunding);

			this.MainStatus = GetStatus (expenseItemResponse.itemStatus);
			this.ReceiptStatus = GetStatus (expenseItemResponse.itemReceiptControlstatus);

			this.StatementNumber = expenseItemResponse.itemStatementnumber;
			this.CanSplit = expenseItemResponse.canSplit;
			this.CanUnsplit = expenseItemResponse.canUnsplit;
			this.ProjectId = expenseItemResponse.projectID;
			this.ProductId = expenseItemResponse.productid;
			this.DepartmentId = expenseItemResponse.DepartmentId;
			this.TravelRequestId = expenseItemResponse.Travelrequestid;

			this.Infochar1 = expenseItemResponse.ItemInfoChar1;
			this.Infochar2 = expenseItemResponse.ItemInfoChar2;
			this.Infochar3 = expenseItemResponse.ItemInfoChar3;
			this.Infochar4 = expenseItemResponse.ItemInfoChar4;
			this.Infochar5 = expenseItemResponse.ItemInfoChar5;
			this.Infochar6 = expenseItemResponse.ItemInfoChar6;
			this.Infochar7 = expenseItemResponse.ItemInfoChar7;
			this.Infochar8 = expenseItemResponse.ItemInfoChar8;
			this.Infonum1 = expenseItemResponse.ItemInfoNum1;
			this.Infonum2 = expenseItemResponse.ItemInfoNum2;
			this.PaymentMethodId = expenseItemResponse.paymentMethodID;

			this.Attendees.Populate (expenseItemResponse.Attendees);

			this.ResetChanged ();
		}

		private ScopeTypeEnum GetScope (string scope) {
			scope = scope.ToUpperInvariant ();

			switch (scope) {
				case "COMP":
					return ScopeTypeEnum.Company;
				case "PRIV":
					return ScopeTypeEnum.Private;
				default:
					return ScopeTypeEnum.Unknown;
			}
		}

		public override bool Equals (object obj) {
			if (ReferenceEquals (null, obj))
				return false;

			if (ReferenceEquals (this, obj))
				return true;

			if (obj.GetType () != this.GetType ())
				return false;

			if (((ExpenseItem)obj).IsNew)
				return false;

			return this.Id.Equals (((ExpenseItem)obj).Id);
		}

		public async void SetTravelRequestIdAsync (int id) {
			int? oldId = this.TravelRequestId;
			this.TravelRequestId = id;

			if (Preferences.Instance.IsGTPEnabled) {
				try {
					if (this.ParentExpense.IsNew)
						await this.ParentExpense.CreateAsync (true);
					else
						await this.ParentExpense.SaveAsync (this);
				} catch (Exception) {
					this.TravelRequestId = oldId;
				}
			}
		}
	}
}