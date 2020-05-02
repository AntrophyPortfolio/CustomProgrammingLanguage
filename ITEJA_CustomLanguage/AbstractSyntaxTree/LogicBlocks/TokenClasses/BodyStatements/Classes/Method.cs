using ITEJA_CustomLanguage.AbstractSyntaxTree.LogicBlocks.TokenClasses.BodyStatements;
using ITEJA_CustomLanguage.AbstractSyntaxTree.LogicBlocks.TokenClasses.Variables;
using System;
using System.Collections.Generic;
using System.Text;

namespace ITEJA_CustomLanguage.AbstractSyntaxTree.LogicBlocks.TokenClasses.BodyStatements.Classes
{
    public class Method : IMethod
    {
        public StringBuilder Name { get; set; } = new StringBuilder();

        public IList<IStatement> Statements { get; } = new List<IStatement>();

        public IList<IVariable> LocalVariables { get; } = new List<IVariable>();

        public IBodyStatement Parent { get; set; }
        public IList<IVariable> ParametersList { get; set; } = new List<IVariable>();

        public void Execute()
        {
            foreach (var statement in Statements)
            {
                statement.Execute();
            }
        }

        public void AssignParameters(IList<IVariable> variablesList)
        {
            if (variablesList.Count != ParametersList.Count)
            {
                throw new ArgumentException("The number of parameters don't match for this method");
            }
            for (int i = 0; i < ParametersList.Count; i++)
            {
                if (variablesList[i].GetType().Equals(ParametersList[i].GetType()))
                {
                    ParametersList[i].Value = variablesList[i].Value;
                }
                else
                {
                    throw new ArgumentException("The parameters don't match.");
                }
            }
        }
    }
}
