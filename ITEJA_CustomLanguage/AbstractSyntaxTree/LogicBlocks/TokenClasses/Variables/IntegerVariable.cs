using ITEJA_CustomLanguage.AbstractSyntaxTree.LogicBlocks.TokenClasses.Variables.CalculateInteger;
using ITEJA_CustomLanguage.Lexer;
using System.Collections.Generic;
using System.Text;

namespace ITEJA_CustomLanguage.AbstractSyntaxTree.LogicBlocks.TokenClasses.Variables
{
    public class IntegerVariable : IIntegerVariable
    {
        public StringBuilder Value { get; set; } = new StringBuilder();
        public StringBuilder Name { get; set; } = new StringBuilder();
        public Stack<Token> TokensExpression { get; set; } = new Stack<Token>();

        public void Calculate()
        {
            if (TokensExpression.Count != 0)
            {
                Expression expr = new Expression(TokensExpression);
                Value.Clear();
                Value.Append(expr.Calculate().ToString());
            }
        }
    }
}
