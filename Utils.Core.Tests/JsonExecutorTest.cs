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
            tester.ExecuteAndVerify(new Dictionary<string, string>(){});
        }

        [Test(Description = "Methods can return dictionary and expected values are matched against to it.")]
        public void TestWithMethodReturnDictionary()
        {
            var tester = new JsonExecutor(ReadTestFile("MethodReturnDictionary.json"), ReadTestFile("config.json"));
            tester.ExecuteAndVerify(new Dictionary<string, string>() { });
        }

        [Test(Description = "Methods inputs can be another function.")]
        public void TestWithMethodInputAsFunctions()
        {
            var tester = new JsonExecutor(ReadTestFile("MethodInputAsFunctoid.json"), ReadTestFile("config.json"));
            var results =  tester.Execute(new Dictionary<string, string>() { });
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
            tester.ExecuteAndVerify(new Dictionary<string, string>() { });
        }

        private string ReadTestFile(string fileName)
        {
            return 
                File.ReadAllText(Path.Combine(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestFiles"), fileName));
        }
    }
}
