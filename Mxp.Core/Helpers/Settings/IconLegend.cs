using System;

namespace Mxp.Core.Helpers
{
	public class IconLegend
	{
		public enum IconsEnum {
			Compliant,
			NotCompliantPolicy,
			NotCompliant,
			ReceiptsAttached,

			Accepted,
			Rejected,
			Pending,

			Approved,
			Refused,
			PendingSchedule,
		}

		public IconsEnum Icon { get; set; }
		public string Legend { get; set; }

		public IconLegend (IconsEnum iconName, string legend) {
			this.Icon = iconName;
			this.Legend = legend;
		}
	}
}