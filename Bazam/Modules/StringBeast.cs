using System;
using System.Text;

namespace Bazam.Modules
{
    public static class StringBeast
    {
        public static string Capitalize(string input)
        {
            return Capitalize(input, false);
        }

        public static string Capitalize(string input, bool lowercaseRest)
        {
            string output = string.Empty;
            if (input.Length > 0) {
                output = input.Substring(0, 1).ToUpper();
                if (input.Length > 1) {
                    output += (lowercaseRest ? input.Substring(1).ToLower() : input.Substring(1));
                }
            }
            return output;
        }

        public static string StripIntegers(string input)
        {
            StringBuilder output = new StringBuilder();
            int dummy;
            foreach (Char c in input.ToCharArray()) {
                if (Int32.TryParse(c.ToString(), out dummy))
                    output.Append(c);
            }
            return output.ToString();
        }
    }
}
