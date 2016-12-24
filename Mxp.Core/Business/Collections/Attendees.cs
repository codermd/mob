using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Mxp.Core.Utils;

using Mxp.Core.Services;
using Mxp.Core.Services.Responses;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace Mxp.Core.Business
{
	public class Attendees : SGCollection<Attendee>
	{
		public Attendees (Model modelParent = null) : base (modelParent) {

		}

		public Attendees (List<AttendeeResponse> attendeeResponses, ExpenseItem expenseItemParent) : base (attendeeResponses, expenseItemParent) {

		}

		public async Task FetchRecentlyUsedAttendeesAsync () {
			await AttendeeService.Instance.FetchGenericAsync<Attendee, AttendeeResponse> (this, AttendeeService.ApiEnum.GetRecentAttendees.GetRoute ());
		}

		public async Task<List<Attendee>> FetchRecentlyUsedAttendees () {
			return await AttendeeService.Instance.FetchRecentAttendeesAsync ();
		}
			
		public async Task FetchRelatedAttendeesAsync (Attendee relatedAttendee) {
			await AttendeeService.Instance.FetchRelatedAttendeesAsync (this, relatedAttendee);
		}

		public async Task AddAsync (Attendee attendee, ExpenseItem expenseItem = null, bool force = false) {
			if (!force)
				attendee.TryValidate ();

			if (expenseItem != null)
				this.ParentModel = expenseItem;

			attendee.SetCollectionParent (this);

			if (this.ParentModel is ExpenseItem && !this.GetParentModel<ExpenseItem> ().ParentExpense.IsNew) {
				await AttendeeService.Instance.AddAttendeeAsync (this, attendee);
				await ((ExpenseItem)this.ParentModel).ParentExpense.SaveAsync ((ExpenseItem)this.ParentModel, true, false);
			} else
				this.AddItem (attendee);

			if (this.ParentModel is ExpenseItem)
				this.GetParentModel<ExpenseItem> ().NotifyPropertyChanged ("Attendees");
		}

		public async Task RemoveAsync (Attendee attendee) {
			if (this.ParentModel is ExpenseItem && !this.GetParentModel<ExpenseItem> ().ParentExpense.IsNew) {
				await AttendeeService.Instance.RemoveAttendeeAsync (this, attendee);
				await ((ExpenseItem)this.ParentModel).ParentExpense.SaveAsync ((ExpenseItem)this.ParentModel, true, false);
			} else
				attendee.RemoveFromCollectionParent<Attendee> ();

			if (this.ParentModel is ExpenseItem)
				this.GetParentModel<ExpenseItem> ().NotifyPropertyChanged ("Attendees");
		}

		public override void AddItem (Attendee attendee, bool notify = false) {
			base.AddItem (attendee, notify);

			if (this.ParentModel is ExpenseItem)
				this.GetParentModel<ExpenseItem> ().NotifyPropertyChanged ("Attendees");
		}
	}
}