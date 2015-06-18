using System;
using System.Collections.Generic;
using System.Linq;

namespace Bazam.Modules
{
    public static class EnuMaster
    {
        public static IEnumerable<T> GetValues<T>() where T: struct
        {
            if (!typeof(T).IsEnum) {
                throw new EnuMasterException("When using EnuMaster.GetValues<T>(), the type argument must be an enumeration.");
            }

            return Enum.GetValues(typeof(T)).Cast<T>();
        }

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
                throw new InvalidOperationException("When using EnuMaster.Parse, the type argument must be an enumeration.");
            }

            T retVal = default(T);
            try {
                retVal = (T)Enum.Parse(typeof(T), input, ignoreCase);
            }
            catch (Exception) {
                if (throwExceptionOnFail)
                    throw new EnuMasterException("EnuMaster was unable to find a matching entry in the enum \"" + typeof(T).Name + "\" for \"" + input + "\".");
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