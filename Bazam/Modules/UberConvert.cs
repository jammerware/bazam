using System;
using System.Drawing;
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

        public static Color StringToColor(string input)
        {
            return StringToColor(input, true);
        }

        public static Color StringToColor(string input, bool throwOnError)
        {
            input = input.ToLower();

            // check known named colors
            string[] knownColors = Enum.GetNames(typeof(KnownColor));
            foreach (string knownColor in knownColors) {
                if (knownColor.ToLower() == input)
                    return Color.FromName(input);
            }

            // not a known color. hopefully it's a hex representation...
            input = input.Replace("#", string.Empty);
            string r, g, b;

            if (input.Length == 6 || input.Length == 3) {
                if (input.Length == 6) {
                    r = input.Substring(0, 2);
                    g = input.Substring(2, 2);
                    b = input.Substring(4, 2);
                }
                else {
                    string inputRed = input.Substring(0, 1);
                    string inputGreen = input.Substring(1, 1);
                    string inputBlue = input.Substring(2, 1);

                    r = inputRed + inputRed;
                    g = inputGreen + inputGreen;
                    b = inputBlue + inputBlue;
                }

                return Color.FromArgb(Convert.ToInt32(r, 16), Convert.ToInt32(g, 16), Convert.ToInt32(b, 16));
            }

            if(throwOnError)
                throw new ArgumentException("An invalid argument was passed to StringToColor.", "input");

            return default(Color);
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

        #region Color To...
        public static string ColorToHex(Color input)
        {
            string hR = string.Format("{0:X}", input.R);
            string hG = string.Format("{0:X}", input.G);
            string hB = string.Format("{0:X}", input.B);

            if (hR.Length < 2)
                hR = "0" + hR;
            if (hG.Length < 2)
                hG = "0" + hG;
            if (hB.Length < 2)
                hB = "0" + hB;

            return "#" + hR + hG + hB;
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