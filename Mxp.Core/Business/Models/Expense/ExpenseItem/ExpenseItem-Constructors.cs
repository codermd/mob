using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Mxp.Core.Utils;
using Mxp.Core.Services.Responses;

namespace Mxp.Core.Business
{
	public partial class ExpenseItem
	{
		public ExpenseItem () : base () {
			this.Attendees = new Attendees (this);

			this.InnerSplittedItems = new ExpenseItems (this);

			this.CCMatch = false;
		}

		public ExpenseItem (ExpenseItemResponse expenseItemResponse) : this () {
			this.Populate (expenseItemResponse);
		}
	}
}