using ITEJA_CustomLanguage.AbstractSyntaxTree.LogicBlocks.TokenClasses.Variables;
using ITEJA_CustomLanguage.Lexer;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System;

namespace ITEJA_CustomLanguage.AbstractSyntaxTree.LogicBlocks.TokenClasses.BodyStatements.Classes
{
    class ForCycleStatement : IForCycleStatement
    {
        public IIntegerVariable InnerCounterVariable { get; set; } = new IntegerVariable();
        public IIntegerVariable MaximumAllowedCounter { get; set; } = new IntegerVariable();
        public IList<Token> ComparisonOperators { get; set; } = new List<Token>();
        public bool IsIncremental { get; set; }
        public IList<IStatement> Statements { get; } = new List<IStatement>();
        public IList<IVariable> LocalVariables { get; } = new List<IVariable>();
        public IBodyStatement Parent { get; set; }
        private readonly Stack<Token> expressionTokens = new Stack<Token>();
        private ICondition condition;
        public void Execute()
        {
            UpdateExpression();
            CycleStatements();
        }

        private void CycleStatements()
        {
            condition = new Condition();
            if (condition.IsConditionTrue(expressionTokens, this))
            {
                foreach (var statement in Statements)
                {
                    statement.Execute();
                }
                AddStepToInnerCounterVariable();
                CycleStatements();
            }
        }

        private void AddStepToInnerCounterVariable()
        {
            if (IsIncremental)
            {
                foreach (var localVariable in LocalVariables)
                {
                    if (localVariable.Name.Equals(InnerCounterVariable.Name.ToString()))
                    {
                        int previousValue = int.Parse(localVariable.Value.ToString());
                        localVariable.Value.Clear();
                        localVariable.Value.Append(previousValue + 1);
                        break;
                    }
                }
            }
            else
            {
                foreach (var localVariable in LocalVariables)
                {
                    if (localVariable.Name.Equals(InnerCounterVariable.Name.ToString()))
                    {
                        int previousValue = int.Parse(localVariable.Value.ToString());
                        localVariable.Value.Clear();
                        localVariable.Value.Append(previousValue - 1);
                        break;
                    }
                }
            }
            UpdateExpression();
        }

        private void UpdateExpression()
        {
            Token innerCounterToken = new Token
            {
                Type = TokenType.NumberCharacters,
                Value = InnerCounterVariable.Value.ToString()
            };

            Token maxCounterToken = new Token
            {
                Type = TokenType.NumberCharacters,
                Value = MaximumAllowedCounter.Value.ToString()
            };
            expressionTokens.Clear();
            expressionTokens.Push(innerCounterToken);
            foreach (var oper in ComparisonOperators)
            {
                expressionTokens.Push(oper);
            }
            expressionTokens.Push(maxCounterToken);
        }
    }
}
