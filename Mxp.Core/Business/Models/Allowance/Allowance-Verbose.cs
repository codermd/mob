using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Mxp.Core.Utils;

namespace Mxp.Core.Business
{
	public partial class Allowance
	{
		public override string VAmountLC {
			get {
				return Math.Round (this.GrossAmountCC, 2) + " " + this.Currency.Iso;
			}
		}

		public string VSegmentSectionTitle {
			get {
				return Labels.GetLoggedUserLabel (Labels.LabelEnum.TotalAmount) + " : " + this.VAmountLC;
			}
		}

		public override string VDetailsBarTitle {
			get {
				return Labels.GetLoggedUserLabel (Labels.LabelEnum.Allowance);
			}
		}
	}
}