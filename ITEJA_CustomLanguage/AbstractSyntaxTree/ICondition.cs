using ITEJA_CustomLanguage.AbstractSyntaxTree.LogicBlocks.TokenClasses.BodyStatements;
using ITEJA_CustomLanguage.Lexer;
using System.Collections.Generic;

namespace ITEJA_CustomLanguage.AbstractSyntaxTree
{
    public interface ICondition
    {
        bool IsConditionTrue(Stack<Token> parExpressionTokens, IBodyStatement parent);
    }
}
