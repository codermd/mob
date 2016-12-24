using System;

namespace Mxp.Core.Services.Responses
{
	public class CurrencyResponse : Response
	{
		public int CurrencyID { get; set; }
		public string CurrencyName { get; set; }
		public string CurrencyISO { get; set; }
		public int CurrencyLastupdateby { get; set; }
		public string CurrencyLastupdateOn { get; set; }
		public string CurrencyMajor { get; set; }
		public bool CurrencyActive { get; set; }
		public string CurrencyVersionLock { get; set; }
		public int CurrencyCreatedBy { get; set; }
		public string CurrencyCreatedOn { get; set; }

		public CurrencyResponse () {}
	}
}