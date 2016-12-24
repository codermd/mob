using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Mxp.Core.Services.Responses;
using System.Collections.ObjectModel;
using Mxp.Core.Utils;
using System.Diagnostics;
using System.Linq;

namespace Mxp.Core.Business
{
	public class ExpenseItems : SGCollection<ExpenseItem>
	{
		public ExpenseItems (Model modelParent = null) : base (modelParent) {

		}

		public ExpenseItems (IEnumerable<Response> collection, Model model = null) : base (collection, model) {

		}

		public override void Populate (IEnumerable<Response> collection) {
			if (this.Count > 0 && this [0].IsNew) {
				this [0].Populate ((ExpenseItemResponse)collection.ElementAt (0));
			} else if (this.Count > 0) {
				ExpenseItems expenseItems = new ExpenseItems (this.ParentModel);
				collection.ForEach (expenseItemResponse => {
					ExpenseItem item = this.SingleOrDefault (expenseItem => expenseItem.Id == ((ExpenseItemResponse)expenseItemResponse).itemID);
					if (item != null) {
						item.Populate (expenseItemResponse as ExpenseItemResponse);
						expenseItems.Add (item);
					} else
						expenseItems.Add (new ExpenseItem (expenseItemResponse as ExpenseItemResponse));
				});
				this.ReplaceWith (expenseItems);
			} else
				base.Populate (collection);
		}
	}
}