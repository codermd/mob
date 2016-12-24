using Mxp.Core.Business;
using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media.Imaging;

namespace Mxp.Win
{
    class BoolToCollapsedConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, String culture)
        {
            return (Boolean)value ? Visibility.Collapsed : Visibility.Visible;            
        }
        public object ConvertBack(object value, Type targetType, object parameter, String culture)
        {
            throw new NotImplementedException();
        }
    }
}
