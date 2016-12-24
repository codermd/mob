using System;
using System.Collections.Generic;
using RestSharp.Portable;
using System.Diagnostics;

namespace Mxp.Core.Business
{
	public partial class Expense
	{
		public virtual string VAmountLC {
			get {
				return Math.Round (this.AmountLC, 2) + " " + this.Currency.Iso;
			}
		}
			
		public string VAmountCC {
			get {
				return Math.Round (this.AmountCC, 2) + " " + LoggedUser.Instance.Currency.Iso;
			}
		}

		public bool AreCurrenciesDifferent {
			get {
				return !this.Currency.Equals (LoggedUser.Instance.Currency);
			}
		}

		public string VDate {
			get {
				return this.Date.Value.ToString ("dddd d");
			}
		}

		public string VDateHeader {
			get {
				return this.Date.Value.ToString ("MMMM yyyy");
			}
		}

		public string VTitle {
			get {
				if (this.IsSplit) {
					return Labels.GetLoggedUserLabel(Labels.LabelEnum.Splitted) + " (" + this.ExpenseItems.Count + ")";
				} else {
					return this.CategoryName;
				}
			}
		}

		public virtual string VDetailsBarTitle {
			get {
				return Labels.GetLoggedUserLabel(Labels.LabelEnum.Expense);
			}
		}

		public virtual bool CanShowReceiptIcon {
			get {
				return true;
			}
		}

		public bool IsPaidByCreditCard {
			get {
				if (this.IsSplit)
					return false;

				return  this.ExpenseItems [0].IsPaidByCard;
			}
		}

		public bool IsTempTransaction {
			get {
				return this.ExpenseItems [0].IsTempTransaction;
			}
		}
	}
}