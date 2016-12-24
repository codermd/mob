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
using System.Collections.ObjectModel;
using Windows.Phone.UI.Input;
using Windows.UI.Popups;

namespace Mxp.Win
{
    public sealed partial class ALookUpPageExpense : Page
    {
        public ALookUpPageExpense()
        {
            InitializeComponent();
            this.SuggestionFrom = new ObservableCollection<LookupItem>();
            HardwareButtons.BackPressed += HardwareButtons_BackPressed;
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

        private Field Field { get; set; }
        private LookupItems LUItems { get; set; }
        public ObservableCollection<LookupItem> SuggestionFrom { get; set; }
        private void SearchTB_GotFocus(object sender, RoutedEventArgs e)
        {
            if (SearchTB.Text == "Search")
            {
                SearchTB.Text = "";
                SolidColorBrush Brush1 = new SolidColorBrush();
                Brush1.Color = Windows.UI.Colors.Blue;
                SearchTB.Foreground = Brush1;
            }
        }
        private void SearchTB_LostFocus(object sender, RoutedEventArgs e)
        {
            if (SearchTB.Text == String.Empty)
            {
                SearchTB.Text = "Search";
                SolidColorBrush Brush2 = new SolidColorBrush();
                Brush2.Color = Windows.UI.Colors.Gray;
                SearchTB.Foreground = Brush2;
            }
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Field = e.Parameter as Field;
            Title.Text = (LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.Select));

        }
        private async void SearchTB_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (SearchTB.Text != Labels.GetLoggedUserLabel(Labels.LabelEnum.Search))
            {
                try
                {
                    //TODO
                    await ((LookupField)Field).FetchItems(sender.Text);
                }
                catch (Exception error)
                {
                    MessageDialog messageDialog = new MessageDialog(error.GetExceptionMessage());
                    messageDialog.Commands.Add(new UICommand("OK", (command) => { }));
                    messageDialog.ShowAsync();
                    return;
                }


                LUItems = ((LookupField)Field).Results;
                LookupResultItem packagesResult = new LookupResultItem();
                foreach (LookupItem item in LUItems)
                {
                    packagesResult.items.Add(new Mxp.Win.LookupResultItem.Item(item));
                }
                sender.ItemsSource = packagesResult.items;
            }
        }
        private void SearchTB_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            LookupItem item = ((LookupResultItem.Item)args.SelectedItem).LUItem;
            Field.Value = item;
            Frame.GoBack();
        }
    }

}

