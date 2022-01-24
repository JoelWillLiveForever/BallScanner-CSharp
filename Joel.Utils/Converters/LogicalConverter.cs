using System;
using System.Globalization;
using System.Windows.Data;

namespace Joel.Utils.Converters
{
    public class LogicalConverter : IValueConverter
    {
        public int LimitWidth { get; set; }

        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return LimitWidth > (int)value;
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
