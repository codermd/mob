using System;
using Mxp.Core.Services.Responses;
using System.Linq;
using Mxp.Core.Services;

namespace Mxp.Core.Business
{
	public partial class SpendCatcherExpense : Model, ICountriesFor
	{
		public int Id { get; set; }
		public DateTime Date { get; set; }
		public Product Product { get; set; }
		public int CountryId { get; set; }
		public Country Country { get; set; }
		public bool IsPaidByCC { get; set; }
		public string AttachmentPath { get; set; }
		public string Base64Image { get; set; }

		public SpendCatcherExpense () {

		}

		public SpendCatcherExpense (String base64Image) {
			this.Base64Image = base64Image;
		}

		#region ICountriesFor

		public Countries Countries {
			get {
				return LoggedUser.Instance.Countries.ForExpense;
			}
		}

		#endregion

		public override bool Equals (object obj) {
			if (ReferenceEquals (null, obj))
				return false;

			if (ReferenceEquals (this, obj))
				return true;

			if (obj.GetType () != GetType ())
				return false;

			return this.Id.Equals (((SpendCatcherExpense)obj).Id);
		}

		public override int GetHashCode () {
			return this.Id.GetHashCode ();
		}
	}
}
