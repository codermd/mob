using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Mxp.Core.Utils;

namespace Mxp.Core.Business
{
	public partial class ExpenseItem
	{
		public bool IsNew {
			get {
				return !this.Id.HasValue;
			}
		}

		public Expense ParentExpense {
			get {
				// Return null if parent is not typeof Expense
				return this.GetModelParent<ExpenseItem> () as Expense;
			}
		}

		public Expenses ParentExpensesCollection => this.ParentExpense.GetCollectionParent<Expenses, Expense> ();

		public bool CanEditCurrency {
			get {
				Model model = this.GetModelParent<ExpenseItem> ();

				if (model is ExpenseItem)
					return false;

				return ((Expense)model).ExpenseItems.Count == 1;
			}
		}

		public bool CanShowSplit {
			get {
				return !this.IsNew
					&& (this.ParentExpense.IsFromApproval
						|| (this.ParentExpense.Report?.IsClosed ?? false)
						|| this.ParentExpense is Mileage
					    || this.ParentExpense is Allowance ? false : this.CanSplit);
			}
		}

		public bool CanShowUnsplit {
			get {
				return !this.IsNew
					&& (this.ParentExpense.IsFromApproval
						|| (this.ParentExpense.Report?.IsClosed ?? false)
						|| this.ParentExpense is Mileage
					    || this.ParentExpense is Allowance ? false : this.CanUnsplit);
			}
		}

		public bool CanShowDelete {
			get {
				return !this.IsNew
					        && !(bool)this.ParentExpense?.IsSplit
					        && (bool)this.ParentExpense?.CanDelete;
			}
		}

		public bool CanManageAttendees {
			get {
				return !this.ParentExpense.IsFromApproval && (!this.ParentExpense.Report?.IsClosed ?? true);
			}
		}

		public bool CanShowAttendees {
			get {
				return this.ParentExpense.CanShowAttendees && this.Product.CanShowPermission (this.Product.Attendees);
			}
		}

		public bool CanShowPolicyTip {
			get {
				if (this.ParentExpense != null && this.ParentExpense.IsNew)
					return false;

				return this.PolicyRule == PolicyRules.Orange || this.PolicyRule == PolicyRules.Red;
			}
		}

		public bool IsTransactionByCard => (bool)this.ParentExpense?.IsTransactionByCard;

		public bool IsTransactionBySpendCatcher {
			get {
				return this.PaymentMethodId == 154 || this.PaymentMethodId == 161;
			}
		}

		public bool IsTempTransaction {
			get {
				return this.PaymentMethodId == 146 || this.PaymentMethodId == 149 || this.PaymentMethodId == 155 || this.PaymentMethodId == 162;
			}
		}

		internal bool IsPaidByCard {
			get {
				return !String.IsNullOrWhiteSpace (this.StatementNumber)
					&& this.StatementNumber != "0";
			}
		}

		public bool IsReadyToSplit {
			get {
				return this.InnerSplittedItems != null && this.InnerSplittedItems.Count > 0;
			}
		}

		#region iOS only

		public void SanitizeInnerSplits () {
			ExpenseItems toRemove = new ExpenseItems ();
			this.InnerSplittedItems.ForEach (item => {
				if(item.Product == null) {
					toRemove.Add(item);
				}
			});

			toRemove.ForEach (item => {
				this.InnerSplittedItems.Remove(item);
			});
		}

		#endregion

		public bool CanEditCountry {
			get { 
				if (this.ParentExpense != null && this.ParentExpense.IsSplit)
					return false;
			
				if (this.ParentExpense != null && this.ParentExpense.IsNew)
					return true;

				return (this.CanEdit == 2 || this.CanEdit == 1)
					&& ((this.IsTransactionByCard && Preferences.Instance.ITECountryTRXCRDUser == VisibilityEnum.Show) 
						|| !this.IsTransactionByCard);
			}
		}

		public bool IsOwnedToCompany => this.FundingType == ScopeTypeEnum.Company && this.AccountType == ScopeTypeEnum.Private;

		public bool CanChangeAccountType => !this.IsNew && !this.ParentExpense.IsFromApproval;

		#region ICountriesFor

		public Countries Countries {
			get {
				return this.ParentExpense.Countries;
			}
		}

		#endregion

		public bool IsHighlighted => this.IsOwnedToCompany;

		public string VChangeAccountType => this.AccountType == ScopeTypeEnum.Private ? Labels.GetLoggedUserLabel (Labels.LabelEnum.ChangeExpenseToBusiness) : Labels.GetLoggedUserLabel (Labels.LabelEnum.ChangeExpenseToPrivate);

		public bool CanCopy => this.ParentExpense.CanCopy;

		public bool CanShowSavingOptions => this.ParentExpense.IsNew || this.IsChanged;
	}
}