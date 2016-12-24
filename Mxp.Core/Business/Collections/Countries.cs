using System.Collections.Generic;

using Mxp.Core.Services;
using Mxp.Core.Services.Responses;
using System.Threading.Tasks;
using System.Linq;
using System;
using Mxp.Core.Utils;

namespace Mxp.Core.Business
{
	public class Countries : SGCollection<Country>
	{
		public Countries () {

		}

		public Countries (IEnumerable<Country> countries) : base (countries) {
			
		}

		public IEnumerable<Response> rawResponses { get; private set; }

		public override void Populate (IEnumerable<Response> collection) {
			this.rawResponses = collection;

			base.Populate (collection);

			this.ReplaceWith (this.Sort ());
		}

		public async override Task FetchAsync () {
			await SystemService.Instance.FetchCountriesAsync (this);
		}

		private Countries _recentlyUsedCountry;
		public Countries RecentlyUsedCountry {
			get {
				if (this._recentlyUsedCountry == null)
					this._recentlyUsedCountry = new Countries (this.Where (country => country.Recent));

				return this._recentlyUsedCountry;
			}
		}

		private Countries _countriesForExpense;
		public Countries ForExpense {
			get {
				if (this._countriesForExpense == null)
					this._countriesForExpense = new Countries (this.Where (country => country.ForExpense));

				return this._countriesForExpense;
			}
		}

		private Countries _countriesForAllowance;
		public Countries ForAllowance {
			get {
				if (this._countriesForAllowance == null)
					this._countriesForAllowance = new Countries (this.Where (country => country.ForAllowance));

				return this._countriesForAllowance;
			}
		}

		public Countries Parents {
			get {
				Countries countries = new Countries();

				foreach (Country country in this)
					countries = new Countries(countries.Union(country.Parents));

				return countries;
			}
		}

		public Countries Children {
			get {
				Countries countries = new Countries (LoggedUser.Instance.Countries.Where (country => country.HasParent && this.Contains (country.CountryParent)));

				if (countries.IsEmpty ())
					return countries;

				Countries nextChildrenGenertation = new Countries (countries.Children);

				return new Countries (countries.Union (nextChildrenGenertation));
			}
		}

		public Countries SearchWith (string text) {
			if (String.IsNullOrEmpty(text)) {
				this.ForEach(country => country.IsMatched = false);
				return this;
			}

			Countries selectedCountries = new Countries (this.Where (country => country.Name.IndexOf (text, StringComparison.OrdinalIgnoreCase) >= 0));

			HashSet<Country> hashCountries = new HashSet<Country> (selectedCountries);
			this.ForEach (country => country.IsMatched = hashCountries.Contains (country));
			
			Countries selectedChildren = selectedCountries.Children;
			Countries selectedParents = selectedCountries.Parents;

			Countries countries = new Countries (selectedCountries.Union (selectedChildren).Union (selectedParents));

			countries.ReplaceWith (countries.Sort ());

			return countries;
		}

		private List<Country> Sort () { 
			return this
				.OrderBy(location => location.ParentCountryName)
				.ThenBy(location => {
					if (!location.HasParent)
						return null;

					if (location.HasParent && location.CountryParent.HasParent)
						return location.CountryParent.Name;

					return location.Name;
				})
				.ThenBy(location => {
					if (location.HasParent && location.CountryParent.HasParent)
						return location.Name;

					return null;
				})
				.ToList ();
		}

		#region iOS

		private List<IGrouping<String, Country>> _groupedCountries;
		public List<IGrouping<String, Country>> GroupedCountries {
			get {
				return this.GetGroupedCountries (false);
			}
		}
		public List<IGrouping<String, Country>> GetGroupedCountries (bool whileSearching = false) {
			if (this._groupedCountries == null) {
				IEnumerable<IGrouping<String, Country>> groupedCountries = this.GroupBy (country => country.IndexName)
					.OrderBy (grouping => grouping.Key).ToList ();

				if (whileSearching)
					this._groupedCountries = groupedCountries.ToList ();
				else {
					IEnumerable<IGrouping<String, Country>> recentlyUsedCountries = this.Where (country => country.Recent).GroupBy (country => Labels.GetLoggedUserLabel (Labels.LabelEnum.Recents));
					this._groupedCountries = recentlyUsedCountries.Concat (groupedCountries).ToList ();
				}
			}

			return this._groupedCountries;
		}

		public void ResetGroups () {
			this._groupedCountries = null;
		}

		#endregion
	}
}