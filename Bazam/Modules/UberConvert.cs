using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Bazam.Modules
{
    public static class UberConvert
    {
        #region String to...
        public static bool StringToBoolean(string input)
        {
            return StringToBoolean(input, true);
        }

        public static bool StringToBoolean(string input, bool throwOnError)
        {
            bool output;
            if (Boolean.TryParse(input, out output))
                return output;
            if (throwOnError)
                throw new ArgumentException("An invalid argument was passed to StringToBoolean.", "input");
            return output;
        }

        public static byte StringToByte(string input)
        {
            return StringToByte(input, true);
        }

        public static byte StringToByte(string input, bool throwOnError)
        {
            byte output;
            if (byte.TryParse(input, out output))
                return output;
            if(throwOnError)
                throw new ArgumentException("An invalid argument was passed to StringToByte.", "input");
            return output;
        }

        public static DateTime StringToDateTime(string input)
        {
            return StringToDateTime(input, true);
        }

        public static DateTime StringToDateTime(string input, bool throwOnError)
        {
            DateTime output;
            if(DateTime.TryParse(input, DefaultFormatProvider, DateTimeStyles.None, out output))
                return output;
            if(throwOnError)
                throw new ArgumentException("An invalid argument was passed to StringToDateTime.", "input");
            return default(DateTime);
        }

        public static double StringToDouble(string input)
        {
            return StringToDouble(input, true);
        }

        public static double StringToDouble(string input, bool throwOnError)
        {
            double output;
            if (double.TryParse(input, NumberStyles.None, DefaultFormatProvider, out output))
                return output;
            if(throwOnError)
                throw new ArgumentException("An invalid argument was passed to StringToDouble.", "input");
            return default(double);
        }

        public static string StringToHtml(string input)
        {
            return input.Replace(Environment.NewLine, "<br/>");
        }

        public static int StringToInt(string input)
        {
            return StringToInt(input, true);
        }

        public static int StringToInt(string input, bool throwOnError)
        {
            int output;
            if (int.TryParse(input, NumberStyles.None, DefaultFormatProvider, out output))
                return output;
            if (throwOnError)
                throw new ArgumentException("An invalid argument was passed to StringToInt.", "input");
            return default(int);
        }

        public static long StringToLong(string input)
        {
            return StringToLong(input, true);
        }

        public static long StringToLong(string input, bool throwOnError)
        {
            long output;
            if (long.TryParse(input, NumberStyles.None, DefaultFormatProvider, out output))
                return output;
            if (throwOnError)
                throw new ArgumentException("An invalid argument was passed to StringToLong.", "input");
            return default(long);
        }

        public static short StringToShort(string input)
        {
            return StringToShort(input, true);
        }

        public static short StringToShort(string input, bool throwOnError)
        {
            short output;
            if (short.TryParse(input, NumberStyles.None, DefaultFormatProvider, out output))
                return output;
            if (throwOnError)
                throw new ArgumentException("An invalid argument was passed to StringToShort.", "input");
            return default(short);
        }
        #endregion

        #region HTML To...
        public static string HtmlToString(string input)
        {
            Regex regEx = new Regex("<br\\s*/>");
            return regEx.Replace(input, Environment.NewLine);
        }
        #endregion

        #region Internal
        private static IFormatProvider DefaultFormatProvider
        {
            get { return new CultureInfo("en-US"); }
        }
        #endregion
    }
}