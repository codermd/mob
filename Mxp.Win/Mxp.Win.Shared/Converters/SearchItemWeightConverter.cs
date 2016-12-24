using System;
using Windows.UI.Text;
using Windows.UI.Xaml.Data;
using Mxp.Core.Business;

namespace Mxp.Win.Converters
{
    public class SearchItemWeightConverter : IValueConverter
    {
        public object Convert (object value, Type targetType, object parameter, string language) {
            if (value is Country)
                return ((Country) value).IsMatched ? FontWeights.Bold : FontWeights.Normal;

            return FontWeights.Normal;
        }

        public object ConvertBack (object value, Type targetType, object parameter, string language) {
            throw new NotImplementedException ();
        }
    }
}
