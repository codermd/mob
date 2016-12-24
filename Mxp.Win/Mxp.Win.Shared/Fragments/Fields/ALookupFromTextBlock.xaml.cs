using Mxp.Core.Business;
using Mxp.Win.Helpers;
using System;
using System.Collections.Generic;
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
    public sealed partial class ALookupFromTextBlock : Page
    {
        public ALookupFromTextBlock()
        {
            this.InitializeComponent();
            HardwareButtons.BackPressed += HardwareButtons_BackPressed;
            this.TBListFilter.PlaceholderText = (LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.Search));
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
        private LookupItems LUItems { get; set; }
        public Field Field;
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is Field)
            {
                Field = e.Parameter as Field;
                Title.Text = (LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.Select));
                LUItems = ((LookupField)Field).Results;
                LookupItemsViewModel viewmodel = new LookupItemsViewModel(Field,"");
                try {
                    await viewmodel.LookupAnItem("", Field);
                } catch (Exception error) {
                    MessageDialog messageDialog = new MessageDialog (error.GetExceptionMessage ());
                    messageDialog.Commands.Add (new UICommand ("OK", (command) => { }));
                    messageDialog.ShowAsync ();
                }
                this.DataContext = viewmodel;

            }
        }
        private void ItemTapped(object sender, TappedRoutedEventArgs e)
        {
            if (Field != null)
            {
                FillField(sender as Grid);
            }
            Frame.GoBack();
        }
        private void FillField(Grid sender)
        {
            Field.Value = (sender as Grid).DataContext;
        }
        private async void TBListFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            LookupItemsViewModel viewmodel = new LookupItemsViewModel(Field, ((TextBox)sender).Text);
            try {
                await viewmodel.LookupAnItem(((TextBox)sender).Text, Field);
            } catch (Exception error) {
                MessageDialog messageDialog = new MessageDialog (error.GetExceptionMessage ());
                messageDialog.Commands.Add (new UICommand ("OK", (command) => { }));
                messageDialog.ShowAsync ();
            }
            this.DataContext = viewmodel;
        }
    }
}
