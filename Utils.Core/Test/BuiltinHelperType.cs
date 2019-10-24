using System;
using System.Collections.Generic;

namespace Utils.Core.Test
{
    public static class BuiltinHelperType
    {
        public static int Random(int min, int max)
        {
            return new Random().Next(min, max);
        }

        public static string ToLower(string input)
        {
            return input?.ToLowerInvariant();
        }

        public static object Extract(string key, IDictionary<string, object> result)
        {
            if (result == null)
            {
                return null;
            }

            if (result.TryGetValue(key, out object val))
            {
                return val;
            }

            return null;
        }

        public static string EchoString(string input)
        {
            return input;
        }
    }
}
