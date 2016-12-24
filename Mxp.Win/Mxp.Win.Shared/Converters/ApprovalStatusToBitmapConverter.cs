using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml.Data;
using Mxp.Core.Business;
using Windows.UI.Xaml.Media.Imaging;


namespace Mxp.Win
{
    class ApprovalStatusToBitmapConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, String culture)
        {
            Report.ApprovalStatusEnum status = (Report.ApprovalStatusEnum)value;
            BitmapImage bmpStatus = new BitmapImage();
            switch (status)
            {
                case Report.ApprovalStatusEnum.Accepted:
                    bmpStatus.UriSource = new Uri("ms-appx:" + "/Assets/icons/ReportHasBeenApproved.png");
                    break;
                case Report.ApprovalStatusEnum.Rejected:
                    bmpStatus.UriSource = new Uri("ms-appx:" + "/Assets/icons/ReportHasBeenRefused.png");
                    break;
                case Report.ApprovalStatusEnum.Waiting:
                    bmpStatus.UriSource = new Uri("ms-appx:" + "/Assets/icons/ReportApprovalIsPending.png");
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
