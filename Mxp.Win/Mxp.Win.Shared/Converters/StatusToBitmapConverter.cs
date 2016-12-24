using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml.Data;
using Mxp.Core.Business;
using Windows.UI.Xaml.Media.Imaging;


namespace Mxp.Win
{
    class StatusToBitmapConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, String culture)
        {
            ExpenseItem.Status status = (ExpenseItem.Status)value;
            BitmapImage bmpStatus = new BitmapImage();
            switch (status)
            {
                case ExpenseItem.Status.Accepted:
                    bmpStatus.UriSource = new Uri("ms-appx:" + "/Assets/icons/ReportHasBeenApproved.png");
                    break;
                case ExpenseItem.Status.Rejected:
                    bmpStatus.UriSource = new Uri("ms-appx:" + "/Assets/icons/ReportHasBeenRefused.png");
                    break;
                case ExpenseItem.Status.Other:
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
