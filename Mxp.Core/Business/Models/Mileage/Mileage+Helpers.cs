using System;
using System.Linq;
using Mxp.Core.Utils;
using System.Collections.Generic;
using System.Diagnostics;

namespace Mxp.Core.Business
{
	public partial class Mileage
	{
		public static bool CanCreateMileage {
			get {
				return Preferences.Instance.CarsCount > 0;
			}
		}

		public override bool IsChanged {
			get {
				return this.MileageSegments.Any (segment => segment.IsChanged) || base.IsChanged;
			}
		}

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

		public bool CanShowMap {
			get {
				return Preferences.Instance.MILMap == PermissionEnum.Mandatory;
			}
		}

		public bool IsEditable {
			get {
				return !this.IsFromApproval && (!this.Report?.IsClosed ?? true);
			}
		}

		public override PermissionEnum GetPermissionForKey (String fieldName) {
			return Preferences.Instance.GetPropertyValue<PermissionEnum> (fieldName);
		}

		public bool CanShowJourneysList {
			get {
				return LoggedUser.Instance.FavouriteJourneys.Count > 0;
			}
		}

		public Actionables ListJourneys (Action onFinish) {
			List<Actionable> actions = new List<Actionable> () {
				new Actionable ("+ New", onFinish)
			};

			LoggedUser.Instance.FavouriteJourneys.ForEach (journey => {
				actions.Add (
					new Actionable (journey.Name, () => {
						this.SetJourney (journey);
						onFinish ();
					})
				);
			});

			return new Actionables (Labels.GetLoggedUserLabel (Labels.LabelEnum.SelectJourney), actions);
		}

		public override bool CanCopy => false;
	}
}