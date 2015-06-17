using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Bazam.Wpf.ValueConverters
{
    public class ObjectVisibilityConverter : IValueConverter
    {
        public Object Convert(Object value, Type targetType, Object parameter, CultureInfo culture)
        {
            return (value == null ? Visibility.Collapsed : Visibility.Visible);
        }

        public object ConvertBack(Object value, Type targetType, Object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
