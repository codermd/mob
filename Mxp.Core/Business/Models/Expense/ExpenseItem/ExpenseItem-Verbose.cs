using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Mxp.Core.Utils;

namespace Mxp.Core.Business
{
	public partial class ExpenseItem
	{
		public string VAmountLC {
			get {
				return VRoundedAmountLC + " " + this.Currency.Iso;
			}
		}

		public string VAmountCC {
			get {
				return Math.Round (this.AmountCC, 2) + " " + LoggedUser.Instance.Currency.Iso;
			}
		}

		public string VRoundedAmountLC {
			get {
				return Math.Round (this.AmountLC, 2).ToString ();
			}
		}

		public bool AreCurrenciesDifferent {
			get {
				return !this.Currency.Equals (LoggedUser.Instance.Currency);
			}
		}

		public string VCategoryName { 
			get { 
				if (this.Product == null) {
					return null;
				}

				return this.Product.ExpenseCategory.Name;
			}
		}

		public string VTitleSplit {
			get {
				return this.VCategoryName + " : " + this.RemainingAmount;
			}
		}

		public string VRemainingAmount {
			get {
				return Labels.GetLoggedUserLabel (Labels.LabelEnum.Amount) + " (< " + this.RemainingAmount + ")";
			}
		}
	}
}