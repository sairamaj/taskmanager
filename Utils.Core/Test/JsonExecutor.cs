using System;
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
            _methodProxy = new MethodProxy(config.Select(c => c.TypeName));
        }

        public IDictionary<string, object> Execute(IDictionary<string, object> variables)
        {
            IDictionary<string, object> results = new Dictionary<string, object>();
            foreach (var test in _tests)
            {
                var newParameters = EvaluateParameters(test.Parameters, variables);
                results[test.Name] = this._methodProxy.Execute(test.Api, newParameters);
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
                    var finalExpectedValue = EvaluateParameters(new Dictionary<string, object>()
                    {
                        {"result", test.ReturnValue}
                    }, variables).First();
                    Verify(test, () => result, finalExpectedValue.Value);
                }
                else if (result is IDictionary<string, object>)
                {
                    Console.WriteLine("============ Expected ===============");
                    var evaluatedExpectedValues = EvaluateParameters(test.GetExpectedResults(), variables);
                    foreach (var kv in evaluatedExpectedValues)
                    {
                        Console.WriteLine($"--> {kv.Key}  |{kv.Value}|{kv.Value?.GetType()}");
                    }
                    Console.WriteLine("===========================");
                    Console.WriteLine("============ Actual ===============");
                    foreach (var kv in result as IDictionary<string, object>)
                    {
                        Console.WriteLine($"--> {kv.Key}  |{kv.Value}|{kv.Value?.GetType()}");
                    }
                    Console.WriteLine("===========================");
                    // Verify dictionary.
                    (result as IDictionary<string, object>).Should().BeEquivalentTo(evaluatedExpectedValues);
                }
                else
                {
                    throw new NotSupportedException($"{test.Api} returning {result.GetType()} is not supported for ");
                }
            }
        }

        IDictionary<string, object> EvaluateParameters(IDictionary<string, object> parameters, IDictionary<string, object> variables)
        {
            var newParameterValues = new Dictionary<string, object>(StringComparer.InvariantCultureIgnoreCase);
            if (parameters == null)
            {
                return newParameterValues;
            }

            foreach (var parameter in parameters)
            {
                if (parameter.Value is System.String[])
                {
                    var evaluatedItems = new List<string>();
                    foreach (var val in parameter.Value as System.String[])
                    {
                        if (TryExpression(val, out var expression))
                        {
                            evaluatedItems.Add(Evaluate(expression, variables)?.ToString());
                        }
                        else
                        {
                            evaluatedItems.Add(val);
                        }
                    }
                    newParameterValues[parameter.Key] = evaluatedItems.ToArray();
                    continue;
                }

                var value = parameter.Value;
                if (TryExpression(value?.ToString(), out var expression2))
                {
                    value = Evaluate(expression2, variables);
                }

                newParameterValues[parameter.Key] = value;
            }

            return newParameterValues;
        }

        private object Evaluate(string expression, IDictionary<string, object> variables)
        {
            object evaluatedValue = expression;
            var expressionInfo = Evaluator.Parse(expression);
            var evaluatedParameters = new Dictionary<string, object>(StringComparer.InvariantCultureIgnoreCase);
            if (expressionInfo.MethodData != null)
            {
                foreach (var arg in expressionInfo.MethodData.Arguments)
                {
                    evaluatedParameters[arg.Name] = EvaluateValue(arg, variables);
                }

                evaluatedValue = _methodProxy.Execute(expressionInfo.MethodData.Name, evaluatedParameters);
            }
            else if (expressionInfo.Variable != null)
            {
                evaluatedValue = EvaluateValue(expressionInfo.Variable, variables);
            }

            return evaluatedValue;
        }

        private object EvaluateValue(Variable variable, IDictionary<string, object> variables)
        {
            object variableValue;
            if (variables.TryGetValue(variable.Name, out variableValue))
            {
                return variableValue;
            }

            return null;
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

        private bool TryExpression(string val, out string expression)
        {
            expression = null;
            if (val == null)
            {
                return false;
            }

            if (val.StartsWith("${") && val.EndsWith("}"))
            {
                expression = val.Substring("${".Length).TrimEnd('}');
                return true;
            }

            return false;
        }
    }
}
