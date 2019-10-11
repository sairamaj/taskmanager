using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Utils.Core.Expressions
{
    class MethodExtractWalker : CSharpSyntaxWalker
    {
        private readonly string _expression;
        private bool _methodInvocationVisited;
        private bool _methodNameExtracted;
        private MethodData _methodData;
        private Variable _variable;
        private int _currentArgumentCount;


        public MethodExtractWalker(string expression)
        {
            _expression = expression;
        }

        public MethodData Method => _methodData;
        public Variable Variable => _variable;

        public override void Visit(SyntaxNode node)
        {
            Console.WriteLine($"node:{node.GetType()} {node.GetText()}");
            base.Visit(node);
        }

        public override void VisitInvocationExpression(InvocationExpressionSyntax node)
        {
            this._methodInvocationVisited = true;
            Console.WriteLine(node.GetText().ToString());
            base.VisitInvocationExpression(node);
        }

        public override void VisitIdentifierName(IdentifierNameSyntax node)
        {
            if (this._methodInvocationVisited && !_methodNameExtracted)
            {
                _methodData = new MethodData(node.GetText().ToString());
                _methodNameExtracted = true;
            }
            

            base.VisitIdentifierName(node);
        }

        public override void VisitArgument(ArgumentSyntax node)
        {
            if (_methodData == null)
            {
                throw new InvalidSyntaxException($"Method name not found in {this._expression}");
            }

            _currentArgumentCount++;
            var val = node.GetText()?.ToString();
            var parts = val.Split('=');
            var argName = string.Empty;
            var argData = string.Empty;
            if (parts.Length > 1)
            {
                argName = parts[0];
                argData = parts[1];
            }
            else
            {
                argName = $"arg{_currentArgumentCount}";
                argData = val;
            }

            _methodData.AddParameter(argName, argData, _currentArgumentCount);
            base.VisitArgument(node);
        }

        public override void VisitMemberAccessExpression(MemberAccessExpressionSyntax node)
        {
            if (_methodData == null)
            {
                var text = node.GetText()?.ToString();
                if (text.StartsWith("var.", StringComparison.CurrentCultureIgnoreCase))
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
