using ITEJA_CustomLanguage.Lexer;
using System.Collections.Generic;

namespace ITEJA_CustomLanguage.AbstractSyntaxTree.LogicBlocks.TokenClasses
{
    public interface IRedefinitionStatement : IStatement
    {
        public Token IdentifierToken { get; set; }
        public Stack<Token> TokensExpression { get; set; }
    }
}
