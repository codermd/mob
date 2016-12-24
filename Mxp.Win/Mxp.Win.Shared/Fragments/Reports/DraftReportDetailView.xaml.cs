using Mxp.Core.Business;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
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
    public sealed partial class DraftReportDetailView
    {
        private Grid ReceiptsProgressGrid;
        private ProgressRing ReceiptsProgressRing;
        private CoreApplicationView view;
        private ReceiptsGallery ReceiptsGallery;
        public Collection<TableSectionModel> CollectionFields { get; set; }
        public Report Report { get; set; }
        public Expenses Expenses { get; set; }
        public Collection<Field> Fields { get; set; }
        private static int currentSection = 0;
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
        public DraftReportDetailView()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            this.InitializeNavigationHelper();
            this.view = CoreApplication.GetCurrentView();
            MainController.Instance.ExpenseReceiptRemovedRequest += this.MainControllerExpenseReceiptRemovedRequest;
            this.LoadLabels();

            this.HubReport.DataContext = this;
        }
        void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            e.Handled = true;
        }
        private void LoadLabels()
        {
            this.Title.Text = (LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.CreateReport));
            this.SelectableExpensesHeader.Text = (LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.Expenses));
            this.ReceiptsHeader.Text = (LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.Receipts));
            this.DeletableExpensesHeader.Text = (LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.Expenses));
            this.DetailsHeader.Text = (LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.Details));
            this.DeleteReport_Button.Label = (LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.Delete));
            this.Save_Button.Label = (LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.CreateReport));
            this.Submit_Button.Label = (LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.Submit));
            this.Cancel_Button.Label = (LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.Cancel));
            this.AddExpense_Button.Label = (LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.AddExpense));
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            HardwareButtons.BackPressed -= this.HardwareButtons_BackPressed;
            this.navigationHelper.OnNavigatedFrom(e);
            this.UnbindModels();
        }
        private void HubReport_SectionsInViewChanged(object sender, SectionsInViewChangedEventArgs e)
        {
            var tag = this.HubReport.SectionsInView[0].Tag.ToString();
            currentSection = int.Parse(tag);
            switch (tag)
            {
                case "0":
                    this.AddExpense_Button.Visibility = Visibility.Collapsed;
                    break;
                case "1":
                    this.AddExpense_Button.Visibility = Visibility.Visible;
                    break;
                case "2":
                    this.AddExpense_Button.Visibility = Visibility.Collapsed;
                    break;
                case "3":
                    this.AddExpense_Button.Visibility = Visibility.Collapsed;
                    break;
            }
        }
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedTo(e);
            while (this.navigationHelper.CanGoForward())
                this.Frame.ForwardStack.RemoveAt(0);
            if (e.Parameter != null)
            {
                if (e.Parameter.GetType() == typeof(Report))
                {
                    if (this.Report == null)
                        this.Report = e.Parameter as Report;
                    this.CollectionFields = new Collection<TableSectionModel>();
                    this.CollectionFields.Add(new TableSectionModel(Labels.GetLoggedUserLabel(Labels.LabelEnum.General), this.Report.GetMainFields()));
                    this.CollectionFields.Add(new TableSectionModel(Labels.GetLoggedUserLabel(Labels.LabelEnum.Details), this.Report.GetAllFields()));
                    this.ReceiptsSection.Visibility = this.Report.CanShowReceipts ? Visibility.Visible : Visibility.Collapsed;
                    if (this.Report.IsNew)
                    {
                        this.SelectableExpensesSection.Visibility = Visibility.Visible;
                        this.DeletableExpensesSection.Visibility = Visibility.Collapsed;
                        this.AddExpense_Button.Visibility = Visibility.Collapsed;
                        this.DeleteReport_Button.Visibility = Visibility.Collapsed;
                        this.Save_Button.Visibility = Visibility.Visible;
                        this.Submit_Button.Visibility = Visibility.Collapsed;

                    }
                    else
                    {
                        this.DeletableExpensesSection.Visibility = Visibility.Visible;
                        this.SelectableExpensesSection.Visibility = Visibility.Collapsed;
                        this.Cancel_Button.Visibility = Visibility.Collapsed;
                        this.Save_Button.Visibility = Visibility.Visible;
                        this.Title.Text = this.Report.VDetailsBarTitle;
                        this.Save_Button.Visibility = Visibility.Collapsed;
                        this.Submit_Button.Visibility = Visibility.Visible;
                    }
                    if (this.HubReport.VisibleSectionsCount == 2)
                    {
                        this.HubReport.SelectedIndexChangedRequest += this.Hub_SelectedIndexChangedRequest;
                    }
                    try {
                        await this.Report.Receipts.FetchAsync();
                    } catch (Exception error) {
                        MessageDialog messageDialog = new MessageDialog (error.GetExceptionMessage ());
                        messageDialog.Commands.Add (new UICommand ("OK", (command) => { }));
                        messageDialog.ShowAsync ();
                    }
                    this.ReceiptsHeader.Text = (LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.Receipts)) + " (" + this.Report.NumberReceipts + ")";
                    this.DeletableExpensesHeader.Text = this.DeletableExpensesHeader.Text + " (" + this.Report.Expenses.Count + ")";
                }
            }
            if (currentSection == 1)
            {
                this.HubReport.ScrollToSection(this.DeletableExpensesSection);
                currentSection = 0;
            }
            this.BindModels();
            this.CheckMargins();
            HardwareButtons.BackPressed += this.HardwareButtons_BackPressed;
        }
        public void ReceiptsProgressStart()
        {
            this.ReceiptsProgressGrid.Visibility = Visibility.Visible;
            this.ReceiptsProgressRing.IsActive = true;
        }
        public void ProgressStart()
        {
            this.ProgressGrid.Visibility = Visibility.Visible;
            this.ProgressRing.IsActive = true;
            this.BottomAppBar.IsEnabled = false;
        }
        public void ProgressFinish()
        {
            this.ProgressGrid.Visibility = Visibility.Collapsed;
            this.ProgressRing.IsActive = false;
            this.BottomAppBar.IsEnabled = true;
        }
        private void Grid_Tapped(object sender, TappedRoutedEventArgs e)
        {
            e.Handled = true;
        }
        public void ReceiptsProgressFinish()
        {
            this.ReceiptsProgressGrid.Visibility = Visibility.Collapsed;
            this.ReceiptsProgressRing.IsActive = false;
        }
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
        private void Hub_SelectedIndexChangedRequest(object sender, EventArgs e)
        {
            if (this.HubReport.SelectedIndex == 0)
            {
                this.AddExpense_Button.Visibility = Visibility.Collapsed;
            }
            else if (this.HubReport.SelectedIndex == 1)
            {
                if (this.Report.CanShowReceipts)
                    this.AddExpense_Button.Visibility = Visibility.Visible;
            }
        }
        private void FieldListLoaded(object sender, RoutedEventArgs e)
        {
            this.ProgressStart();
            this.FillDetailsList();
            this.ProgressFinish ();
        }
        private void FillDetailsList()
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
                        //await Task.Delay(TimeSpan.FromSeconds(0.1));
                        items.Add(item);
                    }
                }
            }
            var result = from t in items
                         group t by t.Group into g
                         select new { Key = g.Key, ItemsField = g };
            this.DetailFieldsSource.Source = result;
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

        private void ReceiptsGallery_Loaded(object sender, RoutedEventArgs e)
        {
            this.ReceiptsProgressStart();
            this.ReceiptsGallery = sender as ReceiptsGallery;
            if (this.Report != null)
            {
                try
                {
                    this.ReceiptsGallery.LoadPhotos(this.Report);
                }
                catch (ValidationError error)
                {
                    MessageDialog messageDialog = new MessageDialog(error.Verbose);
                    messageDialog.Commands.Add(new UICommand((LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.Accept)), (command) => { }));
                    messageDialog.ShowAsync();
                    this.ReceiptsProgressFinish();
                    this.BottomAppBar.IsEnabled = true;
                    return;
                }
                catch (Exception error)
                {
                    Debug.WriteLine(error.Message);
                    this.ReceiptsProgressFinish();
                    this.BottomAppBar.IsEnabled = true;
                    return;
                }
            }
            this.ReceiptsProgressFinish();
        }
        private async void ExpensesList_Loaded(object sender, RoutedEventArgs e)
        {
            this.Expenses = this.Report.Expenses;
            var result = from t in this.Expenses.ToList<Expense>()
                         group t by t.VDateHeader into g
                         select new { Key = g.Key, Items = g };

            this.ExpensesSource.Source = result;
        }
        private void Add_Expense(object sender, RoutedEventArgs e)
        {
            currentSection = 1;
            this.Frame.Navigate(typeof(AddReportPage), this.Report);
        }
        private async void ExpensesCreationList_Loaded(object sender, RoutedEventArgs e)
        {
            this.Expenses = LoggedUser.Instance.BusinessExpenses;

            var result = from t in this.Expenses.ToList<Expense>()
                         group t by t.VDateHeader into g
                         select new { Key = g.Key, Items = g };

            this.ExpensesCreationSource.Source = result;
        }
        private void DiscardChanges_Click(object sender, RoutedEventArgs e)
        {
            this.Report.ResetChanged();
            this.navigationHelper.GoBack();
        }
        private async void Save_Report(object sender, RoutedEventArgs e)
        {
            this.ProgressStart();
            if (this.Report.IsNew)
            {
                this.ProgressStart();

				try
                {
				    this.ProgressStart();
                    this.BottomAppBar.IsEnabled = false;


                    await this.Report.SaveAsync();
                    this.Report.NotifyPropertyChanged(string.Empty);
                    this.Frame.Navigate(typeof(DraftReportDetailView), this.Report);

				    this.navigationHelper.GoBack();

                }
                catch (ValidationError error)
                {
                    MessageDialog messageDialog = new MessageDialog(error.Verbose);
                    messageDialog.Commands.Add(new UICommand((LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.Accept)), (command) => { }));
                    messageDialog.ShowAsync();
                    this.ProgressFinish();
                    return;
                }
                catch (Exception error)
                {
                    Debug.WriteLine(error.Message);

                    this.ProgressFinish();
                    return;
                }
            }
            else
            {
                try
                {
                    this.ProgressStart();
                    await this.Report.SubmitAsync();
                    this.navigationHelper.GoBack();

                }
                catch (Exception error)
                {
                    MessageDialog messageDialog = new MessageDialog(error.GetExceptionMessage());
                    messageDialog.Commands.Add(new UICommand((LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.Accept)), (command) => { }));
                    messageDialog.ShowAsync();
                    this.ProgressFinish();
                    return;
                }
            }
            this.ProgressFinish();

        }
        public void BindModels()
        {
            if (this.Report != null)
                this.Report.PropertyChanged += this.HandlePropertyChanged;
        }
        public void UnbindModels()
        {
            this.Report.PropertyChanged -= this.HandlePropertyChanged;
            MainController.Instance.ExpenseReceiptRemovedRequest -= this.MainControllerExpenseReceiptRemovedRequest;
        }
        void HandlePropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (this.Report.IsChanged)
            {
                this.Save_Button.Visibility = Visibility.Visible;
                this.Cancel_Button.Visibility = Visibility.Visible;
            }
            if (this.Report.IsNew)
            {
                this.Save_Button.Visibility = Visibility.Visible;
                this.Cancel_Button.Visibility = Visibility.Visible;
            }
        }
        private void AddReceipt()
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
                        this.BottomAppBar.IsEnabled = true;
                        return;
                    }
                    catch (Exception error)
                    {
                        Debug.WriteLine(error.Message);

                        this.ReceiptsProgressFinish();
                        this.BottomAppBar.IsEnabled = true;
                        return;
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
                        this.BottomAppBar.IsEnabled = true;
                    }
                    catch (Exception error)
                    {
                        Debug.WriteLine(error.Message);
                        this.ReceiptsProgressFinish();
                        this.BottomAppBar.IsEnabled = true;
                    }
                }

                this.BottomAppBar.IsEnabled = true;
                this.ReceiptsGallery.Showed = false;
                try
                {
                    this.ReceiptsGallery.LoadLastPhoto(this.Report);
                }
                catch (ValidationError error)
                {
                    MessageDialog messageDialog = new MessageDialog(error.Verbose);
                    messageDialog.Commands.Add(new UICommand((LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.Accept)), (command) => { }));
                    messageDialog.ShowAsync();
                    this.ReceiptsProgressFinish();
                    this.BottomAppBar.IsEnabled = true;
                    return;
                }
                catch (Exception error)
                {
                    Debug.WriteLine(error.Message);
                    this.ReceiptsProgressFinish();
                    this.BottomAppBar.IsEnabled = true;
                    return;
                }
                this.ReceiptsProgressFinish();

            }
            this.ReceiptsHeader.Text = (LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.Receipts)) + " (" + this.Report.NumberReceipts + ")";
        }
        private async void DeleteReport_Click(object sender, RoutedEventArgs e)
        {
            var messageDialog = new MessageDialog(LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.DoYouConfirm), LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.Delete) + " " + LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.Report));
            messageDialog.Commands.Add(new UICommand((LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.Yes)), (command) => { this.DeleteReport(); }));
            messageDialog.Commands.Add(new UICommand((LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.No)), (command) => {/* autoLogin command here*/ }));
            messageDialog.DefaultCommandIndex = 1;
            await messageDialog.ShowAsync();
        }
        private async void DeleteReport()
        {
            this.ProgressStart();
            try
            {
                await this.Report.DeleteAsync();
                this.navigationHelper.GoBack();
                this.ProgressFinish();
            }
            catch (Exception error)
            {
                MessageDialog messageDialog = new MessageDialog(error.GetExceptionMessage());
                messageDialog.Commands.Add(new UICommand((LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.Accept)), (command) => { }));
                messageDialog.ShowAsync();
                this.ProgressFinish();
                return;
            }
        }
        private void MainControllerExpenseReceiptRemovedRequest(object sender, EventArgs e)
        {
            var frame = (Frame)Window.Current.Content;
            frame.Navigate(typeof(DraftReportDetailView), this.Report);
            frame.BackStack.RemoveAt(frame.BackStack.Count - 1);
        }
        private async void DeleteExpense(object sender, EventArgs e)
        {
            this.ProgressStart();
            Expense exp = ((ExpenseDeletableListElement)sender).DataContext as Expense;
            if (exp.CanRemove)
            {
                try
                {
                    await this.Report.Expenses.RemoveReportExpenseAsync(exp);
                    await this.Report.Expenses.FetchAsync();
                    this.Report.NotifyPropertyChanged(string.Empty);
                    var frame = (Frame)Window.Current.Content;
                    currentSection = 1;
                    frame.Navigate(typeof(DraftReportDetailView), this.Report);
                    frame.BackStack.RemoveAt(frame.BackStack.Count - 1);
                    currentSection = 0;
                    this.ProgressFinish();
                }
                catch (Exception error)
                {
                    MessageDialog messageDialog = new MessageDialog(error.GetExceptionMessage());
                    messageDialog.Commands.Add(new UICommand((LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.Accept)), (command) => { }));
                    messageDialog.ShowAsync();
                    this.ProgressFinish();
                    return;
                }
            }
            this.ProgressFinish();
        }

        private void AddReceipt_Tapped(object sender, TappedRoutedEventArgs e)
        {
            try
            {
                this.AddReceipt();
            }
            catch (Exception error)
            {
                Debug.WriteLine(error.Message);
                this.BottomAppBar.IsEnabled = true;
                MainController.Instance.ReceiptsLoaded();
            }
        }
    }
}