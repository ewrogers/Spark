using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Spark.Common
{
    [ValueConversion(typeof(bool), typeof(Visibility))]
    public sealed class BooleanVisibilityConverter : IValueConverter
    {
        #region IValueConverter Methods
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is bool))
                throw new ArgumentException("The value must be a boolean value");

            return (bool)value ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is Visibility))
                throw new ArgumentException("The value must be a visibility value");

            return (Visibility)value == Visibility.Visible;
        }
        #endregion
    }
}
