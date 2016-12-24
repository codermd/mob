using System;
using System.Collections.ObjectModel;

using Mxp.Core.Services.Responses;
using System.Collections.Generic;
using Mxp.Core.Services;
using System.Threading.Tasks;
using Mxp.Core.Services.Google;
using System.Linq;
using System.Collections.Specialized;
using Mxp.Core.Utils;
using System.ComponentModel;
using System.Threading;

namespace Mxp.Core.Business
{
	public class MileageSegments : SGCollection<MileageSegment>
	{
		public const int Max = 6;
		public const int Min = 2;

		public Directions Directions { get; set; }

		private CancellationTokenSource cts;

		public override void TryValidate () {
			if (this.Count > Max)
				throw new ValidationError ("ERROR", Labels.GetLoggedUserLabel (Labels.LabelEnum.MileageMaxSegment));

			if (this.Count < Min)
				throw new ValidationError ("ERROR", Labels.GetLoggedUserLabel (Labels.LabelEnum.MileageMinSegment));

			if (this.Any (segment => !segment.IsLocationValid))
				throw new ValidationError ("Error", Labels.GetLoggedUserLabel (Labels.LabelEnum.MileageSegmentValidation));
		}

		public MileageSegments () : base () {

		}

		public MileageSegments (Itinerary parent) : base (parent) {

		}

		public MileageSegments (IEnumerable<MileageSegment> enumerable) : base (enumerable) {

		}

		public MileageSegments (Mileage parent) : base (parent) {
			this.PropertyChanged += HandlePropertyChanged;
		}

		private void HandlePropertyChanged (object sender, PropertyChangedEventArgs e) {
			this.FetchDirectionsAsync (e.PropertyName == "IsChanged");	
		}

		public override async Task FetchAsync () {
			await MileageService.Instance.FetchMileageSegmentsAsync (this);
		}

		public void AddNewItem () {
			this.AddItem (new MileageSegment ());
		}

		public void AddReturningItem () {
			if (this.Count == 0 || this.IsFirstEqualsLastSegment)
				return;

			this.AddItem ((MileageSegment) this.First().Clone ());
		}

		public bool IsFirstEqualsLastSegment {
			get {
				return this.First ().Equals (this.Last ());
			}
		}

		public override void AddItem (MileageSegment item, bool notify = true) {
			if (!this.CanAdd)
				return;

			base.AddItem (item, notify);

			if (notify) {
				this.GetParentModel<Mileage> ()?.AddModifiedObject ("IsChanged");
				this.NotifyPropertyChanged ("IsChanged");
			}
		}

		public async Task AddPrediction (Prediction prediction) {
			MileageSegment segment = new MileageSegment ();
			await segment.FetchLocationsAsync (prediction);
			this.AddItem (segment);
		}

		public void AddLatLong (double latitude, double longitude) {
			MileageSegment segment = new MileageSegment (latitude, longitude);
			this.AddItem (segment);
		}

		protected override void RemoveItem (int index) {
			this.RemoveItemAt (index);
		}

		public void RemoveItemAt (int index, bool force = false) {
			if (!this.CanRemove && !force)
				return;
			
			base.RemoveItem (index);

			this.GetParentModel<Mileage> ()?.AddModifiedObject ("IsChanged");
			this.NotifyPropertyChanged ("IsChanged");
		}

		public void ChangeItemAt (int index, MileageSegment segment) {
			this.RemoveItemAt (index, true);
			this.InsertItem (index, segment);
		}

		public bool CanRemove {
			get {
				return this.Count > 2;
			}
		}

		public bool CanAdd {
			get {
				return this.Count < MileageSegments.Max;
			}
		}

		public bool CanManage {
			get {
				return !this.GetParentModel<Mileage> ().IsFromApproval && (!this.GetParentModel<Mileage> ().Report?.IsClosed ?? true);
			}
		}

		private async void FetchDirectionsAsync (bool isChanged) {
			if (this.Count < 2)
				return;

			MileageSegments segments = new MileageSegments (this.Where (segment => segment.IsLocationValid));

			if (segments.Count < 2)
				return;

			this.cts?.Cancel ();

			this.cts = new CancellationTokenSource ();

			try {
				this.Directions = await GoogleService.Instance.FetchDirectionsAsync (segments, this.cts.Token);
			} catch (Exception) {
				return;
			}

			this.GetParentModel<Mileage> ().CalculatedDistance = this.Directions?.TotalDistance ?? 0;

			if (isChanged)
				this.GetParentModel<Mileage> ().ResetDistances ();

			this.GetParentModel<Mileage> ().NotifyPropertyChanged ("Directions");
		}

		public bool IsAnyLocationsValid {
			get {
				return this.Any (segment => segment.IsLocationValid);
			}
		}
	}
}