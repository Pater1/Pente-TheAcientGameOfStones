using System;
using System.Globalization;
using System.Windows.Data;

namespace Pente.Converters
{
    public class WinnerNameToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return $"{value?.ToString()} is the winner!";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}