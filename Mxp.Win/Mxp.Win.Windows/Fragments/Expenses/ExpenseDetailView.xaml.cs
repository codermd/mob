using Mxp.Core.Business;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Windows.UI;

namespace Mxp.Win
{
    public sealed partial class ExpenseDetailView : Page
    {
        public ExpenseDetailView()
        {
            this.InitializeComponent();
        }
        private void Section1GotFocus(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("Section1");
        }
        private void HubExpense_SectionsInViewChanged(object sender, SectionsInViewChangedEventArgs e)
        {
            var tag = HubExpense.SectionsInView[0].Tag.ToString();
            switch (tag)
            {
                case "0":
                    this.AddButton.Visibility = Visibility.Collapsed;
                    break;
                case "1":
                    this.AddButton.Visibility = Visibility.Visible;

                    break;
                case "2":
                    this.AddButton.Visibility = Visibility.Visible;
                    break;
            }
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var param = e.Parameter as Expense;
            ExpenseItem item;
            if (param != null)
            {
                if (param.GetType() == typeof(Expense))
                {
                    item = param.ExpenseItems[0];
                    CollectionFields = item.DetailsFields;               
                }
            }
            else
            {
                item = e.Parameter as ExpenseItem;
                CollectionFields = item.DetailsFields;                
            }
        }
       
        #if WINDOWS_PHONE_APP
        public ListView FieldsListView { get; set; }
#else
        public Grid FieldsListView { get; set; }
#endif
        public Collection<TableSectionModel> CollectionFields { get; set; }
        private void FieldListLoaded(object sender, RoutedEventArgs e)
        {
#if WINDOWS_PHONE_APP
           FieldsListView = (ListView)sender;
#else
            FieldsListView = (Grid)sender;
#endif
        }
    }
}
