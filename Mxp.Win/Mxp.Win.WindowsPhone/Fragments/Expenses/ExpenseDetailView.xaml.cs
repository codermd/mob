using Mxp.Core.Business;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.Phone.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
using Windows.ApplicationModel.Core;
using Windows.Storage.Pickers;
using Windows.ApplicationModel.Activation;
using Windows.Storage;
using Mxp.Core.Utils;
using Windows.UI.Popups;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;
using System.Diagnostics;

namespace Mxp.Win
{
    public sealed partial class ExpenseDetailView : Page
    {

        Grid ReceiptsProgressGrid;
        ProgressRing ReceiptsProgressRing;
        public List<DetailField> SourceFields { get; set; }
        public Collection<Field> Fields { get; set; }
        public ListView FieldsListView { get; set; }
        public Collection<TableSectionModel> CollectionFields { get; set; }
        private readonly NavigationHelper navigationHelper;
        private readonly ObservableDictionary defaultViewModel = new ObservableDictionary();
        private Expense mExpense;
        private Expenses expenseList;
        public static int currentSection = 0;
        bool IsCancelling { get; set; }
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }
        Expense Expense {
            get { return this.mExpense; }
            set {
                this.mExpense = value;
                if (this.mExpense != null) {
                    this.expenseList = this.mExpense.GetCollectionParent<Expense> () as Expenses;
                    if (this.mExpense.ExpenseItems[0].CanChangeAccountType) {
                        this.ConvertExpenseButton.Visibility = Visibility.Visible;
                        this.ConvertExpenseButton.Label = this.mExpense.ExpenseItems[0].VChangeAccountType;
                    }
                }
            }
        }

        private void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {

            if (e.PageState != null && e.PageState.ContainsKey("Expense"))
                Expense = e.PageState["Expense"] as Expense;
            if (e.PageState != null && e.PageState.ContainsKey("ExpenseItem"))
                ExpenseItem = e.PageState["ExpenseItem"] as ExpenseItem;
        }
        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
            if (Expense != null)
                e.PageState["Expense"] = Expense;

            if (ExpenseItem != null)
                e.PageState["ExpenseItem"] = ExpenseItem;

            if (Expense == null || ExpenseItem == null)
                e.PageState.Clear();
        }
        private void InitializeNavigationHelper()
        {
            navigationHelper.LoadState += this.NavigationHelper_LoadState;
            navigationHelper.SaveState += NavigationHelper_SaveState;
        }
        public ExpenseDetailView()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            InitializeNavigationHelper();
            view = CoreApplication.GetCurrentView();
            ShowNewAttendeeDialog();
           
        }
        private void BindEvents()
        {
            HardwareButtons.BackPressed += HardwareButtons_BackPressed;
            MainController.Instance.ExpenseGroupedProductsAddedRequest += MainControllerExpenseGroupedProductsAdded;
            MainController.Instance.ExpenseReceiptRemovedRequest += MainControllerExpenseReceiptRemovedRequest;
            MainController.Instance.AddReceiptRequest += MainControllerAddReceiptRequest;
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

        private void ReceiptsProgressGrid_Loaded(object sender, RoutedEventArgs e)
        {
            this.ReceiptsProgressGrid = (Grid)sender;
        }

        private void ReceiptsProgressRing_Loaded(object sender, RoutedEventArgs e)
        {
            this.ReceiptsProgressRing = (ProgressRing)sender;
        }


        private void MainControllerAddReceiptRequest(object sender, EventArgs e)
        {
            try
            {
                AddReceipt();
            }
            catch (Exception error)
            {
                Debug.WriteLine(error.Message);
                BottomAppBar.IsEnabled = true;
                MainController.Instance.ReceiptsLoaded();
            }
        }
        private async void LoadLabels()
        {
            this.ActivateProgressRing ();
            try
            {
                await Expense.Receipts.FetchAsync();
            }
            catch (Exception error)
            {
                Debug.WriteLine(error.GetExceptionMessage());
            }

            Title.Text = (LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.Expense));
            DetailsHeader.Text = (LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.Details));

            if (!Expense.IsNew)
            {
                ReceiptsHeader.Text = (LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.Receipts)) + " (" + Expense.Receipts.Count + ")";
                AttendeesHeader.Text = (LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.Attendees)) + " (" + Expense.NumberOfAttendees + ")";
            }
            else
            {
                ReceiptsHeader.Text = (LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.Receipts)) + " (" + Expense.Receipts.Count + ")";
                AttendeesHeader.Text = (LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.Attendees)) + " (" + Expense.NumberOfAttendees + ")";
            }


            SplitButton.Label = (LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.Split));
            UnsplitButton.Label = (LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.Unsplit));
            DeleteButton.Label = (LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.Delete));
            SaveExpenseButton.Label = (LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.Save));
            DiscardExpenseButton.Label = (LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.Cancel));
            this.CopyExpenseButton.Label = (LoggedUser.Instance.Labels.GetLabel (Labels.LabelEnum.SaveAndCopy));

            if (!Expense.CanCopy)
                this.CopyExpenseButton.Visibility = Visibility.Collapsed;

            this.DesactivateProgressRing ();
        }
        private void MainControllerExpenseReceiptRemovedRequest(object sender, EventArgs e)
        {
            var frame = (Frame)Window.Current.Content;
            frame.Navigate(typeof(ExpenseDetailView), Expense);
            frame.BackStack.RemoveAt(frame.BackStack.Count - 1);
        }

        async void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            e.Handled = true;
            currentSection = 0;

                
            ActivateProgressRing();
            try
            {
                if (!Expense.IsNew && (Expense.IsChanged || ExpenseItem.IsChanged))
                    await expenseList.FetchAsync();
            }
            catch (Exception error)
            {
                PopMessages.AsyncMessage(error.Message);
            }
            DesactivateProgressRing();

            Frame.Navigate (typeof (MainPage));

        }
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedFrom(e);
            HardwareButtons.BackPressed -= HardwareButtons_BackPressed;
            MainController.Instance.ExpenseGroupedProductsAddedRequest -= MainControllerExpenseGroupedProductsAdded;
            MainController.Instance.ExpenseReceiptRemovedRequest -= MainControllerExpenseReceiptRemovedRequest;
            MainController.Instance.AddReceiptRequest -= MainControllerAddReceiptRequest;
            if (ExpenseItem != null)
                this.ExpenseItem.PropertyChanged -= HandlePropertyChanged;
            if (Expense != null)
                this.Expense.PropertyChanged -= HandlePropertyChanged;
        }
        
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedTo(e);
            bool canEdit;
            if (e.Parameter != null)
            {
                if (e.Parameter.GetType() == typeof(Expense))
                {
                    if (Expense == null)
                        Expense = e.Parameter as Expense;
                    canEdit = Expense.IsEditable;
                    if (ExpenseItem == null)
                        ExpenseItem = Expense.ExpenseItems[0];
                    CollectionFields = ExpenseItem.DetailsFields;

                }
                else if (e.Parameter.GetType() == typeof(ExpenseItem))
                {
                    if (ExpenseItem == null)
                        ExpenseItem = e.Parameter as ExpenseItem;
                    if (Expense == null)
                        Expense = ExpenseItem.ParentExpense;
                    CollectionFields = ExpenseItem.DetailsFields;
                }

                switch (currentSection)
                {
                    case 0:
                        HubExpense.ScrollToSection(DetailsSection);
                        break;
                    case 1:
                        HubExpense.ScrollToSection(ReceiptsSection);
                        break;
                    case 2:
                        HubExpense.ScrollToSection(AttendeesSection);
                        break;
                    default:
                        HubExpense.ScrollToSection(DetailsSection);
                        break;
                }
            }
            ReceiptsSection.Visibility = Expense.CanShowReceipts ? Visibility.Visible : Visibility.Collapsed;
            AttendeesSection.Visibility = ExpenseItem.CanShowAttendees ? Visibility.Visible : Visibility.Collapsed;

            BindEvents();

            LoadLabels();
            if (exp == null || exp != Expense)
            {
                exp = Expense;
            }
        }

        static Expense exp;

        private void ShowButtons()
        {
            if (!this.ExpenseItem.CanShowDelete)
                DeleteButton.Visibility = Visibility.Collapsed;
            else
                DeleteButton.Visibility = Visibility.Visible;

            if (!this.ExpenseItem.CanShowSplit)
                SplitButton.Visibility = Visibility.Collapsed;
            else
                SplitButton.Visibility = Visibility.Visible;

            if (ExpenseItem.CanShowUnsplit)
                UnsplitButton.Visibility = Visibility.Visible;
            else
                UnsplitButton.Visibility = Visibility.Collapsed;

            if (this.Expense.IsNew || this.Expense.IsChanged)
            {
                DiscardExpenseButton.Visibility = Visibility.Visible;
                SaveExpenseButton.Label = (LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.Cancel));
                SaveExpenseButton.Visibility = Visibility.Visible;
                SaveExpenseButton.Label = (LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.Save));
              
            }
            else
            {
                ReceiptsHeader.Text = (LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.Receipts)) + " (" + Expense.Receipts.Count + ")";
                AttendeesHeader.Text = (LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.Attendees)) + " (" + ExpenseItem.Attendees.Count + ")";
                SaveExpenseButton.Visibility = Visibility.Collapsed;
                DiscardExpenseButton.Visibility = Visibility.Collapsed;
            }
        }

        public void BindModels()
        {
            //this.ExpenseItem.PropertyChanged += HandlePropertyChanged;
            this.Expense.PropertyChanged += HandlePropertyChanged;
            MainController.Instance.AttendeeDeletedRequest += Instance_AttendeeDeletedRequest;
        }

        private void Instance_AttendeeDeletedRequest(object sender, EventArgs e)
        {
            AttendeesHeader.Text = (LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.Attendees)) + " (" + ExpenseItem.Attendees.Count + ")";
        }

        void HandlePropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            ShowButtons();
        }
        private async void FillList()
        {
            ActivateProgressRing ();
            List<FieldGroup> items = new List<FieldGroup>();
            if (CollectionFields != null)
            {
                foreach (TableSectionModel col in CollectionFields)
                {
                    List<Field> fields = col.Fields.ToList<Field>();
                    foreach (Field field in fields)
                    {
                        FieldGroup item = new FieldGroup(field, col.Title);
                        await Task.Delay(TimeSpan.FromSeconds(0.05));
                        items.Add(item);
                    }
                }
            }
          
            var result = from t in items
                         group t by t.Group into g
                         select new { Key = g.Key, ItemsField = g };
            DetailFieldsSource.Source = result;
            await Task.Delay(TimeSpan.FromSeconds(1));
            DesactivateProgressRing();
   
        }
        private void FieldListLoaded(object sender, RoutedEventArgs e)
        {
            FieldsListView = (ListView)sender;
            FillList(); BindModels();

            ShowButtons();
        }



        // method to pull out a ScrollViewer
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

        private void HubExpense_SectionsInViewChanged(object sender, SectionsInViewChangedEventArgs e)
        {
            var tag = HubExpense.SectionsInView[0].Tag.ToString();
            currentSection = int.Parse(tag);
        }
        ExpenseItem ExpenseItem { get; set; }
        CoreApplicationView view;

        String ImagePath;


        private void AddReceipt()
        {
            this.ReceiptsProgressStart();
            try
            {
                ImagePath = string.Empty;
                FileOpenPicker filePicker = new FileOpenPicker();
                filePicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
                filePicker.ViewMode = PickerViewMode.Thumbnail;
                filePicker.FileTypeFilter.Clear();
                filePicker.FileTypeFilter.Add(".bmp");
                filePicker.FileTypeFilter.Add(".png");
                filePicker.FileTypeFilter.Add(".jpeg");
                filePicker.FileTypeFilter.Add(".jpg");
                filePicker.PickSingleFileAndContinue();
                view.Activated += viewActivated;
            }
            catch (Exception error)
            {
                PopMessages.AsyncMessage(error.GetExceptionMessage());
                BottomAppBar.IsEnabled = true;
                this.ReceiptsProgressFinish();
            }
            this.ReceiptsProgressFinish();
        }
        private async void viewActivated(CoreApplicationView sender, IActivatedEventArgs args1)
        {
            try
            {
                this.ReceiptsProgressStart();
                FileOpenPickerContinuationEventArgs args = args1 as FileOpenPickerContinuationEventArgs;
                if (args != null)
                {
                    if (args.Files.Count == 0)
                    {
                        this.ReceiptsProgressFinish();
                        view.Activated -= viewActivated;
                        return;
                    }
                    view.Activated -= viewActivated;
                    StorageFile storageFile = args.Files[0];
                    string Base64str = await BitmapHelper.CompressFile(storageFile);
                    if (Expense != null)
                    {
                        try
                        {
                            await Expense.Receipts.AddReceipt(Base64str);

                        }
                        catch (Exception error)
                        {
                            PopMessages.AsyncMessage(error.GetExceptionMessage());
                            BottomAppBar.IsEnabled = true;
                            this.ReceiptsProgressFinish();
                        }
                        try
                        {
                            this.ReceiptsProgressStart();
                            BottomAppBar.IsEnabled = false;
                            await Expense.Receipts.FetchAsync();
                        }
                        catch (Exception error)
                        {
                            PopMessages.AsyncMessage(error.GetExceptionMessage());
                            BottomAppBar.IsEnabled = true;
                            this.ReceiptsProgressFinish();
                        }
                    }
                    this.ReceiptsGallery.Showed = false;
                    try
                    {
                        this.ReceiptsProgressStart();
                        BottomAppBar.IsEnabled = false;
                        this.ReceiptsGallery.LoadLastPhoto(Expense);
                    }
                    catch (Exception error)
                    {
                        PopMessages.AsyncMessage(error.GetExceptionMessage());
                        BottomAppBar.IsEnabled = true;
                        this.ReceiptsProgressFinish();
                    }
                    this.ReceiptsProgressFinish();
                    BottomAppBar.IsEnabled = true;

                }

                BottomAppBar.IsEnabled = true;
                view.Activated -= viewActivated;
                this.ReceiptsProgressFinish();

            }
            catch (Exception error)
            {
                PopMessages.AsyncMessage(error.GetExceptionMessage());
                BottomAppBar.IsEnabled = true;
                this.ReceiptsProgressFinish();
            }
            this.ReceiptsProgressFinish();

            ReceiptsHeader.Text = (LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.Receipts)) + " (" + Expense.Receipts.Count + ")";
        }
        private void ReceiptsGallery_Loaded(object sender, RoutedEventArgs e)
        {
            this.ReceiptsGallery = sender as ReceiptsGallery;
            if (Expense != null)
            {
                try
                {
                    ReceiptsProgressStart();
                    BottomAppBar.IsEnabled = false;
                    this.ReceiptsGallery.LoadPhotos(Expense);
                }
                catch (Exception error)
                {
                    PopMessages.AsyncMessage(error.GetExceptionMessage());
                    this.ReceiptsProgressFinish();
                    BottomAppBar.IsEnabled = true;
                }
                this.ReceiptsProgressFinish();
                BottomAppBar.IsEnabled = true;
            }
        }
        private void AttendeesGallery_Loaded(object sender, RoutedEventArgs e)
        {
            this.AttendeesGallery = sender as AttendeesGallery;
            AttendeesGallery.LoadAttendees(ExpenseItem);
        }
        private void DetailView_Loaded(object sender, RoutedEventArgs e)
        {
            this.DetailView = sender as DetailView;
            this.HubExpense.SectionsInViewChanged += HubExpense_SectionsInViewChanged;
        }
        ReceiptsGallery ReceiptsGallery { get; set; }
        DetailView DetailView { get; set; }
        AttendeesGallery AttendeesGallery { get; set; }

        public void ShowNewAttendeeDialog()
        {
            AttendeeMenu = new MenuFlyout();
            StackPanel actionsStack = new StackPanel();
            Actionables actionables = Attendee.ListShowAttendees(
                actionRecent: () =>
                {
                    this.showRecentUser(null);
                },
                actionBusinessRelation: () =>
                {
                    this.showBusinessRelation(null);
                },
                actionEmployee: () =>
                {
                    this.showEmployee(null);
                },
                actionSpouse: () =>
                {
                    this.showSpouse(null);
                },
                actionHCP: () =>
                {
                    this.showHCP(null);
                },
                actionHCO: () =>
                {
                    this.showHCO(null);
                },
                actionHCU: () =>
                {
                    this.showHCU(null);
                }
            );
            foreach (var act in actionables.Actions)
            {
                ActionsListElement button = new ActionsListElement(act);
                MenuFlyoutItem item = new MenuFlyoutItem();
                item.Text = act.Title;
                item.Tag = act.Action;
                item.Click += DoAction;
                item.DataContext = act;
                this.AttendeeMenu.Items.Add(item);
            }

        }
        private void DoAction(object sender, RoutedEventArgs e)
        {
            Action act = (Action)((MenuFlyoutItem)sender).Tag;
            act();
        }
        public void showRecentUser(object sender)
        {
            currentSection = 2;
            Frame.Navigate(typeof(RecentlyUserTableViewController), ExpenseItem.Attendees);
        }
        public void showEmployee(object sender)
        {
            currentSection = 2;
            Frame.Navigate(typeof(ALookUpPageAttendee), ExpenseItem.Attendees);
        }
        public void showBusinessRelation(object sender)
        {
            currentSection = 2;
            Frame.Navigate(typeof(BusinessFormViewController), ExpenseItem.Attendees);
        }
        public void showSpouse(object sender)
        {
            currentSection = 2;
            Frame.Navigate(typeof(SpouseViewController), ExpenseItem.Attendees);
        }
        public void showHCU(object sender)
        {
            currentSection = 2;
            Frame.Navigate(typeof(HCUViewController), ExpenseItem.Attendees);
        }
        public void showHCP(object sender)
        {
            currentSection = 2;
            Frame.Navigate(typeof(HCPViewController), ExpenseItem.Attendees);
        }
        public void showHCO(object sender)
        {
            currentSection = 2;
            Frame.Navigate(typeof(HCOViewController), ExpenseItem.Attendees);
        }
        private async void DeleteExpense_Click(object sender, RoutedEventArgs e)
        {
            var messageDialog = new Windows.UI.Popups.MessageDialog(LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.DoYouConfirm), LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.Delete) + " " + LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.Expense));
            messageDialog.Commands.Add(new UICommand((LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.Yes)), (command) => { DeleteExpense(); }));
            messageDialog.Commands.Add(new UICommand((LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.No)), (command) => { }));
            messageDialog.DefaultCommandIndex = 1;
            await messageDialog.ShowAsync();
        }
        private async Task DeleteExpense()
        {
            currentSection = 0;
            try
            {
                ActivateProgressRing();
                await this.ExpenseItem.DeleteExpenseAsync();

                navigationHelper.GoBack();
            }
            catch (Exception error)
            {
                PopMessages.AsyncMessage(error.GetExceptionMessage());
                DesactivateProgressRing();
            }
            DesactivateProgressRing();
        }
        private async void SaveChanges_Click (object sender, RoutedEventArgs e) {
            if (!await this.SaveAsync ())
                return;
            currentSection = 0;
            navigationHelper.GoBack ();
        }

        private async Task<bool> SaveAsync () {
            ActivateProgressRing ();
            if (this.ExpenseItem.ParentExpense.IsNew) {
                try {
                    ExpProgressRing.IsActive = true;
                    BottomAppBar.IsEnabled = false;
                    await this.CreateExpense ();
                }
                catch (Exception error) {
                    PopMessages.AsyncMessage (error.GetExceptionMessage());
                }
                DesactivateProgressRing ();
                return true;
            }
            try {
                if (this.ExpenseItem.ParentExpense.IsChanged)
                    await this.Expense.SaveAsync (this.ExpenseItem);
            }
            catch (Exception error) {
                PopMessages.AsyncMessage (error.GetExceptionMessage());
                DesactivateProgressRing ();
                return false;
            }
            DesactivateProgressRing ();

            return true;
        }

        private async void DiscardChanges_Click(object sender, RoutedEventArgs e)
        {
            ActivateProgressRing();
            if (this.ExpenseItem.ParentExpense.IsNew)
            {
                MainController.Instance.ExpensesCreationSuccessed();
                DesactivateProgressRing();
                Frame.Navigate(typeof(MainPage));
                return;
            }
            if (this.IsCancelling)
            {
                DesactivateProgressRing();
                return;
            }
            this.IsCancelling = true;
            this.Expense.ResetChanged();
            this.ExpenseItem.ResetChanged();
            try
            {
                ActivateProgressRing();
                await expenseList.FetchAsync();
                navigationHelper.GoBack();
                this.IsCancelling = false;
            }
            catch (Exception error)
            {
                PopMessages.AsyncMessage(error.GetExceptionMessage());
                this.SaveExpenseButton.Visibility = Visibility.Collapsed;
                this.DiscardExpenseButton.Visibility = Visibility.Collapsed;
            }
            DesactivateProgressRing();
        }
        public async Task CreateExpense()
        {
            ActivateProgressRing();
            try {
                await this.Expense.CreateAsync ();
                MainController.Instance.ExpensesCreationSuccessed ();
                DesactivateProgressRing ();
                Frame.Navigate (typeof(MainPage));
            }
            catch (ValidationError error) {
                PopMessages.AsyncMessage (error.Verbose);
                return;
            }
            catch (Exception ex) {
                Debug.WriteLine (ex.Message);
            }
            finally {
                DesactivateProgressRing ();
            }
            
            this.SaveExpenseButton.Visibility = Visibility.Collapsed;
            this.DiscardExpenseButton.Visibility = Visibility.Collapsed;
        }


        private void SplitExpense_Click(object sender, RoutedEventArgs e)
        {
            ExpenseItemToSplit = new ExpenseItemToSplit(ExpenseItem, null, new CategoriesFromExpenseViewModel(ExpenseItem));
        }
        ExpenseItemToSplit ExpenseItemToSplit { get; set; }
        private void MainControllerExpenseGroupedProductsAdded(object sender, EventArgs e)
        {
            currentSection = 0;
            if (ExpenseItemToSplit != null)
                Frame.Navigate(typeof(ExpenseSplitPage), ExpenseItemToSplit);
        }

        private async void UnsplitExpense_Click(object sender, RoutedEventArgs e)
        {
            var messageDialog = new Windows.UI.Popups.MessageDialog((LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.DoYouConfirm)), LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.Unsplit) + " " + (LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.Expense)));
            messageDialog.Commands.Add(new UICommand((LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.Yes)), (command) => { UnsplitExpense(); }));
            messageDialog.Commands.Add(new UICommand((LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.No)), (command) => {/* autoLogin command here*/ }));
            messageDialog.DefaultCommandIndex = 1;
            await messageDialog.ShowAsync();
        }
        private async void UnsplitExpense()
        {
            ActivateProgressRing();
            try
            {
                await this.ExpenseItem.UnsplitAsync();
                currentSection = 0;
                Frame.Navigate(typeof(MainPage));
            }
            catch (Exception error)
            {
                PopMessages.AsyncMessage(error.GetExceptionMessage());
                DesactivateProgressRing();
                return;
            }
        }
        private void AddAttendee_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.AttendeeMenu.ShowAt((FrameworkElement)sender);
        }
        public MenuFlyout AttendeeMenu { get; set; }
        private void Grid_Tapped(object sender, TappedRoutedEventArgs e)
        {
            e.Handled = true;
        }
        private void AddReceipt_Tapped(object sender, TappedRoutedEventArgs e)
        {
            try
            {
                AddReceipt();
            }
            catch (Exception error)
            {
                Debug.WriteLine(error.Message);
                BottomAppBar.IsEnabled = true;
                MainController.Instance.ReceiptsLoaded();
            }
        }
        private void ActivateProgressRing()
        {
            ExpProgressRing.IsActive = true;
            BottomAppBar.IsEnabled = false;
            ProgressGrid.Visibility = Visibility.Visible;
        }
        private void DesactivateProgressRing()
        {
            ExpProgressRing.IsActive = false;
            BottomAppBar.IsEnabled = true;
            ProgressGrid.Visibility = Visibility.Collapsed;
        }

        private void SpendCatcherLabel_Loaded(object sender, RoutedEventArgs e)
        {
            if (Preferences.Instance.IsGTPEnabled)
                (sender as TextBlock).Text = LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.GTPHeaderMessage);
            else
                (sender as TextBlock).Visibility = Visibility.Collapsed;
        }
        private void AddReceiptButton_Loaded(object sender, RoutedEventArgs e)
        {
            if (!Expense.CanManageReceipts)
                ((Image)sender).Visibility = Visibility.Collapsed;
        }

        private void AddAttendeeButton_Loaded(object sender, RoutedEventArgs e)
        {
            if (!ExpenseItem.CanManageAttendees)
                ((Image)sender).Visibility = Visibility.Collapsed;
        }

        private async void ConvertExpenseButton_OnClick (object sender, RoutedEventArgs e) {
            try {
                ActivateProgressRing ();
                await this.ExpenseItem.ChangeAccountTypeAsync ();

                navigationHelper.GoBack ();
            }
            catch (Exception error) {
                PopMessages.AsyncMessage (error.GetExceptionMessage());
            }
            DesactivateProgressRing ();
        }

        private async void CopyExpense_Click (object sender, RoutedEventArgs e) {
            if (!await this.SaveAsync ())
                return;

            this.Frame.Navigate (typeof (ExpenseDetailView), (Expense)this.Expense.Clone ());
        }
    }
}