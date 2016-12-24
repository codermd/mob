using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Mxp.Core.Utils;
using Mxp.Core.Services;
using System.Threading.Tasks;
using RestSharp.Portable;
using System.Linq;

namespace Mxp.Core.Business
{
	public partial class ExpenseItem
	{
		public async Task SplitAsync () {
			await ExpenseService.Instance.SplitExpenseAsync (this);
			this.ClearSplittedItems ();
			await this.ParentExpense.GetCollectionParent<Expenses, Expense> ().FetchAsync ();
		}

		public async Task UnsplitAsync () {
			await ExpenseService.Instance.UnsplitExpenseAsync (this);
			await this.ParentExpense.GetCollectionParent<Expenses, Expense> ().FetchAsync ();
		}

		public async Task<Products> FetchAvailableProductsAsync () {
			Products products = new Products (this);

			await SystemService.Instance.FetchProductsAsync (products, this.Product);

			return products;
		}
			
		public ExpenseItems InnerSplittedItems { get; set; }

		public void ClearSplittedItems () {
			this.InnerSplittedItems.Clear ();
		}

		public ExpenseItem AddExpenseItem () {
			ExpenseItem expenseItem = new ExpenseItem {
				Quantity = 1
			};
			this.InnerSplittedItems.AddItem (expenseItem);

			return expenseItem;
		}

		public void TryValidateSplit() {
			if (this.InnerSplittedItems.Count < 1) {
				throw new ValidationError ("ERROR", Labels.GetLoggedUserLabel (Labels.LabelEnum.AddSplitItem));
			}
		}

		public Dictionary<string, object> SplitSerialize () {
			this.TryValidateSplit();

			Dictionary<string, object> result = new Dictionary<string, object> ();
			result ["itemId"] = this.Id.ToString ();

			result ["transactionId"] = this.ParentExpense.TransactionId.ToString ();
			List<Dictionary<string,object>> splitSegments = new List<Dictionary<string,object>> ();
			this.InnerSplittedItems.ForEach(expenseItem => {
				Dictionary<string,object> inner = new Dictionary<string,object>();
				inner["itemGrossAmountLC"] = expenseItem.AmountLC.ToString();
				inner["itemVatAmountLC"] = "0.0";
				inner["itemNetAmountLC"] = expenseItem.AmountLC.ToString();
				inner["itemAccount"] = "COMP";
				inner["productId"] = expenseItem.Product.Id.ToString();
				inner["itemDate"] = this.SerizalizeDate(this.ParentExpense.Date.Value);
				inner["itemVatCode"] = "N/A";
				inner["itemVatRate"] = "0.0";
				inner["itemProductQuantity"] = expenseItem.Quantity.ToString();
				inner["itemComments"] = "";
				splitSegments.Add(inner);
			});

			result ["splitSegments"] = splitSegments;
			return result;
		}

		public double RemainingAmount {
			get {
				return Math.Round (this.AmountLC - this.InnerSplittedItems.Sum (expenseItem => expenseItem.AmountLC), 2);
			}
		}
	}
}