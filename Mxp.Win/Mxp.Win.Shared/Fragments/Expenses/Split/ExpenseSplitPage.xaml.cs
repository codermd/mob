using Mxp.Core.Business;
using System;
using System.Collections.Generic;
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
    public sealed partial class ExpenseSplitPage : Page
    {
        ExpenseItemToSplit ExpenseItemToSplit { get; set; }

        Grid CategorySectionGrid { get; set; }
        Grid AmountSectionGrid { get; set; }
        public CategoriesFromExpenseViewModel Categories { get; set; }
        public ExpenseSplitPage()
        {
            this.InitializeComponent();
            MainController.Instance.ExpensesCreationCategoryChoosedRequest += MainControllerExpensesCreationCategoryChoosed;
            MainController.Instance.ExpensesCreationSuccessedRequest += MainControllerExpensesCreationSuccessed;
            HardwareButtons.BackPressed += HardwareButtons_BackPressed;
            CategoriesHeader.Text = (LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.Categories));
            AmountHeader.Text = (LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.Amount));
            Title.Text = (LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.Split));
        }

        void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            e.Handled = true;
            currentView = 0;
            Frame.GoBack();
        }
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            HardwareButtons.BackPressed -= HardwareButtons_BackPressed;
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ExpenseItemToSplit = e.Parameter as ExpenseItemToSplit;
            if (ExpenseItemToSplit != null)
            {
                if (ExpenseItemToSplit.InnerExpenseItem == null)
                {
                    currentView = 1;
                    ExpenseItemToSplit.InnerExpenseItem = new ExpenseItem
                    {
                        Quantity = 1
                    };
                    ExpenseItemToSplit.InnerExpenseItem.SetCollectionParent(this.ExpenseItemToSplit.ExpenseItem.InnerSplittedItems);
                }
                else
                {
                    currentView = 2;
                    ScrollToAmount = false;
                }
                switch (currentView)
                {
                    case 0:
                        break;
                    case 1:
                        break;
                    case 2:
                        CategorySection.Visibility = Visibility.Visible;
                        AmountSection.Visibility = Visibility.Visible;

                        break;
                    default:
                        break;
                }
            }
        }
        bool ScrollToAmount = true;
        private void MainControllerExpensesCreationCategoryChoosed(object sender, EventArgs e)
        {
            AmountSection.Visibility = Visibility.Visible;
        }
        public static int currentView { get; set; }
        private void CategorySectionGrid_Loaded(object sender, RoutedEventArgs e)
        {
            CategorySectionGrid = (Grid)sender;
            SelectedProductToColorConverter.SelectedProduct = ExpenseItemToSplit.InnerExpenseItem.Product;
            CategorySectionGrid.Children.Add(new ACategorySplitUC(ExpenseItemToSplit));  
        }
        private void AmountSectionGrid_Loaded(object sender, RoutedEventArgs e)
        {
            AmountSectionGrid = (Grid)sender;
            AmountSectionGrid.Children.Add(new AAmountSplitUC(ExpenseItemToSplit));
            if (ScrollToAmount)
                HubCreator.ScrollToSection(AmountSection);
            ScrollToAmount = true;
        }
        private void MainControllerExpensesCreationSuccessed(object sender, EventArgs e)
        {
        }
    }
}
