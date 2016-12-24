using Mxp.Core.Business;
using System;
using System.Collections.Generic;
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
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;


namespace Mxp.Win
{
    public sealed partial class PhotoSliderPage : Page
    {
        public PhotoSliderPage()
        {
            this.InitializeComponent();
            HardwareButtons.BackPressed += HardwareButtons_BackPressed;
            Title.Text = (LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.Receipts));
            MainController.Instance.ExpenseReceiptRemovedRequest += MainControllerExpenseReceiptRemovedRequest;
            DeleteButton.Label = (LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.Delete));
        }

        private void MainControllerExpenseReceiptRemovedRequest(object sender, EventArgs e)
        {
            ExpenseDetailView.currentSection = 1;
            Frame.GoBack();
            ExpenseDetailView.currentSection = 0;

        }
        void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            e.Handled = true;
            Frame.GoBack();
        }
        protected async override void OnNavigatedFrom(NavigationEventArgs e)
        {
            HardwareButtons.BackPressed -= HardwareButtons_BackPressed;
            MainController.Instance.ExpenseReceiptRemovedRequest -= MainControllerExpenseReceiptRemovedRequest;
        }
        Receipts Receipts;
        ReceiptsImages RImages;
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Receipts = e.Parameter as Receipts;
            //RImages = e.Parameter as ReceiptsImages;
            foreach (Receipt source in Receipts)
            {
                if (!String.IsNullOrWhiteSpace(source.AttachmentPath))
                    listview.Items.Add(new UriItem { ImageUrl = source.AttachmentPath });
                else
                    listview.Items.Add(new UriItem { ImageUrl = source.base64 });
            }
        }
        private void listview_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(Receipts.CanManage)
                this.DeleteButton.Visibility = Visibility.Visible;
        }
        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {

            MessageDialog messageDialog = new MessageDialog(LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.DoYouConfirm), LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.Delete) + " " + LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.Receipt));
            messageDialog.Commands.Add(new UICommand((LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.Yes)), async (command) => { await DelteReceipt(); DeleteButton.Visibility = Visibility.Collapsed; }));
            messageDialog.Commands.Add(new UICommand((LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.No)), (command) => { }));
            await messageDialog.ShowAsync();
        }
        private async System.Threading.Tasks.Task DelteReceipt()
        {
            DeleteButton.Visibility = Visibility.Collapsed;
            try
            {
                int i = listview.SelectedIndex;
                Receipt r = Receipts[i];
                if (Receipts.CanManage)
                {
                    await Receipts.DeleteReceipt(r);
                    listview.Items.RemoveAt(i);
                }
                 
            }
            catch (ValidationError error)
            {
                MessageDialog messageDialog = new MessageDialog(error.Verbose);
                messageDialog.Commands.Add(new UICommand((LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.Accept)), (command) => { }));
                messageDialog.ShowAsync();
                return;
            }
            catch (Exception error)
            {
                Debug.WriteLine(error.Message);
                return;
            }
        }
    }
}
