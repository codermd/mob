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
    public sealed partial class AAmountUC : UserControl
    {
        public AAmountUC(Expense expense)
        {
            this.InitializeComponent();
            Expense = expense;
            ExpenseItem = Expense.ExpenseItems[0];

            CreateButton.Content = Labels.GetLoggedUserLabel(Labels.LabelEnum.Save);

            SetInitScope();
            FieldsListView.ItemsSource = ExpenseItem.AmountFields;
        }
        private void SetInitScope()
        {
            InputScope scope = new InputScope();
            InputScopeName name = new InputScopeName();
            name.NameValue = InputScopeNameValue.Number;
            scope.Names.Add(name);
        }
        public ExpenseItem ExpenseItem { get; set; }
        public Expense Expense { get; set; }

        public static string savedAmount;
        public static string savedQuantity;
        private static void resetValues()
        {
            savedAmount = null;
            savedQuantity = null;
        }
        private void Currency_Tapped(object sender, TappedRoutedEventArgs e)
        {
            ((Frame)Window.Current.Content).Navigate(typeof(AListFromTextBox), ExpenseItem);
            MainController.Instance.ChangeCurrency();
        }

        private void CreateEXpense_Click(object sender, RoutedEventArgs e)
        {
            ((Frame)Window.Current.Content).Navigate(typeof(ExpenseDetailView), Expense);
            
        }

        private void AmountBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if ((e.Key.ToString() == "188" || e.Key.ToString() == "190") && ((sender as TextBox).Text.Contains(".") || (sender as TextBox).Text.Contains(",")))
                e.Handled = true;
        }
    }
}
