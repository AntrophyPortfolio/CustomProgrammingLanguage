using ITEJA_CustomLanguage.AbstractSyntaxTree.LogicBlocks.TokenClasses.BodyStatements;
using ITEJA_CustomLanguage.AbstractSyntaxTree.LogicBlocks.TokenClasses.Variables;
using ITEJA_CustomLanguage.Lexer;
using System.Collections.Generic;

namespace ITEJA_CustomLanguage.AbstractSyntaxTree.LogicBlocks.TokenClasses
{
    class ConditionIfStatement : IConditionIfStatement
    {
        public IList<IStatement> Statements { get; } = new List<IStatement>();
        public IList<IVariable> LocalVariables { get; } = new List<IVariable>();
        public IBodyStatement Parent { get; set; }
        public IConditionIfStatement ElseCondition { get; set; }
        public Stack<Token> ExpressionTokens { get; set; } = new Stack<Token>();
        private ICondition condition;

        public void Execute()
        {
            condition = new Condition();
            if (condition.IsConditionTrue(MainClass.ReplaceIdentifiersInExpression(ExpressionTokens, Parent), Parent))
            {
                foreach (var statement in Statements)
                {
                    statement.Execute();
                }
            }
            else
            {
                if (ElseCondition != null)
                {
                    foreach (var statement in ElseCondition.Statements)
                    {
                        statement.Execute();
                    }
                }
            }
        }
    }
}
