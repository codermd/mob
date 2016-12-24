using Mxp.Core.Business;
using Mxp.Win.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Phone.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace Mxp.Win
{
    public sealed partial class AListFromTextBox : Page
    {
        public AListFromTextBox()
        {
            this.InitializeComponent();
            HardwareButtons.BackPressed += HardwareButtons_BackPressed;
            this.TBListFilter.PlaceholderText = (LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.Search));
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
        private LookupItems LUItems { get; set; }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is Field)
            {
                Field = e.Parameter as Field;
                switch (Field.Type)
                {
                    case FieldTypeEnum.Lookup:
                        Title.Text = (LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.Select));
                        LUItems = ((LookupField)Field).Results;
                        this.DataContext = new LookupItemsViewModel(Field);
                        ViewSourceItemsByGroup.IsSourceGrouped = false;

                        Type = "Lookup";
                        break;
                    case FieldTypeEnum.Country:
                        Title.Text = (LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.PickupCountry));


                        if (Field is AttendeeFormCountry)
                            this.DataContext = new CountriesViewModel(null, Field);
                        else
                            this.DataContext = new CountriesViewModel();
                        Type = "Country";
                        break;
                    case FieldTypeEnum.Category:
                        Title.Text = (LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.Categories));
                        this.DataContext = new CategoriesViewModel();
                        Type = "Category";
                        break;
                    case FieldTypeEnum.Currency:
                        Title.Text = (LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.Currencies));
                        this.DataContext = new CurrenciesViewModel();
                        Type = "Currency";
                        break;
                    default:
                        break;
                }
            }
            else if (e.Parameter is ExpenseItem)
            {
                ExpenseItem = e.Parameter as ExpenseItem;
                Title.Text = (LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.CustomerCurrency));
                this.DataContext = new CurrenciesViewModel();
                Type = "Currency";
            }
            else
            {
                ExpenseItemCreator creator = e.Parameter as ExpenseItemCreator;
                Expense = creator.Item;
                this.DataContext = new CategoriesViewModel();
                Type = creator.Type;
            }
        }
        String Type = null;
        public Field Field { get; set; }
        ExpenseItem ExpenseItem { get; set; }
        Expense Expense { get; set; }
        private void ItemTapped(object sender, TappedRoutedEventArgs e)
        {
            if (Field != null)
            {
                FillField(sender as Grid);
            }
            if (Expense != null)
            {
                switch (Type)
                {
                    case "CategoryCreator":
                        CreateCategory(sender as Grid);
                        break;
                    case "CountryCreator":
                        CreateCountry(sender as Grid);
                        break;
                }
            }
            else if (ExpenseItem != null)
            {
                switch (Type)
                {
                    case "Currency":
                        FillCurrency(sender as Grid);
                        break;
                }
            }
        }

        private void FillCurrency(Grid sender)
        {
            ExpenseItem.Currency = (sender as Grid).DataContext as Currency;
            Frame.GoBack();
        }
        private void FillField(Grid sender)
        {
            Field.Value = (sender as Grid).DataContext;
            Frame.GoBack();
        }
        private void CreateCategory(Grid sender)
        {
            Expense.ExpenseItems[0].Product = sender.DataContext as Product;
            ExpenseItemCreator itemCountry = new ExpenseItemCreator() { Type = "CountryCreator", Item = Expense };
            this.DataContext = new CountriesViewModel();
        }
        private void CreateCountry(Grid sender)
        {
            ExpenseItem.Country = sender.DataContext as Country;
        }
        private void TBListFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (this.DataContext is CategoriesViewModel)
                this.DataContext = new CategoriesViewModel(((TextBox)sender).Text);
            else if (this.DataContext is CountriesViewModel)
            {
                if (Field is AttendeeFormCountry)
                    this.DataContext = new CountriesViewModel(((TextBox)sender).Text, Field);
                else
                    this.DataContext = new CountriesViewModel(((TextBox)sender).Text);
            }
            else if (this.DataContext is CurrenciesViewModel)
                this.DataContext = new CurrenciesViewModel(((TextBox)sender).Text);
            else if (this.DataContext is LookupItemsViewModel)
                this.DataContext = new LookupItemsViewModel(Field, ((TextBox)sender).Text);
        }
    }
}
