using System;
using Java.Util;
using Java.Lang;
using System.Collections.Generic;
using Mxp.Core.Services.Google;
using Mxp.Core.Utils;
using Android.Gms.Maps.Model;
using System.Linq;

namespace Mxp.Droid.Utils
{
	public static class DirectionsExtentions
	{
		public static IIterable GetPath (this Directions directions) {
			IEnumerable<Coordinate> coordinates = directions.Path;
			
			ArrayList path = new Java.Util.ArrayList (coordinates.Count());

			coordinates.ForEach (coordinate => {
				path.Add (new LatLng (coordinate.lat, coordinate.lng));
			});

			return path;
		}
	}
}