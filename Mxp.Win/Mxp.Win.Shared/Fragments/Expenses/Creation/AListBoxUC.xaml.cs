using Mxp.Core.Business;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Phone.UI.Input;
using Windows.UI;
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
    public sealed partial class AListBoxUC : UserControl
    {
        public event EventHandler SelectedEvent;
        public AListBoxUC(Expense expense, AListboxUCType type)
        {
            this.InitializeComponent();
            Expense = expense;
            ExpenseItem = Expense.ExpenseItems[0];
            Type = type;
            switch(type) {
                case AListboxUCType.CategoryAll:
                case AListboxUCType.CategoryTravel:
                case AListboxUCType.CategoryNonTravel:
                    this.DataContext = new CategoriesViewModel (null, type);
                    break;
                case AListboxUCType.Country:
                    this.DataContext = new CountriesViewModel ();
                    break;
            }
        }
        Grid TappedGrid;
        AListboxUCType Type;
        ExpenseItem ExpenseItem { get; set; }
        Expense Expense { get; set; }
        private void ItemTapped(object sender, TappedRoutedEventArgs e)
        {
            if (this.TBListFilter.FocusState == FocusState.Keyboard)
            {
                TBListFilter.Focus(FocusState.Unfocused);
            }
            ChangeTappedGridColor((Grid)sender);
            switch (Type)
            {
                case AListboxUCType.CategoryAll:
                case AListboxUCType.CategoryTravel:
                case AListboxUCType.CategoryNonTravel:
                    CreateCategory (sender as Grid);
                    break;
                case AListboxUCType.Country:
                    CreateCountry (sender as Grid);
                    break;
            }

            this.SelectedEvent?.Invoke (this, new EventArgs());
        }
        private void ChangeTappedGridColor(Grid grid)
        {
            if (TappedGrid != null)
            {
                TappedGrid.Background = new SolidColorBrush(Color.FromArgb(255, 239, 239, 239));
            }
            TappedGrid = grid;
            TappedGrid.Background = new SolidColorBrush(Color.FromArgb(255, 0, 168, 198));
        }

        public void ClearList () {
            if (this.TappedGrid != null)
                TappedGrid.Background = new SolidColorBrush (Color.FromArgb (255, 239, 239, 239));
        }
        private  void CreateCategory(Grid sender)
        {
            ExpenseItem.Product = sender.DataContext as Product;
            MainController.Instance.ExpensesCreationCategoryChoosed();
        }
        private void CreateCountry(Grid sender)
        {
            ExpenseItem.Country = sender.DataContext as Country;
            MainController.Instance.ExpensesCreationCountryChoosed();
        }

        private void TBListFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (this.DataContext is CategoriesViewModel)
            {
                this.DataContext = new CategoriesViewModel(((TextBox)sender).Text,this.Type);
            }
            else if (this.DataContext is CountriesViewModel)
            {
                this.DataContext = new CountriesViewModel(((TextBox)sender).Text);
            }
        }
    }

    public enum AListboxUCType
    {
        CategoryAll,
        CategoryTravel,
        CategoryNonTravel,
        Country
    }
}