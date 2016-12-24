using System;
using Mxp.Core.Business;
using UIKit;
using MapKit;
using CoreLocation;

namespace Mxp.iOS
{
	public class SegmentPinWrapper : MKAnnotation
	{

		private MileageSegment segment;
		public SegmentPinWrapper (MileageSegment segment)
		{
			this.segment = segment;
		}

		public override CLLocationCoordinate2D Coordinate {
			get {
				return new CLLocationCoordinate2D (this.segment.LocationLatitude.GetValueOrDefault(), this.segment.LocationLongitude.GetValueOrDefault());
			}
		}

		public override string Title {
			get {
				return this.segment.LocationAliasName;
			}
		}


	}
}

