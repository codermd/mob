using System;
using Windows.UI;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using Mxp.Core.Business;

namespace Mxp.Win.Converters
{
    public class ExpenseFundingToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var expense = value as Expense;

            if (expense == null)
                return null;
            
            return new SolidColorBrush(expense.IsHighlighted ? Colors.Red : Colors.Black);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
