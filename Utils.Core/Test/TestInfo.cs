using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace Utils.Core.Test
{
    /// <summary>
    /// Test information.
    /// </summary>
    public class TestInfo
    {
        /// <summary>
        /// Gets or sets test name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets api of the test.
        /// </summary>
        public string Api { get; set; }

        /// <summary>
        /// Gets or sets parameters.
        /// </summary>
        public IDictionary<string,object> Parameters { get; set; }

        /// <summary>
        /// Gets or sets variables.
        /// </summary>
        public IDictionary<string, object> Variables { get; set; }

        /// <summary>
        /// Gets or sets extracts.
        /// </summary>
        public IDictionary<string, object> Extracts { get; set; }

        /// <summary>
        /// Gets or sets expected values.
        /// </summary>
        public IDictionary<string, object> Expected { get; set; }

        /// <summary>
        /// Gets return value.
        /// </summary>
        public object ReturnValue => this.GetExpectedValue<object>("result");

        /// <summary>
        /// Gets expected value.
        /// </summary>
        /// <typeparam name="T">
        /// Type name.
        /// </typeparam>
        /// <param name="name">
        /// Name of the expected parameter.
        /// </param>
        /// <returns>
        /// Gets value.
        /// </returns>
        public T GetExpectedValue<T>(string name)
        {
            name = name.ToLowerInvariant();
            if (this.GetExpectedResults().ContainsKey(name))
            {
                var val = this.GetExpectedResults()[name];
                if (typeof(IConvertible).IsAssignableFrom(val.GetType()))
                {
                    return (T)Convert.ChangeType(val, typeof(T));
                }

                return (T)val;
            }

            return default(T);
        }

        /// <summary>
        /// Gets expected results.
        /// </summary>
        /// <param name="convertJArray">
        /// A value indicating whether to convert JArray in to string array or not.
        /// </param>
        /// <returns>
        /// Expected results value.
        /// </returns>
        public IDictionary<string, object> GetExpectedResults(bool convertJArray = false)
        {
            if (Expected?.Values == null)
            {
                return new Dictionary<string, object>();
            }

            if (!convertJArray)
            {
                return this.Expected;
            }

            return this.Expected.ToDictionary(
                kv => kv.Key, kv =>
                {
                    if (kv.Value != null && kv.Value.GetType() == typeof(JArray))
                    {
                        return ((JArray)kv.Value).Select(k => k?.ToString()).ToArray();
                    }

                    return kv.Value;
                });
        }
    }
}
