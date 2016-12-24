using Mxp.Core.Business;
using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media.Imaging;

namespace Mxp.Win
{
    class BoolToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, String culture)
        {

            Boolean status = (Boolean)value;
            switch ((string)parameter)
            {
                case "breakfast":
                    return status ? new BitmapImage(new Uri("ms-appx:" + "/Assets/icons/BreakfastIConSelected.png")) :
                        new BitmapImage(new Uri("ms-appx:" + "/Assets/icons/BreakfastICon.png"));
                case "lunch":
                    return status ? new BitmapImage(new Uri("ms-appx:" + "/Assets/icons/LunchIconSelected.png")) :
                        new BitmapImage(new Uri("ms-appx:" + "/Assets/icons/LunchIcon.png"));
                case "dinner":
                    return status ? new BitmapImage(new Uri("ms-appx:" + "/Assets/icons/DinnerIconSelected.png")) :
                        new BitmapImage(new Uri("ms-appx:" + "/Assets/icons/DinnerIcon.png"));
                case "bed":
                    return status ? new BitmapImage(new Uri("ms-appx:" + "/Assets/icons/BedIconSelected.png")) :
                        new BitmapImage(new Uri("ms-appx:" + "/Assets/icons/BedIcon.png"));
                case "info":
                    return status ? new BitmapImage(new Uri("ms-appx:" + "/Assets/icons/InformationsIconSelected.png")) :
                        new BitmapImage(new Uri("ms-appx:" + "/Assets/icons/InformationsIcon.png"));
                case "moon":
                    return status ? new BitmapImage(new Uri("ms-appx:" + "/Assets/icons/MoonIconSelected.png")) :
                        new BitmapImage(new Uri("ms-appx:" + "/Assets/icons/MoonIcon.png"));
                default:
                    return null;
            }
        }
        public object ConvertBack(object value, Type targetType, object parameter, String culture)
        {
            throw new NotImplementedException();
        }
    }
}
