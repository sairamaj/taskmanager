using Microsoft.CodeAnalysis.CSharp;

namespace Utils.Core.Expressions
{
    /// <summary>
    /// Expression evaluator.
    /// </summary>
    public class Evaluator
    {
        /// <summary>
        /// Parse the expression.
        /// </summary>
        /// <param name="expression">
        /// Expression to be parsed.
        /// </param>
        /// <returns>
        /// A <see cref="ExpressionInfo"/> instance.
        /// </returns>
        public static ExpressionInfo Parse(string expression)
        {
            var syntax = SyntaxFactory.ParseExpression(expression);
            var walker = new MethodExtractWalker(expression);
            walker.Visit(syntax);
            return new ExpressionInfo
            {
                MethodData = walker.Method,
                Variable = walker.Variable,
            };
        }
    }
}
