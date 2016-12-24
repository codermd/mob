using System;
using System.Threading.Tasks;

using Mxp.Core.Services;
using Mxp.Core.Services.Responses;
using System.Linq;
using Mxp.Core.Utils;
using System.Collections.Generic;
using System.Collections;

namespace Mxp.Core.Business
{
	public class Currencies : SGCollection<Currency>
	{
		public Currencies () {

		}

		public Currencies (IEnumerable<Currency> currencies) : base (currencies) {

		}

		public IEnumerable<Response> rawResponses;

		public override void Populate (IEnumerable<Response> collection) {
			this.rawResponses = collection;
			base.Populate (collection);
		}

		public async override Task FetchAsync () {
			await SystemService.Instance.FetchCurrenciesAsync (this);
		}

		public Currencies SearchWith (string text) {
			return new Currencies (this.Where (currency => currency.Name.IndexOf (text, StringComparison.OrdinalIgnoreCase) >= 0));
		}

		#region iOS

		private List<IGrouping<String, Currency>> _groupedCurrencies;
		public List<IGrouping<String, Currency>> GroupedCurrencies {
			get {
				return this.GetGroupedCurrencies (false);
			}
		}
		public List<IGrouping<String, Currency>> GetGroupedCurrencies (bool whileSearching = false) {
			if (this._groupedCurrencies == null) {
				IEnumerable<IGrouping<String, Currency>> groupedCountries = this.GroupBy (currency => currency.Name.Substring (0, 1)).OrderBy (grouping => grouping.Key).ToList ();

				if (whileSearching)
					this._groupedCurrencies = groupedCountries.ToList ();
				else {
					IEnumerable<Currency> customerCurrencyList = new List<Currency> (1) {
						this.Single (currency => currency.Id == LoggedUser.Instance.Preferences.FldCurrencyId)
					};
					IEnumerable<IGrouping<String, Currency>> customerCurrency = customerCurrencyList.GroupBy (currency => Labels.GetLoggedUserLabel (Labels.LabelEnum.CustomerCurrency));
					this._groupedCurrencies = customerCurrency.Concat (groupedCountries).ToList ();
				}
			}

			return this._groupedCurrencies;
		}

		public void ResetGroups () {
			this._groupedCurrencies = null;
		}

		#endregion
	}
}