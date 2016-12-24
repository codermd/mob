using System;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
using Mxp.Core.Business;
using Mxp.Core.Utils;
using Windows.UI.Popups;
using Mxp.Win.Helpers;

namespace Mxp.Win
{
    public sealed partial class MainPage : Page
    {
        ExpensesView _expensesView;
        ReportsView _reportsView;
        ApprovalsView _approvalsView;
        SettingsView _settingsView;
        private readonly NavigationHelper navigationHelper;
        private readonly ObservableDictionary defaultViewModel = new ObservableDictionary();
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }
        private void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            if (e.PageState != null && e.PageState.ContainsKey("Expenses"))
                _expensesView = e.PageState["Expenses"] as ExpensesView;
            if (e.PageState != null && e.PageState.ContainsKey("Reports"))
                _reportsView = e.PageState["Reports"] as ReportsView;
            if (e.PageState != null && e.PageState.ContainsKey("Approvals"))
                _approvalsView = e.PageState["Approvals"] as ApprovalsView;
        }
        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
            if (_expensesView != null)
                e.PageState["Expenses"] = _expensesView;
            if (_reportsView != null)
                e.PageState["Reports"] = _reportsView;
            if (_approvalsView != null)
                e.PageState["Approvals"] = _approvalsView;
        }
        private void InitializeNavigationHelper()
        {
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            navigationHelper.SaveState += NavigationHelper_SaveState;

        }

        public MainPage()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            InitializeNavigationHelper();
            MainController.Instance.StartMainProgressRingRequest += StartMainProgressRing;
            MainController.Instance.FinishMainProgressRingRequest += FinishMainProgressRing;
            MainController.Instance.MainPageClearRequest += MainPageClear;
            MainController.Instance.MainPageExpensesTabRequest += MainPageExpensesTab;
            this.NavigationCacheMode = NavigationCacheMode.Required;
            MainPageExpensesTab(null, null);
        }

        private void MainPageClear(object sender, EventArgs e)
        {
            if (MainPlace.Children.ToArray().Length != 0)
                MainPlace.Children.Clear();
            this.MainPlace.Children.Add(_expensesView);
            RefreshButtons(true, false, false, false);
            toRefresh = true;
        }
        bool toRefresh;

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedTo(e);
            while (navigationHelper.CanGoBack())
                Frame.BackStack.RemoveAt(0);
            while (navigationHelper.CanGoForward())
                Frame.ForwardStack.RemoveAt(0);
            AddReportButton.Label = LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.New) + " " + LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.Report);
            AddExpenseButton.Label = LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.New) + " " + LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.Expense);
            RefreshExpensesButton.Label = LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.Refresh);
            ExpenseToggle.Label = LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.Expenses);
            ReportsToggle.Label = LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.Reports);
            ApprovalsToggle.Label = LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.Approvals);
            SettingsToggle.Label = LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.Settings);
            LogoutButton.Label = LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.Logout);
            _settingsView = new SettingsView();
            ShowNewExpenseDialog();
            if (MainController.Instance.InExpenses)
                LoadExpenses(null, null);
            if (toRefresh) {
                RefreshButton(null, null);
                toRefresh = false;
            }
        }
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);
        }

        private void MainPageExpensesTab(object sender, EventArgs e)
        {
            LoadExpenses(null, null);
        }

        private void StartMainProgressRing(object sender, EventArgs e)
        {
            bottomBar.IsEnabled = false;
        }
        private void FinishMainProgressRing(object sender, EventArgs e)
        {
            bottomBar.IsEnabled = true;
        }
        private void RefreshButtons(bool exp, bool rep, bool app, bool set)
        {
            MainController.Instance.InExpenses = exp;
            MainController.Instance.InReports = rep;
            MainController.Instance.InApprovals = app;
            MainController.Instance.InSettings = set;
            this.ReportsToggle.IsChecked = rep;
            this.ApprovalsToggle.IsChecked = app;
            this.SettingsToggle.IsChecked = set;
            this.ExpenseToggle.IsChecked = exp;
        }
        private void LoadExpenses(object sender, RoutedEventArgs e)
        {
            RefreshExpensesButton.Visibility = Visibility.Visible;
            AddExpenseButton.Visibility = Visibility.Visible;
            AddReportButton.Visibility = Visibility.Collapsed;
            LogoutButton.Visibility = Visibility.Collapsed;
            if (_expensesView == null)
            {
                _expensesView = new ExpensesView();
            }
            if (MainPlace.Children.ToArray().Length != 0)
                MainPlace.Children.Clear();
            this.MainPlace.Children.Add(_expensesView);
            RefreshButtons(true, false, false, false);
        }
        private void LoadReports(object sender, RoutedEventArgs e)
        {
            RefreshExpensesButton.Visibility = Visibility.Visible;
            AddExpenseButton.Visibility = Visibility.Collapsed;
            AddReportButton.Visibility = Visibility.Visible;
            LogoutButton.Visibility = Visibility.Collapsed;
            if (_reportsView == null)
                _reportsView = new ReportsView();
            if (MainPlace.Children.ToArray().Length != 0)
                MainPlace.Children.Clear();
            this.MainPlace.Children.Add(_reportsView);
            RefreshButtons(false, true, false, false);
        }
        private void LoadApprovals(object sender, RoutedEventArgs e)
        {
            RefreshExpensesButton.Visibility = Visibility.Visible;
            AddExpenseButton.Visibility = Visibility.Collapsed;
            AddReportButton.Visibility = Visibility.Collapsed;
            LogoutButton.Visibility = Visibility.Collapsed;
            if (_approvalsView == null)
                _approvalsView = new ApprovalsView();
            if (MainPlace.Children.ToArray().Length != 0)
                MainPlace.Children.Clear();
            this.MainPlace.Children.Add(_approvalsView);
            RefreshButtons(false, false, true, false);
        }
        private void LoadSettings(object sender, RoutedEventArgs e)
        {
            AddExpenseButton.Visibility = Visibility.Collapsed;
            AddReportButton.Visibility = Visibility.Collapsed;
            RefreshExpensesButton.Visibility = Visibility.Collapsed;
            LogoutButton.Visibility = Visibility.Visible;
            if (_settingsView == null)
                _settingsView = new SettingsView();
            if (MainPlace.Children.ToArray().Length != 0)
                MainPlace.Children.Clear();
            this.MainPlace.Children.Add(_settingsView);
            RefreshButtons(false, false, false, true);
        }
        private void RefreshButton(object sender, RoutedEventArgs e)
        {
            MainController.Instance.refreshButton();
        }
        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            this.BottomAppBar.IsOpen = true;
        }
        private void AddExpense_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(ExpenseCreation));
        }
        private void AddAllowance_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(AllowanceCreation));
        }
        public void ShowNewExpenseDialog()
        {
            StackPanel actionsStack = new StackPanel();
            NewExpenseFlyout = new MenuFlyout();
            Actionables actionables = Expense.ListShowAddExpenses(
                actionExpense: () =>
                {
                    this.createNewExpense(null);
                },
                actionMileage: () =>
                {
                    this.createNewMileage(null);
                },
                actionAllowance: () =>
                {
                    this.CreateNewAllowance(null);
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
                this.NewExpenseFlyout.Items.Add(item);
            }
        }
        private void DoAction(object sender, RoutedEventArgs e)
        {
            Action act = (Action)((MenuFlyoutItem)sender).Tag;
            act();
        }
        public void createNewExpense(object sender)
        {
            Frame.Navigate(typeof(ExpenseCreation), Expense.NewInstance());
        }
        public void createNewMileage(object sender)
        {
            var newMileage = Mileage.NewInstance();
            newMileage.MileageSegments.AddNewItem();
            newMileage.MileageSegments.AddNewItem();
            Frame.Navigate(typeof(MileageDetailView), newMileage);
        }
        public void CreateNewAllowance(object sender)
        {
            Frame.Navigate(typeof(AllowanceCreation), Allowance.NewInstance());
        }
        private void CreateNewReport(object sender, RoutedEventArgs e)
        {
            Report report = Report.NewInstance ();
            Frame.Navigate(typeof(DraftReportDetailView), report);
        }
        private void Grid_Tapped(object sender, TappedRoutedEventArgs e)
        {
            e.Handled = true;
        }
        private void Logout_Clicked(object sender, RoutedEventArgs e)
        {
            StartMainProgressRing(null, null);
            MainController.Instance.MainPageClear();

            try
            {
                LoadExpenses(null, null);
                this.MainPlace.Children.Clear();
                MainController.Instance.MainPageClear();

                ((Frame)Window.Current.Content).Navigate(typeof(LoginPage));
                while (Frame.BackStack.Count > 0)
                    Frame.BackStack.RemoveAt(Frame.BackStack.Count - 1);
                FinishMainProgressRing(null, null);
                this.NavigationCacheMode = NavigationCacheMode.Disabled;
                this.NavigationCacheMode = NavigationCacheMode.Required;
            }
            catch (ValidationError er)
            {
                MainController.Instance.MainPageClear();
                PopMessages.AsyncMessage(er.Verbose);
                FinishMainProgressRing(null, null);
                return;
            }
            FinishMainProgressRing(null, null);
        }
        public MenuFlyout NewExpenseFlyout { get; set; }

        private void AddExpenseButton_Click(object sender, RoutedEventArgs e)
        {
            this.NewExpenseFlyout.ShowAt(popupgrid);
        }
    }
}
