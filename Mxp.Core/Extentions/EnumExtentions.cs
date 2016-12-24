using System;
using Mxp.Core.Business;

namespace Mxp.Core.Utils
{
	public static class EnumExtentions
	{
		public static string GetString (this Preferences.UnisSystemEnum unit) {
			switch (unit) {
				case Preferences.UnisSystemEnum.Metric:
					return "Km";
				case Preferences.UnisSystemEnum.Imperial:
					return "Miles";
				default:
					return null;
			}
		}

		public static string GetString (this Report.ApprovalStatusEnum status) {
			switch (status) {
				case Report.ApprovalStatusEnum.Accepted:
					return Labels.GetLoggedUserLabel (Labels.LabelEnum.Accepted);
				case Report.ApprovalStatusEnum.Rejected:
					return Labels.GetLoggedUserLabel (Labels.LabelEnum.Rejected);
				case Report.ApprovalStatusEnum.Waiting:
					return Labels.GetLoggedUserLabel (Labels.LabelEnum.Waiting);
				default:
					return null;
			}
		}

		public static string GetString (this ExpenseItem.ScopeTypeEnum scope) {
			switch (scope) {
				case ExpenseItem.ScopeTypeEnum.Company:
					return "COMP";
				case ExpenseItem.ScopeTypeEnum.Private:
					return "PRIV";
				default:
					return null;
			}
		}
	}
}