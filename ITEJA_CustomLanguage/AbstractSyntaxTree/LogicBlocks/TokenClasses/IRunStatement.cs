using ITEJA_CustomLanguage.AbstractSyntaxTree.LogicBlocks.TokenClasses.Variables;
using System.Collections.Generic;
using System.Text;

namespace ITEJA_CustomLanguage.AbstractSyntaxTree.LogicBlocks.TokenClasses
{
    public interface IRunStatement : IStatement
    {
        StringBuilder Name { get; set; }
        IList<IVariable> TokenParametersList { get; set; }
    }
}