using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;

namespace Mxp.Win
{
    public static class BitmapHelper
    {

        public static void ScaleImage(ref uint width, ref uint height)
        {
            var ratioX = (double)1200 / width;
            var ratioY = (double)1200 / height;
            var ratio = Math.Min(ratioX, ratioY);
            width = (uint)(width * ratio);
            height = (uint)(height * ratio);
        }

        public async static Task<String> CompressFile(StorageFile storageFile)
        {
            string Base64String = "";
            using (var imageStream = await storageFile.OpenAsync(Windows.Storage.FileAccessMode.Read))
            {
                //create decoder and encoder
                BitmapDecoder dec = await BitmapDecoder.CreateAsync(imageStream);
                BitmapTransform transform = new BitmapTransform();
                BitmapPixelFormat pixelFormat = dec.BitmapPixelFormat;
                BitmapAlphaMode alpha = dec.BitmapAlphaMode;
                //read the PixelData
                PixelDataProvider pixelProvider = await dec.GetPixelDataAsync(
                    pixelFormat,
                    alpha,
                    transform,
                    ExifOrientationMode.RespectExifOrientation,
                    ColorManagementMode.ColorManageToSRgb
                    );
                byte[] pixels = pixelProvider.DetachPixelData();

                BitmapImage bi = new BitmapImage();

                uint widthImage = dec.OrientedPixelWidth;
                uint heightImage = dec.OrientedPixelHeight;

                using (IRandomAccessStream stream = new InMemoryRandomAccessStream())
                {
                    var propertySet = new Windows.Graphics.Imaging.BitmapPropertySet();
                    var qualityValue = new Windows.Graphics.Imaging.BitmapTypedValue(
                        0.6, // Maximum quality
                        Windows.Foundation.PropertyType.Single
                        );
                    propertySet.Add("ImageQuality", qualityValue);

                    //write changes to the BitmapEncoder
                    BitmapEncoder enc = await BitmapEncoder.CreateAsync(BitmapEncoder.JpegEncoderId, stream, propertySet);
                    enc.SetPixelData(
                        pixelFormat,
                        alpha,
                        widthImage,
                        heightImage,
                        dec.DpiX,
                        dec.DpiY,
                        pixels
                        );
                    BitmapHelper.ScaleImage(ref widthImage, ref heightImage);

                    enc.BitmapTransform.ScaledHeight = heightImage;
                    enc.BitmapTransform.ScaledWidth = widthImage;

                    BitmapBounds bounds = new BitmapBounds();
                    bounds.Height = heightImage;
                    bounds.Width = widthImage;
                    bounds.X = 0;
                    bounds.Y = 0;
                    enc.BitmapTransform.Bounds = bounds;

                    await enc.FlushAsync();
                    bi.SetSource(stream);

                    var reader = new DataReader(stream.GetInputStreamAt(0));
                    await reader.LoadAsync((uint)stream.Size);
                    byte[] byteArray = new byte[stream.Size];
                    reader.ReadBytes(byteArray);
                    Base64String += Convert.ToBase64String(byteArray);
                }
            }
            return Base64String;
        }
    }
}



