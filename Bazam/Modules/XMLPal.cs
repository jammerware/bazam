using System;
using System.Globalization;
using System.Xml.Linq;
using Bazam.Slugging;

namespace Bazam.Modules
{
    public static class XmlPal
    {
        private static IFormatProvider provider = new CultureInfo("en-US");

        public static bool? GetBool(XAttribute input)
        {
            return (input == null ? null : new Nullable<bool>(Convert.ToBoolean(input.Value, provider)));
        }

        public static bool? GetBool(XElement input)
        {
            return (input == null ? null : new Nullable<bool>(Convert.ToBoolean(input.Value, provider)));
        }

        public static DateTime? GetDate(XAttribute input)
        {
            return (input == null ? null : new Nullable<DateTime>(Convert.ToDateTime(input.Value, provider)));
        }

        public static DateTime? GetDate(XElement input)
        {
            return (input == null ? null : new Nullable<DateTime>(Convert.ToDateTime(input.Value, provider)));
        }

        public static double? GetDouble(XElement el)
        {
            return (el == null || el.Value == string.Empty ? null : new Nullable<double>(Convert.ToDouble(el.Value, provider)));
        }

        public static double? GetDouble(XAttribute attr)
        {
            return (attr == null || attr.Value == string.Empty ? null : new Nullable<double>(Convert.ToDouble(attr.Value, provider)));
        }

        public static int? GetInt(XAttribute attr)
        {
            return (attr == null || attr.Value == string.Empty ? null : new Nullable<int>(Convert.ToInt32(attr.Value, provider)));
        }

        public static int? GetInt(XElement el)
        {
            return (el == null || el.Value == string.Empty ? null : new Nullable<int>(Convert.ToInt32(el.Value, provider)));
        }

        public static long? GetLong(XAttribute attr)
        {
            return (attr == null || attr.Value == string.Empty ? null : new Nullable<long>(long.Parse(attr.Value, provider)));
        }

        public static string GetString(XAttribute attr)
        {
            return (attr == null ? null : attr.Value.Trim());
        }

        public static string GetString(XElement el)
        {
            return (el == null ? null : el.Value.Trim());
        }

        public static T Get<T>(XAttribute attr)
        {
            string data = null;

            if (attr != null) {
                if (typeof(T) == typeof(string)) {
                    data = attr.Value;
                }
                else {
                    data = GetString(attr);
                }
            }

            return Get<T>(data);
        }

        public static T Get<T>(XElement el)
        {
            string data = null;

            if (el != null) {
                if (typeof(T) == typeof(string)) {
                    data = el.Value;
                }
                else {
                    data = GetString(el);
                }
            }

            return Get<T>(data);
        }

        private static T Get<T>(string data)
        {
            if (data != null) {
                return (T)Convert.ChangeType(data, typeof(T), new CultureInfo("en-US"));
            }

            return default(T);
        }

        public static XElement ToXElement(ISluggable input, string elementName, string attributeName)
        {
            if (input != null) {
                XElement retVal = new XElement(elementName);
                retVal.SetAttributeValue(attributeName, Slugger.Slugify(input.GetSlugBase()));
                return retVal;
            }
            return null;
        }
    }
}