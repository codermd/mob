using System;
using System.Collections.Generic;
using Mxp.Core.Utils;

namespace Mxp.Core.Business
{
	public partial class Attendee
	{
		public static bool CanShowAddAttendeeSpouse {
			get {
				return LoggedUser.Instance.Preferences.HCPShowHideSpouse == VisibilityEnum.Show;
			}
		}

		public static bool CanShowAddAttendeeHCP {
			get {
				return LoggedUser.Instance.Preferences.HCPAllowHCP == VisibilityEnum.Show;
			}
		}

		public static bool CanShowAddAttendeeHCO {
			get {
				return LoggedUser.Instance.Preferences.HCPAllowHCO == VisibilityEnum.Show;
			}
		}

		public static bool CanShowAddAttendeeHCU {
			get {
				return LoggedUser.Instance.Preferences.HCPAllowManualEntry == VisibilityEnum.Show;
			}
		}

		public static Actionables ListShowAttendees (Action actionRecent, Action actionBusinessRelation, Action actionEmployee,
			Action actionSpouse, Action actionHCP, Action actionHCO, Action actionHCU) {
				List<Actionable> actions = new List<Actionable> () {
					new Actionable (Labels.GetLoggedUserLabel (Labels.LabelEnum.Recent), actionRecent),
					new Actionable (Labels.GetLoggedUserLabel (Labels.LabelEnum.BusinessRelation), actionBusinessRelation),
					new Actionable (Labels.GetLoggedUserLabel (Labels.LabelEnum.Employee), actionEmployee)
				};

			if (Attendee.CanShowAddAttendeeSpouse)
				actions.Add (new Actionable (Labels.GetLoggedUserLabel (Labels.LabelEnum.Spouse), actionSpouse));

			if (Attendee.CanShowAddAttendeeHCP)
				actions.Add (new Actionable (Labels.GetLoggedUserLabel (Labels.LabelEnum.Hcp), actionHCP));

			if (Attendee.CanShowAddAttendeeHCO)
				actions.Add (new Actionable (Labels.GetLoggedUserLabel (Labels.LabelEnum.Hco), actionHCO));

			if (Attendee.CanShowAddAttendeeHCU)
				actions.Add (new Actionable (Labels.GetLoggedUserLabel (Labels.LabelEnum.Hcu), actionHCU));

			return new Actionables (Labels.GetLoggedUserLabel (Labels.LabelEnum.ChooseAttendee), actions);
		}

		public bool IsMedical {
			get {
				switch (this.Type) {
					case AttendeeTypeEnum.HCP:
					case AttendeeTypeEnum.HCO:
					case AttendeeTypeEnum.UCP:
						return true;
					default:
						return false;
				}
			}
		}
	}
}