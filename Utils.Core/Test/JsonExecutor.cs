using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using FluentAssertions;
using Newtonsoft.Json;

namespace Utils.Core.Test
{
    public class JsonExecutor
    {
        private readonly IEnumerable<TestInfo> _tests;
        private readonly MethodProxy _methodProxy;

        public JsonExecutor(string dataJson, string configJson)
        {
            Console.WriteLine(configJson);
            this._tests = JsonConvert.DeserializeObject<IEnumerable<TestInfo>>(dataJson);
            var config = JsonConvert.DeserializeObject<IEnumerable<TestConfig>>(configJson);
            _methodProxy = new MethodProxy(config.Select(c=>c.TypeName));

        }

        public void ExecuteAndVerify(IDictionary<string, string> variables)
        {
            foreach (var test in _tests)
            {
                Console.WriteLine($"Executing {test.Name}");
                test.LogParameters();
                test.LogExpected();

                var result = this._methodProxy.Execute(test.Api, test.Parameters);
                Console.WriteLine(result);
                Verify(test, ()=> result, test.ReturnValue);
            }
        }

        public void Verify<T>(TestInfo test, Expression<Func<T>> selectorExpression, object expectedValue)
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

            if (actual != null && actual.GetType().IsArray)
            {
                actual.Should().BeEquivalentTo(expectedValue, $"{test.Name} {propertyName} did fail");
            }
            else
            {
                actual.Should().Be(expectedValue, $"{test.Name} {propertyName} did fail");
            }
        }
    }
}
