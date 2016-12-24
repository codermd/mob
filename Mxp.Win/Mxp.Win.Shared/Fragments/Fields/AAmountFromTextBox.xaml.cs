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
    public sealed partial class AAmountFromTextBox : Page
    {
        public AAmountFromTextBox()
        {
            this.InitializeComponent();
            SetInitScope();
            HardwareButtons.BackPressed += HardwareButtons_BackPressed;
            Title.Text = (LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.Amount));
        }
        void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            e.Handled = true;
            e.Handled = true;
            Frame.GoBack();
            resetValues();
        }
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            HardwareButtons.BackPressed -= HardwareButtons_BackPressed;
        }
        private void SetInitScope()
        {
            InputScope scope = new InputScope();
            InputScopeName name = new InputScopeName();
            name.NameValue = InputScopeNameValue.Number;
            scope.Names.Add(name);
            AmountBox.InputScope = scope;
        }

        public ExpenseItem item { get; set; }

        static string savedAmount;
        static string savedQuantity;
        private static void resetValues()
        {
            savedAmount = null;
            savedQuantity = null;
        }


        ExpenseItem ExpenseItem;


        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Field = e.Parameter as Field;
            ExpenseItem = Field.GetModel<ExpenseItem>();
            item = Field.GetModel<ExpenseItem>();
            this.AmountLabel.Text = Labels.GetLoggedUserLabel(Labels.LabelEnum.Amount);
            this.CurrencyLabel.Text = Labels.GetLoggedUserLabel(Labels.LabelEnum.Currency);
            this.QuantityLabel.Text = Labels.GetLoggedUserLabel(Labels.LabelEnum.Quantity);

            AmountBox.Text = item.AmountLC.ToString();
            QuantityBox.Text = item.Quantity.ToString();

            CurrencyBlock.Text = item.Currency.VName;
            if (!String.IsNullOrWhiteSpace(savedAmount))
                AmountBox.Text = savedAmount;
            if (!String.IsNullOrWhiteSpace(savedQuantity))
                QuantityBox.Text = savedQuantity;
        }
        public Field Field { get; set; }

        private void Currency_Tapped(object sender, TappedRoutedEventArgs e)
        {
            ((Frame)Window.Current.Content).Navigate(typeof(AListFromTextBox), item);
            savedAmount = AmountBox.Text;
            savedQuantity = QuantityBox.Text;
        }

        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(AmountBox.Text))
            {
                AmountBox.Text = "0";
                item.AmountLC = 0;
            }
            else
            {
                try
                {
                    item.AmountLC = Double.Parse(AmountBox.Text.Replace(',', '.'), CultureInfo.InvariantCulture);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    return;
                }
            }
            if (String.IsNullOrEmpty(QuantityBox.Text))
            {
                QuantityBox.Text = "1";
            }

            if (String.IsNullOrEmpty(QuantityBox.Text))
            {
                item.Quantity = 1;
            }
            else
            {
                item.Quantity = int.Parse(QuantityBox.Text);
            }
            Frame.GoBack();
            resetValues();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
            resetValues();
        }

        private void AmountBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {

            if ((e.Key.ToString() == "188" || e.Key.ToString() == "190") && ((sender as TextBox).Text.Contains(".") || (sender as TextBox).Text.Contains(",")))
                e.Handled = true;
        }

        private void Up(object sender, KeyRoutedEventArgs e)
        {

        }
    }
}
