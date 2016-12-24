using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml.Data;
using Mxp.Core.Business;
using Windows.UI.Xaml.Media.Imaging;
using System.Diagnostics;

namespace Mxp.Win
{
    class TypeToNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, String culture)
        {
            // bool param = (bool)parameter;
            if (value is Country)
            {
                string name = "";
                for (int i = 0; i < (value as Country).PaddingLeft; i++)
                    name += "        ";
                name += (value as Country).Name;
                return name;

            }
            else if (value is Product)
                return (value as Product).ExpenseCategory.Name;
            else if (value is Currency)
                return (value as Currency).VName;
            else if (value is LookupItem)
                return (value as LookupItem).VTitle;
            else
                return "";

        }
        public object ConvertBack(object value, Type targetType, object parameter, String culture)
        {
            throw new NotImplementedException();
        }
    }
}
