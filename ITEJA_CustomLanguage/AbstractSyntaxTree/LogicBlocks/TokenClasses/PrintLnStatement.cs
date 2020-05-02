using ITEJA_CustomLanguage.AbstractSyntaxTree.LogicBlocks.TokenClasses.BodyStatements;
using ITEJA_CustomLanguage.AbstractSyntaxTree.LogicBlocks.TokenClasses.Variables;
using System;

namespace ITEJA_CustomLanguage.AbstractSyntaxTree.LogicBlocks.TokenClasses
{
    public class PrintLnStatement : IStatement
    {
        public IVariable Variable { get; set; }
        public IBodyStatement Parent { get; set; }

        public void Execute()
        {
            Console.WriteLine(Variable.Value);
        }
    }
}
