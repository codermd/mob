using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml.Data;
using Mxp.Core.Business;
using Windows.UI.Xaml.Media.Imaging;


namespace Mxp.Win
{
    class AttendeeTypeToBitmapConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, String culture)
        {
           
            AttendeeTypeEnum type = (AttendeeTypeEnum)value;
          
            BitmapImage bmpPolicy = new BitmapImage();
            switch (type)
            {
                case AttendeeTypeEnum.Employee:
                    bmpPolicy.UriSource = new Uri("ms-appx:" + "/Assets/icons/ic_employee_attendee.png");
                    break;
                case AttendeeTypeEnum.Business:
                    bmpPolicy.UriSource = new Uri("ms-appx:" + "/Assets/icons/ic_business_attendee.png");
                    break;             
            }
            return bmpPolicy;            
        }
        public object ConvertBack(object value, Type targetType, object parameter, String culture)
        {
            throw new NotImplementedException();
        }

    
    }
}
