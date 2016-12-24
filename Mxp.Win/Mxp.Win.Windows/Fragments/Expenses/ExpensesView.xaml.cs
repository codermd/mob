using Mxp.Core.Business;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Mxp.Win
{
    public partial class ExpensesView : UserControl
    {
        protected virtual Expenses mExpenses
        {
            get
            {
                return LoggedUser.Instance.Expenses;
            }
        }
        private ItemCollection _Collection = new ItemCollection();
        public ItemCollection Collection
        {
            get
            {
                return this._Collection;
            }
        }
        public ExpensesView()
        {
            this.InitializeComponent();
            MainController.Instance.refreshButtonRequest += MainControllerRefreshButton;
            AddGroups();
        }
        private void MainControllerRefreshButton(object sender, EventArgs e)
        {      
            if (MainController.Instance.InExpenses)
            {
                AddGroups();
            }
        }
        public async void AddGroups()
        {
            await this.mExpenses.FetchAsync();     
            var result = from t in mExpenses.ToList<Expense>()
                         group t by t.VDateHeader into g
                         select new { Key = g.Key, Items = g };
            Type type = result.GetType();
            groupInfoCVS.Source = result;
        } 
    }
    public class ItemCollection : IEnumerable<Object>
    {
        private System.Collections.ObjectModel.ObservableCollection<ExpenseListElement> itemCollection = new System.Collections.ObjectModel.ObservableCollection<ExpenseListElement>();
        public IEnumerator<Object> GetEnumerator()
        {
            return itemCollection.GetEnumerator();
        }
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        public void Add(ExpenseListElement item)
        {
            itemCollection.Add(item);
        }
    }
}
