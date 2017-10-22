using System;
using System.Globalization;
using System.Windows.Data;

namespace Pente.Converters
{
    public class PlayerNameLabelConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return $"{value?.ToString()}'s Stats";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}