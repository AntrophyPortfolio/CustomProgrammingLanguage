using ITEJA_CustomLanguage.Lexer;
using System.Collections.Generic;

namespace ITEJA_CustomLanguage.AbstractSyntaxTree.LogicBlocks.TokenClasses.BodyStatements
{
    public interface IConditionIfStatement : IBodyStatement
    {
        IConditionIfStatement ElseCondition { get; set; }
        Stack<Token> ExpressionTokens { get; set; }
    }
}
