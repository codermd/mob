using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml.Data;
using Mxp.Core.Business;
using Windows.UI.Xaml.Media.Imaging;


namespace Mxp.Win
{
    class CountryToBitmapConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, String culture)
        {          
                return new BitmapImage(new Uri("ms-appx:" + "/Assets/flags/" + (string)value));   
        }
        public object ConvertBack(object value, Type targetType, object parameter, String culture)
        {
            throw new NotImplementedException();
        }
    }
}
