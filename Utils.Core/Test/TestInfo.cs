using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using FluentAssertions;
using Newtonsoft.Json.Linq;

namespace Utils.Core.Test
{
    public class TestInfo
    {
        public string Name { get; set; }
        public string Api { get; set; }
        public IDictionary<string,object> Parameters { get; set; }
        public IDictionary<string, object> Expected { get; set; }

        public object GetParameter(string name)
        {
            if (this.Parameters == null)
            {
                return null;
            }

            if (this.Parameters.ContainsKey(name))
            {
                return this.Parameters[name];
            }

            return null;
        }

        public object ReturnValue => GetExpectedValue<object>("result");

        public void LogParameters()
        {
            if (this.Parameters == null)
            {
                return;
            }

            foreach (var parameter in this.Parameters)
            {
                Console.WriteLine($"{parameter.Key}:|{parameter.Value}|");
            }
        }

        public void LogExpected()
        {
            foreach (var kv in this.GetExpectedResults())
            {
                Console.WriteLine($"{kv.Key}:|{kv.Value}|{kv.Value.GetType()}");
            }
        }

        public T GetExpectedValue<T>(string name)
        {
            name = name.ToLowerInvariant();
            if (GetExpectedResults().ContainsKey(name))
            {
                var val = GetExpectedResults()[name];
                if (typeof(IConvertible).IsAssignableFrom(val.GetType()))
                {
                    return (T)Convert.ChangeType(val, typeof(T));
                }

                return (T)val;
            }

            return default(T);
        }

        public void Verify<T>(Expression<Func<T>> selectorExpression)
        {
            if (selectorExpression == null)
            {
                throw new ArgumentNullException(nameof(selectorExpression));
            }

            var actual = selectorExpression.Compile()();

            var body = selectorExpression.Body as MemberExpression;
            if (body == null)
            {
                return;
            }

            var propertyName = body.Member.Name;

            var expected = GetExpectedValue<T>(propertyName);
            if (actual != null && actual.GetType().IsArray)
            {
                var expectedObject = Convert.ChangeType(expected, expected.GetType());
                actual.Should().BeEquivalentTo(expectedObject, $"{this.Name} {propertyName} did fail");
            }
            else
            {
                actual.Should().Be(expected, $"{this.Name} {propertyName} did fail");
            }
        }

        private IDictionary<string, object> GetExpectedResults()
        {
            if (this.Expected == null || this.Expected.Values == null)
            {
                return new Dictionary<string, object>();
            }

            return this.Expected.ToDictionary(
                kv => kv.Key.ToLowerInvariant(), kv =>
                {
                    if (kv.Value.GetType() == typeof(JArray))
                    {
                        return ((JArray)kv.Value).Select(k => k?.ToString()).ToArray();
                    }

                    return kv.Value;
                });
        }

    }

}
