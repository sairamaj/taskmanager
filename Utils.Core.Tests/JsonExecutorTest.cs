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
        [Test(Description = "successful test case.")]
        public void SuccessfulJsonExecutorTest()
        {
            var tester = new JsonExecutor(ReadTestFile("MathAdd.json"), ReadTestFile("config.json"));
            tester.ExecuteAndVerify(new Dictionary<string, string>(){});
        }

        private string ReadTestFile(string fileName)
        {
            return 
                File.ReadAllText(Path.Combine(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestFiles"), fileName));
        }
    }
}
