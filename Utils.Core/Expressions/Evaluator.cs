using Microsoft.CodeAnalysis.CSharp;

namespace Utils.Core.Expressions
{
    public class Evaluator
    {
        public static MethodData Parse(string expression)
        {
            var syntax = SyntaxFactory.ParseExpression(expression);
            var walker = new MethodExtractWalker(expression);
            walker.Visit(syntax);
            return walker.Method;
        }
    }
}
