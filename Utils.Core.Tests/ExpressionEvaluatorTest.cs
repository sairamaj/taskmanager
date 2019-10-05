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
            var expressionInfo = Evaluator.Parse("random()");
            expressionInfo.MethodData.Should().NotBeNull();
            expressionInfo.MethodData.Name.Should().Be("random");
            expressionInfo.MethodData.Arguments.Count().Should().Be(0);
        }

        [Test(Description = "Method with multiple arguments")]
        public void MethodWithMultipleArguments()
        {
            var expressionInfo = Evaluator.Parse("add(10,20)");
            expressionInfo.MethodData.Should().NotBeNull();
            expressionInfo.MethodData.Name.Should().Be("add");
            expressionInfo.MethodData.Arguments.Should().BeEquivalentTo(new Argument("arg1","10",1){IsVariable = false}, new Argument("arg2","20",2){IsVariable = false});
        }

        [Test(Description = "Method with named arguments")]
        public void MethodWithNamedArguments()
        {
            var expressionInfo = Evaluator.Parse("add(num1=10,num2=20)");
            expressionInfo.MethodData.Should().NotBeNull();
            expressionInfo.MethodData.Name.Should().Be("add");
            expressionInfo.MethodData.Arguments.Should().BeEquivalentTo(new Argument("num1","10",1){IsVariable = false}, new Argument("num2","20",2){IsVariable = false});
        }

        [Test(Description = "Method with variables in arguments")]
        public void MethodWithVariablesArguments()
        {
            var expressionInfo = Evaluator.Parse("add(num1=var.mynum1,num2=var.mynum2)");
            expressionInfo.MethodData.Should().NotBeNull();
            expressionInfo.MethodData.Name.Should().Be("add");
            expressionInfo.MethodData.Arguments.Should().BeEquivalentTo(new Argument("num1","mynum1",1){IsVariable = true}, new Argument("num2","mynum2",2){IsVariable = true});
        }

        [Test(Description = "Variable declaration")]
        public void VariableDeclaration()
        {
            var expressionInfo = Evaluator.Parse("var.mynum1");
            expressionInfo.Variable.Should().NotBeNull();
            expressionInfo.Variable.Name.Should().Be("mynum1");
        }
    }
}
