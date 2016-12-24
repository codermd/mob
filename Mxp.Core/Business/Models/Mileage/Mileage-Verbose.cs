using System;
using System.Collections.ObjectModel;
using Mxp.Core.Business;
using System.Collections.Generic;
using Mxp.Core.Services.Responses;

namespace Mxp.Core.Business
{
	public partial class Mileage
	{
		public override string VDetailsBarTitle {
			get {
				return Labels.GetLoggedUserLabel(Labels.LabelEnum.Mileage);
			}
		}

		public override bool CanShowReceiptIcon {
			get {
				return false;
			}
		}
	}
}