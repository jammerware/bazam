using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Bazam.Wpf.ValueConverters
{
    public class IntegerVisibilityConverter : IValueConverter
    {
        public Object Convert(Object value, Type targetType, Object parameter, CultureInfo culture)
        {
            bool invert = System.Convert.ToBoolean(parameter);
            int typedVal = System.Convert.ToInt32(value);

            return (typedVal > 0 && !invert) ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(Object value, Type targetType, Object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
