using System;
using Mxp.Core.Business;
using System.Threading.Tasks;
using Mxp.Core.Services.Responses;
using System.Collections.Generic;
using Mxp.Core.Services;
using Mxp.Core.Utils;

namespace Mxp.Core.Business
{
	public class SpendCatcherExpenses : SGCollection<SpendCatcherExpense>
	{
		public SpendCatcherExpenses () {
			
		}

		public async override Task FetchAsync () {
			await SpendCatcherService.Instance.FetchExpensesAsync (this);

			IList<Task> tasks = new List<Task> ();

			this.ForEach (expense => {
				if (expense.Country == null)
					tasks.Add (Task.Run (async () => {
						expense.Country = await Country.FetchAsync (expense.CountryId);
					}));
			});

			await Task.WhenAll (tasks);

			this.Loaded = true;
		}

		public async Task SendAsync () {
			List<Task> tasks = new List<Task> (this.Count);

			this.ForEach (spendCatcherExpense => tasks.Add (SpendCatcherService.Instance.AddExpenseAsync (spendCatcherExpense)));

			await Task.WhenAll (tasks);
		}
	}
}