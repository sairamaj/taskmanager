using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using Utils.Core.Expressions;

namespace Utils.Core.Tests
{
    [TestFixture(Description = "Expression evaluator test cases")]
    public class ExpressionEvaluatorTest
    {
        [Test(Description = "Method with no arguments")]
        public void MethodWithNoArguments()
        {
            var methodInfo = Evaluator.Parse("random()");
            methodInfo.Should().NotBeNull();
            methodInfo.Name.Should().Be("random");
            methodInfo.Arguments.Count().Should().Be(0);
        }

        [Test(Description = "Method with multiple arguments")]
        public void MethodWithMultipleArguments()
        {
            var methodInfo = Evaluator.Parse("add(10,20)");
            methodInfo.Should().NotBeNull();
            methodInfo.Name.Should().Be("add");
            methodInfo.Arguments.Should().BeEquivalentTo(new Argument("arg1","10",1){IsVariable = false}, new Argument("arg2","20",2){IsVariable = false});
        }

        [Test(Description = "Method with named arguments")]
        public void MethodWithNamedArguments()
        {
            var methodInfo = Evaluator.Parse("add(num1=10,num2=20)");
            methodInfo.Should().NotBeNull();
            methodInfo.Name.Should().Be("add");
            methodInfo.Arguments.Should().BeEquivalentTo(new Argument("num1","10",1){IsVariable = false}, new Argument("num2","20",2){IsVariable = false});
        }

        [Test(Description = "Method with variables in arguments")]
        public void MethodWithVariablesArguments()
        {
            var methodInfo = Evaluator.Parse("add(num1=var.mynum1,num2=var.mynum2)");
            methodInfo.Should().NotBeNull();
            methodInfo.Name.Should().Be("add");
            methodInfo.Arguments.Should().BeEquivalentTo(new Argument("num1","mynum1",1){IsVariable = true}, new Argument("num2","mynum2",2){IsVariable = true});
        }
    }
}
