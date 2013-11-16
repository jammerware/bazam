using System;

namespace Bazam.DataNinja
{
    public static class DataFormatting
    {
        public static bool IsNull(object input)
        {
            return input == null || Convert.IsDBNull(input);
        }

        public static object IsNull(object input, object defaultValue)
        {
            return (IsNull(input) ? defaultValue : input);
        }
    }
}
