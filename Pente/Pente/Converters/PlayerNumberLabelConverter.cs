using System;
using System.Globalization;
using System.Windows.Data;

namespace Pente.Converters
{
    public class PlayerNumberLabelConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (int.TryParse(value?.ToString(), out int i))
            {
                return $"Player {i}'s Stats:";
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}