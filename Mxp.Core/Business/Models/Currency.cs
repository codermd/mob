using System;
using System.Globalization;
using Mxp.Core.Services.Responses;

namespace Mxp.Core.Business
{
	public class Currency : Model
	{
		public int Id { get; set; }
		public bool Active { get; set; }
		public int CreatedBy { get; set; }
		public DateTime CreatedOn { get; set; }
		public string Iso { get; set; }
		public DateTime LastUpdateOn { get; set; }
		public int LastUpdateBy { get; set; }
		public string Major { get; set; }
		public string Name { get; set; }
		public string VersionLock { get; set; }

		public string VName {
			get {
				return String.Format("{0} ({1})", this.Name, this.Iso);
			}
		}
	
		public Currency (CurrencyResponse currencyResponse) {
			this.Id = currencyResponse.CurrencyID;
			this.Active = currencyResponse.CurrencyActive;
			this.CreatedBy = currencyResponse.CurrencyCreatedBy;
			DateTime createdOn;
			this.CreatedOn = DateTime.TryParse (currencyResponse.CurrencyCreatedOn, out createdOn) ? createdOn : default (DateTime);
			this.Iso = currencyResponse.CurrencyISO;
			DateTime lastUpdateOn;
			this.LastUpdateOn = DateTime.TryParse (currencyResponse.CurrencyLastupdateOn, out lastUpdateOn) ? lastUpdateOn : default (DateTime);
			this.LastUpdateBy = currencyResponse.CurrencyLastupdateby;
			this.Major = currencyResponse.CurrencyMajor;
			this.Name = currencyResponse.CurrencyName;
			this.VersionLock = currencyResponse.CurrencyVersionLock;
		}

		public override string ToString () {
			return this.Iso;
		}

		public override bool Equals (object obj) {
			if (!(obj is Currency))
				return false;

			if (this.Id != ((Currency)obj).Id)
				return false;

			return true;
		}
	}
}