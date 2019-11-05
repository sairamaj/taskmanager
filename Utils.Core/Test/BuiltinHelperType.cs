using System;
using System.Collections.Generic;
using System.Linq;

namespace Utils.Core.Test
{
    /// <summary>
    /// Helper functions for expression evaluations.
    /// </summary>
    public static class BuiltinHelperType
    {
        /// <summary>
        /// Gets random number.
        /// </summary>
        /// <param name="min">
        /// Minimum number.
        /// </param>
        /// <param name="max">
        /// Maximum number.
        /// </param>
        /// <returns>
        /// A random number.
        /// </returns>
        public static int Random(int min, int max)
        {
            return new Random().Next(min, max);
        }

        /// <summary>
        /// Gets lower case of given string.
        /// </summary>
        /// <param name="input">
        /// Input string.
        /// </param>
        /// <returns>
        /// Lower case of the given string.
        /// </returns>
        public static string ToLower(string input)
        {
            return input?.ToLowerInvariant();
        }

        /// <summary>
        /// Reverses the given string.
        /// </summary>
        /// <param name="input">
        /// Input string.
        /// </param>
        /// <returns>
        /// Reversed string.
        /// </returns>
        public static string Reverse(string input)
        {
            return new string(input.ToCharArray().Reverse().ToArray());
        }

        /// <summary>
        /// Extracts the value of given key from the dictionary.
        /// </summary>
        /// <param name="key">
        /// Key value.
        /// </param>
        /// <param name="result">
        /// Dictionary input.
        /// </param>
        /// <returns>
        /// Value in the dictionary if present, otherwise null.
        /// </returns>
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

        /// <summary>
        /// Gets back the given string.
        /// </summary>
        /// <param name="input">
        /// Input string.
        /// </param>
        /// <returns>
        /// Returns the same string given.
        /// </returns>
        public static string EchoString(string input)
        {
            return input;
        }

        /// <summary>
        /// Echos the given dictionary.
        /// </summary>
        /// <param name="inputs">
        /// Input string.
        /// </param>
        /// <returns>
        /// Returns the same dictionary.
        /// </returns>
        public static IDictionary<string, object> EchoDictionary(IDictionary<string, object> inputs)
        {
            return inputs;
        }
    }
}
