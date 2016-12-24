using System;
using Windows.Phone.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Mxp.Core.Business;

namespace Mxp.Win.Fragments.Fields
{
    public sealed partial class ASplitedList
    {
        Grid CategorySectionGrid { get; set; }
        Grid NonTravelCategorySectionGrid { get; set; }

        private AListBoxUC _categoryListbox;
        private AListBoxUC _categoryNonTravelListbox;

        public ASplitedList()
        {
            this.InitializeComponent ();
            HardwareButtons.BackPressed += this.HardwareButtons_BackPressed;
        }

        void HardwareButtons_BackPressed (object sender, BackPressedEventArgs e) {
            e.Handled = true;
            this.Frame.GoBack ();
        }

        protected override void OnNavigatedFrom (NavigationEventArgs e) {
            HardwareButtons.BackPressed -= this.HardwareButtons_BackPressed;
            switch (this.Field.Type) {
                case FieldTypeEnum.Category:
                    MainController.Instance.ExpensesCreationCategoryChoosedRequest -= MainControllerExpensesCreationCategoryChoosed;
                    break;
            }
        }
        protected override void OnNavigatedTo (NavigationEventArgs e) {
            if (e.Parameter is Field) {
                this.Field = e.Parameter as Field;
                switch (this.Field.Type) {
                    case FieldTypeEnum.Category:
                        MainController.Instance.ExpensesCreationCategoryChoosedRequest += MainControllerExpensesCreationCategoryChoosed;
                        this.Title.Text = LoggedUser.Instance.Labels.GetLabel (Labels.LabelEnum.Categories);
                        this.CategoriesHeader.Text = LoggedUser.Instance.Labels.GetLabel (Labels.LabelEnum.TravelFilter);
                        this.CategoriesHeaderNonTravel.Text = LoggedUser.Instance.Labels.GetLabel (Labels.LabelEnum.NonTravelFilter);
                        Expense = (Field.Model as ExpenseItem)?.ParentExpense;
                        break;
                }
            }
        }

        private void MainControllerExpensesCreationCategoryChoosed (object sender, EventArgs e) {
            this.Frame.GoBack ();
        }

        private void CategorySectionGrid_Loaded (object sender, RoutedEventArgs e) {
            CategorySectionGrid = (Grid)sender;
            _categoryListbox = new AListBoxUC (Expense, AListboxUCType.CategoryTravel);
            CategorySectionGrid.Children.Add (_categoryListbox);
        }

        private void NonTravelCategorySectionGrid_OnLoaded (object sender, RoutedEventArgs e) {
            NonTravelCategorySectionGrid = (Grid)sender;
            _categoryNonTravelListbox = new AListBoxUC (Expense, AListboxUCType.CategoryNonTravel);
            NonTravelCategorySectionGrid.Children.Add (_categoryNonTravelListbox);
        }

        public Field Field { get; set; }
        private Expense Expense { get; set; }
        
    }
}
