using Mxp.Core.Business;
using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Popups;
using Windows.UI.Xaml.Media.Imaging;
using System.Threading.Tasks;
using System.IO;
using System.Net.Http;
using System.Diagnostics;

namespace Mxp.Win.Helpers
{
    class ReceiptsViewModel
    {
        public List<BitmapImage> _images = new List<BitmapImage>();
        public List<String> Uris = new List<String>();
        Receipts Receipts;

        public ReceiptsViewModel(Expense expense = null, Report report = null)
        {
            if (expense != null)

                InitItems(expense);
            else
                InitItems(report);

        }
        private async void InitItems(Report report)
        {
            try
            {
                await report.Receipts.FetchAsync();
            }
            catch (Exception error)
            {
                MessageDialog messageDialog = new MessageDialog(error.GetExceptionMessage());
                messageDialog.Commands.Add(new UICommand((LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.Accept)), (command) => { }));
                messageDialog.ShowAsync();
                return;
            }
            Receipts = report.Receipts;
            foreach (Receipt r in Receipts)
                await DownLoadImage(r);
        }
        private async void InitItems(Expense expense)
        {
            try
            {
                await expense.Receipts.FetchAsync();
            }
            catch (Exception error)
            {
                MessageDialog messageDialog = new MessageDialog(error.GetExceptionMessage());
                messageDialog.Commands.Add(new UICommand((LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.Accept)), (command) => { }));
                messageDialog.ShowAsync();
                return;
            }
            Receipts = expense.Receipts;
            foreach (Receipt r in Receipts)
                Uris.Add(r.AttachmentPath);
        }
        private async Task DownLoadImage(Receipt r)
        {
            try
            {
                string imageUrl = "";
                var memStream = new MemoryStream();
                var bitmap = new BitmapImage();
                if (!String.IsNullOrWhiteSpace(r.AttachmentPath))
                {
                    imageUrl = r.AttachmentPath;
                    var client = new HttpClient();
                    Stream stream = await client.GetStreamAsync(imageUrl);
                    await stream.CopyToAsync(memStream);
                }
                else if (!String.IsNullOrWhiteSpace(r.base64))
                {
                    imageUrl = r.base64;
                    Byte[] bitmapData = Convert.FromBase64String(FixBase64ForImage(imageUrl));
                    System.IO.MemoryStream streamBitmap = new MemoryStream(bitmapData);
                    await streamBitmap.CopyToAsync(memStream);
                }
                memStream.Position = 0;
                bitmap.SetSource(memStream.AsRandomAccessStream());
                bitmap.DecodePixelWidth = 90;

                _images.Add(bitmap);
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
        public string FixBase64ForImage(string Image)
        {
            System.Text.StringBuilder sbText = new System.Text.StringBuilder(Image, Image.Length);
            sbText.Replace("\r\n", String.Empty); sbText.Replace(" ", String.Empty);
            return sbText.ToString();
        }
    }
}
