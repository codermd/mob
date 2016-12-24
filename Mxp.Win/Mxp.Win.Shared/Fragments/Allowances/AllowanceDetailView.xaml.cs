using Mxp.Core.Business;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
    public sealed partial class AllowanceDetailView : Page
    {
        public Allowance Allowance { get; set; }
        ExpenseItem ExpenseItem { get; set; }
        public Collection<TableSectionModel> CollectionFields { get; set; }
        public Collection<Field> Fields { get; set; }
        public Collection<AllowanceSegment> AllowanceSegmentCol { get; set; }
        private readonly NavigationHelper navigationHelper;
        private readonly ObservableDictionary defaultViewModel = new ObservableDictionary();
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }
        private void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            if (e.PageState != null && e.PageState.ContainsKey("Allowance"))
                Allowance = e.PageState["Allowance"] as Allowance;
            if (e.PageState != null && e.PageState.ContainsKey("ExpenseItem"))
                ExpenseItem = e.PageState["ExpenseItem"] as ExpenseItem;

        }
        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
            if (Allowance != null)
                e.PageState["Allowance"] = Allowance;
            if (ExpenseItem != null)
                e.PageState["ExpenseItem"] = ExpenseItem;
            if (Allowance == null || ExpenseItem == null)
                e.PageState.Clear();

        }
        private void InitializeNavigationHelper()
        {
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            navigationHelper.SaveState += NavigationHelper_SaveState;

        }
        public AllowanceDetailView()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            LoadLabels();
            HardwareButtons.BackPressed += HardwareButtons_BackPressed;

        }
        private void LoadLabels()
        {
            Title.Text = (LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.Allowance));
            DeleteButton.Label = (LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.Delete));
            DiscardButton.Label = (LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.Cancel));
            SaveButton.Label = (LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.Accept));
        }
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedFrom(e);
            this.Allowance.PropertyChanged -= HandlePropertyChanged;
            this.ExpenseItem.PropertyChanged -= HandlePropertyChanged;
            HardwareButtons.BackPressed -= HardwareButtons_BackPressed;


        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedTo(e);

            if (Allowance == null)
            {
                Allowance = (Allowance)e.Parameter;
                
            }
            if (Allowance.IsChanged)
            {
                try
                {
                    this.ProgressRing.IsActive = true;
                    this.BottomAppBar.IsEnabled = false;
                }
                catch (Exception error)
                {
                    MessageDialog messageDialog = new MessageDialog(error.GetExceptionMessage());
                    messageDialog.Commands.Add(new UICommand((LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.Accept)), (command) => { }));
                    messageDialog.ShowAsync();
                    this.ProgressRing.IsActive = false;
                    this.BottomAppBar.IsEnabled = true;
                    return;
                }
                this.ProgressRing.IsActive = false;
                this.BottomAppBar.IsEnabled = true;
            }
            if (ExpenseItem == null)
                ExpenseItem = Allowance.ExpenseItems[0];

            this.TotalTB.Text = LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.Total) + " : " + Allowance.VAmountCC;

            FillList();
            if (this.Allowance.IsNew || this.Allowance.IsChanged || this.ExpenseItem.IsChanged)
            {
                SaveButton.Visibility = Visibility.Visible;
                DiscardButton.Visibility = Visibility.Visible;
            }
            this.Allowance.PropertyChanged += HandlePropertyChanged;
            this.ExpenseItem.PropertyChanged += HandlePropertyChanged;
            ProgressRing.IsActive = false;
        }

        private async void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            e.Handled = true;

            ProgressGrid.Visibility = Visibility.Visible;
            try
            {
                if (Allowance.IsChanged)
                {
                    await Allowance.FetchAsync();
                    await LoggedUser.Instance.BusinessExpenses.FetchAsync();
                }
            }
            catch (Exception error)
            {
                PopMessages.AsyncMessage(error.Message);
            }
            ProgressGrid.Visibility = Visibility.Collapsed;
        }

        private void HandlePropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (this.Allowance.IsNew || this.Allowance.IsChanged || this.ExpenseItem.IsChanged)
            {
                SaveButton.Visibility = Visibility.Visible;
                DiscardButton.Visibility = Visibility.Visible;
            }
        }
        private async void SaveButtonClick(object sender, RoutedEventArgs e)
        {
            try
            {
                this.ProgressRing.IsActive = true;
                this.BottomAppBar.IsEnabled = false;
                await Allowance.SaveAsync();
            }
            catch (Exception error)
            {
                MessageDialog messageDialog = new MessageDialog(error.GetExceptionMessage());
                messageDialog.Commands.Add(new UICommand((LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.Accept)), (command) => { }));
                messageDialog.ShowAsync();

                this.ProgressRing.IsActive = false;
                this.BottomAppBar.IsEnabled = true;
                return;
            }
            navigationHelper.GoBack();
        }
        private async void DiscardButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.ProgressRing.IsActive = true;
                this.BottomAppBar.IsEnabled = false;
                this.Allowance.ResetChanged();
                await LoggedUser.Instance.BusinessExpenses.FetchAsync();
                navigationHelper.GoBack();
            }
            catch (Exception error)
            {
                MessageDialog messageDialog = new MessageDialog(error.GetExceptionMessage());
                messageDialog.Commands.Add(new UICommand((LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.Accept)), (command) => { }));
                messageDialog.ShowAsync();
                this.ProgressRing.IsActive = false;
                this.BottomAppBar.IsEnabled = true;
            }
        }
        private async void FillList()
        {
            try
            {
                this.ProgressRing.IsActive = true;
                this.BottomAppBar.IsEnabled = false;
                await this.Allowance.FetchAsync();

            }
            catch (Exception error)
            {
                MessageDialog messageDialog = new MessageDialog(error.GetExceptionMessage());
                messageDialog.Commands.Add(new UICommand((LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.Accept)), (command) => { }));
                messageDialog.ShowAsync();
                this.ProgressRing.IsActive = false;
                this.BottomAppBar.IsEnabled = true;
                return;
            }
            Fields = ExpenseItem.GetAllFields();
            this.ProgressRing.IsActive = false;
            this.BottomAppBar.IsEnabled = true;
            AllowanceSegmentCol = Allowance.AllowanceSegments;
            this.FieldsListView.ItemsSource = Fields;
            SegmentsListView.ItemsSource = AllowanceSegmentCol;
            DeleteButton.Visibility = Allowance.ExpenseItems[0].CanShowDelete ? Visibility.Visible : Visibility.Collapsed;
            //EnableScroll();
        }
        private async void EnableScroll()
        {
            ProgressRing.IsActive = true;
            ProgressGrid.Visibility = Visibility.Visible;
            await Task.Delay(TimeSpan.FromSeconds(4));
            ScrollViewer viewer = GetScrollViewer(FieldsListView);
            viewer.VerticalScrollBarVisibility = ScrollBarVisibility.Hidden;
            ProgressRing.IsActive = false;
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
        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var messageDialog = new Windows.UI.Popups.MessageDialog(LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.DoYouConfirm), LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.Delete) + " " + LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.Allowance));
            messageDialog.Commands.Add(new UICommand((LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.Yes)), (command) => { DeleteAllowance(); }));
            messageDialog.Commands.Add(new UICommand((LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.No)), (command) => { }));
            messageDialog.DefaultCommandIndex = 1;
            await messageDialog.ShowAsync();
        }
        private async void DeleteAllowance()
        {
            ProgressGrid.Visibility = Visibility.Visible;
            this.ProgressRing.IsActive = true;
            this.BottomAppBar.IsEnabled = false;
            try
            {
                await this.ExpenseItem.DeleteExpenseAsync();
            }
            catch (Exception error)
            {
                MessageDialog messageDialog = new MessageDialog(error.GetExceptionMessage());
                messageDialog.Commands.Add(new UICommand((LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.Accept)), (command) => { }));
                messageDialog.ShowAsync();

                this.ProgressRing.IsActive = false;
                this.BottomAppBar.IsEnabled = true;
                ProgressGrid.Visibility = Visibility.Collapsed;
                return;
            }

            this.ProgressRing.IsActive = false;
            this.BottomAppBar.IsEnabled = true;
            ProgressGrid.Visibility = Visibility.Collapsed;
            navigationHelper.GoBack();
        }
    }
}
