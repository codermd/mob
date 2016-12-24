using Mxp.Core.Business;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Phone.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.ComponentModel;
using Mxp.Core.Services.Google;
using Mxp.Core.Services;
using Windows.UI.Popups;
using Windows.Devices.Geolocation;
using System.Diagnostics;

namespace Mxp.Win
{

    public sealed partial class NewMileageSegmentPage : Page
    {
        Mileage Mileage { get; set; }
        MileageSegment MileageSegment { get; set; }
        List<Prediction> Predictions { get; set; }

        public NewMileageSegmentPage()
        {
            this.InitializeComponent();
            MileageSegment = new MileageSegment();
            HardwareButtons.BackPressed += HardwareButtons_BackPressed;
            Title.Text = (LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.Location));
        }
        void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            e.Handled = true;
            Frame.GoBack();
        }
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            HardwareButtons.BackPressed -= HardwareButtons_BackPressed;
            this.Mileage.PropertyChanged -= HandlePropertyChanged;
        }

        private void HandlePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            //to implement if need
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Mileage = e.Parameter as Mileage;
            Mileage.PropertyChanged += HandlePropertyChanged;
        }

        private async void SearchTB_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            Prediction item = ((PredictionResultItem.Item)args.SelectedItem).PredictionItem;
            try
            {
                this.ProgressRing.IsActive = true;
                this.BottomAppBar.IsEnabled = false;
                await this.MileageSegment.FetchLocationsAsync(item);
            }
            catch (Exception error)
            {
                MessageDialog messageDialog = new MessageDialog(error.GetExceptionMessage());
                messageDialog.Commands.Add(new UICommand("OK", (command) => { }));
                messageDialog.ShowAsync();

                this.ProgressRing.IsActive = false;
                this.BottomAppBar.IsEnabled = true;
                return;
            }

            if (MileageSegment != null)
            {
                this.ProgressRing.IsActive = true;
                this.BottomAppBar.IsEnabled = false;
                MileageSegment.SetCollectionParent(Mileage.MileageSegments);
                this.Mileage.MileageSegments.AddItem(this.MileageSegment);
                Frame.Navigate(typeof(MileageDetailView), Mileage);

            }
            this.ProgressRing.IsActive = false;
            this.BottomAppBar.IsEnabled = true;
        }

        private async void SearchTB_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {

            try
            {
                Predictions = (await GoogleService.Instance.FetchPlacesLocationsAsync(SearchTB.Text)).predictions;
                PredictionResultItem packagesResult = new PredictionResultItem();
                foreach (Prediction item in Predictions)
                {
                    packagesResult.items.Add(new Mxp.Win.PredictionResultItem.Item(item));
                }
                sender.ItemsSource = packagesResult.items;
            }
            catch (Exception error)
            {
                MessageDialog messageDialog = new MessageDialog(error.GetExceptionMessage());
                messageDialog.Commands.Add(new UICommand("OK", (command) => { }));
                messageDialog.ShowAsync();
                return;
            }
        }



        private async void GoBack_Click(object sender, RoutedEventArgs e)
        {
            if (!this.Mileage.MileageSegments.IsFirstEqualsLastSegment)
            {
                this.Mileage.MileageSegments.AddReturningItem();
                Frame.Navigate(typeof(MileageDetailView), Mileage);
            }
        }

        private void UserProgressRing_Tapped(object sender, TappedRoutedEventArgs e)
        {
            e.Handled = true;
        }

        private void SearchTB_Loaded(object sender, RoutedEventArgs e)
        {
            SearchTB.Focus(FocusState.Keyboard);
        }
        private async void Image_Tapped(object sender, TappedRoutedEventArgs e)
        {


            Geolocator geolocator = new Geolocator();
            geolocator.DesiredAccuracyInMeters = 10;

            try
            {
                this.ProgressRing.IsActive = true;
                this.BottomAppBar.IsEnabled = false;

                Geoposition geoposition = await geolocator.GetGeopositionAsync(
                    maximumAge: TimeSpan.FromSeconds(1),
                    timeout: TimeSpan.FromSeconds(10)
                    );

                double latitude = geoposition.Coordinate.Latitude;
                double longitude = geoposition.Coordinate.Longitude;
                MileageSegment.SetCurrentLocation(latitude, longitude);
                MileageSegment.SetCollectionParent(Mileage.MileageSegments);
                this.Mileage.MileageSegments.AddItem(this.MileageSegment);
                Frame.Navigate(typeof(MileageDetailView), Mileage);



            }
            catch (ValidationError error)
            {
                MessageDialog messageDialog = new MessageDialog(error.Verbose);
                messageDialog.Commands.Add(new UICommand("OK", (command) => { }));
                messageDialog.ShowAsync();

                this.ProgressRing.IsActive = false;
                this.BottomAppBar.IsEnabled = true;
                return;
            }
            catch (Exception ex)
            {
                if ((uint)ex.HResult == 0x80004004)
                {
                    MessageDialog messageDialog = new MessageDialog(ex.Message + "\n\nlocation  is disabled in phone settings.");
                    messageDialog.Commands.Add(new UICommand("OK", (command) => { }));
                    messageDialog.ShowAsync();
                }
                else
                {
                    MessageDialog messageDialog = new MessageDialog(ex.Message);
                    messageDialog.Commands.Add(new UICommand("OK", (command) => { }));
                    messageDialog.ShowAsync();
                }

                this.ProgressRing.IsActive = false;
                this.BottomAppBar.IsEnabled = true;
            }
        }


    }
}
