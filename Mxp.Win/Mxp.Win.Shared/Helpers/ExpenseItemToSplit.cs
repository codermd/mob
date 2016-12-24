using Mxp.Core.Business;
using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml.Controls;

namespace Mxp.Win
{
    public class ExpenseItemToSplit
    {
        public ExpenseItemToSplit(ExpenseItem item, int? splitIndex, CategoriesFromExpenseViewModel availableProducts,ExpenseItem innerExpenseItem = null)
        {
            this.ExpenseItem = item;
            this.SplitIndex = splitIndex;
            this.AvailableProducts = availableProducts;
            this.InnerExpenseItem = innerExpenseItem;

        }
        public ExpenseItem ExpenseItem{ get; set; }
        public int? SplitIndex{ get; set; }       
        public CategoriesFromExpenseViewModel AvailableProducts{ get; set; }
        public ExpenseItem InnerExpenseItem{ get; set; }
    }
}

