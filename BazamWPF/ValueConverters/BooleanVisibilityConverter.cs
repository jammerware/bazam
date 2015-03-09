using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace BazamWPF.ValueConverters
{
    public class BooleanVisibilityConverter : IValueConverter
    {
        public Object Convert(Object value, Type targetType, Object parameter, CultureInfo culture)
        {
            bool typedParam = System.Convert.ToBoolean(parameter);
            bool typedInput = System.Convert.ToBoolean(value);

            if (typedParam) typedInput = !typedInput;
            return typedInput ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(Object value, Type targetType, Object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}