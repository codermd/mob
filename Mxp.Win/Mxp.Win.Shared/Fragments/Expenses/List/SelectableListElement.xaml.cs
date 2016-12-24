using Mxp.Core.Business;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

namespace Mxp.Win
{
    public sealed partial class SelectableListElement : UserControl
    {
        private static readonly DependencyProperty InternalDataContextProperty = DependencyProperty.Register(
            "InternalDataContext", typeof(object), typeof(SelectableListElement), new PropertyMetadata(default(object), PropertyChangedCallback));

        private static void PropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            SelectableListElement control = dependencyObject as SelectableListElement;

            if (dependencyPropertyChangedEventArgs.NewValue is Expense && control.Report != null)
                control.SetCheckbox(dependencyPropertyChangedEventArgs.NewValue as Expense, control.Report);
        }

        public static readonly DependencyProperty ReportProperty = DependencyProperty.Register ("Report", typeof(Report), typeof(SelectableListElement), new PropertyMetadata (default(Report), ReportPropertyChangedCallback));

        private static void ReportPropertyChangedCallback (DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs) {
            SelectableListElement control = dependencyObject as SelectableListElement;

            if (dependencyPropertyChangedEventArgs.NewValue != null)
                control.SetCheckbox (control.DataContext as Expense, (Report)dependencyPropertyChangedEventArgs.NewValue);
        }

        public Report Report {
            get { return (Report) GetValue (ReportProperty); }
            set { SetValue (ReportProperty, value); }
        }


        public bool Selected { get; set; }
        public SelectableListElement()
        {
            this.InitializeComponent();

            SetBinding(InternalDataContextProperty,new Binding());
        }
        public void AddSplit()
        {
            List<ExpenseItem> items = new List<ExpenseItem>();
            foreach (var exp in expense.ExpenseItems)
            {
                items.Add(exp);
            }
            ExpenseItem item = new ExpenseItem();
            ItemsList.ItemsSource = items;
        }
        public void setTitle(String title)
        {
            this.Title.Text = title;
        }
        public void setPrice1(string price)
        {
            this.Price1.Text = price;
        }
        public void setDate(string date)
        {
            this.Date.Text = date;
        }
        private async void Grid_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (sender is Grid)
            {
                if (((Grid)sender).DataContext is ExpenseItem)
                {
                    ExpenseItem exp = (ExpenseItem)((Grid)sender).DataContext;
                    ((Frame)Window.Current.Content).Navigate(typeof(ExpenseDetailView), exp);
                }
            }
            if (this.DataContext is Mileage)
            {
                Mileage mileage = this.DataContext as Mileage;
                MainController.Instance.StartMainProgressRing();

                try
                {
                    await mileage.MileageSegments.FetchAsync();
                    ((Frame)Window.Current.Content).Navigate(typeof(MileageDetailView), mileage);
                }
                catch (Exception error)
                {
                    MessageDialog messageDialog = new MessageDialog(error.GetExceptionMessage());
                    messageDialog.Commands.Add(new UICommand("OK", (command) => { }));
                    messageDialog.ShowAsync();
                    MainController.Instance.FinishMainProgressRing();

                    return;
                }
                MainController.Instance.FinishMainProgressRing();
            }
            else if (this.DataContext is Allowance)
            {
                ((Frame)Window.Current.Content).Navigate(typeof(AllowanceDetailView), (sender as Grid).DataContext);
            }
            else if (this.DataContext is Expense)
            {
                expense = (Expense)this.DataContext;
                AddSplit();
                if (expense.IsSplit)
                {
                    if (this.GridSplit.Visibility == Visibility.Visible)
                        this.GridSplit.Visibility = Visibility.Collapsed;
                    else
                        this.GridSplit.Visibility = Visibility.Visible;
                }
                else
                {
                    ((Frame)Window.Current.Content).Navigate(typeof(ExpenseDetailView), expense);
                }
            }
            e.Handled = true;
        }
        Expense expense;

        private void ImageItemPolicy_Loaded(object sender, RoutedEventArgs e)
        {
            ExpenseItem item = ((Image)sender).DataContext as ExpenseItem;
            BitmapImage bmpPolicy = new BitmapImage();
            switch (item.PolicyRule)
            {
                case ExpenseItem.PolicyRules.Green:
                    bmpPolicy.UriSource = new Uri("ms-appx:" + "/Assets/icons/ExpenseIsCompliant.png");
                    break;
                case ExpenseItem.PolicyRules.Orange:
                    bmpPolicy.UriSource = new Uri("ms-appx:" + "/Assets/icons/ExpenseNotCompliantPolicy.png");
                    break;
                case ExpenseItem.PolicyRules.Red:
                    bmpPolicy.UriSource = new Uri("ms-appx:" + "/Assets/icons/ExpenseNotCompliant.png");
                    break;
            }
            ((Image)sender).Source = bmpPolicy;
        }

        private void CheckBox_Clicked(object sender, RoutedEventArgs e)
        {
            expense = this.DataContext as Expense;

            if (this.expense.IsFromApproval)
                this.expense.ExpenseItems[0].Toggle ();
            else {
                if ((bool)((CheckBox)sender).IsChecked)
                {
                    Report.Expenses.Add(expense);
                }
                else
                {
                    Report.Expenses.Remove(expense);
                }
            }
        }

        private void SplitCheckBox_Clicked(object sender, RoutedEventArgs e)
        {

        }

        private void SplitCheck_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void SetCheckbox (Expense expense, Report report) {
            if (expense == null)
                return;

            if (expense.IsFromApproval)
                this.SelectionCheckBox.IsChecked = expense.ExpenseItems[0].StatusForApprovalReport == ExpenseItem.Status.Accepted;
            else {
                this.SelectionCheckBox.IsChecked = report.Expenses.Contains(expense);
                this.SelectionCheckBox.IsEnabled = expense.IsDeselectable;
            }
        }
    }
}