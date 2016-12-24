using System;
using Mxp.Core.Services.Responses;
using System.Linq;
using Mxp.Core.Services;

namespace Mxp.Core.Business
{
	public partial class SpendCatcherExpense
	{
		public SpendCatcherExpense (SpendCatcherResponse response) {
			this.Populate (response);	
		}

		private void Populate (SpendCatcherResponse response) {
			this.Id = response.fldSpendCatcherInfoId;

			if (response.fldCountryId != 0) {
				this.Country = LoggedUser.Instance.Countries.Single (country => country.Id == response.fldCountryId);
				this.CountryId = response.fldCountryId;
			}

			if (response.fldSpendCatcherInfoProductId != 0)
				this.Product = LoggedUser.Instance.Products.Single (product => product.Id == response.fldSpendCatcherInfoProductId);

			this.IsPaidByCC = response.fldIsPaidByCC;

			if (!String.IsNullOrEmpty(response.fldAttachmentPath)) {
				Uri result;
				Uri.TryCreate (new Uri (Service.BaseUrl), response.fldAttachmentPath, out result);
				this.AttachmentPath = result != null ? result.ToString () : null;
			}

			if (!String.IsNullOrEmpty (response.fldSpendCatcherInfoCreatedOn)) {
				DateTime date;
				DateTime.TryParse (response.fldSpendCatcherInfoCreatedOn, out date);
				this.Date = date;
			}
		}
	}
}
