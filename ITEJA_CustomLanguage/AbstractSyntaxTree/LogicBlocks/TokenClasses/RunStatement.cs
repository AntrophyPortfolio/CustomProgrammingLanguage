using ITEJA_CustomLanguage.AbstractSyntaxTree.LogicBlocks.TokenClasses.BodyStatements;
using ITEJA_CustomLanguage.AbstractSyntaxTree.LogicBlocks.TokenClasses.Variables;
using ITEJA_CustomLanguage.Lexer;
using System;
using System.Collections.Generic;
using System.Text;

namespace ITEJA_CustomLanguage.AbstractSyntaxTree.LogicBlocks.TokenClasses
{
    class RunStatement : IRunStatement
    {
        public StringBuilder Name { get; set; } = new StringBuilder();
        public IList<IVariable> TokenParametersList { get; set; } = new List<IVariable>();
        public IBodyStatement Parent { get; set; }

        public void Execute()
        {
            foreach (var method in MainClass.Methods)
            {
                if (method.Name.Equals(Name))
                {
                    method.AssignParameters(TokenParametersList);
                    method.Execute();
                }
            }
        }
    }
}
