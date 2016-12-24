using Mxp.Core.Business;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Diagnostics;
using Windows.Foundation;
using System.Threading.Tasks;
using Windows.UI.Popups;

namespace Mxp.Win
{
    public class CategoriesFromExpenseViewModel
    {
        private List<IGrouping<string, Product>> _itemsByGroup = new List<IGrouping<string, Product>>();
        public List<IGrouping<string, Product>> ItemsByGroup { get { return _itemsByGroup; } }

        public CategoriesFromExpenseViewModel(ExpenseItem item)
        {
            InitItems(item);
        }
        protected async void InitItems(ExpenseItem item) {
            Products products;
            try {
                products = await item.FetchAvailableProductsAsync();
            } catch (Exception error) {
                MessageDialog messageDialog = new MessageDialog (error.GetExceptionMessage ());
                messageDialog.Commands.Add (new UICommand ("OK", (command) => { }));
                messageDialog.ShowAsync ();
                return;
            }

            _itemsByGroup = products.GetGroupedProducts();

            MainController.Instance.ExpenseGroupedProductsAdded();
        }
    }
}

