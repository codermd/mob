using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace Mxp.Win
{
    public class SafeImage : UserControl
    {
        private SynchronizationContext uiThread;

        public static readonly DependencyProperty SafePathProperty =
            DependencyProperty.Register("SafePath", typeof(string), typeof(SafeImage),
            new PropertyMetadata(default(string), OnSourceWithCustomRefererChanged));

        public string SafePath
        {
            get
            {
                try { return (string)GetValue(SafePathProperty); } catch (Exception e) { Debug.WriteLine(e.Message); return null; }
            }
            set { SetValue(SafePathProperty, value); }
        }


        private static void OnSourceWithCustomRefererChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            SafeImage safeImage = o as SafeImage;
            safeImage.OnLoaded(null, null);
            //OnLoaded(null, null);
            if (e.NewValue == null)
                return;
        }
        public SafeImage()
        {
            Content = new Image();
            uiThread = SynchronizationContext.Current;

            Loaded += OnLoaded;
            Unloaded += OnUnloaded;
        }

        private async void OnLoaded(object _sender, RoutedEventArgs _routedEventArgs)
        {
            if (_sender != null)
            {
                var image = Content as Image;
                if (image == null)
                    return;
                var path = SafePath; //(string)GetValue(SafePathProperty);
                                     //image.Source = new BitmapImage(new Uri(SafePath));
                Debug.WriteLine(path);
                var bitmapImage = image.Source as BitmapImage;
                if (bitmapImage != null)
                    bitmapImage.UriSource = null;
                image.Source = null;
                if (String.IsNullOrEmpty(path) && _sender != null)
                {
                    //new base64 image 
                    if ((_sender as SafeImage).DataContext is BitmapImage)
                    {
                        if ((_sender as SafeImage).DataContext != null)
                            bitmapImage = (_sender as SafeImage).DataContext as BitmapImage;
                        if (bitmapImage != null)
                            image.Source = bitmapImage;
                    }
                    else if ((_sender as SafeImage).DataContext is UriItem)
                    {
                        UriItem uri = (_sender as SafeImage).DataContext as UriItem;
                        if (!String.IsNullOrWhiteSpace(uri.ImageUrl))
                        {
                            var memStream = new MemoryStream();
                            var bitmap = new BitmapImage();

                            Byte[] bitmapData = Convert.FromBase64String(FixBase64ForImage(path));
                            System.IO.MemoryStream streamBitmap = new MemoryStream(bitmapData);
                            await streamBitmap.CopyToAsync(memStream);
                            memStream.Position = 0;
                            bitmap.SetSource(memStream.AsRandomAccessStream());
                            image.Source = bitmap;
                        }
                    }
                    return;
                }
                // If local image, just load it (non-local images paths starts with "http")
                if (path.StartsWith("/") && _sender != null)
                {
                    var memStream = new MemoryStream();
                    var bitmap = new BitmapImage();

                    Byte[] bitmapData = Convert.FromBase64String(FixBase64ForImage(path));
                    System.IO.MemoryStream streamBitmap = new MemoryStream(bitmapData);
                    await streamBitmap.CopyToAsync(memStream);
                    memStream.Position = 0;
                    bitmap.SetSource(memStream.AsRandomAccessStream());
                    image.Source = bitmap;

                    //var memStream = new MemoryStream();
                    //Byte[] bitmapData = Convert.FromBase64String(FixBase64ForImage(path));
                    //System.IO.MemoryStream streamBitmap = new MemoryStream(bitmapData);
                    //await streamBitmap.CopyToAsync(memStream);

                    //memStream.Position = 0;
                    //bitmapImage.SetSource(memStream.AsRandomAccessStream());

                    //image.Source = bitmapImage;
                    return;
                }
                if (path.Contains("ms-appx:/"))
                {
                    image.Source = new BitmapImage { UriSource = new Uri(path, UriKind.Absolute) };
                }
                else
                {
                    var request = WebRequest.Create(path) as HttpWebRequest;
                    request.AllowReadStreamBuffering = true;
                    request.BeginGetResponse(result =>
                    {
                        try
                        {
                            Stream imageStream = request.EndGetResponse(result).GetResponseStream();
                            uiThread.Post(_ =>
                            {

                                if (path != this.SafePath)
                                {
                                    return;
                                }
                                if (imageStream == null)
                                {
                                    image.Source = new BitmapImage { UriSource = new Uri(path, UriKind.Relative) };
                                    return;
                                }

                                bitmapImage = new BitmapImage();
                                bitmapImage.SetSourceAsync(imageStream.AsRandomAccessStream());
                                image.Source = bitmapImage;
                            }, null);
                        }
                        catch (WebException e)
                        {
                            Debug.WriteLine(e.Message);
                        }
                    }, null);
                }
            }
        }
        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            var image = Content as Image;
            if (image == null)
                return;
            var bitmapImage = image.Source as BitmapImage;
            if (bitmapImage != null)
                bitmapImage.UriSource = null;
            image.Source = null;
        }
        public string FixBase64ForImage(string Image)
        {
            System.Text.StringBuilder sbText = new System.Text.StringBuilder(Image, Image.Length);
            sbText.Replace("\r\n", String.Empty); sbText.Replace(" ", String.Empty);
            return sbText.ToString();
        }
    }
}
