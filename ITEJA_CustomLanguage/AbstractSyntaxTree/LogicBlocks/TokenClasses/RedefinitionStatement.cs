using ITEJA_CustomLanguage.AbstractSyntaxTree.LogicBlocks.TokenClasses.BodyStatements;
using ITEJA_CustomLanguage.AbstractSyntaxTree.LogicBlocks.TokenClasses.Variables;
using ITEJA_CustomLanguage.Lexer;
using System.Linq;
using System.Collections.Generic;
using System;

namespace ITEJA_CustomLanguage.AbstractSyntaxTree.LogicBlocks.TokenClasses
{
    class RedefinitionStatement : IRedefinitionStatement
    {
        public IBodyStatement Parent { get; set; }
        public Token IdentifierToken { get; set; }
        public Stack<Token> TokensExpression { get; set; } = new Stack<Token>();

        public void Execute()
        {
            IVariable variable = MainClass.FindIdentifier(Parent, IdentifierToken.Value);
            if (variable is IIntegerVariable integerVariable)
            {
                if (TokensExpression.Any(x=>x.Type == TokenType.StringCharacters))
                {
                    throw new InvalidOperationException("String cannot be put into integer datatype!");
                }
                integerVariable.TokensExpression = MainClass.ReplaceIdentifiersInExpression(TokensExpression, Parent);
                integerVariable.Calculate();
            }
            else if (variable is IStringVariable stringVariable)
            {
                if (TokensExpression.Any(x=>x.Type == TokenType.NumberCharacters))
                {
                    throw new InvalidOperationException("Integer cannot be put into string datatype!");
                }
                UpdateStringVariableDefinition(stringVariable);
            }
        }

        private void UpdateStringVariableDefinition(IStringVariable stringVariable)
        {
            stringVariable.Value.Clear();
            var replacedExpressions = MainClass.ReplaceIdentifiersInExpression(TokensExpression, Parent);
            foreach (var str in replacedExpressions)
            {
                if (str.Type != TokenType.Plus)
                {
                    stringVariable.Value.Append(str.Value);
                }
            }
        }
    }
}
