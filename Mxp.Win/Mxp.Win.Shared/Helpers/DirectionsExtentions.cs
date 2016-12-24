using Mxp.Core.Services.Google;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Windows.Devices.Geolocation;
using Windows.Foundation;

namespace Mxp.Win.Helpers
{
    public static class DirectionsExtentions
    {
        public static IEnumerable<BasicGeoposition> GetPath(this Directions directions)
        {
            IEnumerable<Coordinate> coordinates = directions.Path;

            List<BasicGeoposition> path = new List<BasicGeoposition>();


            foreach (Coordinate coordinate in coordinates)
            {
                BasicGeoposition pos = new BasicGeoposition();
                pos.Latitude = coordinate.lat;
                pos.Longitude = coordinate.lng;
                path.Add(pos);
                
            }
            return path;
        }
    }
}
