using System;
using Android.Content;
using Android.Gms.Maps;
using Mxp.Core.Services.Google;
using Android.Gms.Maps.Model;
using Android.Widget;
using Mxp.Core.Business;
using Mxp.Core.Utils;
using Android.Graphics;
using Mxp.Droid.Utils;
using System.Threading.Tasks;
using Java.Lang;
using System.Collections.Generic;

namespace Mxp.Droid.Helpers
{
	public class GoogleMapHelper
	{
		private GoogleMap mMap;
		private Context mContext;
		private Mileage mMileage;

		private Polyline mCurrentPolyline;
		private List<Marker> mMarkers;

		public GoogleMapHelper (Context context, Mileage mileage) {
			this.mContext = context;
			this.mMileage = mileage;

			this.mMarkers = new List<Marker> ();
		}

		public void OnMapReady (GoogleMap googleMap) {
			this.mMap = googleMap;

			// TODO Xml equivalent ?
			this.mMap.MapType = GoogleMap.MapTypeTerrain;
			this.mMap.UiSettings.MapToolbarEnabled = false;
//			this.mMap.UiSettings.ZoomControlsEnabled = true;

			this.Refresh ();
		}

		private void Refresh () {
			this.mMap.Clear ();

			this.RefreshMarkersOnMap ();
			this.RefreshPolylinesOnMap ();
		}

		public void RefreshMarkersOnMap () {
			if (this.mMarkers.Count > 0) {
				this.mMarkers.ForEach (marker => marker.Remove ());
				this.mMarkers.Clear ();
			}

			if (this.mMileage.MileageSegments.Count == 0 || !this.mMileage.MileageSegments.IsAnyLocationsValid || this.mMap == null)
				return;

			LatLngBounds.Builder interestZoneBuilder = new LatLngBounds.Builder ();

			this.mMileage.MileageSegments.ForEach ((MileageSegment segment) => {
				if (!segment.IsLocationValid)
					return;
				LatLng point = new LatLng(segment.LocationLatitude.Value, segment.LocationLongitude.Value);
				this.mMarkers.Add (this.mMap.AddMarker (new MarkerOptions().SetPosition (point).SetTitle (segment.LocationAliasName)));
				interestZoneBuilder.Include (point);
			});
				
			CameraUpdate cameraUpdate = CameraUpdateFactory.NewLatLngBounds (interestZoneBuilder.Build(), this.mContext.Resources.DisplayMetrics.WidthPixels, this.mContext.Resources.DisplayMetrics.HeightPixels, 200);
			this.mMap.AnimateCamera (cameraUpdate);
		}

		public void RefreshPolylinesOnMap () {
			if (this.mCurrentPolyline != null) {
				this.mCurrentPolyline.Remove ();
				this.mCurrentPolyline = null;
			}

			if (this.mMileage.MileageSegments.Count < 2 || this.mMap == null)
				return;

			Directions directions = this.mMileage.MileageSegments.Directions;

			if (directions == null)
				return;

			if (directions.IsEmpty) {
				Toast.MakeText (this.mContext, "Can't fetch directions", ToastLength.Short).Show ();
				return;
			}

			PolylineOptions polylineOptions = new PolylineOptions ()
				.InvokeWidth (10)
				.InvokeColor (Color.Blue)
				.AddAll (directions.GetPath ());

			this.mCurrentPolyline = this.mMap.AddPolyline (polylineOptions);
		}
	}
}