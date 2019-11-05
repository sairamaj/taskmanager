using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Utils.Core.Expressions
{
    /// <summary>
    /// Method extract walker.
    /// </summary>
    internal class MethodExtractWalker : CSharpSyntaxWalker
    {
        /// <summary>
        /// Expression.
        /// </summary>
        private readonly string _expression;

        /// <summary>
        /// method invocation visited flag.
        /// </summary>
        private bool _methodInvocationVisited;

        /// <summary>
        /// method name extracted flag.
        /// </summary>
        private bool _methodNameExtracted;

        /// <summary>
        ///  method data.
        /// </summary>
        private MethodData _methodData;

        /// <summary>
        /// variable.
        /// </summary>
        private Variable _variable;

        /// <summary>
        /// Current argument count.
        /// </summary>
        private int _currentArgumentCount;

        /// <summary>
        /// Initializes a new instance of the <see cref="MethodExtractWalker"/> class.
        /// </summary>
        /// <param name="expression">
        /// Expression from which method or variable is extracted.
        /// </param>
        public MethodExtractWalker(string expression)
        {
            this._expression = expression;
        }

        /// <summary>
        /// Gets method data.
        /// </summary>
        public MethodData Method => this._methodData;

        /// <summary>
        /// Gets variable data.
        /// </summary>
        public Variable Variable => this._variable;

        /// <summary>
        /// Visits syntax node.
        /// </summary>
        /// <param name="node">
        /// A <see cref="SyntaxNode"/> instance.
        /// </param>
        public override void Visit(SyntaxNode node)
        {
            Console.WriteLine($"node:{node.GetType()} {node.GetText()}");
            base.Visit(node);
        }

        /// <summary>
        /// Visits invocation expression.
        /// </summary>
        /// <param name="node">
        /// A <see cref="InvocationExpressionSyntax"/> instance.
        /// </param>
        public override void VisitInvocationExpression(InvocationExpressionSyntax node)
        {
            this._methodInvocationVisited = true;
            Console.WriteLine(node.GetText().ToString());
            base.VisitInvocationExpression(node);
        }

        /// <summary>
        /// Visits identifier name.
        /// </summary>
        /// <param name="node">
        /// A <see cref="IdentifierNameSyntax"/> instance.
        /// </param>
        public override void VisitIdentifierName(IdentifierNameSyntax node)
        {
            if (this._methodInvocationVisited && !this._methodNameExtracted)
            {
                this._methodData = new MethodData(node.GetText().ToString());
                this._methodNameExtracted = true;
            }

            base.VisitIdentifierName(node);
        }

        /// <summary>
        /// Visits arguments.
        /// </summary>
        /// <param name="node">
        /// A <see cref="ArgumentSyntax"/> instance.
        /// </param>
        public override void VisitArgument(ArgumentSyntax node)
        {
            if (this._methodData == null)
            {
                throw new InvalidSyntaxException($"Method name not found in {this._expression}");
            }

            this._currentArgumentCount++;
            var val = node.GetText()?.ToString();
            var parts = val?.Split('=');
            var argName = string.Empty;
            var argData = string.Empty;
            if (parts.Length > 1)
            {
                argName = parts[0];
                argData = parts[1];
            }
            else
            {
                argName = $"arg{this._currentArgumentCount}";
                argData = val;
            }

            this._methodData.AddParameter(argName, argData, this._currentArgumentCount);
            base.VisitArgument(node);
        }

        /// <summary>
        /// Visits member access expression.
        /// </summary>
        /// <param name="node">
        /// A <see cref="MemberAccessExpressionSyntax"/> instance.
        /// </param>
        public override void VisitMemberAccessExpression(MemberAccessExpressionSyntax node)
        {
            if (this._methodData == null)
            {
                var text = node.GetText()?.ToString();
                if (text != null && text.StartsWith("var.", StringComparison.CurrentCultureIgnoreCase))
                {
                    // must be variable
                    this._variable = new Variable(text);
                }
                else
                {
                    this._methodData = new MethodData(node.GetText().ToString());
                    this._methodNameExtracted = true;
                    this._methodInvocationVisited = true;
                }
            }

            base.VisitMemberAccessExpression(node);
        }
    }
}
