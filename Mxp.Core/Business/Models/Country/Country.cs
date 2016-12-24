using System;

using Mxp.Core.Services.Responses;
using System.Linq;
using Xamarin.Forms;
using System.Threading.Tasks;
using Mxp.Core.Services;

namespace Mxp.Core.Business
{
	public class Country : Model
	{
		public enum CountryTypeEnum {
			Unknown,
			Country,
			Region,
			City
		}

		public int Id { get; set; }
		public bool ForAllowance { get; set; }
		public bool ForExpense { get; set; }

		public bool IsMatched { get; set; }

		public bool IsCountry => this.CountryType == CountryTypeEnum.Country;

		private int countryParentId;
		public Country CountryParent {
			get {
				if (this.Id == this.countryParentId)
					return this;

				return LoggedUser.Instance.Countries.FirstOrDefault (location => location.Id == this.countryParentId);
			}
		}

		public Countries Parents {
			get {
				Countries countries = new Countries ();

				if (!this.HasParent)
					return countries;
				else {
					countries.Add (this.CountryParent);
					return new Countries (countries.Union (this.CountryParent.Parents));
				}
			}
		}

		public string Iso3Name { get; set; }
		public string IsoName { get; set; }
		public string Name { get; set; }

		public CountryTypeEnum CountryType { get; set; }

		public bool Recent { get; set; }

		private int CurrencyId;

		private Currency _currency;
		public Currency Currency {
			get {
				if (this._currency == null)
					this._currency = LoggedUser.Instance.Currencies.Single (currency => currency.Id == this.CurrencyId);

				return this._currency;
			}
			set {
				this._currency = value;
				this.CurrencyId = this._currency.Id;
			}
		}
	
		public Country (CountryResponse countryResponse) {
			this.Id = countryResponse.CountryID;
			this.ForAllowance = countryResponse.CountryForAllowance;
			this.ForExpense = countryResponse.CountryForExpense;
			this.countryParentId = countryResponse.CountryIDParent;
			this.Iso3Name = countryResponse.CountryISO3Name;
			this.IsoName = countryResponse.CountryIsoname;
			this.Name = countryResponse.CountryName.Split (new string[] { " - " }, StringSplitOptions.None) [1];
			this.CountryType = this.GetCountryType (countryResponse.CountryType);
			this.CurrencyId = countryResponse.CurrencyID;
			this.Recent = countryResponse.recent;
		}

		private CountryTypeEnum GetCountryType (int type) {
			switch (type) {
				case 2:
					return CountryTypeEnum.Country;
				case 3:
					return CountryTypeEnum.Region;
				case 4:
					return CountryTypeEnum.City;
				default:
					return CountryTypeEnum.Unknown;
			}
		}

		public string ParentCountryName {
			get {
				if (this.Id == this.countryParentId)
					return this.Name;

				return this.CountryParent.ParentCountryName;
			}
		}

		public bool HasParent {
			get {
				return this.Id != this.countryParentId && this.CountryParent != null;
			}
		}

		public string IndexName {
			get {
				return this.ParentCountryName.Substring (0, 1);
			}
		}

		public int PaddingLeft {
			get {
				int padding = 0;

				if (this.CountryParent != null && (int)this.CountryType > (int)this.CountryParent.CountryType) {
					padding = 1;

					if (this.CountryType == Country.CountryTypeEnum.City
						&& this.CountryParent.CountryParent != null
						&& (int)this.CountryParent.CountryType > (int)this.CountryParent.CountryParent.CountryType)
						padding = 2;
				}

				return padding;
			}
		}

		public string VName {
			get {
				return String.Format ("{0} - {1}", this.IsoName, this.Name);
			}
		}

		public override bool Equals (object obj) {
			if (ReferenceEquals (null, obj))
				return false;

			if (ReferenceEquals (this, obj))
				return true;

			if (obj.GetType() != GetType())
				return false;

			return this.Id == ((Country)obj).Id;
		}

		public override int GetHashCode () {
			return this.Id.GetHashCode ();
		}

		public string VResourceName {
			get {
				string iso = this.IsoName.ToUpper ();

				if (Device.OS == TargetPlatform.iOS)
					return iso;

				// http://stackoverflow.com/questions/10764347/android-put-an-image-name-package-throws-compile-time-error
				// Conflict alpha-2 -> alpha-3
				if (iso == "DO")
					iso = "DOM";
				else if (iso == "IN")
					iso = "IND";

				return iso;
			}
		}

		#region iOS

		public string VFormattedResourceName {
			get {
				return String.Format ("{0}.png", this.VResourceName);
			}
		}

		#endregion

		public Country MasterCountry => this.Parents.SingleOrDefault (country => country.CountryType == CountryTypeEnum.Country);

		public async Task<Country> GetMasterCountryAsync () {
			if (this.IsCountry)
				return this;

			if (this.HasParent)
				return await this.CountryParent.GetMasterCountryAsync ();

			Country country;

			try {
				country = await SystemService.Instance.FetchCountryAsync (this.Id, true);
			} catch (Exception) {
				return null;
			}

			if ((bool)country?.IsCountry)
				return country;

			return null;
		}

		public static async Task<Country> FetchAsync (int id) {
			try {
				return await SystemService.Instance.FetchCountryAsync (id);
			} catch (Exception) {
				return null;
			}
		}
	}
}