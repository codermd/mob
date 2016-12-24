using System.Threading.Tasks;
using Mxp.Core.Services;
using System.Collections.Generic;
using Mxp.Core.Utils;
using System;

namespace Mxp.Core.Business
{
	public partial class Expense
	{
		public async Task SaveAsync (ExpenseItem expenseItem, bool force = false, bool notify = true) {
			if (notify)
				LoggedUser.Instance.NotifyMessageChanged (new AlertMessage (AlertMessage.MessageTypeEnum.StartLoading, Labels.GetLoggedUserLabel (Labels.LabelEnum.Saving) + "..."));
			
			try {
				if (!force)
					this.TryValidateEdition (expenseItem);
				
				await ExpenseService.Instance.SaveExpenseAsync (expenseItem);

				if (expenseItem.ParentExpense.GetModelParent<Expense> () is Report && expenseItem.ParentExpense.GetModelParent<Expense, Report> ().IsOpen) {
					await Task.WhenAll (
						LoggedUser.Instance.OpenReports.FetchAsync (),
						LoggedUser.Instance.DraftReports.FetchAsync ()
					);
				}

				this.ResetChanged ();
			} catch (Exception e) {
				if (notify)
					LoggedUser.Instance.NotifyMessageChanged (new AlertMessage (AlertMessage.MessageTypeEnum.Error, "Error", e is ValidationError ? ((ValidationError)e).Verbose : Service.NoConnectionError));

				throw;
			}

			if (notify)
				LoggedUser.Instance.NotifyMessageChanged (new AlertMessage (AlertMessage.MessageTypeEnum.StopLoading));
		}

		public async virtual Task CreateAsync (bool ignoreAttendee = false) {
			LoggedUser.Instance.NotifyMessageChanged (new AlertMessage (AlertMessage.MessageTypeEnum.StartLoading, Labels.GetLoggedUserLabel (Labels.LabelEnum.Saving) + "..."));

			try {
				this.TryValidateCreation ();

				// Copy needed because of Populate from SaveAsync which erase attendees list
				Attendees toAdd = new Attendees (this.ExpenseItems [0]);
				this.ExpenseItems [0].Attendees.ForEach (attendee => toAdd.Add (attendee));

				await this.SaveAsync (this.ExpenseItems [0], true, false);

				List<Task> tasks = new List<Task> () {
					this.Receipts.UploadBase64Images ()
				};

				if (!ignoreAttendee) {
					toAdd.ForEach (attendee => {
						tasks.Add (AttendeeService.Instance.AddAttendeeAsync (this.ExpenseItems [0].Attendees, attendee));
					});
				}
				
				await Task.WhenAll (tasks.ToArray ());

				await this.SaveAsync (this.ExpenseItems [0], true, false);

				this.ResetChanged ();

				// This is useful for keeping consistence across references
				if (!LoggedUser.Instance.BusinessExpenses.Contains (this))
					LoggedUser.Instance.BusinessExpenses.AddItem (this);

				await LoggedUser.Instance.BusinessExpenses.FetchAsync ();
			} catch (Exception e) {
				LoggedUser.Instance.NotifyMessageChanged (new AlertMessage (AlertMessage.MessageTypeEnum.Error, "Error", e is ValidationError ? ((ValidationError)e).Verbose : Service.NoConnectionError));
				throw;
			}

			LoggedUser.Instance.NotifyMessageChanged (new AlertMessage (AlertMessage.MessageTypeEnum.StopLoading));
		}

		public void TryValidateEdition (ExpenseItem expenseItem) {
			foreach (TableSectionModel tsm in expenseItem.DetailsFields) {
				foreach (Field field in tsm.Fields) {
					field.TryValidate ();
				}
			}
		}

		public virtual void TryValidateCreation () {
			this.TryValidateEdition (this.ExpenseItems [0]);
		}
	}
}