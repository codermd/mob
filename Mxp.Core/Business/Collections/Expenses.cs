using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Mxp.Core.Services;
using Mxp.Core.Services.Responses;
using System.Linq;
using System.Collections.ObjectModel;
using Mxp.Core.Utils;
using System.Diagnostics;

namespace Mxp.Core.Business
{
	public class Expenses : SGCollection<Expense>
	{
		public enum ExpensesTypeEnum {
			Business,
			Private,
			Report
		}

		public ExpensesTypeEnum ExpensesType { get; }

		public Expenses (ExpensesTypeEnum expensesTypeEnum, Model model = null) : base (model) {
			this.ExpensesType = expensesTypeEnum;
		}

		public Expenses (List<ExpenseResponse> expenseResponses) : base (expenseResponses) {

		}

		public Expenses (IEnumerable<Expense> collection, Model model) : base (collection, model) {

		}

		public override async Task FetchAsync () {
			await ExpenseService.Instance.FetchExpensesAsync (this);

			IList<Task> tasks = new List<Task> ();

			this.ForEach (expense => {
				if (expense.Country == null)
					tasks.Add (Task.Run (async () => {
						expense.Country = await Country.FetchAsync (expense.CountryId);
					}));
			});

			await Task.WhenAll (tasks);

			this.ReplaceWith (this.OrderByDescending (expense => expense.Date).ToList ());

			this.ResetSectionnedExpenses ();
		}

		public override V GetInstance<V> (Response response) {
			Expense expense = null;

			ExpenseResponse expenseResponse = (ExpenseResponse)response;

			switch (expenseResponse.fldPaymentMethodID) {
				case Mileage.PAYEMENT_METHOD_ID:
					expense = new Mileage (expenseResponse);
					break;
				case Allowance.PAYEMENT_METHOD_ID:
					expense = new Allowance (expenseResponse);
					break;
				default:
					expense = new Expense (expenseResponse);
					break;
			}

			return (V) expense;
		}

		public override void AddItem (Expense item, bool notify = false) {
			base.AddItem (item, notify);
			this.ResetSectionnedExpenses ();
		}

		public override void Populate (IEnumerable<Response> collection) {
			this.ResetSectionnedExpenses ();

			if (this.IsEmpty ()) {
				base.Populate (collection);
				return;
			}

			Expenses expenses = new Expenses (this.ExpensesType, this.ParentModel);

			collection.ForEach (expenseResponse => {
				Expense item = this.SingleOrDefault (expense => expense.Id == ((ExpenseResponse)expenseResponse).minItemID);

				if (item != null) {
					item.Populate (expenseResponse as ExpenseResponse);
					expenses.Add (item);
				} else
					expenses.Add (this.GetInstance<Expense> (expenseResponse));
			});

			this.ReplaceWith (expenses);
		}

		#region iOS only

		public override void ResetSectionnedExpenses () {
			this._sectionnedExpenses = null;
			this._orderedKeys = null;
		}

		private List<int> _orderedKeys;
		public List<int> getOrderedKeys () {
			if (this._orderedKeys == null) {
				this._orderedKeys = this.getSectionnedExpenses ().Keys.OrderBy (key => key).ToList ();
				this._orderedKeys.Reverse ();
			}

			return this._orderedKeys;
		}

		public void DeploySplits () {
			this.removeExpenseItems ();
			this.ForEach (expense => {
				if (expense.IsSplit) {
					this.unsplitExpense (expense);
				}
			});
		}

		private Dictionary<int, List<Model>> _sectionnedExpenses;
		public Dictionary<int, List<Model>> getSectionnedExpenses () {
			if (this._sectionnedExpenses == null) {
				this._sectionnedExpenses = new Dictionary<int, List<Model>> ();

				var result = this.GroupBy (x => x.OrderKey);
				foreach (var expenseGroup in result) {
					List<Model> expen = expenseGroup.ToList<Model> ();
					expen.Sort ((model1, model2) => {
						var exp1 = model1 as Expense;
						var exp2 = model2 as Expense;
						return DateTime.Compare (exp1.Date.Value, exp2.Date.Value) * -1;
					});
					this._sectionnedExpenses.Add (expenseGroup.Key, expen);
				}
			}

			return this._sectionnedExpenses;
		}

		public Collection<RowSection> unsplitExpense (Expense exp) {
			Collection<RowSection> result = new Collection<RowSection> ();

			foreach (int key in this.getOrderedKeys ()) {
				List<Model> models = this.getSectionnedExpenses () [key];
				for (int i = 0; i < models.Count; i++) {
					Model currentModel = models [i];
					if (currentModel.Equals (exp)) {
						int addIndex = i + 1;
						exp.ExpenseItems.ForEach (ei => {
							models.Insert (addIndex, ei);
							int section = this.getOrderedKeys ().IndexOf (key);
							result.Add (new RowSection (addIndex, section));
							addIndex++;
						});
					}
				}
			}

			return result;
		}

		public Collection<RowSection> removeExpenseItems () {
			Collection<RowSection> result = new Collection<RowSection> ();

			foreach (int key in this.getOrderedKeys ()) {
				List<Model> models = this.getSectionnedExpenses () [key];
				for (int i = 0; i < models.Count; i++) {
					Model currentModel = models [i];
					if (currentModel is ExpenseItem) {
						int section = this.getOrderedKeys ().IndexOf (key);
						result.Add (new RowSection (i, section));
					}
				}
			}

			foreach (int key in this.getOrderedKeys ()) {
				List<Model> models = this.getSectionnedExpenses () [key];

				List<Model> keep = new List<Model> ();
				for (int i = 0; i < models.Count; i++) {
					Model currentModel = models [i];
					if (!(currentModel is ExpenseItem)) {
						keep.Add (currentModel);
					}
				}

				models.Clear ();

				keep.ForEach (model => {
					models.Add (model);
				});
			}

			return result;
		}

		public class RowSection
		{
			public RowSection (int row, int section) {
				this.Row = row;
				this.Section = section;
			}
			public int Row;
			public int Section;
		}

		#endregion
	}
}