using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml.Data;
using Mxp.Core.Business;
using Windows.UI.Xaml.Media.Imaging;


namespace Mxp.Win
{
    class ReceiptStatusToBitmapConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, String culture)
        {
            Report.ReceiptStatusEnum status = (Report.ReceiptStatusEnum)value;
            BitmapImage bmpStatus = new BitmapImage();
            switch (status)
            {
                case Report.ReceiptStatusEnum.Green:
                    bmpStatus.UriSource = new Uri("ms-appx:" + "/Assets/icons/ReportAcceptedByController.png");
                    break;
                case Report.ReceiptStatusEnum.Orange:
                    bmpStatus.UriSource = new Uri("ms-appx:" + "/Assets/icons/ReportHasBeenOrange.png");
                    break;
                case Report.ReceiptStatusEnum.Red:
                    bmpStatus.UriSource = new Uri("ms-appx:" + "/Assets/icons/ReportRejectedByController.png");
                    break;
                case Report.ReceiptStatusEnum.Black:
                    bmpStatus.UriSource = new Uri("ms-appx:" + "/Assets/icons/ReportIsPending.png");
                    break;
            }
            return bmpStatus;            
        }
        public object ConvertBack(object value, Type targetType, object parameter, String culture)
        {
            throw new NotImplementedException();
        }
    }
}
