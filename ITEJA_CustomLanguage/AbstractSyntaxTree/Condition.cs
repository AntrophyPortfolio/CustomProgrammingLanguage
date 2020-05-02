using ITEJA_CustomLanguage.AbstractSyntaxTree.LogicBlocks;
using ITEJA_CustomLanguage.AbstractSyntaxTree.LogicBlocks.TokenClasses.BodyStatements;
using ITEJA_CustomLanguage.AbstractSyntaxTree.LogicBlocks.TokenClasses.Variables;
using ITEJA_CustomLanguage.AbstractSyntaxTree.LogicBlocks.TokenClasses.Variables.CalculateInteger;
using ITEJA_CustomLanguage.Lexer;
using System;
using System.Collections.Generic;
using System.IO;

namespace ITEJA_CustomLanguage.AbstractSyntaxTree
{
    public class Condition : ICondition
    {
        private Stack<Token> expressionTokens;
        private IBodyStatement parent;
        private string comparisonOperator = "";
        private double leftResult;
        private double rightResult;

        private void ParseExpression()
        {
            Stack<Token> leftSide = new Stack<Token>();
            Stack<Token> rightSide = new Stack<Token>();
            IList<Token> comparisonOperators = new List<Token>();
            bool isLeftSide = true;
            while (expressionTokens.Count != 0)
            {
                Token newToken = expressionTokens.Pop();
                if (IsComparsionOperatorFound(newToken))
                {
                    comparisonOperators.Add(newToken);
                    isLeftSide = false;
                }
                else if (isLeftSide)
                {
                    if (newToken.Type == TokenType.Identifier)
                    {
                        ChangeTokenData(newToken);
                    }
                    leftSide.Push(newToken);
                }
                else
                {
                    if (newToken.Type == TokenType.Identifier)
                    {
                        ChangeTokenData(newToken);

                    }
                    rightSide.Push(newToken);
                }
            }
            Expression exprLeft = new Expression(leftSide);
            leftResult = exprLeft.Calculate();

            Expression exprRight = new Expression(rightSide);
            rightResult = exprRight.Calculate();

            foreach (var oper in comparisonOperators)
            {
                comparisonOperator += oper.Value.ToString();
            }
        }

        private void ChangeTokenData(Token newToken)
        {
            IVariable existingVariable = MainClass.FindIdentifier(parent, newToken.Value.ToString());
            if (existingVariable is IIntegerVariable)
            {
                newToken.Type = TokenType.NumberCharacters;
            }
            else
            {
                newToken.Type = TokenType.StringCharacters;
            }
            newToken.Value = existingVariable.Value.ToString();
        }

        public bool IsConditionTrue(Stack<Token> parExpressionTokens, IBodyStatement parParent)
        {
            expressionTokens = new Stack<Token>(parExpressionTokens);
            parent = parParent;
            ParseExpression();
            return GetResultOfCondition();
        }

        private bool GetResultOfCondition()
        {
            return comparisonOperator switch
            {
                "<" => leftResult < rightResult,
                ">" => leftResult > rightResult,
                "<=" => leftResult <= rightResult,
                ">=" => leftResult >= rightResult,
                "==" => leftResult == rightResult,
                "!=" => leftResult != rightResult,
                _ => throw new InvalidDataException("Invalid operator encountered.")
            };
        }
        private bool IsComparsionOperatorFound(Token token)
        {
            return token.Type == TokenType.LessThan || token.Type == TokenType.HigherThan
                || token.Type == TokenType.ExclMark || token.Type == TokenType.Equals;
        }
    }
}
