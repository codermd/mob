using System;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

using Mxp.Core.Services;
using Mxp.Core.Services.Responses;
using System.Linq;
using Mxp.Core.Utils;

namespace Mxp.Core.Business
{
	public partial class Mileage : Expense
	{
		public const int PAYEMENT_METHOD_ID = 14;

		public enum DistanceTypeEnum {
			Business,
			Commute,
			Private
		}

		public override bool IsSplit {
			get {
				return false;
			}
		}

		public Vehicles Vehicles { get; set; }

		public override DateTime? Date {
			set {
				base.Date = value;

				if (base.Date != null && this.IsNew)
					this.RefreshVehicles ();
			}
		}

		public int ItineraryId { get; set; }
	
		private int _odometerFrom;
		public int OdometerFrom {
			get {
				return this._odometerFrom;
			}
			set {
				this._odometerFrom = value;
				this._odometerTo = this.OdometerFrom + this.BusinessDistance;
			}
		}

		private int _odometerTo;
		public int OdometerTo {
			get {
				return this._odometerTo;
			}
			set {
				this._odometerTo = value;
				this.UpdateMileageFromOdometer ();
			}
		}
	
		private Vehicle _vehicle;
		public Vehicle Vehicle {
			get {
				if (this._vehicle == null && this.Vehicles.Count > 0)
					this._vehicle = this.Vehicles [0];
				
				return this._vehicle;
			}
			set {
				this._vehicle = value;
				this.ChangeVehicle ();
			}
		}

		public VehicleCategory VehicleCategory {
			get {
				return this.Vehicle.Category;
			}
			set {
				this.Vehicle.Category = value;
			}
		}

		public MileageSegments MileageSegments { get; set; }

		private int _calculatedDistance;
		public int CalculatedDistance {
			get {
				return this._calculatedDistance;
			}
			set {
				this._calculatedDistance = value;
				this.NotifyPropertyChanged ("CalculatedDistance");
			}
		}

		private int _businessDistance;
		public int BusinessDistance {
			get {
				return this._businessDistance;
			}
			set {
				this._businessDistance = value;
				this.NotifyPropertyChanged ("BusinessDistance");
				this.UpdateOdometerFromMileage (DistanceTypeEnum.Business);
			}
		}

		private int _commuteDistance;
		public int CommuteDistance {
			get {
				return this._commuteDistance;
			}
			set {
				int diffCommute = value - this.CommuteDistance;
				this.BusinessDistance = this.BusinessDistance - diffCommute;
				this._commuteDistance = value;

				this.NotifyPropertyChanged ("CommuteDistance");
				this.UpdateOdometerFromMileage (DistanceTypeEnum.Commute);
			}
		}

		private int _privateDistance;
		public int PrivateDistance {
			get {
				return this._privateDistance;
			}
			set {
				int diffPrivate = value - this.PrivateDistance;
				this.BusinessDistance = this.BusinessDistance - diffPrivate;
				this._privateDistance = value;

				this.NotifyPropertyChanged ("PrivateDistance");
				this.UpdateOdometerFromMileage (DistanceTypeEnum.Private);
			}
		}

		public void UpdateMileageFromOdometer () {
			if (!this.CanDisplayOdometer)
				return;
			
			if (this.OdometerFrom != 0 && this.OdometerTo != 0) {
				if (Preferences.Instance.MILDefaultProduct == Preferences.ProductTypeEnum.Private)
					this._privateDistance = this.OdometerTo - this.OdometerFrom - this.BusinessDistance - this.CommuteDistance;
				else if (Preferences.Instance.MILDefaultProduct == Preferences.ProductTypeEnum.Business)
					this._businessDistance = this.OdometerTo - this.OdometerFrom - this.PrivateDistance - this.CommuteDistance;
			}
				
			if (this.PrivateDistance < 0) {
				this._odometerTo += Math.Abs (this.PrivateDistance);
				this._privateDistance = 0;
			}
		}

		public void UpdateOdometerFromMileage (DistanceTypeEnum distanceType) {
			if (!this.CanDisplayOdometer)
				return;

			if (this.OdometerFrom == 0 || Preferences.Instance.MILMap != PermissionEnum.Mandatory)
				return;

			if (this.OdometerTo == 0)
				this._odometerTo = this.OdometerFrom
					+ Math.Max (0, this.BusinessDistance)
					+ Math.Max (0, this.CommuteDistance)
					+ Math.Max (0, this.PrivateDistance);
			else {
				if (distanceType == DistanceTypeEnum.Commute || distanceType == DistanceTypeEnum.Business)
					this._privateDistance = this.OdometerTo - this.OdometerFrom - this.BusinessDistance - this.CommuteDistance;
				else if (distanceType == DistanceTypeEnum.Private)
					this._businessDistance = this.OdometerTo - this.OdometerFrom - this.PrivateDistance - this.CommuteDistance;
			}

			if (this.PrivateDistance < 0) {
				this._odometerTo += Math.Abs (this.PrivateDistance);
				this._privateDistance = 0;
			}
		}

		public void ChangeVehicle () {
			this.OdometerFrom = this.Vehicle.LastMileage;

			this._odometerTo = this.OdometerFrom
				+ Math.Max (0, this.BusinessDistance)
				+ Math.Max (0, this.CommuteDistance)
				+ Math.Max (0, this.PrivateDistance);
		}

		public bool CanDisplayOdometer {
			get {
				if (!this.IsNew)
					return false;

				return this.CanShowMap
					&& this.Vehicle != null
					&& (Preferences.Instance.MILOdometer == VisibilityEnum.Show
						|| (Preferences.Instance.MILOdometer == VisibilityEnum.Other
							&& Preferences.Instance.MILOdometerOtherValue.Contains (this.Vehicle.Ownership)));
			}
		}

		public bool CanDisplayCommutePrivate (VisibilityEnum MILCommutingPrivate, string MILCommuningPrivateOtherValue) {
			return CanShowMap
				&& (MILCommutingPrivate == VisibilityEnum.Show
					|| MILCommutingPrivate == VisibilityEnum.ReadOnly
					|| (this.Vehicle != null
						&& !String.IsNullOrWhiteSpace (MILCommuningPrivateOtherValue)
						&& MILCommuningPrivateOtherValue.Contains (this.Vehicle.Ownership)));
		}

		public void ResetDistances () {
			this._businessDistance = this.CalculatedDistance;
			this.NotifyPropertyChanged ("BusinessDistance");

			if (Preferences.Instance.MILWithdrawCommuting == VisibilityEnum.Show)
				this._commuteDistance = (int)-Preferences.Instance.FldEmployeeHomeWorkDistance;
			else
				this._commuteDistance = 0;
			this.NotifyPropertyChanged ("CommuteDistance");

			this._privateDistance = 0;
			this.NotifyPropertyChanged ("PrivateDistance");

			this._odometerTo = this.OdometerFrom + this.BusinessDistance;
			this.NotifyPropertyChanged ("OdometerTo");
		}

		private async void RefreshVehicles () {
			try {
				await this.Vehicles.FetchAsync ();
			} catch (Exception) { }

			if (!this.Vehicles.Contains (this.Vehicle)) {
				this.VehicleNumber = null;
				this.Vehicle = null;
			}
		}

		public override void ResetChanged () {
			if (this.MileageSegments != null)
				this.MileageSegments.ForEach (item => item.ResetChanged ());

			base.ResetChanged ();

			this.NotifyPropertyChanged ("IsChanged");
		}
	}
}