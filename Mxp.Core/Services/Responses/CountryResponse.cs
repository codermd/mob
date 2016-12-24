using System;

namespace Mxp.Core.Services.Responses
{
	public class CountryResponse : Response
	{
		public int CountryID { get; set; }
		public string CountryName { get; set; }
		public string CountryIsoname { get; set; }
		public string CurrencyIso { get; set; }
		public int CurrencyID { get; set; }
		public string CountryMainInD { get; set; }
		public string CountryISO3Name { get; set; }
		public int CountryIDParent { get; set; }
		public int CountryType { get; set; }
		public bool CountryForExpense { get; set; }
		public bool CountryForAllowance { get; set; }
		public bool recent { get; set; }

		public CountryResponse () {}
	}
}