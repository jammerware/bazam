using System;
using System.Globalization;
using System.Windows.Data;

namespace Bazam.Wpf.ValueConverters
{
    public class CapitalizationConverter : IValueConverter
    {
        public Object Convert(Object value, Type targetType, Object parameter, CultureInfo culture)
        {
            if(value != null && !string.IsNullOrEmpty(value.ToString())) {
                string input = value.ToString();
                string output = string.Empty;
                bool typedParameter = false;
                if(parameter != null) {
                    typedParameter = System.Convert.ToBoolean(parameter.ToString());
                }

                for (int i = 0; i < input.Length; i++) {
                    if (i == 0 || typedParameter) {
                        output += input.ToCharArray()[i].ToString().ToUpper();
                    }
                    else {
                        output += input.ToCharArray()[i].ToString().ToLower();
                    }
                }
                
                return output;
            }
            return string.Empty;
        }

        public object ConvertBack(Object value, Type targetType, Object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
