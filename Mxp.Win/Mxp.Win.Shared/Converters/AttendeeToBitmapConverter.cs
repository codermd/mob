using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml.Data;
using Mxp.Core.Business;
using Windows.UI.Xaml.Media.Imaging;


namespace Mxp.Win
{
    class AttendeeToBitmapConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, String culture)
        {

            if ((bool)value)
                return new BitmapImage(new Uri("ms-appx:" + "/Assets/icons/AttendeeExpenseCell.png"));
            else
                return null;
                        
        }
        public object ConvertBack(object value, Type targetType, object parameter, String culture)
        {
            throw new NotImplementedException();
        }
    }
}
