using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml.Data;
using Mxp.Core.Business;
using Windows.UI.Xaml.Media.Imaging;


namespace Mxp.Win
{
    class ApprovalToNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, String culture)
        {
            if (value != null)
            {
                if (value is Report)
                    return ((Report)value).Name;
            }
            if (parameter != null)
            {
                if (parameter is Travel)
                    return ((Travel)parameter).Name;
            }          
            return null;
        }
        public object ConvertBack(object value, Type targetType, object parameter, String culture)
        {
            throw new NotImplementedException();
        }
    }
}
