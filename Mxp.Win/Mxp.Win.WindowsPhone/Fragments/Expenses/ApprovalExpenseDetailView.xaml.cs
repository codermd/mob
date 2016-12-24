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
    public sealed partial class ApprovalExpenseDetailView : Page
    {
        public ApprovalExpenseDetailView()
        {
            this.InitializeComponent();
            MainController.Instance.ExpenseGroupedProductsAddedRequest += MainControllerExpenseGroupedProductsAdded;
            HardwareButtons.BackPressed += HardwareButtons_BackPressed;
        }

        private async void LoadLabels()
        {
            Title.Text = (LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.Expense));
            DetailsHeader.Text = (LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.Details));
            ReceiptsHeader.Text = (LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.Receipts));
            AttendeesHeader.Text = (LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.Attendees));
            await Expense.Receipts.FetchAsync();
            if (!Expense.IsNew)
            {
                ReceiptsHeader.Text = (LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.Receipts)) + " (" + Expense.Receipts.Count + ")";
                AttendeesHeader.Text = (LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.Attendees)) + " (" + Expense.NumberOfAttendees + ")";
            }
            else
            {
                ReceiptsHeader.Text = (LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.Receipts));
                AttendeesHeader.Text = (LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.Attendees));
            }
        }

        void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            e.Handled = true;
            currentSection = 0;
            Frame.GoBack();
        }
        private void MainControllerExpenseGroupedProductsAdded(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            HardwareButtons.BackPressed -= HardwareButtons_BackPressed;
            MainController.Instance.ExpenseGroupedProductsAddedRequest -= MainControllerExpenseGroupedProductsAdded;

        }
        public static int currentSection = 0;
        public Expense Expense { get; set; }
        public ExpenseItem ExpenseItem { get; set; }
        public List<DetailField> SourceFields { get; set; }
        public Collection<Field> Fields { get; set; }
        public ListView FieldsListView { get; set; }
        public Collection<TableSectionModel> CollectionFields { get; set; }
        ReceiptsGallery ReceiptsGallery { get; set; }
        DetailView DetailView { get; set; }
        AttendeesGallery AttendeesGallery { get; set; }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            bool canEdit;
            if (e.Parameter != null)
            {
                if (e.Parameter.GetType() == typeof(Expense))
                {
                    Expense = e.Parameter as Expense;
                    canEdit = Expense.IsEditable;
                    ExpenseItem = Expense.ExpenseItems[0];
                    CollectionFields = ExpenseItem.DetailsFields;
                }
                else if (e.Parameter.GetType() == typeof(ExpenseItem))
                {
                    ExpenseItem = e.Parameter as ExpenseItem;
                    Expense = ExpenseItem.ParentExpense;
                    CollectionFields = ExpenseItem.DetailsFields;
                }


                ReceiptsHeader.Text = ReceiptsHeader.Text + " (" + Expense.Receipts.Count + ")";
                AttendeesHeader.Text = AttendeesHeader.Text + " (" + Expense.NumberOfAttendees + ")";

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
                ReceiptsSection.Visibility = Expense.CanShowReceipts ? Visibility.Visible : Visibility.Collapsed;
                AttendeesSection.Visibility = ExpenseItem.CanShowAttendees ? Visibility.Visible : Visibility.Collapsed;
                //BindModels();
                CheckMargins();
                LoadLabels();
            }
        }

        private void CheckMargins()
        {
            int cpt = 0;
            foreach (HubSection section in this.HubExpense.Sections)
            {
                if (section.Visibility == Visibility.Visible)
                    cpt++;
            }
            if (cpt == 2)
                DetailsSection.Margin = new Thickness(0, 0, 60, 0);
        }

        private void BindModels()
        {
            //this.ExpenseItem.PropertyChanged += c;
            //this.Expense.PropertyChanged += HandlePropertyChanged;
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
                        await Task.Delay(TimeSpan.FromSeconds(0.1));
                        items.Add(item);
                    }
                }
            }
            var result = from t in items
                         group t by t.Group into g
                         select new { Key = g.Key, ItemsField = g };
            DetailFieldsSource.Source = result;
        }
        private void FieldListLoaded(object sender, RoutedEventArgs e)
        {
            FieldsListView = (ListView)sender;
            FillList();
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
            if (Expense != null)
            {
                try
                {
                    MainController.Instance.ReceiptsLoading();
                    this.ReceiptsGallery.LoadPhotos(Expense);
                }
                catch (Exception error)
                {
                    MessageDialog messageDialog = new MessageDialog(error.GetExceptionMessage());
                    messageDialog.Commands.Add(new UICommand("OK", (command) => { }));
                    messageDialog.ShowAsync();
                    MainController.Instance.ReceiptsLoaded();
                }
                MainController.Instance.ReceiptsLoaded();
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

        private void HubExpense_SectionsInViewChanged(object sender, SectionsInViewChangedEventArgs e)
        {
            var tag = HubExpense.SectionsInView[0].Tag.ToString();
            currentSection = int.Parse(tag);
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
    }
}