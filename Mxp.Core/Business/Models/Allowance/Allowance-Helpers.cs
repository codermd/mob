using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Mxp.Core.Utils;
using System.Linq;

namespace Mxp.Core.Business
{
	public partial class Allowance
	{
		public override bool CanShowReceipts {
			get {
				return false;
			}
		}

		internal override bool CanShowAttendees {
			get {
				return false;
			}
		}

		public override bool IsChanged {
			get {
				return this.AllowanceSegments.Any (segment => segment.IsChanged) || base.IsChanged;
			}
		}

		public static bool CanCreateAllowance {
			get {
				return Preferences.Instance.CustomerGermanAllowanceIndicator != 0
					&& Preferences.Instance.AllowanceActivationMobile;
			}
		}

		public override bool IsEditable {
			get {
				return base.IsEditable && Preferences.Instance.AllowanceActivationMobile;
			}
		}

		#region ICountriesFor

		public override Countries Countries {
			get {
				return LoggedUser.Instance.Countries.ForAllowance;
			}
		}

		#endregion

		public override bool CanCopy => false;
	}
}