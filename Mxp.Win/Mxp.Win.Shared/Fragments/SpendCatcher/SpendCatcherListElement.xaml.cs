using Mxp.Core;
using Mxp.Core.Business;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
    public sealed partial class SpendCatcherListElement : UserControl
    {
        public SpendCatcherListElement()
        {
            this.InitializeComponent();
        }
        public BitmapImage BitmapImage { get; set; }

        private async Task DownLoadImage(string imageUrl)
        {
            try
            {
                var memStream = new MemoryStream();
                BitmapImage = new BitmapImage();
                if (!String.IsNullOrWhiteSpace(imageUrl))
                {
                    var client = new HttpClient();
                    Stream stream = await client.GetStreamAsync(imageUrl);
                    await stream.CopyToAsync(memStream);
                }
                memStream.Position = 0;
                BitmapImage.SetSource(memStream.AsRandomAccessStream());
                ReceiptImage.Source = BitmapImage;
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

        private void Grid_Tapped(object sender, TappedRoutedEventArgs e)
        {
            (Window.Current.Content as Frame).Navigate(typeof(SpendCatcherReceiptPage), BitmapImage);
        }
        public SpendCatcherExpense SpendCatcherExpense;
        private async void ReceiptImage_Loaded(object sender, RoutedEventArgs e)
        {
            this.SpendCatcherExpense = this.DataContext as SpendCatcherExpense;
            try
            {
                await DownLoadImage(SpendCatcherExpense.AttachmentPath);
            }catch(ValidationError error)
            {
                MessageDialog messageDialog = new MessageDialog(error.Verbose);
                messageDialog.Commands.Add(new UICommand((LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.Accept)), (command) => { }));
                messageDialog.ShowAsync();
                return;
            }
            catch(Exception error)
            {
                Debug.WriteLine(error.Message);

                return;
            }
        }

        private void Title_Loaded(object sender, RoutedEventArgs e)
        {
            if (SpendCatcherExpense.Product != null)
                Title.Text = SpendCatcherExpense.Product.ExpenseCategory.Name;
            else
                Title.Text = LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.No) + " " + LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.Category);
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            //this.SpendCatcherExpense = this.DataContext as SpendCatcherExpense;
        }
    }
}
