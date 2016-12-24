using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml.Data;
using Mxp.Core.Business;
using Windows.UI.Xaml.Media.Imaging;
using Mxp.Core.Helpers;


namespace Mxp.Win
{
    class NameToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, String culture)
        {

            IconLegend.IconsEnum icon = (IconLegend.IconsEnum)value;
      
            BitmapImage bmpStatus = new BitmapImage();
            switch (icon)
            {
                case IconLegend.IconsEnum.Accepted:
                    bmpStatus.UriSource = new Uri("ms-appx:" + "/Assets/icons/ReportAcceptedByController.png");
                    break;
                case IconLegend.IconsEnum.Approved:
                    bmpStatus.UriSource = new Uri("ms-appx:" + "/Assets/icons/ReportHasBeenApproved.png");
                    break;
                case IconLegend.IconsEnum.Compliant:
                    bmpStatus.UriSource = new Uri("ms-appx:" + "/Assets/icons/ExpenseIsCompliant.png");
                    break;
                case IconLegend.IconsEnum.NotCompliant:
                    bmpStatus.UriSource = new Uri("ms-appx:" + "/Assets/icons/ExpenseNotCompliant.png");
                    break;
                case IconLegend.IconsEnum.NotCompliantPolicy:
                    bmpStatus.UriSource = new Uri("ms-appx:" + "/Assets/icons/ExpenseNotCompliantPolicy.png");
                    break;
                case IconLegend.IconsEnum.Pending:
                    bmpStatus.UriSource = new Uri("ms-appx:" + "/Assets/icons/ReportIsPending.png");
                    break;
                case IconLegend.IconsEnum.PendingSchedule:
                    bmpStatus.UriSource = new Uri("ms-appx:" + "/Assets/icons/ReportApprovalIsPending.png");
                    break;
                case IconLegend.IconsEnum.ReceiptsAttached:
                    bmpStatus.UriSource = new Uri("ms-appx:" + "/Assets/icons/DocumentExpenseCell.png");
                    break;
                case IconLegend.IconsEnum.Refused:
                    bmpStatus.UriSource = new Uri("ms-appx:" + "/Assets/icons/ReportHasBeenRefused.png");
                    break;
                case IconLegend.IconsEnum.Rejected:
                    bmpStatus.UriSource = new Uri("ms-appx:" + "/Assets/icons/ReportRejectedByController.png");
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
