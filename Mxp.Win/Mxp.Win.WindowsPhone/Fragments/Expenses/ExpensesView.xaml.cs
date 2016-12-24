using Mxp.Core.Business;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;

namespace Mxp.Win
{
    public partial class ExpensesView : IDisposable
    {
        protected virtual Expenses mBusinessExpenses => LoggedUser.Instance.BusinessExpenses;
        protected virtual Expenses mPrivateExpenses => LoggedUser.Instance.PrivateExpenses;
        SpendCatcherExpenses SpendCatcherExpenses => LoggedUser.Instance.SpendCatcherExpenses;
        private bool _businessExpensesModified;
        private bool _privateExpensesModified;
        


        public ExpensesView()
        {
            this.InitializeComponent();
            this.RefreshGroups();

            MainController.Instance.refreshButtonRequest += this.MainControllerRefreshButton;
            MainController.Instance.MainPageClearRequest += this.MainControllerClearExpenses;

            mBusinessExpenses.CollectionChanged += OnCollectionChanged;
            mPrivateExpenses.CollectionChanged += OnCollectionChanged;
        }

        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            if (sender == mBusinessExpenses)
                _businessExpensesModified = true;
            else
                _privateExpensesModified = true;
        }

        private void MainControllerClearExpenses(object sender, EventArgs e)
        {
            this.ClearExpenses();
        }

        private void MainControllerRefreshButton(object sender, EventArgs e)
        {
            if (MainController.Instance.InExpenses)
            {
                this.RefreshGroups();
            }
        }
        public void ClearExpenses()
        {
            if (this.BusinessExpenseItemsList != null)
                this.businessExpensesSource.Source = null;
            if (this.PrivateExpenseItemsList != null)
                this.privateExpensesSource.Source = null;
            if (this.SpendCatcherExpenseItemsList != null)
                this.SpendCatcherSource.Source = null;
        }


        public async void RefreshGroups()
        {
            if (!Preferences.Instance.IsSpendCatcherEnable)
                this.SpendCatcherSection.Visibility = Visibility.Collapsed;
            else
                this.SpendCatcherSection.Visibility = Visibility.Visible;

            MainController.Instance.StartMainProgressRing();
            this.ProgressGrid.Visibility = Visibility.Visible;
            this.ProgressRefresh.IsActive = true;
            try
            {
                await this.mBusinessExpenses.FetchAsync();
                this.SetExpenseSource(this.mBusinessExpenses,this.businessExpensesSource);
            }
            catch (Exception error)
            {
                Debug.WriteLine(error.Message);

            }
            try {
                await this.mPrivateExpenses.FetchAsync ();
                this.SetExpenseSource (mPrivateExpenses,this.privateExpensesSource);
            }
            catch (Exception error) {
                Debug.WriteLine (error.Message);

            }
            if (Preferences.Instance.IsSpendCatcherEnable)
            {
                try
                {
                    await this.SpendCatcherExpenses.FetchAsync();
                    this.SetSpendCatcherExpenseSource();

                }
                catch (Exception error)
                {
                    Debug.WriteLine(error.Message);
                }
            }

            MainController.Instance.FinishMainProgressRing();
            this.ProgressGrid.Visibility = Visibility.Collapsed;
            this.ProgressRefresh.IsActive = false;

        }

        private void SetSpendCatcherExpenseSource()
        {
            this.SpendCatcherSource.Source = this.SpendCatcherExpenses;
            this.SpendCatcherHeader.Text = LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.SpendCatcher) + " (" + this.SpendCatcherExpenses.Count + ")";

        }

        DebugListView BusinessExpenseItemsList;
        DebugListView PrivateExpenseItemsList;
        DebugListView SpendCatcherExpenseItemsList;
        private void BusinessItemsList_Loaded(object sender, RoutedEventArgs e)
        {
            this.BusinessExpenseItemsList = sender as DebugListView;
            if (this._businessExpensesModified)
            {
                this.SetExpenseSource (this.mBusinessExpenses,this.businessExpensesSource);
                _businessExpensesModified = false;
            }
        }
        private void PrivateItemsList_Loaded (object sender, RoutedEventArgs e)
        {
            this.PrivateExpenseItemsList = sender as DebugListView;
            if (this._privateExpensesModified)
            {
                this.SetExpenseSource (this.mPrivateExpenses, this.privateExpensesSource);
                _privateExpensesModified = false;
            }
        }

        private void SetExpenseSource(Expenses expenselist, CollectionViewSource viewsource)
        {
            var result = from t in expenselist
                         group t by t.VDateHeader into g
                         select new ExpenseListGroupedElement (g) { Key = g.Key };
            viewsource.Source = result;
        }

        private void Grid_Tapped(object sender, TappedRoutedEventArgs e)
        {
            e.Handled = true;
        }

        private void BusinessExpensesHeaderLoaded(object sender, RoutedEventArgs e)
        {
            (sender as TextBlock).Text = LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.BusinessDistance);
        }
        private void PrivateExpensesHeaderLoaded (object sender, RoutedEventArgs e) {
            (sender as TextBlock).Text = LoggedUser.Instance.Labels.GetLabel (Labels.LabelEnum.Private);
        }
        TextBlock SpendCatcherHeader;
        private void SpendCatcherHeaderLoaded(object sender, RoutedEventArgs e)
        {
            this.SpendCatcherHeader = sender as TextBlock;
            this.SpendCatcherHeader.Text = LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.SpendCatcher);
        }

        private void SpendCatcherList_Loaded(object sender, RoutedEventArgs e)
        {
            this.SpendCatcherExpenseItemsList = sender as DebugListView;
            this.SetSpendCatcherExpenseSource();
        }

        private void SpendCatcherLabel_Loaded(object sender, RoutedEventArgs e)
        {
            (sender as TextBlock).Text = LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.SpendCatcherHeaderMessage);
        }

        private class ExpenseListGroupedElement : ObservableCollection<Expense>
        {
            public ExpenseListGroupedElement(IEnumerable<Expense> collection) : base(collection)
            {
            }

            public string Key { get; set; }
        }

        public void Dispose()
        {
            mBusinessExpenses.CollectionChanged += OnCollectionChanged;
            mPrivateExpenses.CollectionChanged += OnCollectionChanged;
        }
    }

    public class ItemCollection : IEnumerable<Object>
    {
        private ObservableCollection<ExpenseListElement> itemCollection = new ObservableCollection<ExpenseListElement>();
        public IEnumerator<Object> GetEnumerator()
        {
            return this.itemCollection.GetEnumerator();
        }
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
        public void Add(ExpenseListElement item)
        {
            this.itemCollection.Add(item);
        }
    }
}
