using ITEJA_CustomLanguage.AbstractSyntaxTree.LogicBlocks.TokenClasses.Variables;
using ITEJA_CustomLanguage.Lexer;
using System.Collections.Generic;
using System.Text;

namespace ITEJA_CustomLanguage.AbstractSyntaxTree.LogicBlocks.TokenClasses.BodyStatements
{
    public interface IForCycleStatement : IBodyStatement
    {
        IIntegerVariable InnerCounterVariable { get; set; }
        IIntegerVariable MaximumAllowedCounter { get; set; }
        IList<Token> ComparisonOperators { get; set; }
        bool IsIncremental { get; set; }
    }
}
