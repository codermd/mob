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
    public sealed partial class ACategorySplitUC : UserControl
    {
        public ACategorySplitUC(ExpenseItemToSplit expenseItemToSplit)
        {
            this.InitializeComponent();
            ExpenseItemToSplit = expenseItemToSplit;
            this.DataContext = ExpenseItemToSplit.AvailableProducts;
            FirstProduct = ExpenseItemToSplit.InnerExpenseItem.Product;

        }
        Grid TappedGrid;
        Product FirstProduct;
        ExpenseItemToSplit ExpenseItemToSplit { get; set; }
        private void ItemTapped(object sender, TappedRoutedEventArgs e)
        {
            ChangeTappedGridColor((Grid)sender);
            CreateCategory(sender as Grid);

        }
        private void ChangeTappedGridColor(Grid grid)
        {
            if (TappedGrid != null)
            {
                Product product = TappedGrid.DataContext as Product;

                if ((product) != FirstProduct)
                    TappedGrid.Background = new SolidColorBrush(Color.FromArgb(255, 239, 239, 239));
                else if (product == FirstProduct)
                    TappedGrid.Background = new SolidColorBrush(Color.FromArgb(128, 0, 168, 198));
            }
            TappedGrid = grid;
            TappedGrid.Background = new SolidColorBrush(Color.FromArgb(255, 0, 168, 198));
        }
        private void CreateCategory(Grid sender)
        {
            ExpenseItemToSplit.InnerExpenseItem.Product = sender.DataContext as Product;
            MainController.Instance.ExpensesCreationCategoryChoosed();
        }
    }
}