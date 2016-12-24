using Mxp.Core.Business;
using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace Mxp.Win
{
    class SelectedProductToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, String culture)
        {
            if (SelectedProduct == null)
                return new SolidColorBrush(Color.FromArgb(255, 239, 239, 239));
            else if(SelectedProduct.Equals((Product)value))
                return new SolidColorBrush(Color.FromArgb(128, 0, 168, 198));
            else
                return  new SolidColorBrush(Color.FromArgb(255, 239, 239, 239));
               

        }
        public static Product SelectedProduct;
        public object ConvertBack(object value, Type targetType, object parameter, String culture)
        {
            throw new NotImplementedException();
        }
    }
}
