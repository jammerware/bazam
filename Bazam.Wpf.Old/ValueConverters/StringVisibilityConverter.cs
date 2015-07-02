using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Bazam.Wpf.ValueConverters
{
    public class StringVisibilityConverter : IValueConverter
    {
        public Object Convert(Object value, Type targetType, Object parameter, CultureInfo culture)
        {
            return (value == null || string.IsNullOrEmpty(value.ToString()) ? Visibility.Collapsed : Visibility.Visible);
        }

        public object ConvertBack(Object value, Type targetType, Object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
