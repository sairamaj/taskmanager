using System;
using System.Collections.Generic;
using System.IO;
using FluentAssertions;
using NUnit.Framework;
using Utils.Core.Test;

namespace Utils.Core.Tests
{
    [TestFixture]
    public class JsonExecutorTest
    {
        [Test(Description = "Simple methods which just take parameters and return value.")]
        public void TestWithMethodReturnResults()
        {
            var tester = new JsonExecutor(ReadTestFile("Math.json"), ReadTestFile("config.json"), msg => { });
            tester.ExecuteAndVerify(new Dictionary<string, object>(){});
        }

        [Test(Description = "Methods can return dictionary and expected values are matched against to it.")]
        public void TestWithMethodReturnDictionary()
        {
            var tester = new JsonExecutor(ReadTestFile("MethodReturnDictionary.json"), ReadTestFile("config.json"), msg => { });
            tester.ExecuteAndVerify(new Dictionary<string, object>() { });
        }

        [Test(Description = "Methods inputs can be another function.")]
        public void TestWithMethodInputAsFunctions()
        {
            var tester = new JsonExecutor(ReadTestFile("MethodInputAsFunctoid.json"), ReadTestFile("config.json"), msg => { });
            var results =  tester.Execute(new Dictionary<string, object>() { });
            Console.WriteLine("==========================");
            foreach (var kv in results)
            {
                Console.WriteLine($"{kv.Key}  {kv.Value}");
            }
            Console.WriteLine("==========================");
        }

        [Test(Description = "Methods inputs can be another function.")]
        public void TestWithMethodInputAsFunctionsHavingArguments()
        {
            var tester = new JsonExecutor(ReadTestFile("MethodInputAsFunctoidWithArgs.json"), ReadTestFile("config.json"), msg => { });
            tester.ExecuteAndVerify(new Dictionary<string, object>() { });
        }

        [Test(Description = "Methods inputs can be another function with variables in arguments.")]
        public void TestWithMethodInputAsFunctionsHavingVariablesInArguments()
        {
            var tester = new JsonExecutor(ReadTestFile("MethodInputAsFunctoidWithVariablesInArgs.json"), ReadTestFile("config.json"), msg => { });
            tester.ExecuteAndVerify(new Dictionary<string, object>()
            {
                { "mynum1", 101},
                { "mynum2", 202}
            });
        }

        [Test(Description = "Parameters having variables")]
        public void TestWithParametersWithVariables()
        {
            var tester = new JsonExecutor(ReadTestFile("MathWithVariableInArguments.json"), ReadTestFile("config.json"), msg => { });
            tester.ExecuteAndVerify(new Dictionary<string, object>()
            {
                { "mynum1", 99}
            });
        }

        [Test(Description = "Functoids in Expected values.")]
        public void FunctoidsInExpected()
        {
            var tester = new JsonExecutor(ReadTestFile("MathFuctionsInExpected.json"), ReadTestFile("config.json"), msg => { });
            tester.ExecuteAndVerify(new Dictionary<string, object>() { });
        }

        [Test(Description = "Functoids in Expected values which return dictionary.")]
        public void FunctoidsInExpectedWhichReturnsDictionary()
        {
            var tester = new JsonExecutor(ReadTestFile("MathFuctionsInExpected.json"), ReadTestFile("config.json"), msg => { });
            tester.ExecuteAndVerify(new Dictionary<string, object>() { });
        }

        [Test(Description = "Methods can return dictionary and expected values having functoids.")]
        public void TestWithMethodReturnDictionaryHavingExpectedFunctiods()
        {
            var tester = new JsonExecutor(ReadTestFile("MethodReturnDictionaryWithFunctoidsInExpected.json"), ReadTestFile("config.json"), msg => { });
            tester.ExecuteAndVerify(new Dictionary<string, object>() { });
        }

        [Test(Description = "Methods can return string array in dictionary.")]
        public void TestWithMethodReturnStringArrayInDictionary()
        {
            var tester = new JsonExecutor(ReadTestFile("MethodReturnStringArrayDictionary.json"), ReadTestFile("config.json"), msg => { });
            tester.ExecuteAndVerify(new Dictionary<string, object>() { });
        }

        [Test(Description = "Methods can return string array in dictionary.")]
        public void TestWithMethodReturnStringArrayWithFunctoidsInDictionary()
        {
            var tester = new JsonExecutor(ReadTestFile("MethodReturnStringArrayDictionaryWithFunctoids.json"), ReadTestFile("config.json"), msg => { });
            tester.ExecuteAndVerify(new Dictionary<string, object>() { });
        }

        [Test(Description = "Functoids in Expected values.")]
        public void MathFunctionFaileShouldThrowException()
        {
            var tester = new JsonExecutor(ReadTestFile("ExpectedDidNotMatch.json"), ReadTestFile("config.json"), msg => { });
            Action verify = () => tester.ExecuteAndVerify(new Dictionary<string, object>() { });
            verify.Should().Throw<AssertionException>().WithMessage("Expected actual to be 130L because Math.Add result did fail, but found 30.");
        }

        [Test(Description = "Method taking IEnumerable<int>")]
        public void WithEnumOfInts()
        {
            var tester = new JsonExecutor(ReadTestFile("MethodTakingArrayOfInt.json"), ReadTestFile("config.json"), msg => { });
            tester.ExecuteAndVerify(new Dictionary<string, object>() { });
        }

        [Test(Description = "Method taking Guid")]
        public void WithGuidAsInput()
        {
            var tester = new JsonExecutor(ReadTestFile("MethodTakingGuid.json"), ReadTestFile("config.json"), msg => { });
            tester.ExecuteAndVerify(new Dictionary<string, object>() { });
        }

        [Test(Description = "Method IEnumerable<Person>")]
        public void WithTypeCollections()
        {
            var tester = new JsonExecutor(ReadTestFile("MethodTakingPersonCollections.json"), ReadTestFile("config.json"), msg => { });
            tester.ExecuteAndVerify(new Dictionary<string, object>() { });
        }

        [Test(Description = "Method returning IEnumerable<Person>")]
        public void WithTypeReturningCollections()
        {
            var tester = new JsonExecutor(ReadTestFile("MethodReturningPersonCollections.json"), ReadTestFile("config.json"), msg => { });
            tester.ExecuteAndVerify(new Dictionary<string, object>() { });
        }

        [Test(Description = "Method returning IEnumerable<Person> Having Functions")]
        public void WithTypeReturningCollectionsHavingFunctions()
        {
            var tester = new JsonExecutor(ReadTestFile("MethodReturningPersonCollectionsHavingFunctions.json"), ReadTestFile("config.json"), msg => { });
            tester.ExecuteAndVerify(new Dictionary<string, object>() { });
        }

        [Test(Description = "Method returning null but test case expecting non null")]
        public void WithMethodReturnNullButTestCaseExpectingNonNull()
        {
            var tester = new JsonExecutor(ReadTestFile("MethodReturningNullExpectingNonNull.json"), ReadTestFile("config.json"), msg => { });
            Action expectationsFailed = () => tester.ExecuteAndVerify(new Dictionary<string, object>() { });
            expectationsFailed.Should().Throw<AssertionException>();
        }

        [Test(Description = "Exception validation.")]
        public void ExceptionTest()
        {
            var tester = new JsonExecutor(ReadTestFile("ExpectedExceptionTest.json"), ReadTestFile("config.json"), msg => { });
            tester.ExecuteAndVerify(new Dictionary<string, object>() { });
        }

        [Test(Description = "Partial array validation.")]
        [Ignore("This partial validation is not done yet.")]
        public void ForPartialArrayValidation()
        {
            var tester = new JsonExecutor(ReadTestFile("MethodReturnStringArrayDictionaryContainValidation.json"), ReadTestFile("config.json"), msg => { });
            tester.ExecuteAndVerify(new Dictionary<string, object>() { });
        }

        [Test(Description = "Variables extract.")]
        public void ExtractPlainStringReturnMethodInToVariableAndUseInNextTest()
        {
            var tester = new JsonExecutor(ReadTestFile("ExtractStrigToVariable.json"), ReadTestFile("config.json"), msg => { });
            tester.ExecuteAndVerify(new Dictionary<string, object>() { });
        }

        [Test(Description = "Methods taking dictionary.")]
        public void MethodTakingDictionary()
        {
            var tester = new JsonExecutor(ReadTestFile("MethodTakingDictionary.json"), ReadTestFile("config.json"), msg => { });
            tester.ExecuteAndVerify(new Dictionary<string, object>() { });
        }

        [Test(Description = "Methods taking dictionary with variables.")]
        public void MethodTakingDictionaryWithVariables()
        {
            var tester = new JsonExecutor(ReadTestFile("MethodTakingDictionaryWithVariables.json"), ReadTestFile("config.json"), msg => { });
            tester.ExecuteAndVerify(new Dictionary<string, object>()
            {
                {"item1" , "val1" },
                {"item2" , "val2" }
            });
        }

        [Test(Description = "BuiltinType Random method.")]
        [Ignore("Need to enable this.")]
        public void BuiltinTypeRandomMethod()
        {
            var tester = new JsonExecutor(ReadTestFile("BuiltinTypeRandomMethod.json"), ReadTestFile("config.json"), msg => { });
            tester.ExecuteAndVerify(new Dictionary<string, object>() { });
        }

        [Test(Description = "DefineVariable.")]
        public void DefineVariable()
        {
            var tester = new JsonExecutor(ReadTestFile("DefineVariable.json"), ReadTestFile("config.json"), msg => { });
            tester.ExecuteAndVerify(new Dictionary<string, object>() { });
        }

        [Test(Description = "DefineVariableWithFunctions.")]
        public void DefineVariableWithFunctions()
        {
            var tester = new JsonExecutor(ReadTestFile("DefineVariableWithFunctions.json"), ReadTestFile("config.json"), msg => { });
            tester.ExecuteAndVerify(new Dictionary<string, object>() { });
        }

        [Test(Description = "ExtractStringToVariableUsingExtracts.")]
        public void ExtractStringToVariableUsingExtracts()
        {
            var tester = new JsonExecutor(ReadTestFile("ExtractStringToVariableUsingExtracts.json"), ReadTestFile("config.json"), msg => { });
            tester.ExecuteAndVerify(new Dictionary<string, object>() { });
        }
        
        private string ReadTestFile(string fileName)
        {
            return 
                File.ReadAllText(Path.Combine(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestFiles"), fileName));
        }
    }
}
