using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Bazam.Slugging;

namespace Bazam.Modules
{
    public static class XMLPal
    {
        private static IFormatProvider provider = new CultureInfo("en-US");

        public static bool GetBool(XAttribute input)
        {
            return (input == null ? false : Convert.ToBoolean(input.Value, provider));
        }

        public static bool GetBool(XElement input)
        {
            return (input == null ? false : Convert.ToBoolean(input.Value, provider));
        }

        public static DateTime GetDate(XAttribute input)
        {
            return (input == null ? DateTime.MinValue : Convert.ToDateTime(input.Value, provider));
        }

        public static DateTime GetDate(XElement input)
        {
            return (input == null ? DateTime.MinValue : Convert.ToDateTime(input.Value, provider));
        }

        public static DateTime GetDateFromTwitterFormat(XElement input)
        {
            if (input == null)
                return DateTime.MinValue;

            string[] parts = input.Value.Split(' ');
            string output = parts[1] + " " + parts[2] + " " + parts[5] + " " + parts[3];
            return Convert.ToDateTime(output, provider);
        }

        public static double GetDouble(XElement el)
        {
            return (el == null || el.Value == string.Empty ? 0 : Convert.ToDouble(el.Value, provider));
        }

        public static double GetDouble(XAttribute attr)
        {
            return (attr == null || attr.Value == string.Empty ? 0 : Convert.ToDouble(attr.Value, provider));
        }

        public static int GetInt(XAttribute attr)
        {
            return (attr == null || attr.Value == string.Empty ? 0 : Convert.ToInt32(attr.Value, provider));
        }

        public static int GetInt(XElement el)
        {
            return (el == null || el.Value == string.Empty ? 0 : Convert.ToInt32(el.Value, provider));
        }

        public static long GetLong(XAttribute attr)
        {
            return (attr == null || attr.Value == string.Empty ? 0 : long.Parse(attr.Value, provider));
        }

        public static string GetString(XAttribute attr)
        {
            return (attr == null ? string.Empty : attr.Value);
        }

        public static string GetString(XElement el)
        {
            return (el == null ? string.Empty : el.Value);
        }

        public static string GetSanitizedString(XAttribute attr)
        {
            if (attr == null) { return string.Empty; }
            else { return GetSanitizedString(attr.Value); }
        }

        public static string GetSanitizedString(XElement el)
        {
            if (el == null) { return string.Empty; }
            else { return GetSanitizedString(el.Value); }
        }

        private static string GetSanitizedString(string input)
        {
            return Regex.Replace(input.Replace("\\n", Environment.NewLine).Trim(), "\\s(\\s)+", Environment.NewLine + Environment.NewLine);
        }

        public static XElement ToXElement(Slugger input, string elementName, string attributeName)
        {
            if (input != null) {
                XElement retVal = new XElement(elementName);
                retVal.SetAttributeValue(attributeName, input.GetSlug());
                return retVal;
            }
            return null;
        }
    }
}