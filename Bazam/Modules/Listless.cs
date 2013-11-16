using System;
using System.Collections;
using System.Linq;
using System.Text;

namespace Bazam.Modules
{
    public static class Listless
    {
        public static string ListToString(IEnumerable input)
        {
            return ListToString(input, ",");
        }

        public static string ListToString(IEnumerable input, string delimiter)
        {
            string retVal = string.Empty;
            foreach(object item in input) {
                retVal += item.ToString() + delimiter;
            }

            if (retVal == string.Empty) {
                return retVal;
            }

            return retVal.Substring(0, retVal.Length - delimiter.Length);
        }
    }
}
