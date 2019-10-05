using Microsoft.CodeAnalysis.CSharp;

namespace Utils.Core.Expressions
{
    public class Evaluator
    {
        public static ExpressionInfo Parse(string expression)
        {
            var syntax = SyntaxFactory.ParseExpression(expression);
            var walker = new MethodExtractWalker(expression);
            walker.Visit(syntax);
            return new ExpressionInfo
            {
                MethodData = walker.Method,
                Variable = walker.Variable
            };
        }
    }
}
