using System;
using System.Collections.Generic;
using Mxp.Core.Utils;
using System.Linq;
using Mxp.Core.Business;

namespace Mxp.Core.Services.Google
{
	public class Directions
	{
		public List<Route> routes { get; set; }
		public string status { get; set; }

		public Directions () {

		}

		public bool IsEmpty {
			get {
				return this.routes.Count == 0;
			}
		}

		private int? _totalDistance;
		public int TotalDistance {
			get {
				if (this._totalDistance.HasValue)
					return this._totalDistance.Value;

				if (this.IsEmpty) {
					this._totalDistance = 0;
					return _totalDistance.Value;
				}

				double distance = this.routes [0].legs.Sum (leg => leg.distance.value) / 1000;

				if (Preferences.Instance.MilUnit == Preferences.UnisSystemEnum.Imperial)
					distance *= 0.621371;

				this._totalDistance = (int) distance;

				return _totalDistance.Value;
			}
		}

		public IEnumerable<Coordinate> Path {
			get {
				return this.IsEmpty ? new List<Coordinate> () : Directions.Decode (routes [0].overview_polyline.points);
			}
		}

		/**
		* Decodes an encoded path string into a sequence of LatLngs.
		* Source : Google Maps Utility Library
		*/
		private static IEnumerable<Coordinate> Decode (string encodedPath) {
			int length = encodedPath.Length;

			List<Coordinate> path = new List<Coordinate> ();
			int index = 0;
			int lat = 0;
			int lng = 0;

			while (index < length) {
				int result = 1;
				int shift = 0;
				int b;
				do {
					b = encodedPath.ToCharArray ()[index++] - 63 - 1;
					result += b << shift;
					shift += 5;
				} while (b >= 0x1f);
				lat += (result & 1) != 0 ? ~(result >> 1) : (result >> 1);

				result = 1;
				shift = 0;
				do {
					b = encodedPath.ToCharArray ()[index++] - 63 - 1;
					result += b << shift;
					shift += 5;
				} while (b >= 0x1f);
				lng += (result & 1) != 0 ? ~(result >> 1) : (result >> 1);

				path.Add(new Coordinate(lat * 1e-5, lng * 1e-5));
			}

			return path;
		}
	}
}