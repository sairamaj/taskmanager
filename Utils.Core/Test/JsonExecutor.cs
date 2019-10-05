﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using FluentAssertions;
using Newtonsoft.Json;
using Utils.Core.Expressions;

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

        public IDictionary<string,object> Execute(IDictionary<string, object> variables)
        {
            IDictionary<string,object> results = new Dictionary<string, object>();
            foreach (var test in _tests)
            {
                var newParameters = EvaluateParameters(test.Parameters, variables);
                results[test.Name]  = this._methodProxy.Execute(test.Api, newParameters);
            }

            return results;
        }

        public void ExecuteAndVerify(IDictionary<string, object> variables)
        {
            foreach (var test in _tests)
            {
                Console.WriteLine($"Executing {test.Name}");
                test.LogParameters();
                test.LogExpected();

                var newParameters = EvaluateParameters(test.Parameters, variables);
                var result = this._methodProxy.Execute(test.Api, newParameters);
                if (result == null)
                {
                    continue;       // nothing to verify.
                }

                if (result.GetType().IsPrimitive)
                {
                    Console.WriteLine(result);
                    Verify(test, () => result, test.ReturnValue);
                }
                else if (result is IDictionary<string, object>)
                {
                    // Verify dictionary.
                    (result as IDictionary<string,object>).Should().BeEquivalentTo(test.Expected);
                }
                else
                {
                    throw new NotSupportedException($"{test.Api} returning {result.GetType()} is not supported for ");
                }
            }
        }

        IDictionary<string, object> EvaluateParameters(IDictionary<string, object> parameters, IDictionary<string, object> variables)
        {
            var newParameterValues = new Dictionary<string, object>();
            foreach (var parameter in parameters)
            {
                if (parameter.Value != null && parameter.Value.ToString().StartsWith("${"))
                {
                    var mInfo = Evaluator.Parse(parameter.Value.ToString().Substring("${".Length).TrimEnd('$'));
                    var evaluatedParameters = new Dictionary<string, object>(StringComparer.InvariantCultureIgnoreCase);
                    foreach (var arg in mInfo.Arguments)
                    {
                        evaluatedParameters[arg.Name] = EvaluateValue(arg, variables);
                    }

                    var val = _methodProxy.Execute(mInfo.Name, evaluatedParameters);
                    newParameterValues[parameter.Key] = val;
                }
                else
                {
                    newParameterValues[parameter.Key] = parameter.Value;
                }
            }

            return newParameterValues;
        }

        private object EvaluateValue(Argument arg, IDictionary<string, object> variables)
        {
            var value = arg.Val;
            if (value == null)
            {
                return null;
            }

            if (!arg.IsVariable)
            {
                return value;
            }

            var variablename = arg.Val;
            object variableValue;
            if (variables.TryGetValue(variablename, out variableValue))
            {
                return variableValue;
            }

            return null;
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
