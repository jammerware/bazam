using System;
using System.Collections.Generic;
using System.Linq;

namespace Bazam.Extensions
{
    public static class EnumerableExtensions
    {
        public static T Random<T>(this IEnumerable<T> helper)
        {
            T[] array = helper.ToArray();
            return array[new Random().Next(array.Length)];
        }
    }
}