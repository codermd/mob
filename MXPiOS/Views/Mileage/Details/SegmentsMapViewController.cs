
using System;
using CoreGraphics;

using Foundation;
using UIKit;
using Mxp.Core.Business;
using System.Threading.Tasks;
using Mxp.Core.Services.Google;
using MapKit;
using Mxp.iOS.Utils;
using CoreLocation;
using System.Collections.Generic;
using Mxp.Core.Utils;

namespace Mxp.iOS
{
	public partial class SegmentsMapViewController : MXPViewController
	{

		public MileageSegments segments;

		public SegmentsMapViewController () : base ("SegmentsMapViewController", null)
		{
		}

		public override void DidReceiveMemoryWarning ()
		{
			base.DidReceiveMemoryWarning ();

		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
		}

		private MKPolylineRenderer render;

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
			this.RefreshAnnotations ();
			this.MapView.OverlayRenderer += (mapview, polyline) => {
				this.render = new MKPolylineRenderer(polyline as MKPolyline);
				render.StrokeColor = UIColor.Black;
				render.LineWidth = 2;
				return render;
			};

			this.segments.GetParentModel<Mileage>().PropertyChanged += HandlePropertyChanged;


			if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad) {
				this.NavigationItem.SetLeftBarButtonItem (new UIBarButtonItem(Labels.GetLoggedUserLabel (Labels.LabelEnum.Close), UIBarButtonItemStyle.Done, (sender, args)=>{
					this.DismissViewController(true, null);
				}), true);
			}
		}


		public override void ViewWillDisappear (bool animated)
		{
			base.ViewWillDisappear (animated);
			this.segments.GetParentModel<Mileage>().PropertyChanged -= HandlePropertyChanged;
		}


		void HandlePropertyChanged (object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			if (e.PropertyName.Equals ("Directions")) {
				this.RefreshAnnotations ();
			}
		}

		public async void RefreshAnnotations(){
			CLLocationCoordinate2D[] coordinates = this.segments.Directions.GetPath ();
			MKPolyline line = MKPolyline.FromCoordinates (coordinates);

			this.MapView.AddOverlay (line);

			if (coordinates.Length > 0) {
				this.MapView.SetCenterCoordinate (coordinates [0], true);
			}

			List<SegmentPinWrapper> annotations = new List<SegmentPinWrapper> ();
			this.segments.ForEach (segment => annotations.Add(new SegmentPinWrapper(segment)));
//			this.MapView.AddAnnotationObjects (annotations.ToArray ());

			this.MapView.AddAnnotations (annotations.ToArray ());

			if (coordinates.Length == 0) {
				return;
			}

			double minX = Int32.MaxValue;
			double maxX = Int32.MinValue;
			double minY = Int32.MaxValue;
			double maxY = Int32.MinValue;
			double avrX = 0.0d;
			double avrY = 0.0d;
			for (int i = 0; i < coordinates.Length; i++) {
				CLLocationCoordinate2D ccoord = coordinates [i];
				if (minX > ccoord.Latitude) {
					minX = ccoord.Latitude;
				}
				if (maxX < ccoord.Latitude) {
					maxX = ccoord.Latitude;
				}

				if (minY > ccoord.Longitude) {
					minY = ccoord.Longitude;
				}
				if (maxY < ccoord.Longitude) {
					maxY = ccoord.Longitude;
				}
				avrX += ccoord.Latitude;
				avrY += ccoord.Longitude;
			}
			avrX = avrX / (double)coordinates.Length;
			avrY = avrY / (double)coordinates.Length;
			var center = new CLLocationCoordinate2D (avrX, avrY);

			var distX = new CLLocation (minX, center.Longitude).DistanceFrom (new CLLocation (maxX, center.Longitude));
			var distY = new CLLocation (center.Latitude, minY).DistanceFrom (new CLLocation (center.Latitude, maxY));

			MKCoordinateRegion reg = MKCoordinateRegion.FromDistance (center, distX, distY);
			this.MapView.SetRegion (reg, true);

		}

	}
}

