using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace Pente.Converters
{
    public class TimerIntToSecondConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (int.TryParse(value?.ToString(), out int i))
            {
                return i < 2 ? $"Turn Timer: {i} second" : $"Turn Timer: {i} seconds";
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}