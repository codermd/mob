using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Controls.Maps;
using Windows.Phone.UI.Input;
using Mxp.Core.Business;
using System.Collections.ObjectModel;
using Windows.UI.Popups;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Mxp.Win
{
    public sealed partial class MileageDetailView : Page
    {
        public Mileage Mileage { get; set; }
        public static ProgressRing Progressring;
        ExpenseItem ExpenseItem { get; set; }
        public Collection<TableSectionModel> CollectionFields { get; set; }
        public Collection<Field> Fields { get; set; }
        public MileageSegments MileageSegments { get; set; }
        private readonly NavigationHelper navigationHelper;
        private readonly ObservableDictionary defaultViewModel = new ObservableDictionary();
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }
        private void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            if (e.PageState != null && e.PageState.ContainsKey("Mileage"))
                Mileage = e.PageState["Mileage"] as Mileage;
            if (e.PageState != null && e.PageState.ContainsKey("MileageSegments"))
                MileageSegments = e.PageState["MileageSegments"] as MileageSegments;
            if (e.PageState != null && e.PageState.ContainsKey("ExpenseItem"))
                ExpenseItem = e.PageState["ExpenseItem"] as ExpenseItem;
        }
        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
            if (ExpenseItem != null)
                e.PageState["ExpenseItem"] = ExpenseItem;
            if (MileageSegments != null)
                e.PageState["MileageSegments"] = MileageSegments;
            if (Mileage != null)
                e.PageState["Mileage"] = Mileage;
        }
        private void InitializeNavigationHelper()
        {
            navigationHelper.LoadState += this.NavigationHelper_LoadState;
            navigationHelper.SaveState += NavigationHelper_SaveState;
        }
        public MileageDetailView()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            InitializeNavigationHelper();
            LoadLabels();
            HardwareButtons.BackPressed += HardwareButtons_BackPressed;

        }

        private async void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            e.Handled = true;
            ActivateProgressRing();
            try
            {
                if (Mileage.IsChanged || ExpenseItem.IsChanged)
                    await LoggedUser.Instance.BusinessExpenses.FetchAsync();
            }
            catch (Exception error)
            {
                PopMessages.AsyncMessage(error.Message);
            }
            DesactivateProgressRing();
        }

        private void LoadLabels()
        {
            Title.Text = (LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.Mileage));
            SaveButton.Label = (LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.Save));
            DiscardButton.Label = (LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.Cancel));
            DeleteButton.Label = (LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.Delete));
            SegmentDetails.Text = (LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.SegmentDetails));
            MapButton.Label = (LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.ShowMap));
            ButtonAddLocation.Content = "+" + (LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.Location));
        }
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedFrom(e);
            this.Mileage.PropertyChanged -= HandlePropertyChanged;
            HardwareButtons.BackPressed -= HardwareButtons_BackPressed;
        }
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedTo(e);
            while (this.navigationHelper.CanGoForward())
                this.Frame.ForwardStack.RemoveAt(0);
            if (Mileage == null)
                Mileage = (Mileage)e.Parameter;
            if (MileageSegments == null)
            {
                MileageSegments = Mileage.MileageSegments;
                if (!Mileage.IsNew)
                    await this.Mileage.MileageSegments.FetchAsync();
            }
            if (ExpenseItem == null)
                ExpenseItem = Mileage.ExpenseItems[0];
            else
                await this.Mileage.Vehicles.FetchAsync();
            mapClicked = false;
            int i = 0;
            foreach (MileageSegment segment in MileageSegments)
            {
               
                MileageSegmentUC item = new MileageSegmentUC(segment, i);
                item.DeleteSegmentRequest += DeleteSegmentRequest;
                SegmentsListView.Items.Add(item);
                i++;
            }
            if (Mileage.IsNew)
                await this.Mileage.Vehicles.FetchAsync();
            Collection<Field> fields = ExpenseItem.GetAllFields();
            CollectionFields = ExpenseItem.DetailsFields;
            if (!filled)
                FillList();
            DiscardButton.IsEnabled = true;
            SaveButton.IsEnabled = true;
            DeleteButton.Visibility = Visibility.Visible;

            filled = true;
            if (this.Mileage.IsNew || this.Mileage.IsChanged)
            {

                SaveButton.Visibility = Visibility.Visible;
                DiscardButton.Visibility = Visibility.Visible;
            }
            this.Mileage.PropertyChanged += HandlePropertyChanged;
            ButtonAddLocation.Visibility = Visibility.Visible;
            Progressring = this.ProgressRing;
            CheckDeleteButtonVisibility();
        }
        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);
            mapClicked = true;
        }
        bool filled;
        private void CheckDeleteButtonVisibility()
        {
            Visibility v;
            if (this.MileageSegments.CanRemove)
                v = Visibility.Visible;
            else
                v = Visibility.Collapsed;
            foreach (var item in SegmentsListView.Items)
            {
                ((MileageSegmentUC)item).ChangeButtonVisibility(v);
            }
        }
        private void DeleteSegmentRequest(object sender, EventArgs e)
        {
            if (MileageSegments.CanRemove)
            {
                this.MileageSegments.RemoveItemAt(((MileageSegmentUC)sender).Index, true);
                SegmentsListView.Items.Remove(sender);
            }
            CheckDeleteButtonVisibility();
        }
        private void HandlePropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (this.Mileage.IsNew || this.ExpenseItem.IsChanged || this.Mileage.IsChanged)
            {
                SaveButton.Visibility = Visibility.Visible;
                DiscardButton.Visibility = Visibility.Visible;
            }
        }
        private async void SaveButtonClick(object sender, RoutedEventArgs e)
        {
            ActivateProgressRing();

            try
            {
                if (Mileage.IsNew)
                    await Mileage.CreateAsync();
                else
                    await Mileage.SaveAsync();
            }
            catch (ValidationError error)
            {
                MessageDialog messageDialog = new MessageDialog(error.Verbose);
                messageDialog.Commands.Add(new UICommand((LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.Accept)), (command) => { }));
                messageDialog.ShowAsync();
                DesactivateProgressRing();
                return;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            SaveButton.Visibility = Visibility.Collapsed;
            DiscardButton.Visibility = Visibility.Collapsed;
            DesactivateProgressRing();

            Frame.GoBack();
        }
        private void DiscardButton_Click(object sender, RoutedEventArgs e)
        {
            if (ExpenseItem != null)
            {
                ActivateProgressRing();
                try
                {
                    Frame.GoBack();
                    mapClicked = false;
                }
                catch (Exception error)
                {
                    MessageDialog messageDialog = new MessageDialog(error.GetExceptionMessage());
                    messageDialog.Commands.Add(new UICommand((LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.Accept)), (command) => { }));
                    messageDialog.ShowAsync();

                    return;
                }
                SaveButton.Visibility = Visibility.Collapsed;
                DiscardButton.Visibility = Visibility.Collapsed;
                DesactivateProgressRing();
            }
        }
        private async void FillList()
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
            //Progressring.IsActive = true;
            BottomAppBar.IsEnabled = false;
            ProgressGrid.Visibility = Visibility.Visible;
            var result = from t in items
                         group t by t.Group into g
                         select new { Key = g.Key, ItemsField = g };
            DetailFieldsSource.Source = result;
            await Task.Delay(TimeSpan.FromSeconds(1));
            //Progressring.IsActive = false;
            BottomAppBar.IsEnabled = true;
            ProgressGrid.Visibility = Visibility.Collapsed;
        }
        private void ActivateProgressRing()
        {
            Progressring.IsActive = true;
            BottomAppBar.IsEnabled = false;
            ProgressGrid.Visibility = Visibility.Visible;
        }
        private void DesactivateProgressRing()
        {
            Progressring.IsActive = false;
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
        private async void AddLocation_Click(object sender, RoutedEventArgs e)
        {
            if (MileageSegments.CanAdd)
            {
                MileageSegments.AddNewItem();
                MileageSegmentUC item = new MileageSegmentUC(MileageSegments[MileageSegments.Count - 1], MileageSegments.Count - 1);
                item.DeleteSegmentRequest += DeleteSegmentRequest;
                SegmentsListView.Items.Add(item);
            }
            CheckDeleteButtonVisibility();
        }
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            MessageDialog deleteDialog = new MessageDialog(LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.Delete) + " " + LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.Mileage));
            deleteDialog.Commands.Add(new UICommand((LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.Accept)), (command) => { DeleteMileage(); }));
            deleteDialog.Commands.Add(new UICommand((LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.Cancel)), (command) => { }));
            deleteDialog.ShowAsync();
        }
        private async void DeleteMileage()
        {
            ActivateProgressRing();
            try
            {
                await this.ExpenseItem.DeleteExpenseAsync();
            }
            catch (ValidationError error)
            {
                MessageDialog messageDialog = new MessageDialog(error.Verbose);
                messageDialog.Commands.Add(new UICommand((LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.Accept)), (command) => { }));
                messageDialog.ShowAsync();
                DesactivateProgressRing();
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
                DesactivateProgressRing();
            }
            Frame.GoBack();
            mapClicked = false;
        }

        private void GoBack_Click(object sender, RoutedEventArgs e)
        {
            if (!this.MileageSegments.IsFirstEqualsLastSegment && this.MileageSegments.CanAdd)
            {
                this.MileageSegments.AddReturningItem();
                MileageSegmentUC item = new MileageSegmentUC(MileageSegments[MileageSegments.Count - 1], MileageSegments.Count - 1);
                item.DeleteSegmentRequest += DeleteSegmentRequest;
                SegmentsListView.Items.Add(item);
                CheckDeleteButtonVisibility();
            }
        }
        static bool mapClicked;
        private void MapButton_Clicked(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MapPage), Mileage);
        }
    }
}
