using System;
using System.Globalization;
using System.Windows.Data;

namespace Spark.Common
{
    [ValueConversion(typeof(bool), typeof(bool))]
    public sealed class InvertedBooleanConverter : IValueConverter
    {
        #region IValueConverter Methods
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is bool))
                throw new ArgumentException("The value must be a boolean value");

            return !(bool)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Convert(value, targetType, parameter, culture);
        }
        #endregion
    }
}
