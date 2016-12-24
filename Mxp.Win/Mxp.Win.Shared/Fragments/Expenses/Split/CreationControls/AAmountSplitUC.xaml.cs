using Mxp.Core.Business;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
#if WINDOWS_PHONE_APP
using Windows.Phone.UI.Input;
using Windows.UI.Popups;
#endif
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace Mxp.Win
{
    public sealed partial class AAmountSplitUC : UserControl
    {
        public AAmountSplitUC(ExpenseItemToSplit expenseItemToSplit)
        {
            this.InitializeComponent();
            ExpenseItemToSplit = expenseItemToSplit;


            CreateButton.Content = Labels.GetLoggedUserLabel(Labels.LabelEnum.Save);

    
            FieldsListView.ItemsSource = ExpenseItemToSplit.InnerExpenseItem.AmountFields;
            if (ExpenseItemToSplit.InnerExpenseItem.AmountLC != 0.0)
            {
                CreateButton.Content = "Update Expense";
            }
        }
        public ExpenseItemToSplit ExpenseItemToSplit { get; set; }


        private void CreateEXpense_Click(object sender, RoutedEventArgs e)
        {
            if (!ExpenseItemToSplit.ExpenseItem.InnerSplittedItems.Contains(ExpenseItemToSplit.InnerExpenseItem))
                ExpenseItemToSplit.ExpenseItem.InnerSplittedItems.Add(ExpenseItemToSplit.InnerExpenseItem);
            ((Frame)Window.Current.Content).Navigate(typeof(SplitListPage), ExpenseItemToSplit);
        }

        private void AmountBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if ((e.Key.ToString() == "188" || e.Key.ToString() == "190") && ((sender as TextBox).Text.Contains(".") || (sender as TextBox).Text.Contains(",")))
                e.Handled = true;
        }
    }
}
