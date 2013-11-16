using System;

namespace Bazam.Modules
{
    public static class EnuMaster
    {
        public static T Parse<T>(string input) where T : struct
        {
            return Parse<T>(input, true);
        }

        public static T Parse<T>(string input, bool ignoreCase) where T: struct
        {
            return Parse<T>(input, true, true);
        }

        public static T Parse<T>(string input, bool ignoreCase, bool throwExceptionOnFail) where T : struct
        {
            if (!typeof(T).IsEnum) {
                throw new InvalidOperationException("When using EnuMaster.Parse, the generic type supplied must be an enumeration.");
            }

            T retVal = default(T);
            try {
                retVal = (T)Enum.Parse(typeof(T), input, ignoreCase);
            }
            catch (Exception) {
                if (throwExceptionOnFail)
                    throw;
            }

            return retVal;
        }

        public static bool TryParse<T>(string input, out T result) where T : struct
        {
            return TryParse<T>(input, out result, true);
        }

        public static bool TryParse<T>(string input, out T result, bool ignoreCase) where T : struct
        {
            try {
                result = Parse<T>(input, ignoreCase);
            }
            catch (InvalidOperationException) {
                throw;
            }
            catch {
                result = default(T);
                return false;
            }
            return true;
        }
    }
}