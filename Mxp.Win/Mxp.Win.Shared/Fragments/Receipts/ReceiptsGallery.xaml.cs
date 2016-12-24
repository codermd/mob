using Mxp.Core.Business;
using Mxp.Win.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Windows.System;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Imaging;

namespace Mxp.Win
{
    public sealed partial class ReceiptsGallery : UserControl
    {
        ReceiptsViewModel ReceiptsVM;

        public ReceiptsGallery()
        {
            this.InitializeComponent();
            MainController.Instance.ReceiptsLoadingRequest += this.MainControllerReceiptsLoading;
            MainController.Instance.ReceiptsLoadedRequest += this.MainControllerReceiptsLoaded;
        }

        private void MainControllerReceiptsLoaded(object sender, EventArgs e)
        {
            this.ProgressRing.IsActive = false;
        }
        private void MainControllerReceiptsLoading(object sender, EventArgs e)
        {
            this.ProgressRing.IsActive = true;
        }
        public Expense Expense { get; set; }
        public Report Report { get; set; }
        public Receipts Receipts { get; set; }
        public Boolean Showed { get; set; }
        List<BitmapImage> ListImagesDl;
        public async void LoadPhotos(Expense exp)
        {
            this.MainControllerReceiptsLoading(null, null);
            this.ListImagesDl = new List<BitmapImage>();
            if (!this.Showed && exp != null)
            {
                this.Expense = exp;
                try
                {
                    await this.Expense.Receipts.FetchAsync();
                    this.Showed = true;
                }
                catch (Exception error)
                {
                    MessageDialog messageDialog = new MessageDialog(error.GetExceptionMessage());
                    messageDialog.Commands.Add(new UICommand((LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.Accept)), (command) => { }));
                    messageDialog.ShowAsync();
                    return;
                }
                this.Receipts = this.Expense.Receipts;
                this.ImageGalleryGrid.Items.Clear();
                foreach (Receipt r in this.Receipts)
                {
                    if (!String.IsNullOrWhiteSpace(r.AttachmentPath))
                    {
                        if (r.AttachmentPath.Contains(".pdf"))
                            this.ImageGalleryGrid.Items.Add(new UriItem { ImageUrl = "ms-appx:" + "/Assets/icons/ReportIsPending.png", UrlToLaunch = r.AttachmentPath, IsPDF = true });
                        else
                            this.ImageGalleryGrid.Items.Add(new UriItem { ImageUrl = r.AttachmentPath, UrlToLaunch = r.AttachmentPath, IsPDF = false });
                    }

                    else
                        this.ImageGalleryGrid.Items.Add(new UriItem { ImageUrl = r.base64 });
                }
            }
            this.MainControllerReceiptsLoaded(null, null);
            this.ImageGalleryGrid.IsEnabled = true;
        }
        public async void LoadPhotos(Report rep)
        {
            this.MainControllerReceiptsLoading(null, null);
            this.ListImagesDl = new List<BitmapImage>();
            if (!this.Showed && rep != null)
            {
                this.Report = rep;
                try
                {
                    await this.Report.Receipts.FetchAsync();
                    this.Showed = true;
                }
                catch (Exception error)
                {
                    MessageDialog messageDialog = new MessageDialog(error.GetExceptionMessage());
                    messageDialog.Commands.Add(new UICommand((LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.Accept)), (command) => { }));
                    messageDialog.ShowAsync();
                    return;
                }
                this.Receipts = this.Report.Receipts;
                this.ImageGalleryGrid.Items.Clear();
                foreach (Receipt r in this.Receipts)
                {
                    if (!String.IsNullOrWhiteSpace(r.AttachmentPath))
                    {
                        if (r.AttachmentPath.Contains(".pdf"))
                            this.ImageGalleryGrid.Items.Add(new UriItem { ImageUrl = "ms-appx:" + "/Assets/icons/ReportIsPending.png", UrlToLaunch = r.AttachmentPath, IsPDF = true });
                        else
                            this.ImageGalleryGrid.Items.Add(new UriItem { ImageUrl = r.AttachmentPath, IsPDF = false });

                    }
                    else
                        this.ImageGalleryGrid.Items.Add(new UriItem { ImageUrl = r.base64 });
                }
            }
            this.MainControllerReceiptsLoaded(null, null);
            this.ImageGalleryGrid.IsEnabled = true;
        }
        public string FixBase64ForImage(string Image)
        {
            System.Text.StringBuilder sbText = new System.Text.StringBuilder(Image, Image.Length);
            sbText.Replace("\r\n", String.Empty); sbText.Replace(" ", String.Empty);
            return sbText.ToString();
        }
        private async Task DownLoadImage(Receipt r)
        {
            try
            {
                string imageUrl;
                var memStream = new MemoryStream();
                var bitmap = new BitmapImage();
                if (!String.IsNullOrWhiteSpace(r.AttachmentPath))
                {
                    imageUrl = r.AttachmentPath;
                    var client = new HttpClient();
                    Stream stream = await client.GetStreamAsync(imageUrl);
                    await stream.CopyToAsync(memStream);

                    r.base64 = Convert.ToBase64String (memStream.ToArray ());
                }
                else if (!String.IsNullOrWhiteSpace(r.base64))
                {
                    imageUrl = r.base64;
                    Byte[] bitmapData = Convert.FromBase64String(this.FixBase64ForImage(imageUrl));
                    MemoryStream streamBitmap = new MemoryStream(bitmapData);
                    await streamBitmap.CopyToAsync(memStream);
                }
                memStream.Position = 0;
                bitmap.SetSource(memStream.AsRandomAccessStream());

                this.ImageGalleryGrid.Items.Add (new UriItem { ImageUrl = r.base64 });
                this.ListImagesDl.Add (bitmap);
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
                MessageDialog messageDialog = new MessageDialog(error.Message);
                messageDialog.Commands.Add(new UICommand((LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.Accept)), (command) => { }));
                messageDialog.ShowAsync();
                return;
            }
        }
        private void Img_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (!this.ProgressRing.IsActive)
            {
                ReceiptsImages r = new ReceiptsImages(this.Receipts, this.ListImagesDl);
                ((Frame)Window.Current.Content).Navigate(typeof(PhotoSliderPage), r);
            }
        }


        public async void LoadLastPhoto(Expense expense)
        {
            try
            {
                //await Expense.Receipts.FetchAsync();
                await this.DownLoadImage(this.Receipts[this.Receipts.Count - 1]);
                this.Showed = true;
                this.MainControllerReceiptsLoaded(null, null);
                this.ImageGalleryGrid.IsEnabled = true;
            }
            catch (ValidationError error)
            {
                MessageDialog messageDialog = new MessageDialog(error.Verbose);
                messageDialog.Commands.Add(new UICommand((LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.Accept)), (command) => { }));
                messageDialog.ShowAsync();
                return;
            }
            catch (Exception ex)
            {
                MessageDialog messageDialog = new MessageDialog(ex.Message);
                messageDialog.Commands.Add(new UICommand((LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.Accept)), (command) => { }));
                messageDialog.ShowAsync();
                return;
            }
        }
        public async void LoadLastPhoto(Report report)
        {
            try
            {
                await this.DownLoadImage(this.Receipts[this.Receipts.Count - 1]);
                this.Showed = true;
                this.MainControllerReceiptsLoaded(null, null);
                this.ImageGalleryGrid.IsEnabled = true;
            }
            catch (ValidationError error)
            {
                MessageDialog messageDialog = new MessageDialog(error.Verbose);
                messageDialog.Commands.Add(new UICommand((LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.Accept)), (command) => { }));
                messageDialog.ShowAsync();
            }
            catch (Exception ex)
            {
                MessageDialog messageDialog = new MessageDialog(ex.Message);
                messageDialog.Commands.Add(new UICommand((LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.Accept)), (command) => { }));
                messageDialog.ShowAsync();
            }
        }

        private async void SafeImage_Tapped(object sender, TappedRoutedEventArgs e)
        {
            UriItem item = (sender as SafeImage).DataContext as UriItem;
            if (!item.IsPDF)
                ((Frame)Window.Current.Content).Navigate(typeof(PhotoSliderPage), this.Receipts);
            else
            {
                await Launcher.LaunchUriAsync(new Uri(item.UrlToLaunch));
            }
        }

        private object UriContext(Frame content)
        {
            throw new NotImplementedException();
        }
    }
    public class ReceiptsImages
    {
        public ReceiptsImages(Receipts r, List<BitmapImage> images)
        {
            this.Receipts = r;
            this.Images = images;
        }
        public Receipts Receipts { get; set; }
        public List<BitmapImage> Images { get; set; }
    }
}