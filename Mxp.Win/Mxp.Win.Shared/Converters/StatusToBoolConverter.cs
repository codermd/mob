using Mxp.Core.Business;
using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml.Data;

namespace Mxp.Win
{
    class StatusToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, String culture)
        {
            ExpenseItem.Status status = (ExpenseItem.Status)value;
            return status == ExpenseItem.Status.Accepted;
           
        }
        public object ConvertBack(object value, Type targetType, object parameter, String culture)
        {
            throw new NotImplementedException();
        }
    }
}
