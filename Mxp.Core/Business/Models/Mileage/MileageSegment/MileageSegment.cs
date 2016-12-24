using System;

using Mxp.Core.Services;
using Mxp.Core.Services.Responses;
using System.Threading.Tasks;
using Mxp.Core.Services.Google;
using System.Globalization;
using Mxp.Core.Helpers;

namespace Mxp.Core.Business
{
	public class MileageSegment : Model, ICloneable
	{
		public override void NotifyPropertyChanged (string name) {
			base.NotifyPropertyChanged (name);

			if (this.GetCollectionParent<MileageSegments, MileageSegment> () != null) {
				if (!name.Equals ("ResetChanged"))
					this.AddModifiedObject (name);
				
				this.GetCollectionParent<MileageSegments, MileageSegment> ().NotifyPropertyChanged (name);
			}
		}

		private double? _locationLatitude;
		public double? LocationLatitude {
			get {
				return this._locationLatitude;
			}
			set {
				this._locationLatitude = value;

				this.AddModifiedObject ("LocationLatitude");
				this.NotifyPropertyChanged ("IsChanged");
			}
		}
		public double? _locationLongitude;
		public double? LocationLongitude {
			get {
				return this._locationLongitude;
			}
			set {
				this._locationLongitude = value;

				this.AddModifiedObject ("LocationLongitude");
				this.NotifyPropertyChanged ("IsChanged");
			}
		}

		private string _locationAliasName;
		public string LocationAliasName { 
			get {
				return this._locationAliasName;
			} 
			set {
				this._locationAliasName = value;

				this.AddModifiedObject ("LocationAliasName");
				this.NotifyPropertyChanged ("IsChanged");
			}
		}

		public MileageSegment () {

		}

		public MileageSegment (double latitude, double longitude) {
			this.SetCurrentLocation (latitude, longitude);
		}


		public MileageSegment (MileageSegmentResponse mileageSegmentResponse) {
			this.Populate (mileageSegmentResponse);
		}

		public void Populate (MileageSegmentResponse mileageSegmentResponse) {
			this.LocationLatitude = mileageSegmentResponse.LocationLatitude;
			this.LocationLongitude = mileageSegmentResponse.LocationLongitude;
			this.LocationAliasName = mileageSegmentResponse.LocationAliasName;

			this.ResetChanged ();
		}

		public async void SetCurrentLocation (double latitude, double longitude) {
			this.LocationLatitude = latitude;
			this.LocationLongitude = longitude;

			this.LocationAliasName = "Current location";

			try {
				await GoogleService.Instance.GetLocationNameAsync (this);
			} catch (Exception) {
				this.LocationLatitude = default(double);
				this.LocationLongitude = default (double);
				this.LocationAliasName = String.Empty;
			}
		}

		public object Clone () {
			// Shallow copy !
			return this.MemberwiseClone ();
		}

		public bool IsLocationValid {
			get {
				return this.LocationLatitude.HasValue && this.LocationLongitude.HasValue;
			}
		}

		public string Coordinate {
			get {
				NumberFormatInfo nfi = new NumberFormatInfo ();
				nfi.NumberDecimalSeparator = ".";

				return String.Format ("{0},{1}", this.LocationLatitude.Value.ToString (nfi), this.LocationLongitude.Value.ToString (nfi));
			}
		}

		public async Task FetchLocationsAsync (Prediction prediction) {
			this.LocationAliasName = prediction.description;

			await GoogleService.Instance.FetchGeocodingLocationAsync (this);
			// OR
//			await MileageService.Instance.FetchPlaceLocationAsync (this, prediction.place_id);
		}

		public override bool Equals (object obj) {
			if (!(obj is MileageSegment))
				return false;

			MileageSegment mileageSegment = (MileageSegment) obj;

			if ((!this.LocationLatitude.HasValue || !this.LocationLongitude.HasValue)
				|| (!mileageSegment.LocationLatitude.HasValue || !mileageSegment.LocationLongitude.HasValue))
				return false;

			return this.LocationLatitude.Value.Equals (mileageSegment.LocationLatitude.Value)
				       && this.LocationLongitude.Value.Equals (mileageSegment.LocationLongitude.Value);
		}
	}
}