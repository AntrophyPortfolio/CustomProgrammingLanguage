using ITEJA_CustomLanguage.Lexer;
using System.Collections.Generic;

namespace ITEJA_CustomLanguage.AbstractSyntaxTree.LogicBlocks.TokenClasses.Variables
{
    public interface IIntegerVariable : IVariable
    {
        Stack<Token> TokensExpression { get; set;  }
        /// <summary>
        /// The method calculates the expression and saves the value to property
        /// </summary>
        void Calculate();
    }
}
