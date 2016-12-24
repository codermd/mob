using System;
using System.Collections.Generic;
using RestSharp.Portable;
using Mxp.Core.Services;
using System.Threading.Tasks;

namespace Mxp.Core.Business
{
	public partial class Report
	{
		public async Task SaveAsync () {
			bool wasNew = this.IsNew;

			this.TryValidate ();

			await ReportService.Instance.SaveReportAsync (this);

			if (wasNew) {
				this.SetCollectionParent (LoggedUser.Instance.DraftReports);
				await this.Receipts.UploadBase64Images ();
				await LoggedUser.Instance.DraftReports.FetchAsync ();
			}

			await LoggedUser.Instance.BusinessExpenses.FetchAsync ();
		}

		public async Task SubmitAsync () {
			if (!this.IsSubmitable)
				throw new ValidationError ("Error", Labels.GetLoggedUserLabel (Labels.LabelEnum.Invalid));

			await ReportService.Instance.SubmitReportAsync (this);
			await LoggedUser.Instance.OpenReports.FetchAsync ();

			this.RemoveFromCollectionParent<Report> ();
		}

		public async Task DeleteAsync () {
			if (!this.IsDeletable)
				throw new ValidationError ("Error", Labels.GetLoggedUserLabel (Labels.LabelEnum.Unauthorized));

			await ReportService.Instance.DeleteReportAsync (this);

			this.RemoveFromCollectionParent<Report> ();

			await LoggedUser.Instance.BusinessExpenses.FetchAsync ();
		}

		public async Task CancelAsync () {
			if (!this.IsCancelable)
				throw new ValidationError ("Error", Labels.GetLoggedUserLabel (Labels.LabelEnum.Unauthorized));

			await ReportService.Instance.DeleteReportAsync (this);

			this.RemoveFromCollectionParent<Report> ();

			await LoggedUser.Instance.BusinessExpenses.FetchAsync ();
		}
	}
}