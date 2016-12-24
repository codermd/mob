using System.Collections.Generic;
using Mxp.Core.Utils;
using RestSharp.Portable;
using System.Threading.Tasks;
using Mxp.Core.Services;

namespace Mxp.Core.Business
{
	public partial class Allowance
	{
		public void CreateJourneyAllowanceSerialization (RestRequest request) {
			request.AddParameter ("fldcountryid", this.Country.Id.ToString ());
			request.AddParameter ("fldcountrytext", this.Country.Name);

			request.AddParameter ("fldjourneysegmentdatefrom", SerizalizeDate (this.DateFrom));

			request.AddParameter ("fldjourneysegmentdateto", this.SerizalizeDate (this.DateTo));

			request.AddParameter ("fldjourneysegmenttimefrom", this.DateFrom.ToString ("HH:mm"));
			request.AddParameter ("fldjourneysegmenttimeto", this.DateTo.ToString ("HH:mm"));

			request.AddParameter ("fldjourneysegmentindicator1", this.Breakfast.ToString ().ToLower ());
			request.AddParameter ("fldjourneysegmentindicator2", this.Lunch.ToString ().ToLower ());
			request.AddParameter ("fldjourneysegmentindicator3", this.Dinner.ToString ().ToLower ());
			request.AddParameter ("fldjourneysegmentindicator4", this.Lodging.ToString ().ToLower ());
			request.AddParameter ("fldjourneysegmentindicator5", this.Info.ToString ().ToLower ());
			request.AddParameter ("fldjourneysegmentindicator6", this.WorkNight.ToString ().ToLower ());
			request.AddParameter ("fldjourneysegmentlocation", this.Location);
			request.AddParameter ("fldjourneysegmentpurpose", this.Comment);
		}

		public void AddJourneyAllowanceSerialization (Dictionary<string, object> dict) {
			dict ["journeyId"] = this.journeyId.ToString ();
			dict ["transactionId"] = this.transactionId.ToString ();
			dict ["itemId"] = this.ExpenseItems [0].Id.ToString ();
			dict ["employeeId"] = this.employeeId.ToString ();
			dict ["journeyName"] = this.journeyName;
			dict ["journeyPurpose"] = this.journeyPurpose;
			dict ["netAmountCC"] = this.netAmountCC.ToString ();
			dict ["vatAmountCC"] = this.vatAmountCC.ToString ();
			dict ["legalAmountCC"] = this.legalAmountCC.ToString ();

			if (Preferences.Instance.CanShowPermission (Preferences.Instance.ALLSIinfochar1))
				dict ["ItemInfoChar1"] = this.GetDynamicField ("Infochar1").SerializeValue (this.ExpenseItems [0]);

			if (Preferences.Instance.CanShowPermission (Preferences.Instance.ALLSIinfochar2))
				dict ["ItemInfoChar2"] = this.GetDynamicField ("Infochar2").SerializeValue (this.ExpenseItems [0]);

			if (Preferences.Instance.CanShowPermission (Preferences.Instance.ALLSIinfochar3))
				dict ["ItemInfoChar3"] = this.GetDynamicField ("Infochar3").SerializeValue (this.ExpenseItems [0]);

            if (Preferences.Instance.CanShowPermission (Preferences.Instance.ALLSIinfochar4))
                dict ["ItemInfoChar4"] = this.GetDynamicField ("Infochar4").SerializeValue (this.ExpenseItems [0]);

            if (Preferences.Instance.CanShowPermission (Preferences.Instance.ALLSIinfochar5))
                dict ["ItemInfoChar5"] = this.GetDynamicField ("Infochar5").SerializeValue (this.ExpenseItems [0]);

            if (Preferences.Instance.CanShowPermission (Preferences.Instance.ALLSIinfochar6))
                dict ["ItemInfoChar6"] = this.GetDynamicField ("Infochar6").SerializeValue (this.ExpenseItems [0]);

            if (Preferences.Instance.CanShowPermission (Preferences.Instance.ALLSIinfochar7))
                dict ["ItemInfoChar7"] = this.GetDynamicField ("Infochar7").SerializeValue (this.ExpenseItems [0]);

            if (Preferences.Instance.CanShowPermission (Preferences.Instance.ALLSIinfochar8))
                dict ["ItemInfoChar8"] = this.GetDynamicField ("Infochar8").SerializeValue (this.ExpenseItems [0]);

            if (Preferences.Instance.CanShowPermission (Preferences.Instance.ALLSIinfonum1))
				dict ["ItemInfoNum1"] =this.GetDynamicField ("Infonum1").SerializeValue (this.ExpenseItems [0]);

			if (Preferences.Instance.CanShowPermission (Preferences.Instance.ALLSIinfonum2))
				dict ["ItemInfoNum2"] = this.GetDynamicField ("Infonum2").SerializeValue (this.ExpenseItems [0]);

            if (Preferences.Instance.CanShowPermission (Preferences.Instance.ALLSIMerchantName))
                dict ["merchantName"] = this.MerchantName;

            if (Preferences.Instance.CanShowPermission (Preferences.Instance.ALLSIinvoiceId))
                dict ["invoiceId"] = this.InvoiceId;

            dict ["transactionComments"] = this.Comment;
			dict ["projectId"] = this.ALLSIprojectId;
			dict ["travelRequestId"] = this.ALLSItrqId;
			dict ["departmentId"] = this.ALLSIdepartmentId;
			dict ["grossAmountCC"] = this.GrossAmountCC.ToString ();

			List<Dictionary<string, object>> segmentsResult = new List<Dictionary<string, object>> ();
			this.AllowanceSegments.ForEach (segment => {
				segmentsResult.Add (segment.SerializeSegment ());
			});

			dict ["segments"] = segmentsResult;
		}
			
		public async override Task CreateAsync (bool ignoreAttendee = false) {
			this.TryValidateCreation ();
			await AllowanceService.Instance.PartiallyCreateAllowanceAsync (this);
		}

		public override void TryValidateCreation () {
			foreach (Field field in this.CreationAllowanceFields) {
				field.TryValidate ();
			}
		}

		private bool IsFetch {
			get {
				return this.AllowanceSegments.Count > 0;
			}
		}

		public async Task FetchAsync () {
			if (this.IsFetch)
				return;

			await AllowanceService.Instance.FetchAllowance (this);

			IList<Task> tasks = new List<Task> ();

			this.AllowanceSegments.ForEach (segment => {
				if (segment.Country == null)
					tasks.Add (Task.Run (async () => {
						segment.Country = await Country.FetchAsync (segment.CountryId);
					}));
			});

			await Task.WhenAll (tasks);
		}

		public async Task SaveAsync () {
			this.TryValidateEdition (this.ExpenseItems [0]);

			await AllowanceService.Instance.AddAllowanceAsync (this);

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

		public new void TryValidateEdition (ExpenseItem expenseItem) {
			foreach (Field field in expenseItem.GetAllFields ()) {
				field.TryValidate ();
			}
		}

		public async Task RecalculateAsync () {
			await AllowanceService.Instance.CalculateAllowanceAsync (this);
		}
	}
}