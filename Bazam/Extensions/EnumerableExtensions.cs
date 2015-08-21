using System;
using System.Collections.Generic;
using System.Linq;

namespace Bazam.Extensions
{
    public static class EnumerableExtensions
    {
        public static string Concatenate<T>(this IEnumerable<T> helper)
        {
            return Concatenate(helper, ",");
        }

        public static string Concatenate<T>(this IEnumerable<T> helper, string delimiter)
        {
            string retVal = string.Empty;
            foreach (T item in helper) {
                retVal += item.ToString() + delimiter;
            }

            if (retVal == string.Empty) {
                return retVal;
            }

            return retVal.Substring(0, retVal.Length - delimiter.Length);
        }

        public static T Random<T>(this IEnumerable<T> helper)
        {
            T[] array = helper.ToArray();
            return array[new Random().Next(array.Length)];
        }
    }
}