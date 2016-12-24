using System;
using System.Threading.Tasks;
using Mxp.Core.Utils;
using System.Collections.Generic;
using System.Linq;
using Mxp.Core.Services;

namespace Mxp.Core.Business
{
	public abstract class OpenObjectAbstractCommand : ICommand
	{
		public const string HostUri = "openobject";

		protected OpenObject openObject;
		public MetaOpenObject MetaOpenObject { get; set; }

		public OpenObjectAbstractCommand () {
			
		}

		public OpenObjectAbstractCommand (Uri uri) {
			this.Parse (uri);
		}

		protected abstract void RedirectToDetailsView ();
		protected abstract void RedirectToListView (ValidationError error);

		#region ICommand

		public abstract void RedirectToLoginView (ValidationError error = null);

		public void Parse (Uri uri) {
			Dictionary<String, String> parameters = HttpUtility.ParseQueryString (System.Net.WebUtility.UrlDecode (uri.Query));
			this.openObject = new OpenObject (parameters ["objectType"], parameters ["referenceType"], parameters ["reference"]);
		}

		public async Task InvokeAsync () {
			if (!LoggedUser.Instance.IsAuthenticated) {
				this.RedirectToLoginView ();
				return;
			}

			try {
				await this.FetchObjectAsync ();
			} catch (ValidationError e) {
				this.RedirectToListView (e);
				return;
			} catch (Exception) {
				this.RedirectToListView (new ValidationError ("Error", Service.NoConnectionError));
				return;
			}

			this.RedirectToDetailsView ();
		}

		private async Task FetchObjectAsync () {
			this.MetaOpenObject = await this.openObject.FetchAsync ();

			bool valid = false;

			switch (this.MetaOpenObject.Location) {
				case MetaOpenObject.LocationEnum.PendingExpenses:
					await LoggedUser.Instance.BusinessExpenses.FetchAsync ();
					await LoggedUser.Instance.PrivateExpenses.FetchAsync ();
					valid = LoggedUser.Instance.BusinessExpenses.SingleOrDefault (expense => expense.Id.Value == this.MetaOpenObject.Id) != null;
					break;
				case MetaOpenObject.LocationEnum.DraftReports:
					await LoggedUser.Instance.DraftReports.FetchAsync ();
					if (this.MetaOpenObject.HasFatherId) {
						Report dep = LoggedUser.Instance.DraftReports.SingleOrDefault (report => report.Id.Value == this.MetaOpenObject.FatherId);
						valid = (dep != null ? dep.Expenses.SingleOrDefault (expense => expense.Id == this.MetaOpenObject.Id) != null : false);
					} else
						valid = LoggedUser.Instance.DraftReports.SingleOrDefault (report => report.Id.Value == this.MetaOpenObject.Id) != null;
					break;
				case MetaOpenObject.LocationEnum.OpenReports:
					await LoggedUser.Instance.OpenReports.FetchAsync ();
					if (this.MetaOpenObject.HasFatherId) {
						Report dep = LoggedUser.Instance.OpenReports.SingleOrDefault (report => report.Id.Value == this.MetaOpenObject.FatherId);
						valid = (dep != null ? dep.Expenses.SingleOrDefault (expense => expense.Id == this.MetaOpenObject.Id) != null : false);
					} else
						valid = LoggedUser.Instance.OpenReports.SingleOrDefault (report => report.Id.Value == this.MetaOpenObject.Id) != null;
					break;
				case MetaOpenObject.LocationEnum.ApprovalReports:
					await LoggedUser.Instance.ReportApprovals.FetchAsync ();
					if (this.MetaOpenObject.HasFatherId) {
						ReportApproval dep = LoggedUser.Instance.ReportApprovals.SingleOrDefault (approval => approval.Report.Id.Value == this.MetaOpenObject.FatherId);
						valid = (dep != null ? dep.Report.Expenses.SingleOrDefault (expense => expense.Id == this.MetaOpenObject.Id) != null : false);
					} else
						valid = LoggedUser.Instance.ReportApprovals.SingleOrDefault (approval => approval.Report.Id.Value == this.MetaOpenObject.Id) != null;
					break;
				case MetaOpenObject.LocationEnum.ApprovalTravelRequests:
					await LoggedUser.Instance.TravelApprovals.FetchAsync ();
					valid = LoggedUser.Instance.DraftReports.SingleOrDefault (report => report.Id.Value == this.MetaOpenObject.Id) != null;
					break;
				case MetaOpenObject.LocationEnum.Unknown:
					throw new ValidationError ("Error", "Invalid object");
			}

			if (!valid)
				throw new ValidationError ("Error", "No corresponding object found.");
		}

		#endregion
	}
}