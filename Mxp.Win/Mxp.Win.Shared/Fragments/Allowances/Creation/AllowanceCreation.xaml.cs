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
using Windows.Phone.UI.Input;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace Mxp.Win
{
    public sealed partial class AllowanceCreation : Page
    {
        public AllowanceCreation()
        {
            this.InitializeComponent();
            HardwareButtons.BackPressed += HardwareButtons_BackPressed;
            Title.Text = (LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.CreateAllowance));
        }
        void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            e.Handled = true;
            Frame.GoBack();
        }
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            HardwareButtons.BackPressed -= HardwareButtons_BackPressed;
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {

            this.ProgressRing.IsActive = true;
            this.BottomAppBar.IsEnabled = false;
            Allowance = (Allowance)e.Parameter;
            CollectionFields = this.Allowance.CreationAllowanceSectionFields;
            FillList();

            this.ProgressRing.IsActive = false;
            this.BottomAppBar.IsEnabled = true;
        }

        private void FillList()
        {
            List<FieldGroup> items = new List<FieldGroup>();
            if (CollectionFields != null)
            {
                foreach (TableSectionModel col in CollectionFields)
                {
                    List<Field> fields = col.Fields.ToList<Field>();
                    foreach (Field field in fields)
                    {
                        FieldGroup item = new FieldGroup(field, col.Title);
                        items.Add(item);
                    }
                }
            }
            var result = from t in items
                         group t by t.Group into g
                         select new { Key = g.Key, ItemsField = g };
            DetailFieldsSource.Source = result;
        }

        ExpenseItem ExpenseItem { get; set; }
        Allowance Allowance { get; set; }
        public Collection<TableSectionModel> CollectionFields { get; set; }

        public List<DetailField> SourceFields { get; set; }

        private async void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                this.ProgressRing.IsActive = true;
                this.BottomAppBar.IsEnabled = false;
                await Allowance.CreateAsync();
            }
            catch (Exception error)
            {
                MessageDialog messageDialog = new MessageDialog(error.GetExceptionMessage());
                messageDialog.Commands.Add(new UICommand("OK", (command) => { }));
                messageDialog.ShowAsync();

                this.ProgressRing.IsActive = false;
                this.BottomAppBar.IsEnabled = true;
                return;
            }
            
            Frame.GoBack();
            Frame.Navigate(typeof (AllowanceDetailView),Allowance);
        }

        private void Discard_Click(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }
    }
}
