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
    public sealed partial class ApprovalReportDetailView : Page
    {
        private ListView FieldsListView;
        private ListView ExpensesListView;
        ReceiptsGallery ReceiptsGallery { get; set; }
        public Collection<TableSectionModel> CollectionFields { get; set; }
        public ReportApproval ReportApproval { get; set; }

        Collection<Expense> ExpensesCol { get; set; }
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
            if (e.PageState != null && e.PageState.ContainsKey("ReportApproval"))
                ReportApproval = e.PageState["ReportApproval"] as ReportApproval;
            if (e.PageState != null && e.PageState.ContainsKey("Expenses"))
                Expenses = e.PageState["Expenses"] as Expenses;
        }
        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
            if (ReportApproval != null)
                e.PageState["ReportApproval"] = ReportApproval;
            if (Expenses != null)
                e.PageState["Expenses"] = Expenses;
        }
        private void InitializeNavigationHelper()
        {
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            navigationHelper.SaveState += NavigationHelper_SaveState;
        }
        public ApprovalReportDetailView()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            InitializeNavigationHelper();
            LoadLabels();
        }

        private void LoadLabels()
        {
            ExpensesHeader.Text = (LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.Expenses));
            ReceiptsHeader.Text = (LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.Receipts));
            HistoryHeader.Text = (LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.History));
            DetailsHeader.Text = (LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.Details));
            Approove_Button.Label = (LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.Approve));
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedFrom(e);
        }

        private void HubReport_SectionsInViewChanged(object sender, SectionsInViewChangedEventArgs e)
        {
        }
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedTo(e);
            if (e.Parameter != null)
            {
                if (ReportApproval == null)
                    ReportApproval = e.Parameter as ReportApproval;
                if (Expenses == null)
                    Expenses = ReportApproval.Report.Expenses;

                Title.Text = ReportApproval.Report.VDetailsBarTitle;

                // the list of this section was loaded before OnNavigatedTo
                ExpensesSection.Visibility = Visibility.Visible;
                ReceiptsSection.Visibility = ReportApproval.Report.CanShowReceipts ? Visibility.Visible : Visibility.Collapsed;
                HistorySection.Visibility = ReportApproval.Report.CanShowHistory ? Visibility.Visible : Visibility.Collapsed;
                CollectionFields = new Collection<TableSectionModel>();
                CollectionFields.Add(new TableSectionModel(Labels.GetLoggedUserLabel(Labels.LabelEnum.General), ReportApproval.Report.GetMainFields()));
                CollectionFields.Add(new TableSectionModel(Labels.GetLoggedUserLabel(Labels.LabelEnum.Details), ReportApproval.Report.GetAllFields()));
                FillDetailsList();
                try {
                    await ReportApproval.Report.Receipts.FetchAsync();
                } catch (Exception error) {
                    MessageDialog messageDialog = new MessageDialog (error.GetExceptionMessage ());
                    messageDialog.Commands.Add (new UICommand ("OK", (command) => { }));
                    messageDialog.ShowAsync ();
                }
                ReceiptsHeader.Text = ReceiptsHeader.Text + " (" + ReportApproval.Report.NumberReceipts + ")";
                ExpensesHeader.Text = ExpensesHeader.Text + " (" + ReportApproval.Report.Expenses.Count + ")";
            }
            else
            {
            }
            BindModels();
            CheckMargins();
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
                DetailsSection.Margin = new Thickness(0, 0, 60, 0);
        }
        public void BindModels()
        {
        }

        private void FieldListLoaded(object sender, RoutedEventArgs e)
        {
            FieldsListView = (ListView)sender;


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



        private void ReceiptsGallery_Loaded(object sender, RoutedEventArgs e)
        {
            this.ReceiptsGallery = sender as ReceiptsGallery;

            if (ReportApproval != null)
            {
                ReceiptsGallery.LoadPhotos(ReportApproval.Report);
            }
        }

        private void ExpensesList_Loaded(object sender, RoutedEventArgs e)
        {
            ExpensesListView = (ListView)sender;
            FillExpensesList();
        }
        private async void FillExpensesList()
        {
            Expenses = ReportApproval.Report.Expenses;
            var result = from t in Expenses.ToList<Expense>()
                         group t by t.VDateHeader into g
                         select new { Key = g.Key, Items = g };
            ExpensesSource.Source = result;
        }

        private void AcceptApproval_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(AcceptApprovalPage), ReportApproval.Report);
        }

        private void HistoryList_Loaded(object sender, RoutedEventArgs e)
        {
            ((ListView)sender).ItemsSource = ReportApproval.Report.History;
        }


    }
}
