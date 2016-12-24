using Mxp.Core.Business;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.Phone.UI.Input;
using Windows.Services.Maps;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;
using Windows.UI.Xaml;
using Mxp.Core.Services.Google;
using System.Diagnostics;
using Mxp.Win.Helpers;

namespace Mxp.Win
{
    public sealed partial class MapPage : Page
    {
        public MapPage()
        {
            this.InitializeComponent();
            Title.Text = (LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.Mileage));
            MapService.ServiceToken = "EJFtBPol_9SBHj_WaoMn9g";
            Map.MapServiceToken = "EJFtBPol_9SBHj_WaoMn9g";

        }
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            HardwareButtons.BackPressed += HardwareButtons_BackPressed;
            Mileage = (Mileage)e.Parameter;
            Map.Loaded += MapLoaded;
        }

        private void MapLoaded(object sender, RoutedEventArgs e)
        {
            if (Mileage.MileageSegments[0].IsLocationValid)
            {

                BasicGeoposition north = new BasicGeoposition();
                BasicGeoposition south = new BasicGeoposition();
                north.Latitude = (double)Mileage.MileageSegments[0].LocationLatitude;
                north.Longitude = (double)Mileage.MileageSegments[0].LocationLongitude;
                south.Latitude = (double)Mileage.MileageSegments[0].LocationLatitude;
                south.Longitude = (double)Mileage.MileageSegments[0].LocationLongitude;
                foreach (MileageSegment segment in Mileage.MileageSegments)
                {
                    if (segment.LocationLatitude != null && segment.LocationLongitude != null)
                    {
                        BasicGeoposition push = new BasicGeoposition();
                        push.Latitude = (double)segment.LocationLatitude;
                        push.Longitude = (double)segment.LocationLongitude;
                        AddPushpin(push);

                        if (push.Latitude > north.Latitude)
                            north = push;
                        if (push.Latitude < south.Latitude)
                            south = push;
                    }
                }
                BasicGeoposition topCorner = new BasicGeoposition();
                topCorner.Latitude = north.Latitude;
                topCorner.Longitude = Math.Min(north.Longitude, south.Longitude);

                BasicGeoposition botCorner = new BasicGeoposition();
                botCorner.Latitude = south.Latitude;
                botCorner.Longitude = Math.Max(north.Longitude, south.Longitude);

                GeoboundingBox box = new GeoboundingBox(topCorner, botCorner);
                ZoomMap(box);


                Directions directions = this.Mileage.MileageSegments.Directions;

                if (directions == null)
                    return;

                if (directions.IsEmpty)
                {
                    Debug.WriteLine("coucou");
                    return;
                }

                MapPolyline line = new MapPolyline();
                line.StrokeColor = Colors.Red;
                line.StrokeThickness = 10;
                Geopath path = new Geopath(directions.GetPath());
                line.Path = path;
                Map.MapElements.Add(line);
            }
        }


        private async void ZoomMap(GeoboundingBox box)
        {

            await Map.TrySetViewBoundsAsync(
                    box,
                    null,
                    MapAnimationKind.Linear);
        }
        public void AddPushpin(BasicGeoposition location)
        {
            var pin = new Grid()
            {
                Width = 24,
                Height = 24,
                Margin = new Windows.UI.Xaml.Thickness(-12)
            };
            pin.Children.Add(new Ellipse()
            {
                Fill = new SolidColorBrush(Colors.DodgerBlue),
                Stroke = new SolidColorBrush(Colors.White),
                StrokeThickness = 3,
                Width = 24,
                Height = 24
            });
            MapControl.SetLocation(pin, new Geopoint(location));
            Map.Children.Add(pin);
        }
        void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            e.Handled = true;
            Frame.GoBack();
        }
        public Mileage Mileage { get; set; }
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            HardwareButtons.BackPressed -= HardwareButtons_BackPressed;
        }
        private async void GetRouteAndDirections(BasicGeoposition startLocation, BasicGeoposition endLocation)
        {
            MapService.ServiceToken = "EJFtBPol_9SBHj_WaoMn9g";
            Map.MapServiceToken = "EJFtBPol_9SBHj_WaoMn9g";
            Geopoint startPoint = new Geopoint(startLocation);
            Geopoint endPoint = new Geopoint(endLocation);
            // Get the route between the points.
            MapRouteFinderResult routeResult =
                await MapRouteFinder.GetDrivingRouteAsync(
                startPoint,
                endPoint,
                MapRouteOptimization.Time,
                MapRouteRestrictions.None);
            if (routeResult.Status == MapRouteFinderStatus.Success)
            {
                // Use the route to initialize a MapRouteView.
                MapRouteView viewOfRoute = new MapRouteView(routeResult.Route);
                viewOfRoute.RouteColor = Colors.Yellow;
                viewOfRoute.OutlineColor = Colors.Black;
                // Add the new MapRouteView to the Routes collection
                // of the MapControl.
                Map.Routes.Add(viewOfRoute);
                // Fit the MapControl to the route.
            }
        }
    }
}