using ITEJA_CustomLanguage.AbstractSyntaxTree.LogicBlocks.TokenClasses.Variables;
using System.Collections.Generic;

namespace ITEJA_CustomLanguage.AbstractSyntaxTree.LogicBlocks.TokenClasses.BodyStatements
{
    public interface IBodyStatement : IStatement
    {
        /// <summary>
        /// List of all statements that this body statement has
        /// </summary>
        IList<IStatement> Statements { get; }
        /// <summary>
        /// List of all variables that this body statement has
        /// </summary>
        IList<IVariable> LocalVariables { get; }
    }
}
