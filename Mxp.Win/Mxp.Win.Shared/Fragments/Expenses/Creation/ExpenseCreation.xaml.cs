using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Mxp.Core.Business;
using System.Diagnostics;
using Windows.Phone.UI.Input;
using System.Collections.ObjectModel;
using Windows.UI.Popups;

namespace Mxp.Win
{
    public sealed partial class ExpenseCreation : Page
    {
        public bool IsSplitted => Preferences.Instance.FilterTravelCategory;

        public ExpenseCreation()
        {
            this.InitializeComponent();
            MainController.Instance.ExpensesCreationCategoryChoosedRequest += MainControllerExpensesCreationCategoryChoosed;
            MainController.Instance.ExpensesCreationCountryChoosedRequest += MainControllerExpensesCreationCountryChoosed;
            MainController.Instance.ExpensesCreationAmountChoosedRequest += MainControllerExpensesCreationAmountChoosed;
            MainController.Instance.ChangeCurrencyRequest += Instance_ChangeCurrencyRequest;
            MainController.Instance.ExpensesCreationSuccessedRequest += MainControllerExpensesCreationSuccessed;
            HardwareButtons.BackPressed += HardwareButtons_BackPressed;

            this.Title.Text = LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.CreateExpense);
            this.CategoriesHeader.Text = LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.Categories);
            this.CountriesHeader.Text = LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.Countries);
            this.AmountHeader.Text = LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.Amount);
            

            if (this.IsSplitted) {
                this.CategoriesHeader.Text = LoggedUser.Instance.Labels.GetLabel (Labels.LabelEnum.TravelFilter);
                this.CategoriesHeaderNonTravel.Text = LoggedUser.Instance.Labels.GetLabel (Labels.LabelEnum.NonTravelFilter);
                this.CategorySectionNonTravel.Visibility = Visibility.Visible;
            }
            
        }

        private void Instance_ChangeCurrencyRequest(object sender, EventArgs e)
        {
            currentView = 2;
        }

        void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            e.Handled = true;
            currentView = 0;
            Frame.GoBack();
            AAmountUC.savedAmount = null;
            AAmountUC.savedQuantity = null;
            MainController.Instance.ChangeCurrencyRequest -= Instance_ChangeCurrencyRequest;

        }
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            HardwareButtons.BackPressed -= HardwareButtons_BackPressed;
        }
        static int currentView;
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {

            Expense = e.Parameter as Expense;

            switch (currentView)
            {
                case 0:
                    break;
                case 1:
                    break;
                case 2:
                    CountrySection.Visibility = Visibility.Visible;
                    AmountSection.Visibility = Visibility.Visible;
                    HubCreator.ScrollToSection(AmountSection);
                    
                    break;
                default:
                    break;
            }
        }
        private void MainControllerExpensesCreationAmountChoosed(object sender, EventArgs e)
        {
            Frame.Navigate(typeof(ExpenseDetailView), Expense);
        }

        private void MainControllerExpensesCreationCountryChoosed(object sender, EventArgs e)
        {
            AmountSection.Visibility = Visibility.Visible;
        }

        private void MainControllerExpensesCreationCategoryChoosed(object sender, EventArgs e)
        {
            CountrySection.Visibility = Visibility.Visible;
        }
        private void MainControllerExpensesCreationSuccessed(object sender, EventArgs e)
        {
            currentView = 0;
            MainController.Instance.ChangeCurrencyRequest -= Instance_ChangeCurrencyRequest;
        }
        Expense Expense;
        Products products;

        Grid CategorySectionGrid { get; set; }
        Grid NonTravelCategorySectionGrid { get; set; }
        Grid CountrySectionGrid { get; set; }
        Grid AmountSectionGrid { get; set; }
        private AListBoxUC _categoryListbox;
        private AListBoxUC _categoryNonTravelListbox;

        private void CategorySectionGrid_Loaded(object sender, RoutedEventArgs e)
        {
            CategorySectionGrid = (Grid)sender;
            _categoryListbox = new AListBoxUC (Expense, IsSplitted ? AListboxUCType.CategoryTravel : AListboxUCType.CategoryAll);
            _categoryListbox.SelectedEvent += List_SelectedEvent;
            CategorySectionGrid.Children.Add(_categoryListbox);
        }
        private void NonTravelCategorySectionGrid_OnLoaded (object sender, RoutedEventArgs e) {
            NonTravelCategorySectionGrid = (Grid)sender;
            _categoryNonTravelListbox = new AListBoxUC (Expense, AListboxUCType.CategoryNonTravel);
            _categoryNonTravelListbox.SelectedEvent += List_SelectedEvent;
            NonTravelCategorySectionGrid.Children.Add (_categoryNonTravelListbox);
        }

        private void List_SelectedEvent (object sender, EventArgs e) {
            if ((sender as AListBoxUC) == _categoryListbox)
                _categoryNonTravelListbox?.ClearList ();
            else
                _categoryListbox?.ClearList ();
        }

        private void CountrySectionGrid_Loaded(object sender, RoutedEventArgs e)
        {
            CountrySectionGrid = (Grid)sender;
            CountrySectionGrid.Children.Add(new AListBoxUC(Expense, AListboxUCType.Country));
            HubCreator.ScrollToSection(CountrySection);
            currentView = 1;
        }
        private void AmountSectionGrid_Loaded(object sender, RoutedEventArgs e)
        {
            AmountSectionGrid = (Grid)sender;
            AmountSectionGrid.Children.Add(new AAmountUC(Expense));
            HubCreator.ScrollToSection(AmountSection);
            currentView = 0;
        }
    }
}
