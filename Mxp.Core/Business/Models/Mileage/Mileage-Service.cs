using System;
using System.Collections.ObjectModel;
using Mxp.Core.Business;
using System.Collections.Generic;
using Mxp.Core.Services.Responses;
using Mxp.Core.Services;
using System.Threading.Tasks;

namespace Mxp.Core.Business
{
	public partial class Mileage
	{
		public override void TryValidate () {
			foreach (Field field in this.ExpenseItems[0].GetAllFields ())
				field.TryValidate ();
			foreach (Field field in this.ExpenseItems[0].GetMainFields ())
				field.TryValidate ();

			this.MileageSegments.TryValidate ();
		}

		public async override Task CreateAsync (bool ignoreAttendee = false) {
			await this.SaveAsync ();
		}

		public async Task SaveAsync () {
			this.TryValidate ();

			await MileageService.Instance.AddItineraryAsync (this);
			await MileageService.Instance.SaveMileageAsync (this);

			if (this.IsNew)
				await LoggedUser.Instance.BusinessExpenses.FetchAsync ();
			else
				await this.GetCollectionParent<Expense> ().FetchAsync ();

			if (this.GetModelParent <Expense> () is Report && this.GetModelParent <Expense, Report> ().IsOpen) {
				await Task.WhenAll (
					LoggedUser.Instance.OpenReports.FetchAsync (),
					LoggedUser.Instance.DraftReports.FetchAsync ()
				);
			}

			this.ResetChanged ();
		}
	}
}