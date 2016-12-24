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
    public sealed partial class SplitListPage : Page
    {
        ExpenseItemToSplit ExpenseItemToSplit { get; set; }
        ExpenseItems InnerExpenseItems;
        public SplitListPage()
        {
            this.InitializeComponent();
            HardwareButtons.BackPressed += HardwareButtons_BackPressed;
            Title.Text = (LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.Split));
        }
        void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            e.Handled = true;
            ExpenseSplitPage.currentView = 0;
            ExpenseItemToSplit.ExpenseItem.InnerSplittedItems.Clear();
            Frame.Navigate(typeof(MainPage));
            Frame.Navigate(typeof(ExpenseDetailView), ExpenseItemToSplit.ExpenseItem);
        }
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            HardwareButtons.BackPressed -= HardwareButtons_BackPressed;
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ExpenseItemToSplit = e.Parameter as ExpenseItemToSplit;
            InnerExpenseItems = ExpenseItemToSplit.ExpenseItem.InnerSplittedItems;
            ItemsList.Items.Clear();
            foreach (var item in InnerExpenseItems)
                ItemsList.Items.Add(item);
            Title.Text = ExpenseItemToSplit.ExpenseItem.VTitleSplit;
        }
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            ExpenseSplitPage.currentView = 0;
            ExpenseItemToSplit.ExpenseItem.InnerSplittedItems.Clear();
            Frame.Navigate(typeof(MainPage));
            Frame.Navigate(typeof(ExpenseDetailView), ExpenseItemToSplit.ExpenseItem);
            //Frame.GoBack();
            //Frame.BackStack.RemoveAt(Frame.BackStack.Count - 1);
            //Frame.GoBack();

        }
        private async void Accept_Click(object sender, RoutedEventArgs e)
        {
            this.ProgressRing.IsActive = true;
            this.BottomAppBar.IsEnabled = false;
            try
            {
                if (ExpenseItemToSplit.ExpenseItem.InnerSplittedItems.Count > 0)
                    await ExpenseItemToSplit.ExpenseItem.SplitAsync();
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
            Frame.Navigate(typeof(MainPage));
        }
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            
            ExpenseItemToSplit.InnerExpenseItem = null;
            Frame.BackStack.RemoveAt(Frame.BackStack.Count - 1);

            Frame.Navigate(typeof(ExpenseSplitPage), ExpenseItemToSplit);
        }
        private void SplitListElement_Tapped(object sender, EventArgs e)
        {
            ExpenseItemToSplit.InnerExpenseItem = ((SplitListElement)sender).DataContext as ExpenseItem;
            Frame.Navigate(typeof(ExpenseSplitPage), ExpenseItemToSplit);
        }

        private void DeleteInner_Tapped(object sender, EventArgs e)
        {
            ExpenseItem itemToDelete = ((SplitListElement)sender).DataContext as ExpenseItem;
            ExpenseItemToSplit.ExpenseItem.InnerSplittedItems.Remove(itemToDelete);
            if (ItemsList.Items.Contains(itemToDelete))
                this.ItemsList.Items.Remove(itemToDelete);
            Title.Text = ExpenseItemToSplit.ExpenseItem.VTitleSplit;
        }
    }
}
