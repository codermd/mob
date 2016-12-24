using System;

namespace Mxp.Core.Business
{
	public partial class AllowanceSegment
	{
		public string VAmount {
			get {
				return this.Amount.ToString("n2") + " " + this.Currency.Iso;
			}
		}

		public string VDateFrom {
			get {

				return LoggedUser.Instance.Preferences.VDate(this.DateFrom.Value)+ " (" + this.TimeFrom + ")";
			}
		}

		public string VDateTo {
			get {
				return LoggedUser.Instance.Preferences.VDate(this.DateTo.Value) + " (" + this.TimeTo + ")";
			}
		}
	}
}