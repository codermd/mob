using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml.Data;
using Mxp.Core.Business;
using Windows.UI.Xaml.Media.Imaging;


namespace Mxp.Win
{
    class PolicyExpenseToBitmapConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, String culture)
        {
            BitmapImage bmpPolicy = new BitmapImage();
            String rule = (ExpenseItem.PolicyRules)value +"";
            switch (rule)
            {
                case "Green":
                    bmpPolicy.UriSource = new Uri("ms-appx:" + "/Assets/icons/ExpenseIsCompliant.png");
                    break;
                case "Orange":
                    bmpPolicy.UriSource = new Uri("ms-appx:" + "/Assets/icons/ExpenseNotCompliantPolicy.png");
                    break;
                case "Red":
                    bmpPolicy.UriSource = new Uri("ms-appx:" + "/Assets/icons/ExpenseNotCompliant.png");
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
