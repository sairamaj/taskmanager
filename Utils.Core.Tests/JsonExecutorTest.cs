using System;
using System.Collections.Generic;
using System.IO;
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
            var tester = new JsonExecutor(ReadTestFile("Math.json"), ReadTestFile("config.json"));
            tester.ExecuteAndVerify(new Dictionary<string, object>(){});
        }

        [Test(Description = "Methods can return dictionary and expected values are matched against to it.")]
        public void TestWithMethodReturnDictionary()
        {
            var tester = new JsonExecutor(ReadTestFile("MethodReturnDictionary.json"), ReadTestFile("config.json"));
            tester.ExecuteAndVerify(new Dictionary<string, object>() { });
        }

        [Test(Description = "Methods inputs can be another function.")]
        public void TestWithMethodInputAsFunctions()
        {
            var tester = new JsonExecutor(ReadTestFile("MethodInputAsFunctoid.json"), ReadTestFile("config.json"));
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
            var tester = new JsonExecutor(ReadTestFile("MethodInputAsFunctoidWithArgs.json"), ReadTestFile("config.json"));
            tester.ExecuteAndVerify(new Dictionary<string, object>() { });
        }

        [Test(Description = "Methods inputs can be another function with variables in arguments.")]
        public void TestWithMethodInputAsFunctionsHavingVariablesInArguments()
        {
            var tester = new JsonExecutor(ReadTestFile("MethodInputAsFunctoidWithVariablesInArgs.json"), ReadTestFile("config.json"));
            tester.ExecuteAndVerify(new Dictionary<string, object>()
            {
                { "mynum1", 101},
                { "mynum2", 202}
            });
        }

        [Test(Description = "Parameters having variables")]
        public void TestWithParametersWithVariables()
        {
            var tester = new JsonExecutor(ReadTestFile("MathWithVariableInArguments.json"), ReadTestFile("config.json"));
            tester.ExecuteAndVerify(new Dictionary<string, object>()
            {
                { "mynum1", 99}
            });
        }
        private string ReadTestFile(string fileName)
        {
            return 
                File.ReadAllText(Path.Combine(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestFiles"), fileName));
        }
    }
}
