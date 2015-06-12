using System;
using System.Globalization;
using System.Xml.Linq;
using Bazam.Slugging;

namespace Bazam.Modules
{
    public static class XMLPal
    {
        public static bool? GetBool(XElement el)
        {
            return Get<bool?>(el);
        }

        public static bool? GetBool(XAttribute attr)
        {
            return Get<bool?>(attr);
        }

        public static DateTime? GetDate(XElement el)
        {
            return Get<DateTime?>(el);
        }

        public static DateTime? GetDate(XAttribute attr)
        {
            return Get<DateTime?>(attr);
        }

        public static double? GetDouble(XElement el)
        {
            return Get<double?>(el);
        }

        public static double? GetDouble(XAttribute attr)
        {
            return Get<int?>(attr);
        }

        public static int? GetInt(XAttribute attr)
        {
            return Get<int?>(attr);
        }

        public static int? GetInt(XElement el)
        {
            return Get<int?>(el);
        }

        public static long? GetLong(XAttribute attr)
        {
            return Get<long?>(attr);
        }

        public static long? GetLong(XElement el)
        {
            return Get<long?>(el);
        }

        public static string GetString(XAttribute attr)
        {
            return Get<string>(attr);
        }

        public static string GetString(XElement el)
        {
            return Get<string>(el);
        }

        public static T Get<T>(XAttribute attr)
        {
            string data = GetString(attr);
            return Get<T>(data);
        }

        public static T Get<T>(XElement el)
        {
            string data = GetString(el);
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