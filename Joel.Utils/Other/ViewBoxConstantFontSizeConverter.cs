using System;
using System.Globalization;
using System.Windows.Data;

namespace Joel.Utils.Other
{
    public class ViewBoxConstantFontSizeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is double)) return null;
            double d = (double)value;
            double result = 100 / (d * 14);

            Console.WriteLine("D = " + d);
            Console.WriteLine("RESULT = " + result);

            if (result >= 14) return 14;

            return 100 / d * 14;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
