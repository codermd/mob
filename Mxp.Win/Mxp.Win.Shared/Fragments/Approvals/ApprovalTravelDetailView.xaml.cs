using Mxp.Core.Business;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
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


namespace Mxp.Win
{
    public sealed partial class ApprovalTravelDetailView : Page
    {
        private ListView FieldsListView;
        private ListView FlightsListView;
        private ListView StayListView;
        private ListView RentalCarListView;
        TravelApproval TravelApproval { get; set; }


        public Collection<TableSectionModel> CollectionFields { get; set; }
        public Collection<TableSectionModel> CollectionFlights { get; set; }
        public Collection<TableSectionModel> CollectionStay { get; set; }
        public Collection<TableSectionModel> CollectionRentalCar { get; set; }


        public Report Report { get; set; }
        public Collection<Field> Fields { get; set; }

        private readonly NavigationHelper navigationHelper;
        private readonly ObservableDictionary defaultViewModel = new ObservableDictionary();
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }
        private void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            if (e.PageState != null && e.PageState.ContainsKey("Report"))
                Report = e.PageState["Report"] as Report;
            if (e.PageState != null && e.PageState.ContainsKey("TravelApproval"))
                TravelApproval = e.PageState["TravelApproval"] as TravelApproval;
        }
        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
            if (TravelApproval != null)
                e.PageState["TravelApproval"] = TravelApproval;
            if (Report != null)
                e.PageState["Report"] = Report;
        }
        private void InitializeNavigationHelper()
        {
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            navigationHelper.SaveState += NavigationHelper_SaveState;
        }


        public ApprovalTravelDetailView()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            InitializeNavigationHelper();
            HardwareButtons.BackPressed += HardwareButtons_BackPressed;
            FlightHeader.Text = (LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.Flight));
            StayHeader.Text = (LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.Stay));
            CarHeader.Text = (LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.CarRental));
            DetailsHeader.Text = (LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.Details));
            ApprooveButton.Label = (LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.Approve));
        }
        void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            e.Handled = true;
            Frame.GoBack();
        }
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedFrom(e);
            HardwareButtons.BackPressed -= HardwareButtons_BackPressed;
        }

        private void HubReport_SectionsInViewChanged(object sender, SectionsInViewChangedEventArgs e)
        {

        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedTo(e);
            if (e.Parameter != null)
            {
                TravelApproval = e.Parameter as TravelApproval;
                Title.Text = TravelApproval.VDetailsBarTitle;
                CollectionFields = TravelApproval.Travel.GetMainFields();
                CollectionFlights = TravelApproval.Travel.GetFlightsFields();
                CollectionStay = TravelApproval.Travel.GetStayFields();
                CollectionRentalCar = TravelApproval.Travel.GetCarRentalsFields();
            }
            BindModels();
        }
        public void BindModels()
        {
        }

        private void FieldListLoaded(object sender, RoutedEventArgs e)
        {
            FieldsListView = (ListView)sender;
            FillDetailsList();

        }
        private async void FillDetailsList()
        {
            List<FieldGroup> items = new List<FieldGroup>();
            if (CollectionFields != null)
            {
                foreach (TableSectionModel col in CollectionFields)
                {
                    List<Field> fields = col.Fields.ToList<Field>();
                    foreach (Field field in fields)
                    {
                        FieldGroup item = new FieldGroup(field, col.Title);
                        //await Task.Delay(TimeSpan.FromSeconds(0.5));
                        items.Add(item);
                    }
                }
            }
            ActivateProgressRing();
            var result = from t in items
                         group t by t.Group into g
                         select new { Key = g.Key, ItemsField = g };
            DetailFieldsSource.Source = result;
            await Task.Delay(TimeSpan.FromSeconds(1));
            DesactivateProgressRing();


        }
        private void ActivateProgressRing()
        {
            ProgressRing.IsActive = true;
            BottomAppBar.IsEnabled = false;
            ProgressGrid.Visibility = Visibility.Visible;
        }
        private void DesactivateProgressRing()
        {
            ProgressRing.IsActive = false;
            BottomAppBar.IsEnabled = true;
            ProgressGrid.Visibility = Visibility.Collapsed;
        }

        public static ScrollViewer GetScrollViewer(DependencyObject depObj)
        {
            if (depObj is ScrollViewer) return depObj as ScrollViewer;

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
            {
                var child = VisualTreeHelper.GetChild(depObj, i);

                var result = GetScrollViewer(child);
                if (result != null) return result;
            }
            return null;
        }
        private void Grid_Tapped(object sender, TappedRoutedEventArgs e)
        {
            e.Handled = true;
        }

        private void FlightsListLoaded(object sender, RoutedEventArgs e)
        {
            List<FieldGroup> items = new List<FieldGroup>();
            if (CollectionFlights != null)
            {
                foreach (TableSectionModel col in CollectionFlights)
                {
                    List<Field> fields = col.Fields.ToList<Field>();
                    foreach (Field field in fields)
                    {
                        FieldGroup item = new FieldGroup(field, col.Title);
                        items.Add(item);
                    }
                }
            }
            var result = from t in items
                         group t by t.Group into g
                         select new { Key = g.Key, ItemsField = g };
            FlightsFieldsSource.Source = result;
        }

        private void StayListLoaded(object sender, RoutedEventArgs e)
        {
            List<FieldGroup> items = new List<FieldGroup>();
            if (CollectionStay != null)
            {
                foreach (TableSectionModel col in CollectionStay)
                {
                    List<Field> fields = col.Fields.ToList<Field>();
                    foreach (Field field in fields)
                    {
                        FieldGroup item = new FieldGroup(field, col.Title);
                        items.Add(item);
                    }
                }
            }
            var result = from t in items
                         group t by t.Group into g
                         select new { Key = g.Key, ItemsField = g };
            StayFieldsSource.Source = result;
        }

        private void RentalCarListLoaded(object sender, RoutedEventArgs e)
        {
            List<FieldGroup> items = new List<FieldGroup>();
            if (CollectionRentalCar != null)
            {
                foreach (TableSectionModel col in CollectionRentalCar)
                {
                    List<Field> fields = col.Fields.ToList<Field>();
                    foreach (Field field in fields)
                    {
                        FieldGroup item = new FieldGroup(field, col.Title);
                        items.Add(item);
                    }
                }
            }
            var result = from t in items
                         group t by t.Group into g
                         select new { Key = g.Key, ItemsField = g };
            RentalCarFieldsSource.Source = result;
        }

        private void Approove_Clicked(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(AcceptApprovalPage), TravelApproval.Travel);
        }
    }
}
