using System;
using System.Globalization;
using System.Windows.Data;

namespace Pente.Converters
{
    public class StyleOfWinToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return $"Style of win: {value?.ToString()}";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}