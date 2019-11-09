using System;
using System.Collections.Generic;

namespace JsonExecutorTasks.Code
{
    /// <summary>
    /// Sample test class for testing executors.
    /// </summary>
    public static class Sample
    {
        /// <summary>
        /// Get dictionary info.
        /// </summary>
        /// <returns>
        /// Dictionary of items to test.
        /// </returns>
        public static IDictionary<string, object> GetInfo()
        {
            return new Dictionary<string, object>
            {
                { "result", 0 },
                { "val1", "val1" },
                { "val2", true },
                { "val3", 1.0 },
            };
        }

        /// <summary>
        /// Get info return as dictionary.
        /// </summary>
        /// <returns>
        /// Dictionary of items to test.
        /// </returns>
        public static IDictionary<string, object> GetInfoReturnArrayInDictionary()
        {
            return new Dictionary<string, object>
            {
                { "val1", new string[] { "item1_1", "item1_2" } },
                { "val2", new string[] { "item2_1", "item2_2" } },
            };
        }

        /// <summary>
        /// Process persons for testing as enumerable of objects.
        /// </summary>
        /// <param name="id">
        /// Id for test.
        /// </param>
        /// <param name="persons">
        /// Persons to test.
        /// </param>
        /// <returns>
        /// Persons as string.
        /// </returns>
        public static string ProcessPersons(Guid id, IEnumerable<Person> persons)
        {
            var ret = string.Empty;
            foreach (var p in persons)
            {
                ret += $"{p.Name}-{p.Age}-";
            }

            return ret.TrimEnd('-');
        }

        /// <summary>
        /// Gets persons as null.
        /// </summary>
        /// <param name="id">
        /// Id value.
        /// </param>
        /// <returns>
        /// null always.
        /// </returns>
        public static IEnumerable<Person> GetNullPersons(Guid id)
        {
            return null;
        }

        /// <summary>
        /// Get null object for testing.
        /// </summary>
        /// <returns>
        /// null always.
        /// </returns>
        public static Person GetNullObject()
        {
            return null;
        }

        /// <summary>
        /// Generate exception for testing.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// Throws always.
        /// </exception>
        public static void GenerateException()
        {
            throw new ArgumentNullException($"this is simulated exception");
        }
    }
}
