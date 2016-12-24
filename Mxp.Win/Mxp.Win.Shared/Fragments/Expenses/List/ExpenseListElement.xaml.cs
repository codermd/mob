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
    public sealed partial class ExpenseListElement : UserControl
    {
        public ExpenseListElement()
        {
            this.InitializeComponent();
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

        private async void Grid_Tapped(object sender, TappedRoutedEventArgs e)
        {

            if ((sender as Grid).DataContext is Mileage)
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
            else if ((sender as Grid).DataContext is Allowance)
            {
                ((Frame)Window.Current.Content).Navigate(typeof(AllowanceDetailView), (sender as Grid).DataContext);
            }
            else if ((sender as Grid).DataContext is Expense)
            {
                expense = (Expense)(sender as Grid).DataContext;


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
            else
            {
                ExpenseItem exp = (ExpenseItem)(sender as Grid).DataContext;
                ((Frame)Window.Current.Content).Navigate(typeof(ExpenseDetailView), exp);
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
    }
}