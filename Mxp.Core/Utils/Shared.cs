using System;
using Mxp.Core.Business;

namespace Mxp.Core.Business
{
	public static class Shared
	{
		public static String GetVFormattedStatus (String status) {
			switch(status) {
			case "1":
			case "W":
				return LoggedUser.Instance.Labels.GetLabel (Labels.LabelEnum.Waiting);
			case "2":
			case "E":
				return LoggedUser.Instance.Labels.GetLabel (Labels.LabelEnum.Draft);
			case "3":
			case "S":
				return LoggedUser.Instance.Labels.GetLabel (Labels.LabelEnum.Submitted);
			case "4":
			case "R":
				return LoggedUser.Instance.Labels.GetLabel (Labels.LabelEnum.Rejected);
			case "5":
			case "A":
				return LoggedUser.Instance.Labels.GetLabel (Labels.LabelEnum.Accepted);
			case "9":
				return LoggedUser.Instance.Labels.GetLabel (Labels.LabelEnum.Settled);
			case "I":
				return LoggedUser.Instance.Labels.GetLabel (Labels.LabelEnum.Invoiced);
			case "B":
				return LoggedUser.Instance.Labels.GetLabel (Labels.LabelEnum.Booked);
			case "C":
				return LoggedUser.Instance.Labels.GetLabel (Labels.LabelEnum.Confirmed);
			case "X":
				return LoggedUser.Instance.Labels.GetLabel (Labels.LabelEnum.Cancelled);
			case "T":
				return LoggedUser.Instance.Labels.GetLabel (Labels.LabelEnum.WaitingForValidation);
			case "V":
				return LoggedUser.Instance.Labels.GetLabel (Labels.LabelEnum.Scanned);
			}

			return null;
		}
	}
}