using System;
using System.Collections.Generic;
using Mxp.Core.Business;

namespace Mxp.Core.Helpers
{
	public class IconsLegend
	{
		private static List<IconsLegend> iconsLegend;

		public string Title { get; set; }
		public List<IconLegend> IconsLegendList { get; set; }

		public IconsLegend (string title, List<IconLegend> iconsLegend) {
			this.Title = title;
			this.IconsLegendList = iconsLegend;
		}

		public static List<IconsLegend> All {
			get {
				if (iconsLegend == null) {				
					iconsLegend = new List<IconsLegend> ();

					iconsLegend.Add (new IconsLegend (
						Labels.GetLoggedUserLabel (Labels.LabelEnum.Expenses),
						new List<IconLegend> () {
							new IconLegend (IconLegend.IconsEnum.Compliant, Labels.GetLoggedUserLabel (Labels.LabelEnum.ExpensePolicyGreen)),
							new IconLegend (IconLegend.IconsEnum.NotCompliantPolicy, Labels.GetLoggedUserLabel (Labels.LabelEnum.ExpensePolicyOrange)),
							new IconLegend (IconLegend.IconsEnum.NotCompliant, Labels.GetLoggedUserLabel (Labels.LabelEnum.ExpensePolicyRed)),
							new IconLegend (IconLegend.IconsEnum.ReceiptsAttached, Labels.GetLoggedUserLabel (Labels.LabelEnum.ExpenseReceiptBlack))
						}
					));

					iconsLegend.Add (new IconsLegend (
						Labels.GetLoggedUserLabel (Labels.LabelEnum.Reports),
						new List<IconLegend> () {
							new IconLegend (IconLegend.IconsEnum.Accepted, Labels.GetLoggedUserLabel (Labels.LabelEnum.ReportReceiptGreen)),
							new IconLegend (IconLegend.IconsEnum.Rejected, Labels.GetLoggedUserLabel (Labels.LabelEnum.ReportReceiptRed)),
							new IconLegend (IconLegend.IconsEnum.Pending, Labels.GetLoggedUserLabel (Labels.LabelEnum.ReportReceiptBlack)),

							new IconLegend (IconLegend.IconsEnum.Approved, Labels.GetLoggedUserLabel (Labels.LabelEnum.ReportThumbGreen)),
							new IconLegend (IconLegend.IconsEnum.Refused, Labels.GetLoggedUserLabel (Labels.LabelEnum.ReportThumbRed)),
							new IconLegend (IconLegend.IconsEnum.PendingSchedule, Labels.GetLoggedUserLabel (Labels.LabelEnum.ReportPending)),

							new IconLegend (IconLegend.IconsEnum.Compliant, Labels.GetLoggedUserLabel (Labels.LabelEnum.ReportPolicyGreen)),
							new IconLegend (IconLegend.IconsEnum.NotCompliantPolicy, Labels.GetLoggedUserLabel (Labels.LabelEnum.ReportPolicyOrange)),
							new IconLegend (IconLegend.IconsEnum.NotCompliant, Labels.GetLoggedUserLabel (Labels.LabelEnum.ReportPolicyRed)),
						}
					));
				}

				return iconsLegend;
			}
		}

		public static void Reset () {
			iconsLegend = null;
		}
	}
}