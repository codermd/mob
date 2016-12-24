using Mxp.Core.Business;
using Mxp.Core.Services;
using Mxp.Core.Services.Google;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;


namespace Mxp.Win
{
    public sealed partial class MileageSegmentUC : UserControl
    {
        public MileageSegmentUC(MileageSegment segment, int index)
        {
            this.InitializeComponent();
            this.DataContext = segment;
            MileageSegment = segment;
        }

        public MileageSegment MileageSegment { get; set; }
        public int Index { get { return ((ListView)this.Parent).Items.IndexOf(this); } }
        private void GridLoaded(object sender, RoutedEventArgs e)
        {
            if (((Grid)sender).DataContext != null)
            {
                MileageSegment = ((Grid)sender).DataContext as MileageSegment;
                if (MileageSegment.LocationAliasName != null)
                {
                    this.SearchTB.Text = MileageSegment.LocationAliasName;
                }
            }
            MileageSegment.PropertyChanged += SegmentPropertyChanged;
        }
        private void SegmentPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (MileageSegment.LocationAliasName != null)
                this.SearchTB.Text = MileageSegment.LocationAliasName;
        }
        List<Prediction> Predictions { get; set; }
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
                    messageDialog.Commands.Add(new UICommand((LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.Accept)), (command) => { }));
                    messageDialog.ShowAsync();
                    return;
                }
            
        }
        private async void SearchTB_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            Prediction item = ((PredictionResultItem.Item)args.SelectedItem).PredictionItem;
            try
            {
                this.SearchTB.TextChanged -= SearchTB_TextChanged;
                await this.MileageSegment.FetchLocationsAsync(item);
                this.SearchTB.TextChanged += SearchTB_TextChanged;
            }
            catch (Exception error)
            {
                MessageDialog messageDialog = new MessageDialog(error.GetExceptionMessage());
                messageDialog.Commands.Add(new UICommand((LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.Accept)), (command) => { }));
                messageDialog.ShowAsync();
                return;
            }
        }
        public void ChangeButtonVisibility(Windows.UI.Xaml.Visibility visibility)
        {
            this.DeleteButton.Visibility = visibility;
        }
        private void DeleteSegment_Click(object sender, TappedRoutedEventArgs e)
        {
            if (DeleteSegmentRequest != null)
            {
                DeleteSegmentRequest(this, EventArgs.Empty);
                MileageSegment.PropertyChanged -= SegmentPropertyChanged;
            }
        }
        public event EventHandler DeleteSegmentRequest;
        private async void Image_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Geolocator geolocator = new Geolocator();
            geolocator.DesiredAccuracy = PositionAccuracy.Default;         
            MileageDetailView.Progressring.IsActive = true;
            Geoposition Position = await geolocator.GetGeopositionAsync(
            maximumAge: TimeSpan.FromSeconds(20),
            timeout: TimeSpan.FromSeconds(5)
            );
            try
            {
                double latitude = Position.Coordinate.Latitude;
                double longitude = Position.Coordinate.Longitude;
                MileageSegment.SetCurrentLocation(latitude, longitude);
            }
            catch (ValidationError error)
            {
                MessageDialog messageDialog = new MessageDialog(error.Verbose);
                messageDialog.Commands.Add(new UICommand((LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.Accept)), (command) => { }));
                messageDialog.ShowAsync();

                MileageDetailView.Progressring.IsActive = false;
                return;
            }
            catch (Exception ex)
            {
                if ((uint)ex.HResult == 0x80004004)
                {
                    MessageDialog messageDialog = new MessageDialog(ex.Message + "\n\nlocation  is disabled in phone settings.");
                    messageDialog.Commands.Add(new UICommand((LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.Accept)), (command) => { }));
                    messageDialog.ShowAsync();
                }
                else
                {
                    MessageDialog messageDialog = new MessageDialog(ex.Message);
                    messageDialog.Commands.Add(new UICommand((LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.Accept)), (command) => { }));
                    messageDialog.ShowAsync();
                }

                MileageDetailView.Progressring.IsActive = false;
            }
            MileageDetailView.Progressring.IsActive = false;
        }    
    }
}