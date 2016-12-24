using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Mxp.Core.Utils;
using System.Threading.Tasks;
using Mxp.Core.Services;

namespace Mxp.Core.Business
{
	public partial class ExpenseItem
	{
		public static PolicyRules GetPolicyRule (string policy) {
			switch (policy.ToLower ()) {
				case "r":
					return PolicyRules.Red;
				case "o":
					return PolicyRules.Orange;
				case "g":
				default:
					return PolicyRules.Green;
			}
		}

		public static Status GetStatus (string status) {
			if (String.IsNullOrWhiteSpace (status))
				return Status.Other;

			switch (status.ToUpper ()) {
				case "R":
				case "4":
					return Status.Rejected;
				case "A":
				case "5":
					return Status.Accepted;
				default:
					return Status.Other;
			}
		}
	}
}