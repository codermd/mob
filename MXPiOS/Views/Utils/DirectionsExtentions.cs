using System;
using CoreLocation;
using Mxp.Core.Services.Google;
using System.Collections.Generic;
using Mxp.Core.Utils;
using System.Linq;

namespace Mxp.iOS.Utils
{
	public static class DirectionsExtension
	{
		public static CLLocationCoordinate2D[] GetPath (this Directions directions) {
			if (directions == null) {
				return new CLLocationCoordinate2D []{};
			}
			IEnumerable<Coordinate> coordinates = directions.Path;

			CLLocationCoordinate2D[] path = new CLLocationCoordinate2D [coordinates.Count ()];

			coordinates.ForEach ((coordinate, index) => {
				path[index] = new CLLocationCoordinate2D (coordinate.lat, coordinate.lng);
			});

			return path;
		}
	}
}