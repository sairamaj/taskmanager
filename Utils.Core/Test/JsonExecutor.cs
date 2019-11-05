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
    /// <summary>
    /// Json Executor.
    /// Executes the json which contains api, expressions and variables.
    /// </summary>
    public class JsonExecutor
    {
        /// <summary>
        /// Trace action.
        /// </summary>
        private readonly Action<ExecuteTraceInfo> _traceAction;

        /// <summary>
        /// Tests information.
        /// </summary>
        private readonly IEnumerable<TestInfo> _tests;

        /// <summary>
        /// Method proxy.
        /// </summary>
        private readonly MethodProxy _methodProxy;

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonExecutor"/> class.
        /// </summary>
        /// <param name="dataJson">
        /// Json data to be executed.
        /// </param>
        /// <param name="configJson">
        /// Configuration json which contains types.
        /// </param>
        /// <param name="traceAction">
        /// A <see cref="Action{T}"/> of <see cref="ExecuteTraceInfo"/> for tracking the execution.
        /// </param>
        public JsonExecutor(string dataJson, string configJson, Action<ExecuteTraceInfo> traceAction)
        {
            this._traceAction = traceAction;
            this._tests = JsonConvert.DeserializeObject<IEnumerable<TestInfo>>(dataJson);
            var config = JsonConvert.DeserializeObject<IEnumerable<TestConfig>>(configJson);
            this._methodProxy = new MethodProxy(config?.Select(c => c.TypeName), traceAction);
        }

        /// <summary>
        /// Executes with given variables.
        /// </summary>
        /// <param name="variables">
        /// A <see cref="IDictionary{TKey,TValue}"/> of variables.
        /// </param>
        /// <returns>
        /// Execution results.
        /// </returns>
        public IDictionary<string, object> Execute(IDictionary<string, object> variables)
        {
            IDictionary<string, object> results = new Dictionary<string, object>();
            IDictionary<string, object> allVariables = new Dictionary<string, object>(StringComparer.InvariantCultureIgnoreCase);
            variables.ToList().ForEach(kv => allVariables[kv.Key] = kv.Value);
            foreach (var test in this._tests)
            {
                if (test.Variables != null)
                {
                    var testVariables = this.EvaluateParameters(test.Variables, allVariables);

                    // merge these into all variables
                    testVariables.ToList().ForEach(kv => allVariables[kv.Key] = kv.Value);
                }

                if (!string.IsNullOrWhiteSpace(test.Api))
                {
                    results[test.Name] = this.ExecuteTest(test, allVariables, results, out _);
                }
            }

            return results;
        }

        /// <summary>
        /// Excutes and also verifies the results with expected values.
        /// </summary>
        /// <param name="variables">
        /// A <see cref="IDictionary{TKey,TValue}"/> of variables.
        /// </param>
        public void ExecuteAndVerify(IDictionary<string, object> variables)
        {
            IDictionary<string, object> previousTestResults = new Dictionary<string, object>();
            IDictionary<string, object> allVariables = new Dictionary<string, object>(StringComparer.InvariantCultureIgnoreCase);
            variables.ToList().ForEach(kv => allVariables[kv.Key] = kv.Value);

            foreach (var test in this._tests)
            {
                try
                {
                    if (test.Variables != null)
                    {
                        var testVariables = this.EvaluateParameters(test.Variables, allVariables);

                        // merge these into all variables
                        testVariables.ToList().ForEach(kv => allVariables[kv.Key] = kv.Value);
                    }

                    if (!string.IsNullOrWhiteSpace(test.Api))
                    {
                        var results = this.ExecuteTest(test, allVariables, previousTestResults, out var resultsType);
                        previousTestResults["result"] = results;
                        this.VerifyResults(test, resultsType, results, allVariables);
                        if (test.Extracts != null)
                        {
                            allVariables["result"] = results;
                            var extractVariables = this.EvaluateParameters(test.Extracts, allVariables);
                            extractVariables.ToList().ForEach(kv => allVariables[kv.Key] = kv.Value);
                        }
                    }
                }
                catch (Exception e)
                {
                    var expectedObjectJson = JsonConvert.DeserializeObject<ExpectedExceptionInfo>(
                        JsonConvert.SerializeObject(test.GetExpectedResults()));
                    Console.WriteLine(expectedObjectJson);
                    if (expectedObjectJson.Exception)
                    {
                        this.VerifyException(test, expectedObjectJson, e);
                    }
                    else
                    {
                        throw;
                    }
                }
            }
        }

        /// <summary>
        /// Verifies the results of a test.
        /// </summary>
        /// <param name="test">
        /// A <see cref="TestInfo"/> of test.
        /// </param>
        /// <param name="resultsType">
        /// A <see cref="ResultsType"/> of result type.
        /// </param>
        /// <param name="results">
        /// A <see cref="IDictionary{TKey,TValue}"/> of results.
        /// </param>
        /// <param name="variables">
        /// A <see cref="IDictionary{TKey,TValue}"/> of variables.
        /// </param>
        private void VerifyResults(TestInfo test, ResultsType resultsType, IDictionary<string, object> results, IDictionary<string, object> variables)
        {
            try
            {
                if (resultsType == ResultsType.Primitive)
                {
                    var finalExpectedValue = this.EvaluateParameters(
                        new Dictionary<string, object>()
                    {
                        { "result", test.ReturnValue },
                    }, variables).First();
                    var result = results["result"];
                    this.Verify(test, () => result, finalExpectedValue.Value);
                }
                else if (resultsType == ResultsType.Void)
                {
                    // Nothing to verify.
                }
                else if (resultsType == ResultsType.Object)
                {
                    var evaluatedExpectedValues = this.EvaluateParameters(test.GetExpectedResults(), variables);
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
                        this.SendVerificationTraceInfo(test, returnObject, expectedObject);
                        returnObject.Should().BeEquivalentTo(expectedObject, test.Name);
                    }
                }
                else
                {
                    var evaluatedExpectedValues = this.EvaluateParameters(test.GetExpectedResults(true), variables);
                    if (evaluatedExpectedValues.Any())
                    {
                        Console.WriteLine("===========================");

                        // Verify dictionary.
                        results.Remove("resultType");
                        this.SendVerificationTraceInfo(test, results, evaluatedExpectedValues);
                        results.Should().BeEquivalentTo(evaluatedExpectedValues, test.Name);
                    }
                }
            }
            catch (Exception e)
            {
                var expectedObjectJson = JsonConvert.DeserializeObject<ExpectedExceptionInfo>(
                    JsonConvert.SerializeObject(test.GetExpectedResults()));
                Console.WriteLine(expectedObjectJson);
                if (expectedObjectJson.Exception)
                {
                    this.VerifyException(test, expectedObjectJson, e);
                }
                else
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Verifies exception.
        /// </summary>
        /// <param name="testInfo">
        /// A <see cref="TestInfo"/>.
        /// </param>
        /// <param name="expected">
        /// A <see cref="ExpectedExceptionInfo"/> expected exception info.
        /// </param>
        /// <param name="exception">
        /// A <see cref="Exception"/> of actual.
        /// </param>
        private void VerifyException(TestInfo testInfo, ExpectedExceptionInfo expected, Exception exception)
        {
            this.SendVerificationTraceInfo(testInfo, exception, expected);
            exception.GetType().ToString().Should().Be(expected.ExceptionType);
            exception.Message.Should().Contain(expected.ExceptionMessageLike);
        }

        /// <summary>
        /// Excute tests.
        /// </summary>
        /// <param name="test">
        /// A <see cref="TestInfo"/>.
        /// </param>
        /// <param name="variables">
        /// A <see cref="IDictionary{TKey,TValue}"/> variables.
        /// </param>
        /// <param name="previousTestResults">
        /// A <see cref="IDictionary{TKey,TValue}"/> of previous results.
        /// </param>
        /// <param name="resultsType">
        /// Results type.
        /// </param>
        /// <returns>
        /// Results dictionary.
        /// </returns>
        private IDictionary<string, object> ExecuteTest(
            TestInfo test,
            IDictionary<string, object> variables,
            IDictionary<string, object> previousTestResults,
            out ResultsType resultsType)
        {
            var allParameters = new Dictionary<string, object>();
            test.Parameters?.ToList().ForEach(kv => allParameters[kv.Key] = kv.Value);
            previousTestResults.ToList().ForEach(kv => allParameters[kv.Key] = kv.Value);
            var newParameters = this.EvaluateParameters(allParameters, variables);

            var output = this._methodProxy.Execute(test.Api, newParameters);
            var results = new Dictionary<string, object>()
            {
                { "resultType", this._methodProxy.ReturnType },
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

        /// <summary>
        /// Evaluates parameters.
        /// </summary>
        /// <param name="parameters">
        /// A <see cref="IDictionary{TKey,TValue}"/> parameters.
        /// </param>
        /// <param name="variables">
        /// A <see cref="IDictionary{TKey,TValue}"/> variables.
        /// </param>
        /// <returns>
        /// A <see cref="IDictionary{TKey,TValue}"/> evaluated with variables.
        /// </returns>
        private IDictionary<string, object> EvaluateParameters(IDictionary<string, object> parameters, IDictionary<string, object> variables)
        {
            var newParameterValues = new Dictionary<string, object>(StringComparer.InvariantCultureIgnoreCase);
            if (parameters == null)
            {
                return newParameterValues;
            }

            var allVariables = new Dictionary<string, object>(StringComparer.InvariantCultureIgnoreCase);
            variables.ToList().ForEach(kv => allVariables[kv.Key] = kv.Value);
            if (parameters.TryGetValue("result", out object result))
            {
                allVariables["result"] = result;
            }

            foreach (var parameter in parameters)
            {
                if (parameter.Value is System.String[])
                {
                    var evaluatedItems = new List<string>();
                    foreach (var val in parameter.Value as System.String[])
                    {
                        if (this.TryExpression(val, out var expression))
                        {
                            evaluatedItems.Add(this.Evaluate(expression, allVariables)?.ToString());
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
                    newParameterValues[parameter.Key] = this.ProcessJArray(array, allVariables);
                    continue;
                }

                if (parameter.Value is JObject jObj)
                {
                    newParameterValues[parameter.Key] = this.ProcessJObject(jObj, allVariables);
                }
                else
                {
                    var value = parameter.Value;
                    if (this.TryExpression(value?.ToString(), out var expression2))
                    {
                        value = this.Evaluate(expression2, allVariables);
                    }

                    newParameterValues[parameter.Key] = value;
                }
            }

            return newParameterValues;
        }

        /// <summary>
        /// Process JObject.
        /// </summary>
        /// <param name="jObject">
        /// JObject instance.
        /// </param>
        /// <param name="variables">
        /// Variables to be used.
        /// </param>
        /// <returns>
        /// Evaluated JObject instance.
        /// </returns>
        private JObject ProcessJObject(JObject jObject, IDictionary<string, object> variables)
        {
            if (jObject == null)
            {
                return null;
            }

            if (!jObject.Children().Any())
            {
                return jObject;
            }

            var newToken = new JObject();
            foreach (var childToken in jObject.Children())
            {
                if (childToken is JProperty property)
                {
                    if (this.TryExpression(property.Value?.ToString(), out var expression))
                    {
                        newToken.Add(new JProperty(property.Name, this.Evaluate(expression, variables)?.ToString()));
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

        /// <summary>
        /// Process JArray.
        /// </summary>
        /// <param name="array">
        /// JArray instance.
        /// </param>
        /// <param name="variables">
        /// Variables to be used.
        /// </param>
        /// <returns>
        /// Processed JArray with variables.
        /// </returns>
        private JArray ProcessJArray(JArray array, IDictionary<string, object> variables)
        {
            JArray newArray = new JArray();
            foreach (var token in array)
            {
                var modifiedToken = this.ProcessJToken(token, variables);
                newArray.Add(modifiedToken);
            }

            return newArray;
        }

        /// <summary>
        /// Process JToken.
        /// </summary>
        /// <param name="token">
        /// JToken instance.
        /// </param>
        /// <param name="variables">
        /// Variables to be used.
        /// </param>
        /// <returns>
        /// Processed JToken.
        /// </returns>
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
                    if (this.TryExpression(property.Value?.ToString(), out var expression))
                    {
                        newToken.Add(new JProperty(property.Name, this.Evaluate(expression, variables)?.ToString()));
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

        /// <summary>
        /// Evaluate expression.
        /// </summary>
        /// <param name="expression">
        /// Expression to be evaluated.
        /// </param>
        /// <param name="variables">
        /// Variables to be used.
        /// </param>
        /// <returns>
        /// Evaluated instance.
        /// </returns>
        private object Evaluate(string expression, IDictionary<string, object> variables)
        {
            object evaluatedValue = expression;
            var expressionInfo = Evaluator.Parse(expression);
            var evaluatedParameters = new Dictionary<string, object>(StringComparer.InvariantCultureIgnoreCase);
            if (expressionInfo.MethodData != null)
            {
                foreach (var arg in expressionInfo.MethodData.Arguments)
                {
                    evaluatedParameters[arg.Name] = this.EvaluateValue(arg, variables);
                }

                if (variables.TryGetValue("result", out object result))
                {
                    evaluatedParameters["result"] = result;
                }

                evaluatedValue = this._methodProxy.Execute(expressionInfo.MethodData.Name, evaluatedParameters);
            }
            else if (expressionInfo.Variable != null)
            {
                evaluatedValue = this.EvaluateValue(expressionInfo.Variable, variables);
            }

            return evaluatedValue;
        }

        /// <summary>
        /// Evaluate value.
        /// </summary>
        /// <param name="variable">
        /// A Variable instance.
        /// </param>
        /// <param name="variables">
        /// Variables to be used.
        /// </param>
        /// <returns>
        /// Evaluated variable instance.
        /// </returns>
        private object EvaluateValue(Variable variable, IDictionary<string, object> variables)
        {
            if (variables.TryGetValue(variable.Name, out var variableValue))
            {
                return variableValue;
            }

            return null;
        }

        /// <summary>
        /// Evaluate argument value.
        /// </summary>
        /// <param name="arg">
        /// A <see cref="Argument"/> instance.
        /// </param>
        /// <param name="variables">
        /// Variables to be used.
        /// </param>
        /// <returns>
        /// Evaluated value.
        /// </returns>
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

            var variableName = arg.Val;
            if (variables.TryGetValue(variableName, out var variableValue))
            {
                return variableValue;
            }

            return null;
        }

        /// <summary>
        /// Verifies the test.
        /// </summary>
        /// <typeparam name="T">
        /// Type name.
        /// </typeparam>
        /// <param name="test">
        /// A <see cref="TestInfo"/> instance.
        /// </param>
        /// <param name="selectorExpression">
        /// A <see cref="Func{TResult}"/> expression.
        /// </param>
        /// <param name="expectedValue">
        /// Expected value.
        /// </param>
        private void Verify<T>(TestInfo test, Expression<Func<T>> selectorExpression, object expectedValue)
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

            this.SendVerificationTraceInfo(test, actual, expectedValue);
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

        /// <summary>
        /// Gets expression if given value is a proper expression.
        /// </summary>
        /// <param name="val">
        /// Value to be parsed for expression.
        /// </param>
        /// <param name="expression">
        /// Expression.
        /// </param>
        /// <returns>
        /// true if it is expression.
        /// </returns>
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

        /// <summary>
        /// Send verification trace info.
        /// </summary>
        /// <param name="testInfo">
        /// A <see cref="TestInfo"/> instance.
        /// </param>
        /// <param name="actual">
        /// Actual value.
        /// </param>
        /// <param name="expected">
        /// Expected value.
        /// </param>
        private void SendVerificationTraceInfo(TestInfo testInfo, object actual, object expected)
        {
            this._traceAction(new ExecuteTraceInfo(TraceType.Verification)
            {
                Actual = actual,
                TestInfo = testInfo,
                Expected = expected,
            });
        }
    }
}
