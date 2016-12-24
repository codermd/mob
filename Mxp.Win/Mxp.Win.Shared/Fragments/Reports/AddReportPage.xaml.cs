using Mxp.Core.Business;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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

    public sealed partial class AddReportPage : Page
    {
        Expenses Expenses { get; set; }
        public Report Report { get; set; }
        public Expenses SelectedExpenses { get; set; }

        public AddReportPage()
        {
            this.InitializeComponent();
            HardwareButtons.BackPressed += HardwareButtons_BackPressed;
            CancelButton.Label = (LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.Cancel));
            AcceptButton.Label = (LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.Accept));
            this.ExpensesList.DataContext = this;
        }
        void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            e.Handled = true;
            Frame.GoBack();
        }
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            HardwareButtons.BackPressed -= HardwareButtons_BackPressed;
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {

            if (e.Parameter != null)
            {
                if (e.Parameter.GetType() == typeof(Report))
                {
                    Report = e.Parameter as Report;
                    Expenses = LoggedUser.Instance.BusinessExpenses;
                }
            }
        }

        private async void ExpensesList_Loaded(object sender, RoutedEventArgs e)
        {
            this.ProgressRing.IsActive = true;
            this.BottomAppBar.IsEnabled = false;
            try
            {
                await this.Expenses.FetchAsync();
                this.ProgressRing.IsActive = false;
                this.BottomAppBar.IsEnabled = true;
            }
            catch (Exception error)
            {
                MessageDialog messageDialog = new MessageDialog(error.GetExceptionMessage());
                messageDialog.Commands.Add(new UICommand("OK", (command) => { }));
                messageDialog.ShowAsync();
                this.ProgressRing.IsActive = false;
                this.BottomAppBar.IsEnabled = true;
                return;
            }

            foreach (Expense expense in Expenses)
            {
                ExpensesList.Items.Add(expense);
            }


            //var result = from t in Expenses.ToList<Expense>()
            //             group t by t.VDateHeader into g
            //             select new { Key = g.Key, Items = g };
            //ExpensesSource.Source = result;
        }

        private void CancelAddExpenses_Click(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }

        private async void AddExpenses_Click(object sender, RoutedEventArgs e)
        {
            this.ProgressRing.IsActive = true;
            this.BottomAppBar.IsEnabled = false;
            try
            {
                await Report.SaveAsync();
                this.Report.NotifyPropertyChanged(string.Empty);
            }
            catch (Exception error)
            {
                MessageDialog messageDialog = new MessageDialog(error.GetExceptionMessage());
                messageDialog.Commands.Add(new UICommand("OK", (command) => { }));
                messageDialog.ShowAsync();
                this.ProgressRing.IsActive = false;
                this.BottomAppBar.IsEnabled = true;
                return;
            }
            Frame.GoBack();
        }
    }
}
