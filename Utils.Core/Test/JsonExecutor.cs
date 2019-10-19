using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using FluentAssertions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Utils.Core.Expressions;

namespace Utils.Core.Test
{
    public class JsonExecutor
    {
        private readonly Action<ExecuteTraceInfo> _traceAction;
        private readonly IEnumerable<TestInfo> _tests;
        private readonly MethodProxy _methodProxy;

        public JsonExecutor(string dataJson, string configJson, Action<ExecuteTraceInfo> traceAction)
        {
            _traceAction = traceAction;
            this._tests = JsonConvert.DeserializeObject<IEnumerable<TestInfo>>(dataJson);
            var config = JsonConvert.DeserializeObject<IEnumerable<TestConfig>>(configJson);
            _methodProxy = new MethodProxy(config?.Select(c => c.TypeName), traceAction);
        }

        public IDictionary<string, object> Execute(IDictionary<string, object> variables)
        {
            IDictionary<string, object> results = new Dictionary<string, object>();
            foreach (var test in _tests)
            {
                results[test.Name] = ExecuteTest(test, variables, out _);
            }

            return results;
        }

        public void ExecuteAndVerify(IDictionary<string, object> variables)
        {
            foreach (var test in _tests)
            {
                var results = ExecuteTest(test, variables, out var resultsType);
                if (resultsType == ResultsType.Primitive)
                {
                    var finalExpectedValue = EvaluateParameters(new Dictionary<string, object>()
                    {
                        {"result", test.ReturnValue}
                    }, variables).First();
                    var result = results["result"];
                    Verify(test, () => result, finalExpectedValue.Value);
                }
                else if (resultsType == ResultsType.Void)
                {
                    // Nothing to verify.
                }
                else if (resultsType == ResultsType.Object)
                {
                    var evaluatedExpectedValues = EvaluateParameters(test.GetExpectedResults(), variables);
                    if (evaluatedExpectedValues.Any())
                    {
                        var returnObject = results["result"];
                        var output = JsonConvert.SerializeObject(returnObject, Formatting.Indented);
                        File.WriteAllText(@"c:\temp\test.json", output);
                        var expectedObjectJson = JsonConvert.SerializeObject(evaluatedExpectedValues.First().Value);
                        Console.WriteLine(expectedObjectJson);
                        var returnType = results["resultType"];
                        var expectedObject = JsonConvert.DeserializeObject(expectedObjectJson?.ToString(), returnType as Type);
                        // Verify dictionary.
                        SendVerificationTraceInfo(test, returnObject, expectedObject);
                        returnObject.Should().BeEquivalentTo(expectedObject, test.Name);
                    }
                }
                else
                {
                    var evaluatedExpectedValues = EvaluateParameters(test.GetExpectedResults(true), variables);
                    Console.WriteLine("===========================");
                    // Verify dictionary.
                    results.Remove("resultType");
                    SendVerificationTraceInfo(test, results, evaluatedExpectedValues);
                    results.Should().BeEquivalentTo(evaluatedExpectedValues, test.Name);
                }
            }
        }

        private IDictionary<string, object> ExecuteTest(TestInfo test, IDictionary<string, object> variables, out ResultsType resultsType)
        {
            var newParameters = EvaluateParameters(test.Parameters, variables);
            var output = this._methodProxy.Execute(test.Api, newParameters);
            var results = new Dictionary<string, object>()
            {
                {"resultType", this._methodProxy.ReturnType}
            };

            if (output == null)
            {
                if (this._methodProxy.ReturnType == null)
                {
                    resultsType = ResultsType.Void;
                    return results;
                }

                resultsType = ResultsType.Object;
                results["result"] = null;
                return results;
            }

            if (output.GetType().IsPrimitive)
            {
                resultsType = ResultsType.Primitive;
                results["result"] = output;
                return results;
            }

            if (output is string)
            {
                resultsType = ResultsType.String;
                results["result"] = output;
                return results;
            }

            if (output is IDictionary<string, object>)
            {
                resultsType = ResultsType.Dictionary;
                return output as IDictionary<string, object>;
            }

            resultsType = ResultsType.Object;
            results["result"] = output;
            return results;
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

                if (parameter.Value is JArray array)
                {
                    newParameterValues[parameter.Key] = ProcessJArray(array, variables);
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

        private JArray ProcessJArray(JArray array, IDictionary<string, object> variables)
        {
            JArray newArray = new JArray();
            foreach (var token in array)
            {
                var modifiedToken = ProcessJToken(token, variables);
                newArray.Add(modifiedToken);
            }

            return newArray;
        }

        private JToken ProcessJToken(JToken token, IDictionary<string, object> variables)
        {
            if (!token.Children().Any())
            {
                return token;
            }

            var newToken = new JObject();
            foreach (var childToken in token.Children())
            {
                if (childToken is JProperty property)
                {
                    if (TryExpression(property.Value?.ToString(), out var expression))
                    {
                        newToken.Add(new JProperty(property.Name, Evaluate(expression, variables)?.ToString()));
                    }
                    else
                    {
                        newToken.Add(property);
                    }
                }
                else
                {
                    newToken.Add(childToken);
                }
            }
            return newToken;
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

            SendVerificationTraceInfo(test, actual, expectedValue);
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

        private void SendVerificationTraceInfo(TestInfo testInfo, object actual, object expected)
        {
            this._traceAction(new ExecuteTraceInfo(TraceType.Verification)
            {
                Actual = actual,
                TestInfo = testInfo,
                Expected = expected
            });
        }
    }
}
