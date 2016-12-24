using Mxp.Core.Business;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Core;
using Windows.Phone.UI.Input;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;


namespace Mxp.Win
{
    public sealed partial class OpenReportDetailView
    {
        ReceiptsGallery ReceiptsGallery { get; set; }
        public Collection<TableSectionModel> CollectionFields { get; set; }
        public Report Report { get; set; }

        public Expenses Expenses { get; set; }
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
                this.Report = e.PageState["Report"] as Report;
            if (e.PageState != null && e.PageState.ContainsKey("Expenses"))
                this.Expenses = e.PageState["Expenses"] as Expenses;
        }
        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
            if (this.Report != null)
                e.PageState["Report"] = this.Report;
            if (this.Expenses != null)
                e.PageState["Expenses"] = this.Expenses;
        }
        private void InitializeNavigationHelper()
        {
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;
        }


        public OpenReportDetailView()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            this.InitializeNavigationHelper();
            this.view = CoreApplication.GetCurrentView();
            HardwareButtons.BackPressed += this.HardwareButtons_BackPressed;
            this.LoadLabels();
        }

        private void LoadLabels()
        {
            this.Title.Text = (LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.CreateReport));
            this.ReceiptsHeader.Text = (LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.Receipts));
            this.ExpensesHeader.Text = (LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.Expenses));
            this.HistoryHeader.Text = (LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.History));
            this.DetailsHeader.Text = (LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.Details));
            this.DeleteReport_Button.Label = (LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.Delete));
        }

        void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            e.Handled = true;
        }
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            HardwareButtons.BackPressed -= this.HardwareButtons_BackPressed;
            this.navigationHelper.OnNavigatedFrom(e);
        }
        private void HubReport_SectionsInViewChanged(object sender, SectionsInViewChangedEventArgs e)
        {
            var tag = this.HubReport.SectionsInView[0].Tag.ToString();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedTo(e);
            if (e.Parameter != null)
            {
                if (e.Parameter.GetType() == typeof(Report))
                {
                    this.Report = e.Parameter as Report;
                    this.CollectionFields = new Collection<TableSectionModel>();
                    this.CollectionFields.Add(new TableSectionModel(Labels.GetLoggedUserLabel(Labels.LabelEnum.General), this.Report.GetMainFields()));
                    this.CollectionFields.Add(new TableSectionModel(Labels.GetLoggedUserLabel(Labels.LabelEnum.Details), this.Report.GetAllFields()));
                    this.Title.Text = this.Report.VDetailsBarTitle;
                    this.ReceiptsSection.Visibility = this.Report.CanShowReceipts ? Visibility.Visible : Visibility.Collapsed;
                    this.HistorySection.Visibility = this.Report.CanShowHistory ? Visibility.Visible : Visibility.Collapsed;
                }
            }
            if (this.Report.IsClosed)
            {
                this.DeleteReport_Button.Visibility = Visibility.Collapsed;
                this.CommandBar.Visibility = Visibility.Collapsed;
            }
            this.CheckMargins();
        }
        public void ReceiptsProgressStart()
        {
            this.ReceiptsProgressGrid.Visibility = Visibility.Visible;
            this.ReceiptsProgressRing.IsActive = true;
        }
        public void ReceiptsProgressFinish()
        {
            this.ReceiptsProgressGrid.Visibility = Visibility.Collapsed;
            this.ReceiptsProgressRing.IsActive = false;
        }
        Grid ReceiptsProgressGrid;
        ProgressRing ReceiptsProgressRing;
        private void ReceiptsProgressGrid_Loaded(object sender, RoutedEventArgs e)
        {
            this.ReceiptsProgressGrid = (Grid)sender;
        }

        private void ReceiptsProgressRing_Loaded(object sender, RoutedEventArgs e)
        {
            this.ReceiptsProgressRing = (ProgressRing)sender;
        }



        private void CheckMargins()
        {
            int cpt = 0;
            foreach (HubSection section in this.HubReport.Sections)
            {
                if (section.Visibility == Visibility.Visible)
                    cpt++;
            }
            if (cpt == 2)
                this.DetailsSection.Margin = new Thickness(0, 0, 60, 0);
        }

        private void FieldListLoaded(object sender, RoutedEventArgs e)
        {
            this.FillDetailsList();
        }
        private async void FillDetailsList()
        {
            List<FieldGroup> items = new List<FieldGroup>();
            if (this.CollectionFields != null)
            {
                foreach (TableSectionModel col in this.CollectionFields)
                {
                    List<Field> fields = col.Fields.ToList<Field>();
                    foreach (Field field in fields)
                    {
                        FieldGroup item = new FieldGroup(field, col.Title);
                        await Task.Delay(TimeSpan.FromSeconds(0.1));
                        items.Add(item);
                    }
                }
            }
            this.ActivateProgressRing();
            var result = from t in items
                         group t by t.Group into g
                         select new { Key = g.Key, ItemsField = g };
            this.DetailFieldsSource.Source = result;
            await Task.Delay(TimeSpan.FromSeconds(1));
            this.DesactivateProgressRing();

        }
        private void ActivateProgressRing()
        {
            this.ProgressRing.IsActive = true;
            this.BottomAppBar.IsEnabled = false;
            this.ProgressGrid.Visibility = Visibility.Visible;
        }
        private void DesactivateProgressRing()
        {
            this.ProgressRing.IsActive = false;
            this.BottomAppBar.IsEnabled = true;
            this.ProgressGrid.Visibility = Visibility.Collapsed;
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

        private void ReceiptsGallery_Loaded(object sender, RoutedEventArgs e)
        {
            this.ReceiptsGallery = sender as ReceiptsGallery;
            if (this.Report != null)
            {
                try
                {
                    this.ReceiptsProgressStart();
                    this.ReceiptsGallery.LoadPhotos(this.Report);

                }
                catch (ValidationError error)
                {
                    MessageDialog messageDialog = new MessageDialog(error.Verbose);
                    messageDialog.Commands.Add(new UICommand("OK", (command) => { }));
                    messageDialog.ShowAsync();

                    this.BottomAppBar.IsEnabled = true;
                    this.ReceiptsProgressFinish();
                }
                catch (Exception error)
                {
                    Debug.WriteLine(error.Message);
                    this.BottomAppBar.IsEnabled = true;
                    this.ReceiptsProgressFinish();
                }
                this.ReceiptsProgressFinish();
            }
        }

        private async void ExpensesList_Loaded(object sender, RoutedEventArgs e)
        {
            this.Expenses = this.Report.Expenses;
            var result = from t in this.Expenses.ToList<Expense>()
                         group t by t.VDateHeader into g
                         select new { Key = g.Key, Items = g };
            this.ExpensesSource.Source = result;
        }

        CoreApplicationView view;
        private void Add_Receipt(object sender, RoutedEventArgs e)
        {
            this.ReceiptsProgressStart();
            FileOpenPicker filePicker = new FileOpenPicker();
            filePicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            filePicker.ViewMode = PickerViewMode.Thumbnail;
            filePicker.FileTypeFilter.Clear();
            filePicker.FileTypeFilter.Add(".bmp");
            filePicker.FileTypeFilter.Add(".png");
            filePicker.FileTypeFilter.Add(".jpeg");
            filePicker.FileTypeFilter.Add(".jpg");
            filePicker.PickSingleFileAndContinue();
            this.view.Activated += this.viewActivated;
            this.ReceiptsProgressFinish();
        }
        private async void viewActivated(CoreApplicationView sender, IActivatedEventArgs args1)
        {
            this.ReceiptsProgressStart();
            FileOpenPickerContinuationEventArgs args = args1 as FileOpenPickerContinuationEventArgs;
            if (args != null)
            {
                if (args.Files.Count == 0) return;
                this.view.Activated -= this.viewActivated;
                StorageFile storageFile = args.Files[0];
                string Base64str = await BitmapHelper.CompressFile(storageFile);
                Debug.WriteLine(Base64str);
                if (this.Report != null)
                {
                    try
                    {
                        await this.Report.Receipts.AddReceipt(Base64str);
                    }
                    catch (ValidationError error)
                    {
                        MessageDialog messageDialog = new MessageDialog(error.Verbose);
                        messageDialog.Commands.Add(new UICommand((LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.Accept)), (command) => { }));
                        messageDialog.ShowAsync();
                        this.ReceiptsProgressFinish();
                    }
                    catch (Exception error)
                    {
                        Debug.WriteLine(error.Message);
                        this.ReceiptsProgressFinish();
                    }
                    try
                    {
                        await this.Report.Receipts.FetchAsync();
                    }
                    catch (ValidationError error)
                    {
                        MessageDialog messageDialog = new MessageDialog(error.Verbose);
                        messageDialog.Commands.Add(new UICommand((LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.Accept)), (command) => { }));
                        messageDialog.ShowAsync();
                        this.ReceiptsProgressFinish();
                    }
                    catch (Exception error)
                    {
                        Debug.WriteLine(error.Message);
                        this.ReceiptsProgressFinish();
                    }
                }

                this.ReceiptsGallery.Showed = false;
                try
                {
                    this.ReceiptsGallery.LoadLastPhoto(this.Report);

                }
                catch (ValidationError error)
                {
                    MessageDialog messageDialog = new MessageDialog(error.Verbose);
                    messageDialog.Commands.Add(new UICommand("OK", (command) => { }));
                    messageDialog.ShowAsync();

                    this.BottomAppBar.IsEnabled = true;
                    this.ReceiptsProgressFinish();
                }
                catch (Exception error)
                {
                    Debug.WriteLine(error.Message);

                    this.BottomAppBar.IsEnabled = true;
                    this.ReceiptsProgressFinish();
                }
                this.ReceiptsProgressFinish();
            }
            this.ReceiptsHeader.Text = (LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.Receipts)) + " (" + this.Report.NumberReceipts + ")";
        }

        private async void DeleteReport_Click(object sender, RoutedEventArgs e)
        {
            var messageDialog = new MessageDialog(LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.DoYouConfirm), LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.Delete) + " " + LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.Report));
            messageDialog.Commands.Add(new UICommand((LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.Yes)), (command) => { this.DeleteReport(); }));
            messageDialog.Commands.Add(new UICommand((LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.No)), (command) => { }));
            messageDialog.DefaultCommandIndex = 1;
            await messageDialog.ShowAsync();
        }

        private async void DeleteReport()
        {
            this.ActivateProgressRing();
            this.DeleteReport_Button.IsEnabled = false;
            try
            {
                await this.Report.CancelAsync();
                this.Frame.GoBack();
            }
            catch (Exception error)
            {
                MessageDialog messageDialog = new MessageDialog(error.GetExceptionMessage());
                messageDialog.Commands.Add(new UICommand((LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.Accept)), (command) => { }));
                messageDialog.ShowAsync();
                this.DesactivateProgressRing();
                this.DeleteReport_Button.IsEnabled = true;
                return;
            }

        }

        private void HistoryList_Loaded(object sender, RoutedEventArgs e)
        {
            ((ListView)sender).ItemsSource = this.Report.History;
        }

        private void AddReceipt_Tapped(object sender, TappedRoutedEventArgs e)
        {
            try
            {
                this.Add_Receipt(null, null);
            }
            catch (Exception error)
            {
                Debug.WriteLine(error.Message);

                this.BottomAppBar.IsEnabled = true;
                MainController.Instance.ReceiptsLoaded();
            }

        }

        private void AddReceiptButton_Loaded(object sender, RoutedEventArgs e)
        {
            if (this.Report.IsClosed)
                ((Image)sender).Visibility = Visibility.Collapsed;
        }
    }
}
